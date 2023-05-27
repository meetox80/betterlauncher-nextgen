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
        }
    }
}
