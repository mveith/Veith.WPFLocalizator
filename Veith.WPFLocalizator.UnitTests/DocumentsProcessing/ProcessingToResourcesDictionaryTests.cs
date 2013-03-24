using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Veith.WPFLocalizator.Model;

namespace Veith.WPFLocalizator.UnitTests.DocumentsProcessing
{
    [TestClass]
    public class ProcessingToResourcesDictionaryTests : DocumentsProcessingBase
    {
        private IResourcesDictionary dictionary;
        private DocumentPart testPart;
        private string testKey;
        private string testValue;

        [TestInitialize]
        public new void TestInitialize()
        {
            base.TestInitialize();

            this.dictionary = Substitute.For<IResourcesDictionary>();
            this.dictionaries.Add(this.dictionary);

            this.testValue = "Original value";
            this.testPart = new DocumentPart() { Name = "Text", Value = "Original value" };
            this.parts.Add(testPart);

            this.testKey = "TEST KEY";

            this.keysGenerator.CreateKey(Arg.Any<string>(), testPart).Returns(this.testKey);
        }

        [TestMethod]
        public void IfProcessingDocumentThenInsertValueFromEditedPartToDictionary()
        {
            this.ProcessDocument();

            this.dictionary.Received().Add(Arg.Any<string>(), this.testValue);
        }

        [TestMethod]
        public void IfProcessingDocumentThenInsertKeyForEditedPartToDictionary()
        {
            this.ProcessDocument();

            this.dictionary.Received().Add(this.testKey, Arg.Any<string>());
        }

        [TestMethod]
        public void IfAddingPartToDictionaryThenUseNewValuePrefixFromConfiguration()
        {
            this.configuration.NewValuePrefix = "!";

            this.ProcessDocument();

            this.dictionary.Received().Add(Arg.Any<string>(), "!" + this.testValue);
        }
    }
}
