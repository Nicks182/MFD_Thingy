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
    /// Interaction logic for Win_Config.xaml
    /// </summary>
    public partial class Win_Config : Window
    {
        DB_Logic G_DB_Logic = null;
        DB.Context.Application G_SelectedApp = null;
        public Win_Config()
        {
            G_DB_Logic = new DB_Logic();
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _LoadApps();
        }

        private void Btn_Themes_Click(object sender, RoutedEventArgs e)
        {
            if (Grid_Applications.SelectedItem != null)
            {
                Win_Themes L_Win_Themes = new Win_Themes((Grid_Applications.SelectedItem as DB.Context.Application).Id);
                L_Win_Themes.ShowDialog();
            }
        }

        private void Btn_Actions_Click(object sender, RoutedEventArgs e)
        {
            if (Grid_Applications.SelectedItem != null)
            {
                Win_Actions L_Win_Actions = new Win_Actions((Grid_Applications.SelectedItem as DB.Context.Application).Id);
                L_Win_Actions.ShowDialog();
            }
        }

        private void Btn_Screens_Click(object sender, RoutedEventArgs e)
        {
            if (Grid_Applications.SelectedItem != null)
            {
                Win_Screens L_Win_Screens = new Win_Screens((Grid_Applications.SelectedItem as DB.Context.Application).Id);
                L_Win_Screens.ShowDialog();
            }
        }


        private void Btn_NewApp_Click(object sender, RoutedEventArgs e)
        {
            G_SelectedApp = new DB.Context.Application();
            _ShowEdit();
        }

        private void Btn_EditApp_Click(object sender, RoutedEventArgs e)
        {
            if (Grid_Applications.SelectedItem != null)
            {
                G_SelectedApp = Grid_Applications.SelectedItem as DB.Context.Application;
                Txt_ApplicationName.Text = G_SelectedApp.Name;
                _ShowEdit();
            }
        }

        private void Btn_DeleteApp_Click(object sender, RoutedEventArgs e)
        {
            if (Grid_Applications.SelectedItem != null)
            {
                G_SelectedApp = Grid_Applications.SelectedItem as DB.Context.Application;
                if (MessageBox.Show("Are you sure you want to delete the selected Application? This wil remove any related Themes, Screens, and Actions!", "Warning!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    G_DB_Logic._Delete_App(G_SelectedApp);
                    _LoadApps();
                }
            }
        }



        private void Btn_AppSave_Cancel_Click(object sender, RoutedEventArgs e)
        {
            _LoadApps();
            _ShowData();

        }

        private void Btn_AppSave_Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Txt_ApplicationName.Text) == true)
            {
                MessageBox.Show("Name cannot be empty.");
            }
            else
            {
                G_SelectedApp.Name = Txt_ApplicationName.Text;
                G_DB_Logic._Save_App(G_SelectedApp);
                _LoadApps();
                _ShowData();
            }
            
        }


        private void _LoadApps()
        {
            Grid_Applications.ItemsSource = G_DB_Logic._Get_Apps();

            if(Grid_Applications.SelectedItem == null && Grid_Applications.Items.Count > 0)
            {
                Grid_Applications.SelectedItem = Grid_Applications.Items[0];
            }
        }

        private void _ShowData()
        {
            Grid_Apps.Visibility = Visibility.Visible;
            Grid_NewEditApp.Visibility = Visibility.Collapsed;
        }

        private void _ShowEdit()
        {
            Grid_Apps.Visibility = Visibility.Collapsed;
            Grid_NewEditApp.Visibility = Visibility.Visible;
        }
    }
}
