using Caliburn.Micro;
using DevExpress.Xpf.Core;
using PSMDesktopUI.Library.Helpers;
using System.Threading.Tasks;

namespace PSMDesktopUI.ViewModels
{
    public class SettingsViewModel : Screen
    {
        private readonly ISettingsHelper _settingsHelper;

        private string _apiUrl;
        private string _reportPath;

        private string _servicePrinterName;
        private string _labelPrinterName;

        public string ApiUrl
        {
            get => _apiUrl;

            set
            {
                _apiUrl = value;
                NotifyOfPropertyChange(() => ApiUrl);
            }
        }

        public string ReportPath
        {
            get => _reportPath;

            set
            {
                _reportPath = value;
                NotifyOfPropertyChange(() => ReportPath);
            }
        }

        public string ServicePrinterName
        {
            get => _servicePrinterName;

            set
            {
                _servicePrinterName = value;
                NotifyOfPropertyChange(() => ServicePrinterName);
            }
        }

        public string LabelPrinterName
        {
            get => _labelPrinterName;

            set
            {
                _labelPrinterName = value;
                NotifyOfPropertyChange(() => LabelPrinterName);
            }
        }

        public SettingsViewModel(ISettingsHelper settingsHelper)
        {
            _settingsHelper = settingsHelper;
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            ReadSettingsFromFile();
        }

        public async Task Save()
        {
            // Could optimize this from O(2n) to O(n), but that would be pretty pointless.
            if (!ValidatePrinterName(ServicePrinterName) || !ValidatePrinterName(LabelPrinterName))
            {
                DXMessageBox.Show("Printer yang dipilih tidak dapat ditemukan. Tolong atur ulang nama printer atau pastikan printer yang dipilih terhubung dengan komputer anda.", "Settings");
                return;
            }

            _settingsHelper.Settings.ApiUrl = ApiUrl;
            _settingsHelper.Settings.ReportPath = ReportPath;
            _settingsHelper.Settings.ServicePrinterName = ServicePrinterName;
            _settingsHelper.Settings.LabelPrinterName = LabelPrinterName;

            _settingsHelper.SaveSettingsToFile();
            await TryCloseAsync(true);
        }

        public async Task Cancel()
        {
            await TryCloseAsync(false);
        }

        private bool ValidatePrinterName(string toValidate)
        {
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                if (toValidate == printer) return true;
            }

            return false;
        }

        private void ReadSettingsFromFile()
        {
            _settingsHelper.ReadSettingsFromFile();

            Settings settings = _settingsHelper.Settings;
            
            ApiUrl = settings.ApiUrl;
            ReportPath = settings.ReportPath;
            ServicePrinterName = settings.ServicePrinterName;
            LabelPrinterName = settings.LabelPrinterName;
        }
    }
}
