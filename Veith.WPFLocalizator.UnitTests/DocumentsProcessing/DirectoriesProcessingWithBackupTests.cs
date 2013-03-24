using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Veith.WPFLocalizator.Common;
using Veith.WPFLocalizator.DocumentsProcessing;

namespace Veith.WPFLocalizator.UnitTests.DocumentsProcessing
{
    [TestClass]
    public class DirectoriesProcessingWithBackupTests
    {
        private DirectoriesProcessorWithBackup processor;

        private IDirectoriesProcessor originalProcessor;
        private IBackupTool backupTool;

        private string directory;
        private DocumentProcessorConfiguration configuration;

        [TestInitialize]
        public void TestInitialize()
        {
            this.originalProcessor = Substitute.For<IDirectoriesProcessor>();

            this.backupTool = Substitute.For<IBackupTool>();

            this.processor = new DirectoriesProcessorWithBackup(this.originalProcessor, this.backupTool);

            this.directory = "DIRECTORY";
            this.configuration = new DocumentProcessorConfiguration();
            this.configuration.ResourceFilesPaths.Add("ResFilePath");
        }

        [TestMethod]
        public void IfProcessingDirectoryThenUseOriginalProcessorForProcessing()
        {
            this.ProcessDirectory();

            this.originalProcessor.Received().ProcessDirectory(this.directory, this.configuration);
        }

        [TestMethod]
        public void IfProcessingDirectoryThenBackup()
        {
            this.ProcessDirectory();

            this.backupTool.Received().Backup(this.directory, this.configuration.ResourceFilesPaths);
        }

        [TestMethod]
        public void AfterProcessingDeleteBackupDirectory()
        {
            this.ProcessDirectory();

            this.backupTool.Received().DeleteBackup();
        }

        [TestMethod]
        public void IfProcessingThrowsExceptionThenRevertBackup()
        {
            this.originalProcessor.When(x => x.ProcessDirectory(Arg.Any<string>(), Arg.Any<DocumentProcessorConfiguration>()))
                .Do(x => { throw new Exception(); });

            try
            {
                this.ProcessDirectory();
            }
            catch (Exception)
            {
            }

            this.backupTool.Received().Revert(this.directory, this.configuration.ResourceFilesPaths);
        }

        [TestMethod]
        public void IfProcessingThrowsExceptionThenTrowThisException()
        {
            var exception = new Exception();

            this.originalProcessor.When(x => x.ProcessDirectory(Arg.Any<string>(), Arg.Any<DocumentProcessorConfiguration>()))
                .Do(x => { throw exception; });

            try
            {
                this.ProcessDirectory();
                Assert.Fail("Měla být vyvolána výjimka");
            }
            catch (Exception e)
            {
                Assert.AreEqual(exception, e);
            }
        }

        private void ProcessDirectory()
        {
            this.processor.ProcessDirectory(this.directory, this.configuration);
        }
    }
}
