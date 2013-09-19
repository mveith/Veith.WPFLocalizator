using System.Configuration;
using System.Globalization;
using System.Windows.Input;
using Veith.WPFLocalizator.Infrastructure;
using WPFLocalizeExtension.Engine;

namespace Veith.WPFLocalizator.ViewModel
{
    public class LanguagesViewModel : ViewModelBase
    {
        private const string CzechCultureCode = "cs-cz";
        private const string EnglishCultureCode = "en-gb";
        private const string LanguageConfigurationKey = "DefaultLanguage";

        public LanguagesViewModel()
        {
            var actualLanguage = ConfigurationManager.AppSettings.Get(LanguageConfigurationKey);

            if (actualLanguage == EnglishCultureCode)
            {
                this.SelectEnglish();
            }
            else
            {
                this.SelectCzech();
            }

            this.SelectCzechCommand = new RelayCommand(() => this.SelectCzech());
            this.SelectEnglishCommand = new RelayCommand(() => this.SelectEnglish());
        }

        public ICommand SelectCzechCommand { get; protected set; }

        public ICommand SelectEnglishCommand { get; protected set; }

        public bool IsCzechSelected { get; private set; }

        public bool IsEnglishSelected { get; private set; }

        private static void SelectCulture(string selectedCultureCode)
        {
            LocalizeDictionary.Instance.Culture = CultureInfo.GetCultureInfo(selectedCultureCode);

            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings[LanguageConfigurationKey].Value = selectedCultureCode;
                config.Save();
            }
            catch
            {
            }
        }

        private void SelectCzech()
        {
            SelectCulture(CzechCultureCode);

            this.ChangeLanguage(true, false);
        }

        private void SelectEnglish()
        {
            SelectCulture(EnglishCultureCode);

            this.ChangeLanguage(false, true);
        }

        private void ChangeLanguage(bool isCzechSelected, bool isEnglishSelected)
        {
            this.IsCzechSelected = isCzechSelected;
            this.IsEnglishSelected = isEnglishSelected;

            this.RaisePropertyChanged(() => this.IsCzechSelected);
            this.RaisePropertyChanged(() => this.IsEnglishSelected);
        }
    }
}
