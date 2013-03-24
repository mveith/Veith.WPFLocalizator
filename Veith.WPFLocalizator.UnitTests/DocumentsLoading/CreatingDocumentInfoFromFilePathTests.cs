using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Veith.WPFLocalizator.Model;

namespace Veith.WPFLocalizator.UnitTests.DocumentsLoading
{
    [TestClass]
    public class CreatingDocumentInfoFromFilePathTests : DocumentsLoadingTestsBase
    {
        private string filePath;

        [TestInitialize]
        public new void TestInitialize()
        {
            base.TestInitialize();

            this.AddFilePath("FILEPATH.xaml");
        }

        [TestMethod]
        public void IfCreateDocumentInfoFromFilePathThenUseFileSystemWrapperToObtainFileContent()
        {
            this.fileSystem.GetFileContent(this.filePath).Returns("FILE CONTENT");

            var document = this.LoadDocument();

            Assert.AreEqual("FILE CONTENT", document.Content);
            this.fileSystem.Received().GetFileContent(this.filePath);
        }

        [TestMethod]
        public void DocumentInfoExtensionIsPartAfterLastDotInFilePath()
        {
            var document = this.LoadDocument();

            Assert.AreEqual("XAML", document.Extension);
        }

        [TestMethod]
        public void DocumentInfoFileNameIsFileNameFromFilePath()
        {
            this.AddFilePath("Directory/Subdirecory/fileName.xaml");

            var document = this.LoadDocument();

            Assert.AreEqual("fileName", document.FileName);
        }

        private DocumentInfo LoadDocument()
        {
            return this.LoadDocuments().Single();
        }

        private void AddFilePath(string path)
        {
            this.files.Clear();
            this.filePath = path;
            this.files.Add(this.filePath);
        }
    }
}
