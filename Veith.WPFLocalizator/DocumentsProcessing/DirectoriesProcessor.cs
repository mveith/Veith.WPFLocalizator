using System.Collections.Generic;
using System.Linq;
using Veith.WPFLocalizator.DocumentsLoading;
using Veith.WPFLocalizator.DocumentsParsing;
using Veith.WPFLocalizator.DocumentsSerializing;
using Veith.WPFLocalizator.Model;

namespace Veith.WPFLocalizator.DocumentsProcessing
{
    public interface IDirectoriesProcessor
    {
        void ProcessDirectory(string directory, DocumentProcessorConfiguration configuration);
    }

    public class DirectoriesProcessor : IDirectoriesProcessor
    {
        private readonly IResourcesDictionariesFactory resourcesDictionariesFactory;
        private readonly IDocumentsLoader documentsLoader;
        private readonly IDocumentProcessor documentProcessor;
        private readonly IDocumentSerializer documentSerializer;
        private readonly IDocumentParser documentParser;

        private string selectedDirectory;
        private DocumentProcessorConfiguration configuration;
        private IResourcesDictionary[] dictionaries;

        public DirectoriesProcessor(
            DocumentsTools documentsTools,
            IResourcesDictionariesFactory resourcesDictionariesFactory)
        {
            this.documentsLoader = documentsTools.Loader;
            this.documentParser = documentsTools.Parser;
            this.documentProcessor = documentsTools.Processor;
            this.documentSerializer = documentsTools.Serializer;
            this.resourcesDictionariesFactory = resourcesDictionariesFactory;
        }

        public void ProcessDirectory(string directory, DocumentProcessorConfiguration configuration)
        {
            this.selectedDirectory = directory;
            this.configuration = configuration;

            this.dictionaries = this.GetDictionaries();

            var loadedDocuments = this.LoadDocuments();

            foreach (var document in loadedDocuments)
            {
                this.ProcessDocument(document);
            }
        }

        private IEnumerable<DocumentInfo> LoadDocuments()
        {
            return this.documentsLoader.LoadDocumentsInDirectory(this.selectedDirectory, "xaml");
        }

        private void ProcessDocument(DocumentInfo document)
        {
            var parsedDocument = this.documentParser.ParseDocument(document);
            this.documentProcessor.ProcessDocument(parsedDocument, this.configuration, this.dictionaries);
            this.documentSerializer.SerializeDocument(parsedDocument);
        }

        private IResourcesDictionary[] GetDictionaries()
        {
            return this.configuration.ResourceFilesPaths.Select(p => this.resourcesDictionariesFactory.CreateDictionaryFromFile(p)).ToArray();
        }
    }
}