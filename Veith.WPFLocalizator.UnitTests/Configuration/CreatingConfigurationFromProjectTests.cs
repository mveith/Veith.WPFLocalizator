using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Veith.WPFLocalizator.Common;
using Veith.WPFLocalizator.Configuration;
using Veith.WPFLocalizator.DocumentsProcessing;

namespace Veith.WPFLocalizator.UnitTests.Configuration
{
    [TestClass]
    public class CreatingConfigurationFromProjectTests
    {
        private ConfigurationFactory factory;
        private IFileSystemWrapper fileSystem;

        [TestInitialize]
        public void TestInitialize()
        {
            this.fileSystem = Substitute.For<IFileSystemWrapper>();

            this.factory = new ConfigurationFactory(this.fileSystem);

            var projectConfigurationContent = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" +
                                                @"<Project>
                                                    <Directory>TestData</Directory>
                                                    <ResourcesDictionaries>
                                                      <ResourcesDictionary>Resources.cs-cz.resx</ResourcesDictionary>
                                                      <ResourcesDictionary>Resources.en-gb.resx</ResourcesDictionary>
                                                    </ResourcesDictionaries>
                                                    <Prefix>ProjectPrefix</Prefix>
                                                    <Suffix>ProjectSuffix</Suffix>
                                                    <NamespaceValue>MyNamespaceProject</NamespaceValue>
                                                    <ResourceFileName>ResourcesInProject</ResourceFileName>
                                                    <NewValuePrefix>!</NewValuePrefix>
                                                  </Project>";

            this.fileSystem.GetFileContent(Arg.Any<string>()).Returns(projectConfigurationContent);
        }

        [TestMethod]
        public void IfCreatingConfigurationForProjectThenLoadProjectFile()
        {
            this.factory.CreateConfigurationForProject(@"c:\Projects\TestProject.xml");

            this.fileSystem.Received().GetFileContent(@"c:\Projects\TestProject.xml");
        }

        [TestMethod]
        public void ForLoadedProjectConfigurationFileContentThenParseXmlToResultConfiguration()
        {
            var configuration = this.factory.CreateConfigurationForProject("TestProject");

            Assert.AreEqual("ProjectPrefix", configuration.Prefix);
            Assert.AreEqual("ProjectSuffix", configuration.Suffix);
            Assert.AreEqual("MyNamespaceProject", configuration.NamespaceValue);
            Assert.AreEqual("ResourcesInProject", configuration.ResourceFileName);
            Assert.AreEqual("!", configuration.NewValuePrefix);
            Assert.AreEqual("TestData", configuration.SelectedDirectory);
            Assert.AreEqual("Resources.cs-cz.resx", configuration.ResourceFilesPaths[0]);
            Assert.AreEqual("Resources.en-gb.resx", configuration.ResourceFilesPaths[1]);
        }

        [TestMethod]
        public void IfSerializingConfigurationThenSaveConfigurationToXmlString()
        {
            var configuration = new DocumentProcessorConfiguration()
            {
                NamespaceValue = "NS1",
                Prefix = "PRE",
                NewValuePrefix = "NVP",
                ResourceFileName = "RES",
                Suffix = "SUF",
                SelectedDirectory = "DIR"
            };

            configuration.ResourceFilesPaths.Add("RFP");

            var serialized = this.factory.Serialize(configuration);

            var expected = XDocument.Parse("<?xml version=\"1.0\" encoding=\"utf-8\" ?>" +
                                                @"<Project>
                                                    <Directory>DIR</Directory>
                                                    <ResourcesDictionaries>
                                                      <ResourcesDictionary>RFP</ResourcesDictionary>
                                                    </ResourcesDictionaries>
                                                    <Prefix>PRE</Prefix>
                                                    <Suffix>SUF</Suffix>
                                                    <NamespaceValue>NS1</NamespaceValue>
                                                    <ResourceFileName>RES</ResourceFileName>
                                                    <NewValuePrefix>NVP</NewValuePrefix>
                                                  </Project>").ToStringWithDeclaration();

            Assert.AreEqual(expected, serialized);
        }
    }
}
