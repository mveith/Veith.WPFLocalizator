using System.Collections.ObjectModel;

namespace Veith.WPFLocalizator.DocumentsProcessing
{
    public class DocumentProcessorConfiguration
    {
        public DocumentProcessorConfiguration()
        {
            this.ResourceFilesPaths = new ObservableCollection<string>();
        }

        public string Prefix { get; set; }

        public string Suffix { get; set; }

        public string NamespaceValue { get; set; }

        public string ResourceFileName { get; set; }

        public string NewValuePrefix { get; set; }

        public string SelectedDirectory { get; set; }

        public ObservableCollection<string> ResourceFilesPaths { get; private set; }
    }
}
