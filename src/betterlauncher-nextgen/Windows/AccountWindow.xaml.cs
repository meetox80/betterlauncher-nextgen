using System.Windows;

namespace betterlauncher_nextgen.Windows
{
    public partial class AccountWindow : Window
    {
        public AccountWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MainWindowFrame = new MainWindow();
            MainWindowFrame.Topmost = false;
            this.Hide();
        }
    }
}
