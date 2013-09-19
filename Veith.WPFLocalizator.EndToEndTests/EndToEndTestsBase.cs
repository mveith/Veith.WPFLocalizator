using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Castle.MicroKernel.Registration;
using Veith.WPFLocalizator.DocumentsProcessing;
using Veith.WPFLocalizator.UserInteraction;
using Veith.WPFLocalizator.ViewModel;

namespace Veith.WPFLocalizator.EndToEndTests
{
    public abstract class EndToEndTestsBase
    {
        protected readonly string path = "TestData";
        protected readonly string fileName = "TestXAML.xaml";
        protected readonly string cSharpFileName = "TestCSharpCode.cs";
        protected readonly string[] resourceFilesPaths = new[] { "Resources.cs-cz.resx", "Resources.en-gb.resx" };

        protected readonly string prefix = "{lng:LocText";
        protected readonly string suffix = "}";

        protected readonly string namespaceValue = "MyNamespace";
        protected readonly string resourcesFileName = "Resources";

        protected readonly string selectedFileExtension = ".xaml";
        protected MainWindowViewModel mainWindowViewModel;

        protected string userEditingPostfix = string.Empty;

        protected TestUserInteraction userInteraction;

        private LocalizatorContainer container;

        protected void Initialize()
        {
            Directory.CreateDirectory(this.path);

            File.Copy(Path.Combine("Resources", this.fileName), Path.Combine(this.path, this.fileName), true);
            File.Copy(Path.Combine("Resources", this.cSharpFileName), Path.Combine(this.path, this.cSharpFileName), true);
            foreach (var resourceFileName in this.resourceFilesPaths)
            {
                File.Copy(Path.Combine("Resources", resourceFileName), Path.Combine(this.path, resourceFileName), true);
            }

            var stringPartsNames = ConfigurationManager.AppSettings["StringParts"].Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            var settings = new DocumentProcessorConfiguration()
            {
                Prefix = this.prefix,
                Suffix = this.suffix,
                NamespaceValue = this.namespaceValue,
                ResourceFileName = this.resourcesFileName
            };

            this.container = this.CreateTestContainer(stringPartsNames, settings);

            this.userInteraction = container.Resolve<IUserInteraction>() as TestUserInteraction;

            this.mainWindowViewModel = container.Resolve<MainWindowViewModel>();

            mainWindowViewModel.SelectedDirectory = this.path;

            foreach (var filePath in this.resourceFilesPaths)
            {
                mainWindowViewModel.ResourceFilesPaths.Add(Path.Combine(this.path, filePath));
            }
        }

        protected void TestCleanup()
        {
            Directory.Delete(this.path, true);

            this.container.Dispose();
        }

        protected static string GetAtributeWithKeyValue(IEnumerable<XElement> dataElements, string key)
        {
            var elementForKey = dataElements.Single(de => de.Attribute("name").Value == key);

            return elementForKey.Element("value").Value;
        }

        protected bool IsKeyUsedInFile(string resultText, string pattern, string key)
        {
            return resultText.Contains(string.Format(pattern, this.GetContentForKey(key)));
        }

        protected static string GetContentForKey(string key, string prefix, string namespaceValue, string resourcesFileName, string suffix)
        {
            return prefix + " " + namespaceValue + ":" + resourcesFileName + ":" + key + suffix;
        }

        private string GetContentForKey(string key)
        {
            return GetContentForKey(key, this.prefix, this.namespaceValue, this.resourcesFileName, this.suffix);
        }

        private LocalizatorContainer CreateTestContainer(string[] stringPartsNames, DocumentProcessorConfiguration settings)
        {
            var container = new LocalizatorContainer();
            container.RegisterInitialComponents(new LocalizatorContainerSettings() { ProjectsDirectory = string.Empty, StringPartsNames = stringPartsNames, BackupDirectory = string.Empty });
            container.RegisterMainViewModel(settings);
            container.Register(Component.For<IUserInteraction>().IsDefault().ImplementedBy<TestUserInteraction>().DependsOn(new { userEditingPostfix = userEditingPostfix }));
            container.Register(Component.For<IDirectoriesProcessor>().IsDefault().ImplementedBy<DirectoriesProcessor>().Named("Overriding"));
            return container;
        }
    }
}
