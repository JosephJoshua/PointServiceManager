using Newtonsoft.Json;
using System.IO;

namespace PSMDesktopUI.Library.Helpers
{
    public class Settings
    {
        public string ApiUrl { get; set; }

        public string ReportPath { get; set; }

        public string ServicePrinterName { get; set; }

        public string LabelPrinterName { get; set; }
    }

    public class SettingsHelper : ISettingsHelper
    {
        public Settings Settings { get; private set; } = new Settings();

        private const string FilePath = "settings.json";

        public SettingsHelper()
        {
            Init();
        }

        public void ReadSettingsFromFile()
        {
            string content = File.ReadAllText(FilePath);
            Settings = JsonConvert.DeserializeObject<Settings>(content);
        }

        public void SaveSettingsToFile()
        {
            string data = JsonConvert.SerializeObject(Settings);
            File.WriteAllText(FilePath, data);
        }

        private void Init()
        {
            if (!File.Exists(FilePath))
            {
                // Create file with default settings if it doesn't exist
                Settings.ApiUrl = "http://localhost:3030";
                Settings.ReportPath = "Reports/ServiceInvoice.rpt";
                Settings.ServicePrinterName = "";
                Settings.LabelPrinterName = "";

                SaveSettingsToFile();
            }

            ReadSettingsFromFile();
        }
    }
}
