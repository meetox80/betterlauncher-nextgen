using System;
using XboxAuthNet.OAuth;
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
            try
            {
                var loginHandler = MainWindow.LoginHandler;
                // var session = await loginHandler.AuthenticateInteractively();
                var authenticator = loginHandler.CreateAuthenticatorWithNewAccount();
                authenticator.AddForceMicrosoftOAuthForJE(oauth => oauth.Interactive());
                authenticator.AddXboxAuthForJE(xbox => xbox.Basic());
                authenticator.AddForceJEAuthenticator();
                var session = await authenticator.ExecuteForLauncherAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}