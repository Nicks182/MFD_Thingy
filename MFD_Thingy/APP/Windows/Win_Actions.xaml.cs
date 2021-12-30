using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using MFD_Thingy.API;
using WindowsInput.Native;

namespace MFD_Thingy
{
    /// <summary>
    /// Interaction logic for Win_Actions.xaml
    /// </summary>
    public partial class Win_Actions : Window
    {
        DB_Logic G_DB_Logic = null;
        DB.Context.Action G_SelectedAction = null;
        int G_AppId = 0;
        MT_SimKey G_MT_SimKey = null;

        List<MT_Interaction> G_KeyboardInteractions = null;
        List<MT_Interaction> G_MouseInteractions = null;
        List<MT_Modifier> G_Modifiers = null;

        List<DB.Context.Screen> G_Screens = null;
        List<DB.Context.Action> G_Actions = null;

        public delegate void G_ActionsClosingHandler();
        public event G_ActionsClosingHandler G_OnActionsClosing;

        public Win_Actions(int P_AppId)
        {
            G_AppId = P_AppId;
            G_DB_Logic = new DB_Logic();
            G_MT_SimKey = new MT_SimKey();
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            G_Screens = G_DB_Logic._Get_Screens(G_AppId);
            G_KeyboardInteractions = G_MT_SimKey._GetKeyboardInteractions();
            G_MouseInteractions = G_MT_SimKey._GetMouseInteractions();
            G_Modifiers = G_MT_SimKey._GetModifiers(); ;

            Drop_ActionType.DisplayMemberPath = "Name";
            Drop_ActionType.SelectedValuePath = "Id";
            Drop_ActionType.ItemsSource = _GetInteractionTypes();
            Drop_ActionType.SelectedItem = Drop_ActionType.Items[0];
            Drop_ActionType.SelectionChanged += Drop_ActionType_SelectionChanged;

            Drop_Modifier.DisplayMemberPath = "Name";
            Drop_Modifier.SelectedValuePath = "Id";
            Drop_Modifier.ItemsSource = G_Modifiers;
            Drop_Modifier.SelectedItem = Drop_Modifier.Items[0];

            Drop_Interaction.DisplayMemberPath = "Name";
            Drop_Interaction.SelectedValuePath = "Id";
            Drop_Interaction.ItemsSource = G_KeyboardInteractions;

            Drop_Screen.DisplayMemberPath = "Name";
            Drop_Screen.SelectedValuePath = "Id";
            Drop_Screen.ItemsSource = G_Screens;
            Drop_Screen.Visibility = Visibility.Collapsed;


            _LoadData();
        }



        private void Drop_ActionType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch((Drop_ActionType.SelectedItem as MT_InteractionType).Id)
            {
                case 1:
                    Drop_Interaction.ItemsSource = G_MouseInteractions;
                    Drop_Screen.Visibility = Visibility.Collapsed;
                    Drop_Interaction.Visibility = Visibility.Visible;
                    break;

                case 2:
                    Drop_Screen.Visibility = Visibility.Visible;
                    Drop_Interaction.Visibility = Visibility.Collapsed;
                    break;

                default:
                    Drop_Interaction.ItemsSource = G_KeyboardInteractions;
                    Drop_Screen.Visibility = Visibility.Collapsed;
                    Drop_Interaction.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void Btn_New_Click(object sender, RoutedEventArgs e)
        {
            G_SelectedAction = new DB.Context.Action();
            G_SelectedAction.AppId = G_AppId;
            //G_SelectedAction.Position = G_Actions.Count;

            Txt_ActionName.Text = "";
            Drop_ActionType.SelectedIndex = 0;
            Txt_HoldTime.Text = MT_Utils.G_DefaultHoldTime.ToString();
            Drop_Modifier.SelectedIndex = 0;
            Drop_Interaction.SelectedIndex = 0;
            
            if (Drop_Screen.Items.Count > 0)
            {
                Drop_Screen.SelectedIndex = 0;
            }

            _ShowEdit();
        }

        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            if (Grid_Actions.SelectedItem != null)
            {
                G_SelectedAction = Grid_Actions.SelectedItem as DB.Context.Action;

                
                Txt_ActionName.Text = G_SelectedAction.Name;
                Drop_ActionType.SelectedValue = G_SelectedAction.Type;
                Txt_HoldTime.Text = G_SelectedAction.HoldTime.GetValueOrDefault(0).ToString();
                Drop_Modifier.SelectedItem = G_Modifiers.Where(m => m.Id.ToString().Equals(G_SelectedAction.Modifier)).FirstOrDefault();
                Drop_Interaction.SelectedItem = _GetInteraction(G_SelectedAction.Interaction);

                if (Drop_Screen.Items.Count > 0 && G_SelectedAction.ScreenId != null)
                {
                    Drop_Screen.SelectedItem = G_Screens.Where(s => s.Id == G_SelectedAction.ScreenId.Value).FirstOrDefault();
                }

                _ShowEdit();
            }
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (Grid_Actions.SelectedItem != null)
            {
                G_SelectedAction = Grid_Actions.SelectedItem as DB.Context.Action;
                if (MessageBox.Show("Are you sure you want to delete the selected Action?", "Warning!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    G_DB_Logic._Delete_Action(G_SelectedAction);
                    _LoadData();
                }
            }
        }


        private void Txt_Position_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !MT_Utils._IsTextAllowed(e.Text);
        }

