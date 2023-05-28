using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace betterlauncher_cs
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SetMonitorScaleTo100Percent()
        {
            const string dpiRegistryPath = @"HKEY_CURRENT_USER\Control Panel\Desktop";
            const string dpiRegistryValue = "LogPixels";
            const int WM_DISPLAYCHANGE = 0x007E;
            const int HWND_BROADCAST = 0xFFFF;

            // Set the DPI value to 96 (100% scale)
            Registry.SetValue(dpiRegistryPath, dpiRegistryValue, 96, RegistryValueKind.DWord);

            // Notify the system of the DPI change
            SendMessage((IntPtr)HWND_BROADCAST, WM_DISPLAYCHANGE, IntPtr.Zero, IntPtr.Zero);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        private void Window_Initialized(object sender, System.EventArgs e)
        {
            SetMonitorScaleTo100Percent();

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
