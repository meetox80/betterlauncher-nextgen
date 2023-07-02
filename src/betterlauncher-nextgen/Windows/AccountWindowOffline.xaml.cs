using System.Windows;

namespace betterlauncher_nextgen.Windows
{
    public partial class AccountWindowOffline : Window
    {
        public AccountWindowOffline()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.Topmost = false;
            this.Hide();
        }
    }
}