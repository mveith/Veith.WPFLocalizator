using Veith.WPFLocalizator.DocumentsLoading;
using Veith.WPFLocalizator.DocumentsParsing;
using Veith.WPFLocalizator.DocumentsSerializing;

namespace Veith.WPFLocalizator.DocumentsProcessing
{
    public class DocumentsTools
    {
        public IDocumentsLoader Loader { get; set; }

        public IDocumentParser Parser { get; set; }

        public IDocumentProcessor Processor { get; set; }

        public IDocumentSerializer Serializer { get; set; }
    }
}
