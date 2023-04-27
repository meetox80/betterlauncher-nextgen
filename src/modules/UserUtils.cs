using Newtonsoft.Json;
using System.Net;
using System.Windows;

namespace betterlauncher_cs.modules
{
    internal class UserUtils
    {
        public class IsPremium_Output
        {
            public string status { get; set; }
        }

        public static bool IsPremium(string username)
        {
            WebClient client = new WebClient();
            string result = client.DownloadString($"https://api.minetools.eu/uuid/{username}");
            IsPremium_Output output = JsonConvert.DeserializeObject<IsPremium_Output>(result);
            if (output.status == "ERR")
                return false;
            if (output.status == "OK")
                return true;

            MessageBox.Show("Error while checking premium (UserUtils.IsPremium)");
            return false;
        }
    }
}
