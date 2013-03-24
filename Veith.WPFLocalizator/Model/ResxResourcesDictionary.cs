using System.Xml.Linq;
using Veith.WPFLocalizator.Common;

namespace Veith.WPFLocalizator.Model
{
    public class ResxResourcesDictionary : IResourcesDictionary
    {
        private readonly IFileSystemWrapper fileSystem;
        private readonly string filePath;

        private XDocument xml;

        public ResxResourcesDictionary(string resxFilePath, IFileSystemWrapper fileSystem)
        {
            this.filePath = resxFilePath;
            this.fileSystem = fileSystem;
        }

        public void Add(string key, string value)
        {
            try
            {
                this.fileSystem.SaveFileContent(this.filePath, this.GetContentWithAddedElement(key, value));
            }
            catch (System.Exception e)
            {
                throw new ResourcesDictionaryUpdatingException(e);
            }
        }

        private string GetContentWithAddedElement(string key, string value)
        {
            this.xml = XDocument.Parse(this.fileSystem.GetFileContent(this.filePath));

            this.AddNewElementToXml(key, value);

            return this.xml.ToStringWithDeclaration();
        }

        private void AddNewElementToXml(string key, string value)
        {
            this.xml.Root.Add(this.CreateElementForKeyAndValue(key, value));
        }

        private XElement CreateElementForKeyAndValue(string key, string value)
        {
            var newElement = new XElement("data", new XElement("value", value));

            newElement.Add(new XAttribute("name", key));
            newElement.Add(new XAttribute(this.xml.Root.GetNamespaceOfPrefix("xml") + "space", "preserve"));
            return newElement;
        }
    }
}
