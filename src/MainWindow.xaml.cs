using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.Auth.Microsoft.MsalClient;
using CmlLib.Core.Version;
using CmlLib.Core.VersionMetadata;
using Microsoft.Identity.Client;
using System;
using System.Diagnostics;
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
            handler.Height = 35; handler.Width = 35; handler.RadiusX = 35; handler.RadiusY = 35;
            navbar.Opacity = 0.0f;
            contenthandler.Opacity = 0.0f;
            app = await MsalMinecraftLoginHelper.BuildApplicationWithCache(mslogin_clientid);
        }

        #region MicrosoftLogin
        public static IPublicClientApplication app;
        public static JavaEditionLoginHandler loginHandler;
        public static MSession session;
        public static CancellationTokenSource loginCancel;
        private void mslogin_success(MSession session) => setinfoAsync(session, null);
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

        private async Task setinfoAsync(MSession session, string username)
        {
            contenthandler.SelectedIndex = 1;

            // mslogin
            if (session != null && username == null)
            {
                BitmapImage playerimage_bg_bitmap = new BitmapImage(new Uri($"https://crafatar.com/avatars/{session.UUID}"));
                FormatConvertedBitmap playerimage_bg_grayscale = new FormatConvertedBitmap(playerimage_bg_bitmap, PixelFormats.Gray8, null, 0);
                contenthandler_version_playerimage_bg.Source = playerimage_bg_grayscale;
                contenthandler_version_playerimage.Source = new BitmapImage(new Uri($"https://crafatar.com/renders/body/{session.UUID}"));
                contenthandler_version_playerimage_blurred.Source = contenthandler_version_playerimage.Source;
                contenthandler_version_playernickname.Text = session.Username;
            }

            var launcherver = new CMLauncher(new MinecraftPath());
            MVersionCollection versions = await launcherver.GetAllVersionsAsync();
            foreach (MVersionMetadata ver in versions)
            {
                if (ver.ToString().StartsWith("release"))
                {
                    // Release TAB: Add release items
                    Label releaselabel = new Label();
                    releaselabel.Height = 30;
                    releaselabel.Width = 100;
                    releaselabel.Foreground = new SolidColorBrush(Colors.White);
                    releaselabel.HorizontalAlignment = HorizontalAlignment.Left;
                    releaselabel.FontFamily = new FontFamily("Segoe UI Black");
                    releaselabel.Content = ver.ToString().Split(' ')[1];
                    contenthandler_version_handler_base_release_stackpanel.Children.Add(releaselabel);
                }
            }
        }

        private void redirect_releases(object sender, System.Windows.Input.MouseButtonEventArgs e) { contenthandler_version_handler_base.SelectedIndex = 0; }
        private void redirect_snapshots(object sender, System.Windows.Input.MouseButtonEventArgs e) { contenthandler_version_handler_base.SelectedIndex = 1; }
    }
}