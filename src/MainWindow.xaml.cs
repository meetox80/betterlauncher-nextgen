using System.Windows;

namespace betterlauncher_cs
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, System.EventArgs e)
        {
            windowhandler_bg.Height = 20; windowhandler_bg.Width = 20;
            windowhandler_mask1.Height = 20; windowhandler_mask1.Width = 20;
            windowhandler_mask2.Height = 20; windowhandler_mask2.Width = 20;

            handler_left.Opacity = 0.0f;
            season_bg.Opacity = 0.0f;
            season_bg_mask.Opacity = 0.0f;
            season_bg_title.Opacity = 0.0f;
            season_bg_title_bg.Opacity = 0.0f;
            season_bg_button_github.Opacity = 0.0f;
            launch_button.Opacity = 0.0f;
            launch_progressbar_bg.Opacity = 0.0f;
            launch_progressbar_bar.Opacity = 0.0f;
            launch_progressbar_text.Opacity = 0.0f;
            settings_button.Opacity = 0.0f;
        }
    }
}
