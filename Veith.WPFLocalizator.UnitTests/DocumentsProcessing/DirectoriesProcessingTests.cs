using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Veith.WPFLocalizator.DocumentsLoading;
using Veith.WPFLocalizator.DocumentsParsing;
using Veith.WPFLocalizator.DocumentsProcessing;
using Veith.WPFLocalizator.DocumentsSerializing;
using Veith.WPFLocalizator.Model;

namespace Veith.WPFLocalizator.UnitTests.DocumentsProcessing
{
    [TestClass]
    public class DirectoriesProcessingTests
    {
        private readonly string directoryPath = "DIRECTORY PATH";

        private DirectoriesProcessor processor;

        private IDocumentsLoader documensLoader;
        private IDocumentParser documentParser;
        private IDocumentProcessor documentProcessor;
        private IDocumentSerializer documentSerializer;
        private IResourcesDictionariesFactory resourcesDictionariesFactory;

        private DocumentProcessorConfiguration configuration;
        private List<DocumentInfo> documents;
        private DocumentInfo testDocument;

        [TestInitialize]
        public void TestInitialize()
        {
            this.documents = new List<DocumentInfo>();
            
            this.documensLoader = Substitute.For<IDocumentsLoader>();
            this.documentParser = Substitute.For<IDocumentParser>();
            this.documentProcessor = Substitute.For<IDocumentProcessor>();
            this.documentSerializer = Substitute.For<IDocumentSerializer>();
            this.resourcesDictionariesFactory = Substitute.For<IResourcesDictionariesFactory>();
            this.configuration = new DocumentProcessorConfiguration()
            {
                Prefix = "AA",
                Suffix = "BB",
                ResourceFileName = "CC",
                NamespaceValue = "DD"
            };

            var documentsTools = new DocumentsTools()
            {
                Loader = this.documensLoader,
                Parser = this.documentParser,
                Processor = this.documentProcessor,
                Serializer = this.documentSerializer
            };

            this.processor = new DirectoriesProcessor(documentsTools, resourcesDictionariesFactory);

            this.documensLoader.LoadDocumentsInDirectory(Arg.Any<string>(), Arg.Any<string>())
                .Returns(this.documents);

            this.testDocument = new DocumentInfo();
            this.documents.Add(this.testDocument);

        }

        [TestMethod]
        public void IfProcessingSelectedDirectoryThenLoadDocumentsFromSelectedDirectory()
        {
            this.ProcessSelectedDirectory();

            this.documensLoader.Received().LoadDocumentsInDirectory(this.directoryPath, "xaml");
        }

        [TestMethod]
        public void IfProcessingSelectedDirectoryThenParseLoadedDocuments()
        {
            this.ProcessSelectedDirectory();

            this.documentParser.Received().ParseDocument(this.testDocument);
        }

        [TestMethod]
        public void IfProcessingSelectedDirectoryThenProcessParsedDocument()
        {
            var parsedDocument = new ParsedDocument();
            this.documentParser.ParseDocument(this.testDocument).Returns(parsedDocument);

            this.ProcessSelectedDirectory();

            this.documentProcessor.Received().ProcessDocument(parsedDocument, this.configuration, Arg.Any<IEnumerable<IResourcesDictionary>>());
        }

        [TestMethod]
        public void IfProcessingDirectoryThenSerializeProcessedDocument()
        {
            var parsedDocument = new ParsedDocument();
            this.documentParser.ParseDocument(this.testDocument).Returns(parsedDocument);

            this.ProcessSelectedDirectory();

            this.documentSerializer.Received().SerializeDocument(parsedDocument);
        }

        [TestMethod]
        public void IfProcessingDirectoryThenSetDictionariesToProcessor()
        {
            this.configuration.ResourceFilesPaths.Add("TESTFILEPATH");

            var dictionary = Substitute.For<IResourcesDictionary>();

            this.resourcesDictionariesFactory.CreateDictionaryFromFile("TESTFILEPATH").Returns(dictionary);

            this.ProcessSelectedDirectory();

            this.documentProcessor.Received().ProcessDocument(
                Arg.Any<ParsedDocument>(),
                Arg.Any<DocumentProcessorConfiguration>(),
                Arg.Is<IEnumerable<IResourcesDictionary>>(r => r.Single() == dictionary));
        }

        private void ProcessSelectedDirectory()
        {
            this.processor.ProcessDirectory(this.directoryPath, this.configuration);
        }
    }
}
