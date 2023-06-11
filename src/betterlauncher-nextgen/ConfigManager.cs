using System;
using System.IO;

namespace betterlauncher_nextgen
{
    internal class ConfigManager
    {
        public static string ConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".betterlauncher-cs", "betterlauncher.config");
        public static void EnsureConfig()
        {
            if (!File.Exists(ConfigPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath));
                File.Create(ConfigPath).Dispose();
                // release, snapshot, old_alpha, old_beta, local
                SetKey("ShowVersions", "release,local");
            }
        }

        public static void SetKey(string key, string keyvalue)
        {
            string[] lines = File.ReadAllLines(ConfigPath);

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith(key + "="))
                {
                    lines[i] = $"{key}={keyvalue}";
                    File.WriteAllLines(ConfigPath, lines);
                    return;
                }
            }

            // Key does not exist, add it as a new key-value pair
            string lineToAdd = $"{key}={keyvalue}";
            File.AppendAllText(ConfigPath, lineToAdd);
        }

        public static string GetKey(string key)
        {
            string[] lines = File.ReadAllLines(ConfigPath);

            foreach (string line in lines)
            {
                if (line.StartsWith(key + "="))
                {
                    string value = line.Substring(key.Length + 1);
                    return value;
                }
            }

            return null;
        }
    }
}
