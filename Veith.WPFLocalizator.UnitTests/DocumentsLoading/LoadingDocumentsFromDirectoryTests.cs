using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Veith.WPFLocalizator.DocumentsLoading;

namespace Veith.WPFLocalizator.UnitTests.DocumentsLoading
{
    [TestClass]
    public class LoadingDocumentsFromDirectoryTests : DocumentsLoadingTestsBase
    {
        [TestInitialize]
        public new void TestInitialize()
        {
            base.TestInitialize();
        }

        [TestMethod]
        public void IfLoadingDocumentsInDirectoryThenUseFileSystemWrapperForLoadingFilesInDirectory()
        {
            this.LoadDocuments();

            this.fileSystem.Received().GetFilesWithExtensionInDirectory(this.directoryPath, Arg.Any<string>());
        }

        [TestMethod]
        public void ForEachValidFileInDirectoryCreateDocumentInfo()
        {
            var file1Path = "FilePath1.xaml";
            var file2Path = "FilePath2.xaml";

            this.files.Add(file1Path);
            this.files.Add(file2Path);

            var documents = this.LoadDocuments();

            Assert.AreEqual(2, documents.Length);
            Assert.AreEqual(file1Path, documents[0].FilePath);
            Assert.AreEqual(file2Path, documents[1].FilePath);
        }

        [TestMethod]
        public void IfLoadingDocumentsThenReturnOnlyDocumentsWithSelectedExtension()
        {
            this.selectedExtension = ".cs";

            var documents = this.LoadDocuments();

            this.fileSystem.Received().GetFilesWithExtensionInDirectory(Arg.Any<string>(), this.selectedExtension);
        }

        [TestMethod]
        [ExpectedException(typeof(DocumentsLoadingException))]
        public void IfLoadingDocumentsThrowExceptionThenThrowDocumentsLoadingException()
        {
            this.fileSystem.GetFilesWithExtensionInDirectory(Arg.Any<string>(), Arg.Any<string>()).Returns(x =>
            {
                throw new System.IO.FileNotFoundException();
            });

            this.LoadDocuments();
        }
    }
}
