using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Veith.WPFLocalizator.Common;
using Veith.WPFLocalizator.Model;

namespace Veith.WPFLocalizator.UnitTests.ResourcesDictionaries
{
    [TestClass]
    public class ResxResourcesDictionaryTests
    {
        private ResxResourcesDictionary dictionary;
        private string path;
        private IFileSystemWrapper fileSystem;

        private string key;
        private string value;

        [TestInitialize]
        public void TestInitialize()
        {
            this.path = "TEST PATH";
            this.fileSystem = Substitute.For<IFileSystemWrapper>();

            this.dictionary = new ResxResourcesDictionary(this.path, this.fileSystem);

            this.key = "TEST_KEY";
            this.value = "TEST VALUE";

            var actualContent = new StringBuilder();
            actualContent.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            actualContent.AppendLine("<root>");
            actualContent.AppendLine("</root>");

            this.fileSystem.GetFileContent(this.path).Returns(actualContent.ToString());
        }

        [TestMethod]
        public void IfAddRecordToDictionaryThenUpdateFileContent()
        {
            this.AddRecordToDictionary();

            this.fileSystem.Received().SaveFileContent(this.path, Arg.Any<string>());
        }

        [TestMethod]
        public void IfUpdatingResxFileContentThenAddElementToXML()
        {
            this.AddRecordToDictionary();

            var expectedContent = new StringBuilder();
            expectedContent.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            expectedContent.AppendLine("<root>");
            expectedContent.AppendLine("  <data name=\"TEST_KEY\" xml:space=\"preserve\">");
            expectedContent.AppendLine("    <value>TEST VALUE</value>");
            expectedContent.AppendLine("  </data>");
            expectedContent.Append("</root>");

            var expectedString = expectedContent.ToString();

            this.fileSystem.Received().SaveFileContent(this.path, expectedContent.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ResourcesDictionaryUpdatingException))]
        public void IfSavingResourceDictionaryContentThrowsExceptionThenThrowResourcesDictionaryUpdatingException()
        {
            this.fileSystem.WhenForAnyArgs(fs => fs.SaveFileContent(Arg.Any<string>(), Arg.Any<string>())).Do(x =>
            {
                throw new Exception();
            });

            this.AddRecordToDictionary();
        }

        private void AddRecordToDictionary()
        {
            this.dictionary.Add(this.key, this.value);
        }
    }
}
