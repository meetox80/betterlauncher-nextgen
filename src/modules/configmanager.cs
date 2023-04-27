using System;
using System.IO;

namespace betterlauncher_cs.modules
{
    internal class ConfigManager
    {
        public static string GetConfigFolder() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".betterlauncher");
        public static string GetConfigPath() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".betterlauncher/config.json");

        public static void Prepare()
        {
            // check for betterlauncher folder
            if (!File.Exists(GetConfigFolder()))
                Directory.CreateDirectory(GetConfigFolder());

            // check for betterlauncher config file
            if (!File.Exists(GetConfigPath()))
                File.Create(GetConfigPath());
        }
    }
}
