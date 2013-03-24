using System.Collections.ObjectModel;
using Veith.WPFLocalizator.DocumentsProcessing;
using Veith.WPFLocalizator.Infrastructure;

namespace Veith.WPFLocalizator.ViewModel
{
    public class DocumentProcessorConfigurationViewModel : ViewModelBase
    {
        private readonly DocumentProcessorConfiguration configuration;

        public DocumentProcessorConfigurationViewModel(DocumentProcessorConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string Prefix
        {
            get
            {
                return this.configuration.Prefix;
            }

            set
            {
                this.configuration.Prefix = value;
                this.UpdateSample();
            }
        }

        public string Suffix
        {
            get
            {
                return this.configuration.Suffix;
            }

            set
            {
                this.configuration.Suffix = value;
                this.UpdateSample();
            }
        }

        public string NamespaceValue
        {
            get
            {
                return this.configuration.NamespaceValue;
            }

            set
            {
                this.configuration.NamespaceValue = value;
                this.UpdateSample();
            }
        }

        public string ResourceFileName
        {
            get
            {
                return this.configuration.ResourceFileName;
            }

            set
            {
                this.configuration.ResourceFileName = value;
                this.UpdateSample();
            }
        }

        public string Sample
        {
            get
            {
                return this.Prefix + " " + this.NamespaceValue + ":" + this.ResourceFileName + ":VALUE" + this.Suffix;
            }
        }

        public string NewValuePrefix
        {
            get
            {
                return this.configuration.NewValuePrefix;
            }

            set
            {
                this.configuration.NewValuePrefix = value;
            }
        }

        public string SelectedDirectory
        {
            get
            {
                return this.configuration.SelectedDirectory;
            }

            set
            {
                this.configuration.SelectedDirectory = value;
            }
        }

        public ObservableCollection<string> ResourceFilesPaths
        {
            get
            {
                return this.configuration.ResourceFilesPaths;
            }
        }

        public DocumentProcessorConfiguration OriginalConfiguration
        {
            get
            {
                return this.configuration;
            }
        }

        private void UpdateSample()
        {
            this.RaisePropertyChanged(() => this.Sample);
        }
    }
}
