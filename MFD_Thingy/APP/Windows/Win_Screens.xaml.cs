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
    /// Interaction logic for Win_Screens.xaml
    /// </summary>
    public partial class Win_Screens : Window
    {
        DB_Logic G_DB_Logic = null;
        DB.Context.Screen G_SelectedScreen = null;

        List<DB.Context.Screen> G_Screens = null;
        List<DB.Context.Theme> G_Themes = null;
        
        int G_AppId = 0;

        public Win_Screens(int P_AppId)
        {
            G_AppId = P_AppId;
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

        private void Btn_New_Click(object sender, RoutedEventArgs e)
        {
            G_SelectedScreen = new DB.Context.Screen();
            G_SelectedScreen.AppId = G_AppId;

            Txt_ScreenName.Text = "";
            Drop_Theme.SelectedItem = null;


            _ShowEdit();
        }

        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            if (Grid_Screens.SelectedItem != null)
            {
                G_SelectedScreen = Grid_Screens.SelectedItem as DB.Context.Screen;

                Txt_ScreenName.Text = G_SelectedScreen.Name;
                if (G_SelectedScreen.ThemeId != null)
                {
                    Drop_Theme.SelectedItem = G_Themes.Where(t => t.Id == G_SelectedScreen.ThemeId).FirstOrDefault();
                }
                _ShowEdit();
            }
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (Grid_Screens.SelectedItem != null)
            {
                G_SelectedScreen = Grid_Screens.SelectedItem as DB.Context.Screen;
                if (MessageBox.Show("Are you sure you want to delete the selected Screen?", "Warning!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    G_DB_Logic._Delete_Screen(G_SelectedScreen);
                    _LoadData();
                }
            }
        }



        private void Btn_Done_Save_Click(object sender, RoutedEventArgs e)
        {
            G_SelectedScreen.Name = Txt_ScreenName.Text.Trim();
            if (Drop_Theme.SelectedItem != null && (Drop_Theme.SelectedItem as DB.Context.Theme).Id != -1)
            {
                G_SelectedScreen.ThemeId = (Drop_Theme.SelectedItem as DB.Context.Theme).Id;
            }
            else
            {
                G_SelectedScreen.ThemeId = null;
            }


            G_DB_Logic._Save_Screen(G_SelectedScreen);
            _LoadData();
            _ShowData();
        }

        private void Btn_Done_Cancel_Click(object sender, RoutedEventArgs e)
        {
            _ShowData();
        }



        private void Btn_Constrols_Click(object sender, RoutedEventArgs e)
        {
            if (Grid_Screens.SelectedItem != null)
            {
                G_SelectedScreen = Grid_Screens.SelectedItem as DB.Context.Screen;
                Win_Controls L_Win_Controls = new Win_Controls(G_AppId, G_SelectedScreen.Id);
                L_Win_Controls.ShowDialog();
            }
        }

        private void _LoadData()
        {
            G_Screens = G_DB_Logic._Get_Screens(G_AppId);
            Grid_Screens.ItemsSource = G_Screens;

            if (Grid_Screens.SelectedItem == null && Grid_Screens.Items.Count > 0)
            {
                Grid_Screens.SelectedItem = Grid_Screens.Items[0];
            }
        }

        private void _ShowData()
        {
            Grid_ScreenData.Visibility = Visibility.Visible;
            Grid_ScreenEdit.Visibility = Visibility.Collapsed;
        }

        private void _ShowEdit()
        {
            Grid_ScreenData.Visibility = Visibility.Collapsed;
            Grid_ScreenEdit.Visibility = Visibility.Visible;
        }

    }
}
