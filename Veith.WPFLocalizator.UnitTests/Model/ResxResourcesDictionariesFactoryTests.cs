using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Veith.WPFLocalizator.Common;
using Veith.WPFLocalizator.Model;

namespace Veith.WPFLocalizator.UnitTests.Model
{
    [TestClass]
    public class ResxResourcesDictionariesFactoryTests
    {
        private IFileSystemWrapper fileSystem;
        private ResxResourcesDictionariesFactory factory;
        private string filePath;

        [TestInitialize]
        public void TestInitialize()
        {
            this.filePath = "TESTPATH";

            this.fileSystem = Substitute.For<IFileSystemWrapper>();

            this.factory = new ResxResourcesDictionariesFactory(this.fileSystem);
        }

        [TestMethod]
        public void IfCreatingFileForFileThenReturnResxFile()
        {
            var dictionary = this.factory.CreateDictionaryFromFile(this.filePath);

            Assert.IsInstanceOfType(dictionary, typeof(ResxResourcesDictionary));
        }
    }
}
