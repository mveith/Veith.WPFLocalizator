using System;
using NSubstitute;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Veith.WPFLocalizator.Common;
using Veith.WPFLocalizator.DocumentsSerializing;
using Veith.WPFLocalizator.Model;
using System.Xml.Linq;
using System.Text;

namespace Veith.WPFLocalizator.UnitTests.DocumentsSerializing
{
    [TestClass]
    public class DocumentsSerializingTests
    {
        private DefaultDocumentSerializer serializer;
        private IFileSystemWrapper fileSystem;
        private ParsedDocument document;

        [TestInitialize]
        public void TestInitialize()
        {
            this.fileSystem = Substitute.For<IFileSystemWrapper>();

            this.serializer = new DefaultDocumentSerializer(this.fileSystem);

            this.document = new ParsedDocument();

            this.document.Document = new DocumentInfo() { Content = "<root /> " };

        }

        [TestMethod]
        public void IfSerializingDocumentThenSaveNewContentToFile()
        {
            this.document.Document.FilePath = "TESTPATH";

            this.document.Parts = new[]
            { 
                new DocumentPart() { Name = "Title", Value = "NEW TITLE VALUE" , ElementyTypeName = "Button", IsSelectedForLocalization=  true} 
            };

            this.SerializeDocument();

            this.fileSystem.Received().SaveFileContent("TESTPATH", Arg.Any<string>());
        }

        [TestMethod]
        public void IfDocumentPartsAreEmptyThenSaveOriginalFileContent()
        {
            this.document.Document.Content = "<TESTCONTENT />";

            this.document.Parts = new[]
            { 
                new DocumentPart() { Name = "Title", Value = "NEW TITLE VALUE" , ElementyTypeName = "Button", IsSelectedForLocalization=  true} 
            };

            this.SerializeDocument();

            this.fileSystem.Received().SaveFileContent(Arg.Any<string>(), "<TESTCONTENT />");
        }

        [TestMethod]
        public void IfDocumentPartWasChangedThenSaveContentWithNewValue()
        {
            this.document.Document.Content = "<Button Content=\"OK\"></Button>";

            this.document.Parts = new[] { new DocumentPart() { Name = "Content", Value = "NEW VALUE", ElementyTypeName = "Button", IsSelectedForLocalization = true } };

            this.SerializeDocument();

            this.fileSystem.Received().SaveFileContent(Arg.Any<string>(), "<Button Content=\"NEW VALUE\"></Button>");
        }

        [TestMethod]
        public void UpdatingValuesForEachPart()
        {
            this.document.Document.Content = "<Button Content=\"OK\" Title=\"OldTitle\"></Button>";

            this.document.Parts = new[]
            { 
                new DocumentPart() { Name = "Content", Value = "NEW VALUE" , ElementyTypeName = "Button", IsSelectedForLocalization=  true} ,
                new DocumentPart() { Name = "Title", Value = "NEW TITLE VALUE" , ElementyTypeName = "Button", IsSelectedForLocalization=  true} 
            };

            this.SerializeDocument();

            this.fileSystem.Received().SaveFileContent(Arg.Any<string>(), "<Button Content=\"NEW VALUE\" Title=\"NEW TITLE VALUE\"></Button>");
        }

        [TestMethod]
        public void IfUpdatingValuesForPartsThenUpdateOnlyOneElementForPart()
        {
            var content = "<Grid><Button Content=\"OK\" Width=\"50\"></Button><Button Content=\"Cancel\" Width=\"30\"></Button></Grid>";

            this.document.Document.Content = content;

            this.document.Parts = new[]
            { 
                new DocumentPart() { Name = "Content", Value = "NEW VALUE" , ElementyTypeName = "Button", ElementIndex = 1, IsSelectedForLocalization=  true},
                new DocumentPart() { Name = "Width", Value = "50" , ElementyTypeName = "Button", ElementIndex = 1, IsSelectedForLocalization=  true},
                new DocumentPart() { Name = "Content", Value = "NEW VALUE 2" , ElementyTypeName = "Button", ElementIndex = 2, IsSelectedForLocalization=  true},
                new DocumentPart() { Name = "Width", Value = "30" , ElementyTypeName = "Button" , ElementIndex = 2, IsSelectedForLocalization=  true}
            };

            this.SerializeDocument();

            var expectedContent =
                XDocument.Parse("<Grid><Button Content=\"NEW VALUE\" Width=\"50\"></Button><Button Content=\"NEW VALUE 2\" Width=\"30\"></Button></Grid>")
                .ToString();

            this.fileSystem.Received().SaveFileContent(Arg.Any<string>(), expectedContent);
        }

        [TestMethod]
        public void IfUpdatingValuesForAttributeElementsThenUpdateElementValue()
        {
            this.document.Document.Content = "<Button Title=\"OldTitle\"><Button.Content>OK</Button.Content></Button>";

            this.document.Parts = new[]
            { 
                new DocumentPart() { Name = "Content", Value = "NEW VALUE" , ElementyTypeName = "Button" , ElementIndex = 1, IsSelectedForLocalization=  true} ,
                new DocumentPart() { Name = "Title", Value = "NEW TITLE VALUE" , ElementyTypeName = "Button", ElementIndex = 0, IsSelectedForLocalization=  true} 
            };

            this.SerializeDocument();

            var expectedContent =
                XDocument.Parse("<Button Title=\"NEW TITLE VALUE\"><Button.Content>NEW VALUE</Button.Content></Button>")
                .ToString();

            this.fileSystem.Received().SaveFileContent(Arg.Any<string>(), expectedContent);
        }

        [TestMethod]
        [ExpectedException(typeof(DocumentsSerializingException))]
        public void IfSavingFileThrowsExceptionThenThrowDocumentSerializingException()
        {
            this.fileSystem.WhenForAnyArgs(fs => fs.SaveFileContent(Arg.Any<string>(), Arg.Any<string>())).Do(x =>
            {
                throw new Exception();
            });

            this.SerializeDocument();
        }

        [TestMethod]
        public void ForNotSelectedPartsNotUpdateValue()
        {
            this.document.Document.Content = "<Button Content=\"OK\" Title=\"OldTitle\"></Button>";

            this.document.Parts = new[]
            { 
                new DocumentPart() { Name = "Content", Value = "NEW VALUE" , ElementyTypeName = "Button", IsSelectedForLocalization=  true} ,
                new DocumentPart() { Name = "Title", Value = "NEW TITLE VALUE" , ElementyTypeName = "Button", IsSelectedForLocalization=  false} 
            };

            this.SerializeDocument();

            this.fileSystem.Received().SaveFileContent(Arg.Any<string>(), "<Button Content=\"NEW VALUE\" Title=\"OldTitle\"></Button>");

        }

        [TestMethod]
        public void IfNoPartsAreSelectedForLocalizingThenDoNotSaveFile()
        {
            this.document.Document.Content = "<Button Content=\"OK\" Title=\"OldTitle\"></Button>";

            this.document.Parts = new[]
            { 
                new DocumentPart() { Name = "Title", Value = "NEW TITLE VALUE" , ElementyTypeName = "Button", IsSelectedForLocalization=  false} 
            };

            this.SerializeDocument();

            this.fileSystem.DidNotReceive().SaveFileContent(Arg.Any<string>(), Arg.Any<string>());
        }

        private void SerializeDocument()
        {
            this.serializer.SerializeDocument(this.document);
        }
    }
}
