using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using File = System.IO.File;

namespace betterlauncher_cs.modules
{
    internal class ConfigManager
    {
        public static string GetConfigFolder() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".betterlauncher");
        public static string GetConfigAccountsPath() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".betterlauncher/accounts.json");

        public static void Prepare()
        {
            // check for betterlauncher folder
            if (!File.Exists(GetConfigFolder()))
                Directory.CreateDirectory(GetConfigFolder());

            // check for betterlauncher config file
            if (!File.Exists(GetConfigAccountsPath()))
                File.Create(GetConfigAccountsPath());
        }

        private static void verify()
        {
            if (!File.ReadAllText(GetConfigAccountsPath()).StartsWith("{"))
            {
                JObject accountsjson = new JObject();
                File.WriteAllText(GetConfigAccountsPath(), accountsjson.ToString());
            }
        }

        public static void addaccount(string username)
        {
            if (!Regex.IsMatch(username, "^[a-zA-Z0-9_]{3,16}$"))
            {
                MessageBox.Show("Zła nazwa. Uzyj liter A-Z i cyfr 0-9. Nick moze miec minimalnie 3 znaki do 16.");
                return;
            }

            verify();

            JObject accountsjson;
            using (StreamReader reader = File.OpenText(GetConfigAccountsPath()))
            {
                accountsjson = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
            }

            // anti double username
            foreach (var property in accountsjson.Properties())
            {
                if (property.Value.ToString() == username)
                {
                    MessageBox.Show($"Użytkownik \"{username}\" już istnieje.");
                    return;
                }
            }

            // check if the username exists in the accountsjson
            foreach (var property in accountsjson.Properties())
            {
                if (property.Value.ToString() == username)
                {
                    int suffix = 1;
                    while (true)
                    {
                        string propertyName = $"username-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}-{suffix}";
                        if (accountsjson.Property(propertyName) == null)
                        {
                            accountsjson.Add(new JProperty(propertyName, username));
                            break;
                        }
                        suffix++;
                    }
                    File.WriteAllText(GetConfigAccountsPath(), accountsjson.ToString());
                    return;
                }
            }

            accountsjson.Add(new JProperty($"username-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}", username));

            File.WriteAllText(GetConfigAccountsPath(), accountsjson.ToString());
        }


        public static void removeaccount(string username)
        {
            verify();
            JObject accountsjson;
            using (StreamReader reader = File.OpenText(GetConfigAccountsPath()))
                accountsjson = (JObject)JToken.ReadFrom(new JsonTextReader(reader));

            foreach (var property in accountsjson.Properties())
            {
                if (property.Value.ToString() == username)
                {
                    property.Remove();
                    break;
                }
            }
            File.WriteAllText(GetConfigAccountsPath(), accountsjson.ToString());
        }


        public static int getaccountcount()
        {
            verify();
            JObject accountsjson;
            using (StreamReader reader = File.OpenText(GetConfigAccountsPath()))
                accountsjson = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
            int count = accountsjson.Count;
            return count;
        }

        public static string getaccountname(int pos)
        {
            verify();
            JObject accountsjson;
            using (StreamReader reader = File.OpenText(GetConfigAccountsPath()))
                accountsjson = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
            string accountName = null;
            if (pos >= 0 && pos < accountsjson.Count)
                accountName = accountsjson.Properties().ElementAt(pos).Value.ToString();
            return accountName;
        }

    }
}