        private void Txt_HoldTime_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !MT_Utils._IsTextAllowed(e.Text);
        }

        private void Btn_Done_Cancel_Click(object sender, RoutedEventArgs e)
        {
            _ShowData();
        }

        private void Btn_Done_Save_Click(object sender, RoutedEventArgs e)
        {
            G_SelectedAction.Name           = Txt_ActionName.Text.Trim();
            G_SelectedAction.Type           = (Drop_ActionType.SelectedItem as MT_InteractionType).Id;
            G_SelectedAction.HoldTime       = MT_Utils._GetDefaultHoldTime(Txt_HoldTime.Text);
            G_SelectedAction.Modifier       = (Drop_Modifier.SelectedItem as MT_Modifier).Id.ToString();
            G_SelectedAction.Interaction    = (Drop_Interaction.SelectedItem as MT_Interaction).Name;
            
            if (Drop_Screen.SelectedItem != null)
            {
                G_SelectedAction.ScreenId = (Drop_Screen.SelectedItem as DB.Context.Screen).Id;
            }

            G_DB_Logic._Save_Action(G_SelectedAction);
            _LoadData();
            _ShowData();
        }


        private MT_Modifier _GetModifier(string P_Modifier)
        {
            MT_Modifier L_Mod = G_MT_SimKey._GetModifiers().Where(m => m.Id.ToString() == P_Modifier).FirstOrDefault();

            if (L_Mod == null)
            {
                return G_MT_SimKey._GetModifiers().Where(m => m.Id == MT_ModifierId.None).FirstOrDefault();
            }
            return L_Mod;
        }

        private MT_Interaction _GetInteraction(string P_Interaction)
        {
            
            MT_Interaction L_Key = G_KeyboardInteractions.Where(k => k.Name == P_Interaction).FirstOrDefault();

            switch ((Drop_ActionType.SelectedItem as MT_InteractionType).Id)
            {
                case 1:
                    {
                        L_Key = G_MouseInteractions.Where(k => k.Name == P_Interaction).FirstOrDefault();
                        if (L_Key == null)
                        {
                            return G_MouseInteractions.Where(k => k.Name == "NONE").FirstOrDefault();
                        }
                    }
                    break;

                default:
                    {
                        L_Key = G_KeyboardInteractions.Where(k => k.Name.ToLower() == P_Interaction.ToLower()).FirstOrDefault();

                        if (L_Key == null)
                        {
                            return G_KeyboardInteractions.Where(k => k.Name == VirtualKeyCode.None.ToString()).FirstOrDefault();
                        }
                    }
                    break;
            }

            
            return L_Key;
        }

        private void _LoadData()
        {
            G_Actions = G_DB_Logic._Get_Actions(G_AppId);
            Grid_Actions.ItemsSource = G_Actions;

            if (Grid_Actions.SelectedItem == null && Grid_Actions.Items.Count > 0)
            {
                Grid_Actions.SelectedItem = Grid_Actions.Items[0];
            }
        }

        private void _ShowData()
        {
            Grid_ActionsData.Visibility = Visibility.Visible;
            Grid_ActionsEdit.Visibility = Visibility.Collapsed;
        }

        private void _ShowEdit()
        {
            Grid_ActionsData.Visibility = Visibility.Collapsed;
            Grid_ActionsEdit.Visibility = Visibility.Visible;
        }

        private List<MT_InteractionType> _GetInteractionTypes()
        {
            return new List<MT_InteractionType>
            {
                new MT_InteractionType
                {
                    Id = 0,
                    Name = "Keyboard"
                },
                new MT_InteractionType
                {
                    Id = 1,
                    Name = "Mouse"
                }
                ,
                new MT_InteractionType
                {
                    Id = 2,
                    Name = "Screen"
                }
            };
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            G_OnActionsClosing?.Invoke();
        }
    }

    
}
