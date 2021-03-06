
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using MFD_Thingy.APP.Services;
using QRCoder;

namespace MFD_Thingy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ServerService serverService;
        private MessageBusService messageBusService;

        public MainWindow()
        {
            this.serverService = App.ServiceProvider.GetService(typeof(ServerService)) as ServerService;
            this.messageBusService = App.ServiceProvider.GetService(typeof(MessageBusService)) as MessageBusService;

            this.messageBusService.RegisterEvent("serverstatuschanged", (status) =>
            {
                ServerStatus = this.serverService.GetServerStatus();
                Txt_ServerOutput.Text += DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + ":: Server Status: " + ServerStatus + Environment.NewLine;
                Txt_ServerOutput.ScrollToEnd();

                if (ServerStatus.Contains("Running") == true)
                {
                    _PrintURL();
                }
            });

            this.messageBusService.RegisterEvent("controllermessage", (status) =>
            {
                if (Check_LogAll.IsChecked.GetValueOrDefault(false) == true)
                {
                    Txt_ServerOutput.Text += DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + ":: API Message: " + status + Environment.NewLine;
                    Txt_ServerOutput.ScrollToEnd();
                }
            });


            InitializeComponent();
        }




        public string ServerStatus
        {
            get
            {
                return (string)GetValue(ServerStatusProperty);
            }
            set
            {
                SetValue(ServerStatusProperty, value);
            }
        }

        public readonly
             DependencyProperty ServerStatusProperty
                 = DependencyProperty.Register(
                                     "ServerStatusProperty",
                                     typeof(string),
                                     typeof(MainWindow), new UIPropertyMetadata("started"));

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string bla = "";
        }

        private void Btn_Config_Click(object sender, RoutedEventArgs e)
        {
            Win_Config L_Win_Config = new Win_Config();
            L_Win_Config.ShowDialog();
        }

        private void Btn_Start_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                serverService.RestartServer();
            });
        }

        private void Btn_Stop_Click(object sender, RoutedEventArgs e)
        {
            serverService.StopServer();
        }

        private void _PrintURL()
        {
            Txt_ServerAddress.Text = "http://" + GetLocalIPAddress() + ":5000";

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(Txt_ServerAddress.Text, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20, System.Drawing.Color.White, System.Drawing.Color.Black, true);

            Img_QRCode.Source = BitmapToImageSource(qrCodeImage);
        }

        public string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            
            return "No network adapters with an IPv4 address in the system!";
        }

        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        private void Check_ShowStatic_Checked(object sender, RoutedEventArgs e)
        {
            G_GlobalSettings.ShowStatic = Check_ShowStatic.IsChecked.Value;
        }
    }
}
