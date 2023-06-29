using CmlLib.Core;
using CmlLib.Core.Version;
using CmlLib.Core.VersionMetadata;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace betterlauncher_nextgen
{
    public partial class MainWindow : Window
    {
        #region Variables
        public static int BackgroundCount = 2;
        public static string BetterLauncher_Version = "0.0.0";
        public static string BetterLauncher_SelectedVersion = "";
        #endregion

        #region VersionLoading, Startup
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = $"BetterLauncher - {BetterLauncher_Version}";

            handler_bg.Height = 50; handler_bg.Width = 50; handler_bg_rotatetransform.Angle = 90; handler_bg.Margin = new Thickness(0, 250, 0, 0); handler_bg.Opacity = 0.0f;
            handler_bg_mask.Height = 50; handler_bg_mask.Width = 50; handler_bg_mask_rotatetransform.Angle = 90; handler_bg_mask.Margin = new Thickness(0, 250, 0, 0); handler_bg_mask.Opacity = 0.0f;
            versionhandler.Opacity = 0.0f;
            seasonhandler.Opacity = 0.0f;
            launchbutton.Opacity = 0.0f;
            seasonhandler_2_0.Opacity = 0.0f;
            seasonhandler_title.Opacity = 0.0f;
            seasonhandler_linkrect_github.Opacity = 0.0f;
            versionhandler_settings.Opacity = 0.0f;
            versionhandler_stackpanel_scrollviewer.Opacity = 0.0f;
            versionhandler_blend_top.Opacity = 0.0f;
            versionhandler_blend_bottom.Opacity = 0.0f;
            accountswitcher_button.Opacity = 0.0f;
            ConfigManager.EnsureConfig();

            RotateBackground();

            // get minecraft versions
            var launcher = new CMLauncher(new MinecraftPath());
            MVersionCollection versions = launcher.GetAllVersions();

            string SupportedVersions = ConfigManager.GetKey("ShowVersions");
            string[] supportedVersionsArray = SupportedVersions.Split(',');

            DropShadowEffect dropShadowEffect = new DropShadowEffect { BlurRadius = 25, Opacity = 0.15 };
            int xe = 0; foreach (MVersionMetadata ver in versions)
            {
                foreach (string supportedVersion in supportedVersionsArray)
                {
                    if (ver.ToString().Contains(supportedVersion.Trim()))
                    {
                        xe++;
                        // :<
                        // System.ArgumentException: 'Specified Visual is already a child of another Visual or the root of a CompositionTarget.'
                        Button VersionButton = new Button();
                        VersionButton.Width = 200; VersionButton.Height = 35;
                        if (xe != 1)
                            VersionButton.Margin = new Thickness(0, 20, 0, 0);
                        VersionButton.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#Proxima Nova");
                        VersionButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE5E8EC"));
                        VersionButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF444444"));
                        VersionButton.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC2C1F1"));
                        VersionButton.BorderThickness = new Thickness(0, 0, 0, 0);
                        VersionButton.Resources = new ResourceDictionary();
                        VersionButton.Cursor = Cursors.Hand;
                        VersionButton.Click += VersionButton_Click;
                        VersionButton.Resources.Add(typeof(Border), new Style(typeof(Border))
                        {
                            Setters = { new Setter(Border.CornerRadiusProperty, new CornerRadius(7.5)) }
                        }); VersionButton.Effect = dropShadowEffect;

                        VersionButton.Content = ver.Name;
                        versionhandler_stackpanel.Children.Add(VersionButton);
                    }
                }
            }

            Rectangle MarginRectangle = new Rectangle();
            MarginRectangle.Width = 1; MarginRectangle.Height = 20;
            versionhandler_stackpanel.Children.Add(MarginRectangle);
        }

        private void VersionButton_Click(object sender, RoutedEventArgs e)
        {
            StackPanel stackPanel = FindParentStackPanel(sender as Button);
            if (stackPanel != null)
            {
                foreach (var child in stackPanel.Children)
                {
                    if (child is Button button)
                    {
                        button.BorderThickness = new Thickness(0);
                        BetterLauncher_SelectedVersion = button.Content.ToString();
                    }
                }
            }

            Button ClickedButton = (Button)sender;
            ClickedButton.BorderThickness = new Thickness(1.5);
            BetterLauncher_SelectedVersion = ClickedButton.Content.ToString();
        }

        #region VersionLoading:Utils
        private StackPanel FindParentStackPanel(UIElement element)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(element);
            while (parent != null && !(parent is StackPanel))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return parent as StackPanel;
        }
        #endregion
        #endregion

        #region Background
        public void RotateBackground()
        {
            Random BackgroundRandom = new Random();
            int BackgroundOutput = BackgroundRandom.Next(1, BackgroundCount + 1);
            string imagePath = $"pack://application:,,,/Resources/bg-season-{BackgroundOutput}.jpg";
            BitmapImage BackgroundImage = new BitmapImage(new Uri(imagePath));
            seasonhandler_image.ImageSource = BackgroundImage;
        }

        private void seasonhandler_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // Get the current position of the mouse
            Point mousePosition = e.GetPosition(seasonhandler);

            double OffsetX = mousePosition.X / 10000;
            double OffsetY = mousePosition.Y / 10000;

            seasonhandler_translatetransform.X = OffsetX- 0.10; seasonhandler_translatetransform.Y = OffsetY - 0.10;
        }

        private void Rectangle_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void seasonhandler_linkrect_github_Click(object sender, RoutedEventArgs e) => Process.Start(new ProcessStartInfo { FileName = "https://github.com/lemonekq/betterlauncher-nextgen", UseShellExecute = true });
        #endregion

        #region External Windows
        private void versionhandler_settings_Click(object sender, RoutedEventArgs e)
        {
        }

        private void accountswitcher_button_Click(object sender, RoutedEventArgs e)
        {
            Windows.AccountWindow AccountWindowFrame = new Windows.AccountWindow();

            this.Topmost = true;
            AccountWindowFrame.Show();
            AccountWindowFrame.Left = this.Left + 10; AccountWindowFrame.Top = this.Top + 10;
        }
        #endregion
    }
}
