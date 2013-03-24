using Veith.WPFLocalizator.Infrastructure;

namespace Veith.WPFLocalizator.ViewModel
{
    public class NameSelectingWindowViewModel : ViewModelBase
    {
        private string selectedName;

        public string SelectedName
        {
            get
            {
                return this.selectedName;
            }

            set
            {
                this.selectedName = value;
                this.RaisePropertyChanged(() => this.SelectedName);
                this.RaisePropertyChanged(() => this.IsNameValid);
            }
        }

        public bool IsNameValid
        {
            get
            {
                return !string.IsNullOrEmpty(this.SelectedName) && this.SelectedName.Length > 0;
            }
        }
    }
}
