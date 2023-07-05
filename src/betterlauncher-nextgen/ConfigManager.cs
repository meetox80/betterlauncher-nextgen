using System;
using System.IO;

public class ConfigManager
{
    private static string configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".betterlauncher-cs", "betterlauncher.config");
    public static void EnsureConfigFileExists()
    {
        if (!File.Exists(configPath))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(configPath));
            File.Create(configPath).Dispose();
            SetKey("ShowVersions", "release,local");
        }
    }

    public static void SetKey(string key, string value)
    {
        EnsureConfigFileExists();

        string[] lines = File.ReadAllLines(configPath);
        bool keyExists = false;

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            if (line.StartsWith(key + "="))
            {
                lines[i] = key + "=" + value;
                keyExists = true;
                break;
            }
        }

        if (!keyExists)
        {
            Array.Resize(ref lines, lines.Length + 1);
            lines[lines.Length - 1] = key + "=" + value;
        }

        File.WriteAllLines(configPath, lines);
    }

    public static string GetKey(string key)
    {
        EnsureConfigFileExists();

        string[] lines = File.ReadAllLines(configPath);

        foreach (string line in lines)
        {
            if (line.StartsWith(key + "="))
            {
                return line.Substring(key.Length + 1);
            }
        }

        return "";
    }
}