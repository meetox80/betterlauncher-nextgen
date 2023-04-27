using betterlauncher_cs.modules;
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.Auth.Microsoft.MsalClient;
using Microsoft.Identity.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MessageBox = System.Windows.Forms.MessageBox;

namespace betterlauncher_cs
{
    public partial class MainWindow : Window
    {
        public static string version = "0.0.0";
        public static string mslogin_clientid = "c774e229-39fa-48ea-ad91-b0fb7e75945d";

        public MainWindow()
        {
            // post-app run lag spike on animations delay
            Thread.Sleep(2500 / Environment.ProcessorCount);
            InitializeComponent();
        }

        private async void Window_Initialized(object sender, EventArgs e)
        {
            ConfigManager.Prepare();
            handler.Height = 35; handler.Width = 35; handler.RadiusX = 35; handler.RadiusY = 35;
            navbar.Opacity = 0.0f;
            contenthandler.Opacity = 0.0f;
            app = await MsalMinecraftLoginHelper.BuildApplicationWithCache(mslogin_clientid);
            others_setvisiblity(false);
        }

        #region MicrosoftLogin
        public static IPublicClientApplication app;
        public static JavaEditionLoginHandler loginHandler;
        public static MSession session;
        public static CancellationTokenSource loginCancel;
        private void mslogin_success(MSession session) => setinfo(session, null);
        private async Task LoginAndShowResultOnUI(JavaEditionLoginHandler loginHandler)
        {
            try
            {
                var session = await loginHandler.LoginFromOAuth();
                mslogin_success(session.GameSession);
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("canceled"))
                    return;
                MessageBox.Show(ex.ToString());
            }
        }
        private async void mslogin()
        {
            if (app == null) return;

            loginHandler = new LoginHandlerBuilder()
                .ForJavaEdition()
                .WithMsalOAuth(app, factory => factory.CreateWithEmbeddedWebView())
                .Build();

            await LoginAndShowResultOnUI(loginHandler);
        }
        private void mslogin_init(object sender, System.Windows.Input.MouseButtonEventArgs e) => mslogin();
        #endregion

        private void navbar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void setinfo(MSession session, string username)
        {
            contenthandler.SelectedIndex = 1;

            // mslogin
            if (session != null && username == null)
            {
                BitmapImage playerimage_bg_bitmap = new BitmapImage(new Uri($"https://crafatar.com/avatars/{session.UUID}"));
                FormatConvertedBitmap playerimage_bg_grayscale = new FormatConvertedBitmap(playerimage_bg_bitmap, PixelFormats.Gray8, null, 0);
                playerimage_bg.Source = playerimage_bg_grayscale;
                playerimage.Source = new BitmapImage(new Uri($"https://crafatar.com/renders/body/{session.UUID}"));
                playernickname.Text = session.Username;
            }
        }

        private void addaccount_submit(object sender, KeyEventArgs e)
        {
            // add non-premium account
            if (e.Key == Key.Return)
            {
                ConfigManager.addaccount(contenthandler_login_other_addaccount_label.Text);
                contenthandler_login_other_addaccount_label.Text = "";
                refreshaccountlist();
            }
        }

        private void refreshaccountlist()
        {
            contenthandler_login_other_stackpanel.Children.Clear();

            for (int i = 0; i < ConfigManager.getaccountcount(); i++)
            {
                Label account = new Label();
                account.Content = ConfigManager.getaccountname(i);
                account.Foreground = Brushes.White;
                account.HorizontalAlignment = HorizontalAlignment.Center;
                account.VerticalAlignment = VerticalAlignment.Center;
                account.FontSize = 20;
                account.MouseDown += Account_MouseDown;
                contenthandler_login_other_stackpanel.Children.Add(account);
            }
        }

        private void Account_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {

            }

            if (e.ChangedButton == MouseButton.Right)
            {
                for (int i = 0; i < contenthandler_login_other_stackpanel.Children.Count; i++)
                {
                    int index = contenthandler_login_other_stackpanel.Children.IndexOf((UIElement)sender);

                    if (index >= 0)
                    {
                        ConfigManager.removeaccount(sender.ToString().Split(':')[1].Substring(1));
                        contenthandler_login_other_stackpanel.Children.RemoveAt(index);
                        refreshaccountlist();
                    }
                }
            }
        }

        private void contenthandler_login_otherbutton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            others_setvisiblity(true);
            refreshaccountlist();
        }

        private void others_setvisiblity(bool visiblity)
        {
            if (visiblity == false)
            {
                contenthandler_login_other_addaccount_label.Visibility = Visibility.Collapsed;
                contenthandler_login_other_addaccount.Visibility = Visibility.Collapsed;
                contenthandler_login_other_stackpanel.Visibility = Visibility.Collapsed;
                contenthandler_login_other_scrollviewer.Visibility = Visibility.Collapsed;
                contenthandler_login_other_container.Visibility = Visibility.Collapsed;
            } else
            {
                contenthandler_login_other_addaccount_label.Visibility = Visibility.Visible;
                contenthandler_login_other_addaccount.Visibility = Visibility.Visible;
                contenthandler_login_other_stackpanel.Visibility = Visibility.Visible;
                contenthandler_login_other_scrollviewer.Visibility = Visibility.Visible;
                contenthandler_login_other_container.Visibility = Visibility.Visible;
            }
        }
    }
}