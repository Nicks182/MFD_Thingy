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

namespace MFD_Thingy
{
    /// <summary>
    /// Interaction logic for Win_ControlActions.xaml
    /// </summary>
    public partial class Win_ControlActions : Window
    {
        DB_Logic G_DB_Logic = null;
        DB.Context.Action G_SelectedAction = null;
        DB.Context.Control_Action G_SelectedControlAction = null;

        List<DB.Context.Action> G_ControlActions = null;
        List<DB.Context.Action> G_Actions = null;
        List<DB.Context.Control_Action> G_Control_Actions = null;

        int G_AppId = 0;
        int G_ControlId = 0;

        public Win_ControlActions(int P_AppId, int P_ControlId)
        {
            G_AppId = P_AppId;
            G_ControlId = P_ControlId;
            G_DB_Logic = new DB_Logic();
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Drop_Actions.DisplayMemberPath = "Name";
            Drop_Actions.SelectedValuePath = "Id";

            _LoadDropdownData();
            _LoadData();
        }

        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            // Remove
            if (Grid_ControlActions.SelectedItem != null)
            {
                G_SelectedAction = Grid_ControlActions.SelectedItem as DB.Context.Action;
                G_Control_Actions = G_DB_Logic._Get_Control_Actions(G_ControlId);
                G_SelectedControlAction = G_Control_Actions.Where(ca => ca.ActionId == G_SelectedAction.Id).FirstOrDefault();

                G_DB_Logic._Delete_Control_Action(G_SelectedControlAction);

                _LoadData();
            }
        }


        private void Btn_New_Click(object sender, RoutedEventArgs e)
        {
            G_SelectedControlAction = new DB.Context.Control_Action();
            G_SelectedControlAction.ControlId = G_ControlId;

            _ShowEdit();
        }

        private void Btn_Done_Save_Click(object sender, RoutedEventArgs e)
        {
            if (Drop_Actions.SelectedItem == null)
            {
                MessageBox.Show("Select Action.");
                return;
            }

            G_SelectedControlAction.ActionId = (Drop_Actions.SelectedItem as DB.Context.Action).Id;

            G_DB_Logic._Save_Control_Action(G_SelectedControlAction);
            _LoadData();
            _ShowData();
        }

        private void Btn_Done_Cancel_Click(object sender, RoutedEventArgs e)
        {
            _ShowData();
        }

        private void Btn_MoveUp_Click(object sender, RoutedEventArgs e)
        {
            if (Grid_ControlActions.SelectedItem != null)
            {
                G_SelectedAction = Grid_ControlActions.SelectedItem as DB.Context.Action;

                G_Control_Actions = G_DB_Logic._Get_Control_Actions(G_ControlId);
                G_SelectedControlAction = G_Control_Actions.Where(ca => ca.ActionId == G_SelectedAction.Id).FirstOrDefault();

                int L_CurrentIndex = G_Control_Actions.IndexOf(G_SelectedControlAction);
                int L_NewIndex = L_CurrentIndex - 1;
                if (L_NewIndex > -1)
                {
                    G_Control_Actions.RemoveAt(L_CurrentIndex);
                    G_Control_Actions.Insert(L_NewIndex, G_SelectedControlAction);
                    _UpdatePositions();
                }
            }
        }

        private void Btn_MoveDown_Click(object sender, RoutedEventArgs e)
        {
            if (Grid_ControlActions.SelectedItem != null)
            {
                G_SelectedAction = Grid_ControlActions.SelectedItem as DB.Context.Action;

                G_Control_Actions = G_DB_Logic._Get_Control_Actions(G_ControlId);
                G_SelectedControlAction = G_Control_Actions.Where(ca => ca.ActionId == G_SelectedAction.Id).FirstOrDefault();

                int L_CurrentIndex = G_Control_Actions.IndexOf(G_SelectedControlAction);
                int L_NewIndex = L_CurrentIndex + 1;
                if (L_NewIndex < G_Control_Actions.Count)
                {
                    G_Control_Actions.RemoveAt(L_CurrentIndex);
                    G_Control_Actions.Insert(L_NewIndex, G_SelectedControlAction);
                    _UpdatePositions();
                }
            }
        }

        private void _UpdatePositions()
        {
            for (int i = 0; i < G_Control_Actions.Count; i++)
            {
                G_Control_Actions[i].Position = i;
                G_DB_Logic._Save_Control_Action(G_Control_Actions[i]);
            }

            _LoadData();
        }


        private void Btn_Actions_Click(object sender, RoutedEventArgs e)
        {
            Win_Actions L_Win_Actions = new Win_Actions(G_AppId);
            L_Win_Actions.G_OnActionsClosing += L_Win_Actions_G_OnActionsClosing;
            L_Win_Actions.ShowDialog();
        }

        private void L_Win_Actions_G_OnActionsClosing()
        {
            _LoadDropdownData();
            _LoadData();
        }


        private void _LoadDropdownData()
        {
            G_Actions = G_DB_Logic._Get_Actions(G_AppId);
            Drop_Actions.ItemsSource = G_Actions;
        }


        private void _LoadData()
        {
            G_ControlActions = G_DB_Logic._Get_ControlActions(G_ControlId);
            Grid_ControlActions.ItemsSource = G_ControlActions;

            if (Grid_ControlActions.SelectedItem == null && Grid_ControlActions.Items.Count > 0)
            {
                Grid_ControlActions.SelectedItem = Grid_ControlActions.Items[0];
            }
        }

        private void _ShowData()
        {
            _LoadDropdownData();
            Grid_ControlActionsData.Visibility = Visibility.Visible;
            Grid_ControlActionsEdit.Visibility = Visibility.Collapsed;
        }

        private void _ShowEdit()
        {
            Grid_ControlActionsData.Visibility = Visibility.Collapsed;
            Grid_ControlActionsEdit.Visibility = Visibility.Visible;
        }
    }
}
