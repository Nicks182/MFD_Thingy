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
    /// Interaction logic for Win_Controls.xaml
    /// </summary>
    public partial class Win_Controls : Window
    {
        DB_Logic G_DB_Logic = null;
        DB.Context.Screen_Control G_SelectedControl = null;

        List<DB.Context.Screen_Control> G_Controls = null;
        List<DB.Context.Theme> G_Themes = null;

        int G_AppId = 0;
        int G_ScreenId = 0;

        public Win_Controls(int P_AppId, int P_ScreenId)
        {
            G_AppId = P_AppId;
            G_ScreenId = P_ScreenId;
            G_DB_Logic = new DB_Logic();
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            G_Themes = G_DB_Logic._Get_Themes(G_AppId);
            G_Themes.Insert(0, new DB.Context.Theme
            {
                Id = -1,
                Name = "None"
            });

            Drop_Theme.DisplayMemberPath = "Name";
            Drop_Theme.SelectedValuePath = "Id";
            Drop_Theme.ItemsSource = G_Themes;

            _LoadData();
        }

        private void Btn_Done_Cancel_Click(object sender, RoutedEventArgs e)
        {
            _ShowData();
        }

        private void Btn_Done_Save_Click(object sender, RoutedEventArgs e)
        {
            G_SelectedControl.Name = Txt_ControlName.Text.Trim();

            if(Drop_Theme.SelectedItem != null && (Drop_Theme.SelectedItem as DB.Context.Theme).Id != -1)
            {
                G_SelectedControl.ThemeId = (Drop_Theme.SelectedItem as DB.Context.Theme).Id;
            }
            else
            {
                G_SelectedControl.ThemeId = null;
            }

            G_DB_Logic._Save_Control(G_SelectedControl);
            _LoadData();
            _ShowData();
        }

        private void Btn_New_Click(object sender, RoutedEventArgs e)
        {
            G_SelectedControl = new DB.Context.Screen_Control();
            G_SelectedControl.ScreenId = G_ScreenId;
            G_SelectedControl.Position = G_Controls.Count;

            Txt_ControlName.Text = "";
            Drop_Theme.SelectedItem = null;

            _ShowEdit();
        }

        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            if (Grid_Controls.SelectedItem != null)
            {
                G_SelectedControl = Grid_Controls.SelectedItem as DB.Context.Screen_Control;

                Txt_ControlName.Text = G_SelectedControl.Name;

                if(G_SelectedControl.ThemeId != null)
                {
                    Drop_Theme.SelectedItem = G_Themes.Where(t => t.Id == G_SelectedControl.ThemeId).FirstOrDefault();
                }
                else
                {
                    Drop_Theme.SelectedItem = null;
                }

                _ShowEdit();
            }
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (Grid_Controls.SelectedItem != null)
            {
                G_SelectedControl = Grid_Controls.SelectedItem as DB.Context.Screen_Control;
                if (MessageBox.Show("Are you sure you want to delete the selected Control?", "Warning!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    G_DB_Logic._Delete_Control(G_SelectedControl);

                    _LoadData();
                }
            }
        }

        private void Btn_Actions_Click(object sender, RoutedEventArgs e)
        {
            if (Grid_Controls.SelectedItem != null)
            {
                G_SelectedControl = Grid_Controls.SelectedItem as DB.Context.Screen_Control;
                Win_ControlActions L_Win_ControlActions = new Win_ControlActions(G_AppId, G_SelectedControl.Id);
                L_Win_ControlActions.Show();
            }
        }


        private void Btn_MoveUp_Click(object sender, RoutedEventArgs e)
        {
            if (Grid_Controls.SelectedItem != null)
            {
                G_SelectedControl = Grid_Controls.SelectedItem as DB.Context.Screen_Control;

                int L_CurrentIndex = G_Controls.IndexOf(G_SelectedControl);
                int L_NewIndex = L_CurrentIndex - 1;
                if (L_NewIndex > -1)
                {
                    G_Controls.RemoveAt(L_CurrentIndex);
                    G_Controls.Insert(L_NewIndex, G_SelectedControl);
                    _UpdatePositions();
                }
            }
        }

        private void Btn_MoveDown_Click(object sender, RoutedEventArgs e)
        {
            if (Grid_Controls.SelectedItem != null)
            {
                G_SelectedControl = Grid_Controls.SelectedItem as DB.Context.Screen_Control;

                int L_CurrentIndex = G_Controls.IndexOf(G_SelectedControl);
                int L_NewIndex = L_CurrentIndex + 1;
                if (L_NewIndex < G_Controls.Count)
                {
                    G_Controls.RemoveAt(L_CurrentIndex);
                    G_Controls.Insert(L_NewIndex, G_SelectedControl);
                    _UpdatePositions();
                }
            }
        }

        private void _UpdatePositions()
        {
            for (int i = 0; i < G_Controls.Count; i++)
            {
                G_Controls[i].Position = i;
                G_DB_Logic._Save_Control(G_Controls[i]);
            }

            _LoadData();
        }


        private void _LoadData()
        {
            G_Controls = G_DB_Logic._Get_Screen_Controls(G_ScreenId);
            Grid_Controls.ItemsSource = G_Controls;

            if (Grid_Controls.SelectedItem == null && Grid_Controls.Items.Count > 0)
            {
                Grid_Controls.SelectedItem = Grid_Controls.Items[0];
            }
        }

        private void _ShowData()
        {
            Grid_ControlData.Visibility = Visibility.Visible;
            Grid_ControlEdit.Visibility = Visibility.Collapsed;
        }

        private void _ShowEdit()
        {
            Grid_ControlData.Visibility = Visibility.Collapsed;
            Grid_ControlEdit.Visibility = Visibility.Visible;
        }
    }
}
