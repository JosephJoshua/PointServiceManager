using Caliburn.Micro;
using PSMDesktopUI.Library.Helpers;
using System.Threading.Tasks;

namespace PSMDesktopUI.ViewModels
{
    public class SettingsViewModel : Screen
    {
        private readonly ISettingsHelper _settingsHelper;

        private string _apiUrl;
        private string _reportPath;

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
            _settingsHelper.Settings.ApiUrl = ApiUrl;
            _settingsHelper.Settings.ReportPath = ReportPath;

            _settingsHelper.SaveSettingsToFile();
            await TryCloseAsync(true);
        }

        public async Task Cancel()
        {
            await TryCloseAsync(false);
        }

        private void ReadSettingsFromFile()
        {
            _settingsHelper.ReadSettingsFromFile();

            Settings settings = _settingsHelper.Settings;
            
            ApiUrl = settings.ApiUrl;
            ReportPath = settings.ReportPath;
        }
    }
}
