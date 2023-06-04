using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace betterlauncher_nextgen
{
    public partial class MainWindow : Window
    {
        #region Variables
        public static int BackgroundCount = 2;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            handler_bg.Height = 50; handler_bg.Width = 50; handler_bg_rotatetransform.Angle = 90; handler_bg.Margin = new Thickness(0, 250, 0, 0); handler_bg.Opacity = 0.0f;
            handler_bg_mask.Height = 50; handler_bg_mask.Width = 50; handler_bg_mask_rotatetransform.Angle = 90; handler_bg_mask.Margin = new Thickness(0, 250, 0, 0); handler_bg_mask.Opacity = 0.0f;
            versionhandler.Opacity = 0.0f;
            seasonhandler.Opacity = 0.0f;
            launchbutton.Opacity = 0.0f;
            seasonhandler_2_0.Opacity = 0.0f;
            seasonhandler_title.Opacity = 0.0f;
            
            rotatebg();
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
    }
}
