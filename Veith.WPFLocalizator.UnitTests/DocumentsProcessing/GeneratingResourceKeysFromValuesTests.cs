using Microsoft.VisualStudio.TestTools.UnitTesting;
using Veith.WPFLocalizator.DocumentsProcessing;
using Veith.WPFLocalizator.Model;

namespace Veith.WPFLocalizator.UnitTests.DocumentsProcessing
{
    [TestClass]
    public class GeneratingResourceKeysFromValuesTests
    {
        private ResourcesKeysFromPartValueGenerator generator;
        private string documentName;
        private DocumentPart part;

        [TestInitialize]
        public void TestInitialize()
        {
            this.generator = new ResourcesKeysFromPartValueGenerator();

            this.documentName = "DocName";
            this.part = new DocumentPart() { Value = string.Empty };

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
        public void NewResourceKeyContinuesWithPartValue()
        {
            this.part.Value = "Text";

            var actual = this.GenerateKey().Replace(this.documentName, string.Empty);

            Assert.AreEqual("Text", actual);
        }

        [TestMethod]
        public void IfPartValueContaninsSpacesThenDeleteThem()
        {
            this.part.Value = " TEXT WITH SPACES ";

            var actual = this.GenerateKey().Replace(this.documentName, string.Empty);

            Assert.AreEqual("TEXTWITHSPACES", actual);
        }

        [TestMethod]
        public void EachWordFromValueStartsWithCapitalInKey()
        {
            this.part.Value = "Text with spaces";

            var actual = this.GenerateKey().Replace(this.documentName, string.Empty);

            Assert.AreEqual("TextWithSpaces", actual);
        }

        [TestMethod]
        public void IfPartValueContainsNonLettersThenDeleteThem()
        {
            this.part.Value = ";TEXT-WITH:OTHER,SYMBOLS.?!";

            var actual = this.GenerateKey().Replace(this.documentName, string.Empty);

            Assert.AreEqual("TEXTWITHOTHERSYMBOLS", actual);
        }

        [TestMethod]
        public void RemoveDiacriticsFromPartValue()
        {
            this.part.Value = "Ťěxť Šé Špóúštoů Ďíákřítíký A Žábou";

            var actual = this.GenerateKey().Replace(this.documentName, string.Empty);

            Assert.AreEqual("TextSeSpoustouDiakritikyAZabou", actual);
        }

        private string GenerateKey()
        {
            return this.generator.CreateKey(this.documentName, this.part);
        }
    }
}
