using betterlauncher_cs.modules;
using System;
using System.Threading;
using System.Windows;

namespace betterlauncher_cs
{
    public partial class MainWindow : Window
    {
        public static string version = "0.0.0";

        public MainWindow()
        {
            // post-app run lag spike on animations delay
            Thread.Sleep(100);
            InitializeComponent();
        }

        private async void Window_Initialized(object sender, EventArgs e) // async for l8r?
        {
            configmanager.Prepare();
            handler.Height = 35; handler.Width = 35; handler.RadiusX = 35; handler.RadiusY = 35;
            navbar.Opacity = 0.0f;
            contenthandler.Opacity = 0.0f;
        }
    }
}
