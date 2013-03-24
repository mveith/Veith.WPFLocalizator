using System.Linq;
using System.Xml.Linq;
using Veith.WPFLocalizator.Common;
using Veith.WPFLocalizator.DocumentsProcessing;

namespace Veith.WPFLocalizator.Configuration
{
    public class ConfigurationFactory : IConfigurationFactory
    {
        private readonly IFileSystemWrapper fileSystem;

        public ConfigurationFactory(IFileSystemWrapper fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public DocumentProcessorConfiguration CreateConfigurationForProject(string projectFilePath)
        {
            var projectConfigurationFileContent = this.fileSystem.GetFileContent(projectFilePath);

            var xmlConfigurationRoot = XDocument.Parse(projectConfigurationFileContent).Root;

            var configuration = CreateConfiguration(xmlConfigurationRoot);

            return configuration;
        }

        public string Serialize(DocumentProcessorConfiguration configuration)
        {
            var result = new XDocument();
            result.Declaration = new XDeclaration("1.0", "utf-8", null);

            var root = new XElement("Project");

            result.Add(root);
            root.Add(new XElement("Directory", configuration.SelectedDirectory));
            var dictionaries = new XElement("ResourcesDictionaries");
            foreach (var dictionary in configuration.ResourceFilesPaths)
            {
                dictionaries.Add(new XElement("ResourcesDictionary", dictionary));
            }

            root.Add(dictionaries);
            root.Add(new XElement("Prefix", configuration.Prefix));
            root.Add(new XElement("Suffix", configuration.Suffix));
            root.Add(new XElement("NamespaceValue", configuration.NamespaceValue));
            root.Add(new XElement("ResourceFileName", configuration.ResourceFileName));
            root.Add(new XElement("NewValuePrefix", configuration.NewValuePrefix));

            return result.ToStringWithDeclaration();
        }

        private static DocumentProcessorConfiguration CreateConfiguration(XElement root)
        {
            var configuration = new DocumentProcessorConfiguration()
            {
                Prefix = GetValueForProperty(root, "Prefix"),
                NamespaceValue = GetValueForProperty(root, "NamespaceValue"),
                NewValuePrefix = GetValueForProperty(root, "NewValuePrefix"),
                ResourceFileName = GetValueForProperty(root, "ResourceFileName"),
                Suffix = GetValueForProperty(root, "Suffix"),
                SelectedDirectory = GetValueForProperty(root, "Directory")
            };

            var resourcesDictionaries = root.Element("ResourcesDictionaries").Elements("ResourcesDictionary").Select(e => e.Value);

            foreach (var resDictionary in resourcesDictionaries)
            {
                configuration.ResourceFilesPaths.Add(resDictionary);
            }

            return configuration;
        }

        private static string GetValueForProperty(XElement root, string propertyName)
        {
            return root.Element(propertyName).Value;
        }
    }
}
