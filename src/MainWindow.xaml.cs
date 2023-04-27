using betterlauncher_cs.modules;
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.Auth.Microsoft.MsalClient;
using Microsoft.Identity.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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
        }

        #region MicrosoftLogin
        public static IPublicClientApplication app;
        public static JavaEditionLoginHandler loginHandler;
        public static MSession session;
        public static CancellationTokenSource loginCancel;
        private void mslogin_success(MSession session)
        {
            contenthandler.SelectedIndex = 1;
        }
        private async Task LoginAndShowResultOnUI(JavaEditionLoginHandler loginHandler)
        {
            try
            {
                var session = await loginHandler.LoginFromOAuth();
                mslogin_success(session.GameSession);
            }
            catch (Exception ex)
            {
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
    }
}