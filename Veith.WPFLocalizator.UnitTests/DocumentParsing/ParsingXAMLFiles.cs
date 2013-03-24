using System;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Veith.WPFLocalizator.DocumentsParsing;
using Veith.WPFLocalizator.Model;

namespace Veith.WPFLocalizator.UnitTests.DocumentParsing
{
    [TestClass]
    public class ParsingXAMLFiles
    {
        private XAMLDocumentParser documentParser;
        private DocumentInfo testDocument;

        [TestInitialize]
        public void TestInitialize()
        {
            this.documentParser = new XAMLDocumentParser();
            this.testDocument = new DocumentInfo()
            {
                Extension = "XAML",
                Content = "<xml />",
                FilePath = "FILE PATH.xaml"
            };
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void IfDocumentIsNotXAMLThenThrowInvalidOperationException()
        {
            this.testDocument.Extension = "CS";

            this.ParseDocument();
        }

        [TestMethod]
        public void IfParsingDocumentThenParsedDocumentHasOriginalDocumentInfo()
        {
            var actual = this.ParseDocument();

            Assert.AreEqual(this.testDocument, actual.Document);
        }

        [TestMethod]
        public void IfParsingXAMLThenReturnPartsForXAMLAttributes()
        {
            this.testDocument.Content = "<Button Content=\"OK\" Width=\"60\"></Button>";

            var actual = this.ParseDocument();

            Assert.AreEqual(2, actual.Parts.Count());
            Assert.AreEqual("Content", actual.Parts.First().Name);
            Assert.AreEqual("Width", actual.Parts.Last().Name);
            Assert.AreEqual("OK", actual.Parts.First().Value);
            Assert.AreEqual("60", actual.Parts.Last().Value);
        }

        [TestMethod]
        public void IfXAMLContainsMoreElementsThenReturnPartsFromAllElementsAttributes()
        {
            var testContent = new StringBuilder();
            testContent.AppendLine("<root>");
            testContent.AppendLine("<Button Content=\"OK\"></Button>");
            testContent.AppendLine("<TextBlock Text=\"TestText\"></TextBlock>");
            testContent.AppendLine("</root>");

            this.testDocument.Content = testContent.ToString();

            var actual = this.ParseDocument();

            Assert.AreEqual(2, actual.Parts.Count());
            Assert.AreEqual("Content", actual.Parts.First().Name);
            Assert.AreEqual("Text", actual.Parts.Last().Name);
            Assert.AreEqual("OK", actual.Parts.First().Value);
            Assert.AreEqual("TestText", actual.Parts.Last().Value);
        }

        [TestMethod]
        public void IfXAMLContainsDotAttributeThenUseThisAttribute()
        {
            var testContent = new StringBuilder();
            testContent.AppendLine("<root>");
            testContent.AppendLine("<Button><Button.Content>OK</Button.Content></Button>");
            testContent.AppendLine("</root>");

            this.testDocument.Content = testContent.ToString();

            var actual = this.ParseDocument();

            Assert.AreEqual(1, actual.Parts.Count());
            Assert.AreEqual("Content", actual.Parts.First().Name);
            Assert.AreEqual("OK", actual.Parts.First().Value);
        }

        [TestMethod]
        public void EachPartHaveElementTypeNameFromElement()
        {
            this.testDocument.Content = "<Button Content=\"OK\"></Button>";

            var actual = this.ParseDocument();

            Assert.AreEqual("Button", actual.Parts.First().ElementyTypeName);
        }

        [TestMethod]
        public void IfParentElementHasNameThenSetParentNameToPart()
        {
            this.testDocument.Content = "<Button Content=\"OK\" Name=\"TESTNAME\"></Button>";

            var actual = this.ParseDocument();

            Assert.AreEqual("TESTNAME", actual.Parts.First().ElementName);

        }

        [TestMethod]
        public void EachPartHaveIndexFromElementIndexInAllElementsInDocument()
        {
            this.testDocument.Content = "<Grid><Button Content=\"OK\" /><Button Content=\"OK\" Width=\"50\" /></Grid>";

            var actual = this.ParseDocument().Parts.ToArray();

            Assert.AreEqual(1, actual[0].ElementIndex);
            Assert.AreEqual(2, actual[1].ElementIndex);
            Assert.AreEqual(2, actual[2].ElementIndex);
        }

        [TestMethod]
        public void IfXAMLContainsDotAttributeWithElementThenUseThisElements()
        {
            var testContent = new StringBuilder();
            testContent.AppendLine("<Grid>");
            testContent.AppendLine("<Grid.RowDefinitions><RowDefinition Height=\"*\" /><RowDefinition Height=\"Auto\" /></Grid.RowDefinitions>");
            testContent.AppendLine("</Grid>");

            this.testDocument.Content = testContent.ToString();

            var actual = this.ParseDocument();

            Assert.AreEqual(2, actual.Parts.Count());
            Assert.AreEqual("Height", actual.Parts.First().Name);
            Assert.AreEqual("*", actual.Parts.First().Value);
            Assert.AreEqual("Height", actual.Parts.Last().Name);
            Assert.AreEqual("Auto", actual.Parts.Last().Value);
        }

        private ParsedDocument ParseDocument()
        {
            return this.documentParser.ParseDocument(this.testDocument);
        }
    }
}
