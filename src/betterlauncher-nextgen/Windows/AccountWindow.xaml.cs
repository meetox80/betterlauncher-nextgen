using System;
using System.Windows;
using CmlLib.Core.Auth.Microsoft;

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
            MainWindow.Instance.Topmost = false;
            this.Hide();
        }

        private async void handler_accountselector_microsoft_Click(object sender, RoutedEventArgs e)
        {
            // mslogin :>
            this.Topmost = false;
            MainWindow.Instance.Topmost = false;
            try
            {
                var authenticator = MainWindow.LoginHandler.CreateAuthenticatorWithNewAccount();
                authenticator.AddMicrosoftOAuthForJE(oauth => oauth.Interactive()); // Microsoft OAuth
                authenticator.AddXboxAuthForJE(xbox => xbox.Basic()); // XboxAuth
                authenticator.AddJEAuthenticator(); // JEAuthenticator
                var session = await authenticator.ExecuteForLauncherAsync();
                this.Close();
                MessageBox.Show(session.Username);
            }
            catch (Exception ex)
            {
                if (!ex.ToString().Contains("User canceled auth"))
                {
                    MessageBox.Show(ex.ToString());
                }
                this.Close();
            }
        }
    }
}