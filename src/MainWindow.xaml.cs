﻿using CmlLib.Core;
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
        public static string BetterLauncher_Version = "0.0.0";
        public static string BetterLauncher_MsLoginClientID = "c774e229-39fa-48ea-ad91-b0fb7e75945d";
        public static string BetterLauncher_WantedLaunch = "";

        public MainWindow()
        {
            InitializeComponent();
        }


        private async void Window_Initialized(object sender, EventArgs e)
        {
            // post-app run lag spike on animations delay
            Thread.Sleep(2500 / Environment.ProcessorCount);

            handler.Height = 35; handler.Width = 35; handler.RadiusX = 35; handler.RadiusY = 35;
            navbar.Opacity = 0.0f;
            contenthandler.Opacity = 0.0f;
            app = await MsalMinecraftLoginHelper.BuildApplicationWithCache(BetterLauncher_MsLoginClientID);
            releases_mainlabel.Foreground = new SolidColorBrush(Colors.OrangeRed);
            this.Title = "BetterLauncher - " + BetterLauncher_Version;
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

            var VersionLauncher = new CMLauncher(new MinecraftPath());
            Label WaitingForVersions = new Label();
            WaitingForVersions.Content = "Oczekiwanie na API...";
            WaitingForVersions.Foreground = new SolidColorBrush(Colors.White);
            WaitingForVersions.FontFamily = new FontFamily("Segoe UI Black");
            WaitingForVersions.HorizontalAlignment = HorizontalAlignment.Center;
            WaitingForVersions.VerticalAlignment = VerticalAlignment.Center;
            WaitingForVersions.FontSize = 20;

            contenthandler_version_handler_base_release_stackpanel.Children.Add(WaitingForVersions);

            MVersionCollection VersionsListing = await VersionLauncher.GetAllVersionsAsync();
            contenthandler_version_handler_base_release_stackpanel.Children.Clear();
            foreach (MVersionMetadata Version in VersionsListing)
            {
                Debug.WriteLine(Version.ToString());
                if (Version.ToString().StartsWith("release"))
                {
                    Label ReleasesLabel = new Label();
                    ReleasesLabel.Height = 30;
                    ReleasesLabel.Width = 100;
                    ReleasesLabel.Foreground = new SolidColorBrush(Colors.White);
                    ReleasesLabel.HorizontalAlignment = HorizontalAlignment.Left;
                    ReleasesLabel.FontFamily = new FontFamily("Segoe UI Black");
                    ReleasesLabel.Content = Version.ToString().Split(' ')[1];
                    contenthandler_version_handler_base_release_stackpanel.Children.Add(ReleasesLabel);
                }

                if (Version.ToString().StartsWith("snapshot"))
                {
                    Label SnapshotsLabel = new Label();
                    SnapshotsLabel.Height = 30;
                    SnapshotsLabel.Width = 100;
                    SnapshotsLabel.Foreground = new SolidColorBrush(Colors.White);
                    SnapshotsLabel.HorizontalAlignment = HorizontalAlignment.Left;
                    SnapshotsLabel.FontFamily = new FontFamily("Segoe UI Black");
                    SnapshotsLabel.Content = Version.ToString().Split(' ')[1];
                    contenthandler_version_handler_base_snapshots_stackpanel.Children.Add(SnapshotsLabel);
                }

                if (Version.ToString().StartsWith("local"))
                {
                    Label LocalsLabel = new Label();
                    LocalsLabel.Height = 30;
                    LocalsLabel.Width = 100;
                    LocalsLabel.Foreground = new SolidColorBrush(Colors.White);
                    LocalsLabel.HorizontalAlignment = HorizontalAlignment.Left;
                    LocalsLabel.FontFamily = new FontFamily("Segoe UI Black");
                    LocalsLabel.Content = Version.ToString().Split(' ')[1];
                    contenthandler_version_handler_base_local_stackpanel.Children.Add(LocalsLabel);
                }
            }

            if (contenthandler_version_handler_base_local_stackpanel.Children.Count == 0)
            {
                Label LocalsLabel_Empty = new Label();
                LocalsLabel_Empty.Content = "Brak wersji w .minecraft :<";
                LocalsLabel_Empty.Foreground = new SolidColorBrush(Colors.White);
                LocalsLabel_Empty.FontFamily = new FontFamily("Segoe UI Black");
                LocalsLabel_Empty.HorizontalAlignment = HorizontalAlignment.Center;
                LocalsLabel_Empty.VerticalAlignment = VerticalAlignment.Center;
                LocalsLabel_Empty.FontSize = 20;
                contenthandler_version_handler_base_local_stackpanel.Children.Add(LocalsLabel_Empty);
            }
        }

        private void ResetVersionsMainLabelColors()
        {
            releases_mainlabel.Foreground = new SolidColorBrush(Colors.White);
            snapshots_mainlabel.Foreground = new SolidColorBrush(Colors.White);
            locals_mainlabel.Foreground = new SolidColorBrush(Colors.White);
        }

        private void redirect_releases(object sender, System.Windows.Input.MouseButtonEventArgs e) { ResetVersionsMainLabelColors(); contenthandler_version_handler_base.SelectedIndex = 0; releases_mainlabel.Foreground = new SolidColorBrush(Colors.OrangeRed); }
        private void redirect_snapshots(object sender, System.Windows.Input.MouseButtonEventArgs e) { ResetVersionsMainLabelColors(); contenthandler_version_handler_base.SelectedIndex = 1; snapshots_mainlabel.Foreground = new SolidColorBrush(Colors.OrangeRed); }
        private void redirect_local(object sender, System.Windows.Input.MouseButtonEventArgs e) { ResetVersionsMainLabelColors(); contenthandler_version_handler_base.SelectedIndex = 2; locals_mainlabel.Foreground = new SolidColorBrush(Colors.OrangeRed); }
    }
}