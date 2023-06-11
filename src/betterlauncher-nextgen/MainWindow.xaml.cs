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
        public static string version = "0.0.0";
        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            handler_bg.Height = 50; handler_bg.Width = 50; handler_bg_rotatetransform.Angle = 90; handler_bg.Margin = new Thickness(0, 250, 0, 0); handler_bg.Opacity = 0.0f;
            handler_bg_mask.Height = 50; handler_bg_mask.Width = 50; handler_bg_mask_rotatetransform.Angle = 90; handler_bg_mask.Margin = new Thickness(0, 250, 0, 0); handler_bg_mask.Opacity = 0.0f;
            versionhandler.Opacity = 0.0f;
            seasonhandler.Opacity = 0.0f;
            launchbutton.Opacity = 0.0f;
            seasonhandler_2_0.Opacity = 0.0f;
            seasonhandler_title.Opacity = 0.0f;
            seasonhandler_linkrect_github.Opacity = 0.0f;
            versionhandler_settings.Opacity = 0.0f;
            ConfigManager.EnsureConfig();

            rotatebg();

            // get minecraft versions
            var launcher = new CMLauncher(new MinecraftPath());
            MVersionCollection versions = launcher.GetAllVersions();

            string SupportedVersions = ConfigManager.GetKey("ShowVersions");
            string[] supportedVersionsArray = SupportedVersions.Split(',');
            foreach (MVersionMetadata ver in versions)
            {
                foreach (string supportedVersion in supportedVersionsArray)
                {
                    if (ver.ToString().Contains(supportedVersion.Trim()))
                    {
                        // add element
                        Grid VersionGrid = new Grid();
                        DropShadowEffect dropShadowEffect = new DropShadowEffect { BlurRadius = 25, Opacity = 0.15 };
                        VersionGrid.Effect = dropShadowEffect;
                        VersionGrid.Width = 200;
                        VersionGrid.Margin = new Thickness(0, 20, 0, 0);
                        VersionGrid.Height = 35;

                        Rectangle VersionRectangle = new Rectangle();
                        VersionRectangle.RadiusX = 7.5f; VersionRectangle.RadiusY = 7.5f;
                        VersionRectangle.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE5E8EC"));

                        StackPanel VersionStackPanel = new StackPanel();
                        VersionStackPanel.Orientation = Orientation.Horizontal; VersionStackPanel.VerticalAlignment = VerticalAlignment.Center;

                        TextBlock VersionTextBlock = new TextBlock();
                        VersionTextBlock.Text = ver.Name;
                        VersionTextBlock.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#Proxima Nova");
                        VersionTextBlock.TextAlignment = TextAlignment.Center;
                        VersionTextBlock.Width = 200;
                        VersionTextBlock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF444444"));

                        VersionStackPanel.Children.Add(VersionTextBlock);
                        VersionGrid.Children.Add(VersionRectangle);
                        VersionGrid.Children.Add(VersionStackPanel);
                        versionhandler_stackpanel.Children.Add(VersionGrid);

                        // fix the 1st and last element margin
                        if (versionhandler_stackpanel.Children.Count > 0)
                        {
                            UIElement firstElement = versionhandler_stackpanel.Children[0];
                            if (firstElement is FrameworkElement frameworkElement)
                            {
                                frameworkElement.Margin = new Thickness(0, 0, 0, 0);
                            }
                        }
                    }
                }
            }
        }
        public void rotatebg()
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
    }
}
