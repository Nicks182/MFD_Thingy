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
    /// Interaction logic for Win_Themes.xaml
    /// </summary>
    public partial class Win_Themes : Window
    {
        DB_Logic G_DB_Logic = null;
        DB.Context.Theme G_SelectedTheme = null;
        int G_AppId = 0;
        public Win_Themes(int P_AppId)
        {
            G_AppId = P_AppId;
            G_DB_Logic = new DB_Logic();
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _LoadThemes();
        }

        private void Btn_NewTheme_Click(object sender, RoutedEventArgs e)
        {
            G_SelectedTheme = new DB.Context.Theme();
            G_SelectedTheme.AppId = G_AppId;

            Txt_ItemWidth.Text                      = MT_Utils.G_DefaultItemMinWidth.ToString();
            Txt_ItemSpacing.Text                    = MT_Utils.G_DefaultItemSpacing.ToString();
            Color_Forground.SelectedColor           = MT_Utils.G_DefaultForground;
            Color_Background.SelectedColor          = MT_Utils.G_DefaultBackground;
            Color_ScreenBackground.SelectedColor    = MT_Utils.G_DefaultScreenBackground;

            _ShowEdit();
        }

        private void Btn_EditTheme_Click(object sender, RoutedEventArgs e)
        {
            if (Grid_Themes.SelectedItem != null)
            {
                G_SelectedTheme                         = Grid_Themes.SelectedItem as DB.Context.Theme;

                Txt_ThemeName.Text                      = G_SelectedTheme.Name;
                Txt_ItemWidth.Text                      = MT_Utils._GetDefaultMinWidth(G_SelectedTheme.Item_MinWidth).ToString();
                Txt_ItemSpacing.Text                    = MT_Utils._GetDefaultSpacing(G_SelectedTheme.Item_Spacing).ToString();
                Color_Forground.SelectedColor           = MT_Utils._ConvertStringToColor(G_SelectedTheme.Color_Forground, MT_Utils.G_DefaultForground);
                Color_Background.SelectedColor          = MT_Utils._ConvertStringToColor(G_SelectedTheme.Color_Background, MT_Utils.G_DefaultBackground);
                Color_ScreenBackground.SelectedColor    = MT_Utils._ConvertStringToColor(G_SelectedTheme.Color_ScreenBackground, MT_Utils.G_DefaultScreenBackground);
                
                _ShowEdit();
            }
        }

        private void Btn_DeleteTheme_Click(object sender, RoutedEventArgs e)
        {
            if (Grid_Themes.SelectedItem != null)
            {
                G_SelectedTheme = Grid_Themes.SelectedItem as DB.Context.Theme;
                if (MessageBox.Show("Are you sure you want to delete the selected Theme?", "Warning!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    G_DB_Logic._Delete_Theme(G_SelectedTheme);
                    _LoadThemes();
                }
            }
        }

        private void Btn_ThemeSave_Save_Click(object sender, RoutedEventArgs e)
        {
            G_SelectedTheme.Name = Txt_ThemeName.Text.Trim();
            
            G_SelectedTheme.Item_MinWidth           = MT_Utils._GetDefaultMinWidth(Txt_ItemWidth.Text);
            G_SelectedTheme.Item_Spacing            = MT_Utils._GetDefaultSpacing(Txt_ItemSpacing.Text);
            G_SelectedTheme.Color_Forground         = MT_Utils._ConvertColorToString(Color_Forground.SelectedColor);
            G_SelectedTheme.Color_Background        = MT_Utils._ConvertColorToString(Color_Background.SelectedColor);
            G_SelectedTheme.Color_ScreenBackground  = MT_Utils._ConvertColorToString(Color_ScreenBackground.SelectedColor);

            G_DB_Logic._Save_Theme(G_SelectedTheme);
            _LoadThemes();
            _ShowData();
        }

        private void Btn_ThemeSave_Cancel_Click(object sender, RoutedEventArgs e)
        {
            _ShowData();
        }

        private void Txt_ItemWidth_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !MT_Utils._IsTextAllowed(e.Text);
        }

        private void Txt_ItemSpacing_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !MT_Utils._IsTextAllowed(e.Text);
        }



        private void _LoadThemes()
        {
            Grid_Themes.ItemsSource = G_DB_Logic._Get_Themes(G_AppId);

            if (Grid_Themes.SelectedItem == null && Grid_Themes.Items.Count > 0)
            {
                Grid_Themes.SelectedItem = Grid_Themes.Items[0];
            }
        }

        private void _ShowData()
        {
            Grid_ThemesData.Visibility = Visibility.Visible;
            Grid_ThemesEdit.Visibility = Visibility.Collapsed;
        }

        private void _ShowEdit()
        {
            Grid_ThemesData.Visibility = Visibility.Collapsed;
            Grid_ThemesEdit.Visibility = Visibility.Visible;
        }
    }
}
