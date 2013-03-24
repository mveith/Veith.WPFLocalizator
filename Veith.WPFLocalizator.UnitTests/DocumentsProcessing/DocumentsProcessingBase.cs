using System.Collections.Generic;
using NSubstitute;
using Veith.WPFLocalizator.DocumentsProcessing;
using Veith.WPFLocalizator.Model;
using Veith.WPFLocalizator.UserInteraction;

namespace Veith.WPFLocalizator.UnitTests.DocumentsProcessing
{
    public abstract class DocumentsProcessingBase
    {
        protected DocumentProcessor documentsProcessor;

        protected List<DocumentPart> parts;

        protected string prefix;
        protected string suffix;
        protected string namespaceValue;
        protected string resourceFileName;
        protected string documentName;
        protected IResourcesKeysGenerator keysGenerator;
        protected List<IResourcesDictionary> dictionaries;
        protected IUserInteraction userInteraction;
        protected DocumentProcessorConfiguration configuration;

        protected void TestInitialize()
        {
            this.documentName = "OriginalFileName";
            this.prefix = "{lng:LocText";
            this.suffix = "}";
            this.namespaceValue = "MyNamespace";
            this.resourceFileName = "resourceFile";

            this.parts = new List<DocumentPart>();

            this.keysGenerator = Substitute.For<IResourcesKeysGenerator>();

            this.configuration = new DocumentProcessorConfiguration()
            {
                Prefix = this.prefix,
                Suffix = this.suffix,
                NamespaceValue = this.namespaceValue,
                ResourceFileName = this.resourceFileName
            };

            this.dictionaries = new List<IResourcesDictionary>();

            this.userInteraction = Substitute.For<IUserInteraction>();

            this.documentsProcessor = new DocumentProcessor(new KeysForPartsGenerator(this.userInteraction, this.keysGenerator), new[] { "Text", "Content", "Title", "Header" });
        }

        protected void ProcessDocument()
        {
            var document = new ParsedDocument()
            {
                Parts = this.parts,
                Document = new DocumentInfo()
                {
                    FileName = this.documentName
                }
            };

            this.documentsProcessor.ProcessDocument(document, configuration, this.dictionaries);
        }
    }
}
