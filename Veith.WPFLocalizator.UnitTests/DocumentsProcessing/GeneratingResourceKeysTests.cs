using Microsoft.VisualStudio.TestTools.UnitTesting;
using Veith.WPFLocalizator.DocumentsProcessing;
using Veith.WPFLocalizator.Model;

namespace Veith.WPFLocalizator.UnitTests.DocumentsProcessing
{
    [TestClass]
    public class GeneratingResourceKeysTests
    {
        private DefaultResourcesKeysGenerator generator;
        private string documentName;
        private DocumentPart part;

        [TestInitialize]
        public void TestInitialize()
        {
            this.generator = new DefaultResourcesKeysGenerator();

            this.documentName = "Name";
            this.part = new DocumentPart();

            this.part.ElementyTypeName = "Window";
        }

        [TestMethod]
        public void NewResourceKeyStartsWithDocumentName()
        {
            this.documentName = "TESTNAME";

            var actual = this.GenerateKey();

            Assert.AreEqual(true, actual.StartsWith(this.documentName));
        }

        [TestMethod]
        public void IfPartElementHasNameThenUseNameInKeyAfterDocumentName()
        {
            this.part.ElementName = "TESTELEMENT";

            var actual = this.GenerateKey();

            Assert.AreEqual(true, actual.StartsWith(this.documentName + "TESTELEMENT"));
        }

        [TestMethod]
        public void IfPartElementHasNotNameThenUseElementTypeNameInKeyAfterDocumentName()
        {
            this.part.ElementyTypeName = "Button";

            var actual = this.GenerateKey();

            Assert.AreEqual(true, actual.StartsWith(this.documentName + "Button"));
        }

        [TestMethod]
        public void NewKeyEndsWithPartName()
        {
            this.part.Name = "TESTContent";

            var actual = this.GenerateKey();

            Assert.AreEqual(true, actual.EndsWith("TESTContent"));
        }

        private string GenerateKey()
        {
            return this.generator.CreateKey(this.documentName, this.part);
        }
    }
}
