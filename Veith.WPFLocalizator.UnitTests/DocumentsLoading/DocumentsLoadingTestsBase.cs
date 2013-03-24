using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using Veith.WPFLocalizator.Common;
using Veith.WPFLocalizator.DocumentsLoading;
using Veith.WPFLocalizator.Model;

namespace Veith.WPFLocalizator.UnitTests.DocumentsLoading
{
    public abstract class DocumentsLoadingTestsBase
    {
        protected DocumentsLoader loader;
        protected IFileSystemWrapper fileSystem;
        protected string directoryPath = "DIRECTORY PATH";
        protected string selectedExtension = ".xaml";
        protected List<string> files;

        protected void TestInitialize()
        {
            this.files = new List<string>();
            this.fileSystem = Substitute.For<IFileSystemWrapper>();
            this.loader = new DocumentsLoader(this.fileSystem);

            this.fileSystem.GetFilesWithExtensionInDirectory(this.directoryPath, Arg.Any<string>()).Returns(this.files);
        }

        protected DocumentInfo[] LoadDocuments()
        {
            return this.loader.LoadDocumentsInDirectory(this.directoryPath, this.selectedExtension).ToArray();
        }
    }
}
