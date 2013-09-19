using System;
using System.Configuration;
using System.IO;
using Veith.WPFLocalizator.Configuration;
using Veith.WPFLocalizator.DocumentsProcessing;
using Veith.WPFLocalizator.UserInteraction;

namespace Veith.WPFLocalizator.ViewModel
{
    internal class ApplicationViewModel
    {
        private DocumentProcessorConfiguration configuration;
        private IUserInteraction userInteraction;

        public Castle.Windsor.WindsorContainer Container { get; private set; }

        public MainWindowViewModel MainWindowViewModel { get; private set; }

        public void Initialize()
        {
            var container = new LocalizatorContainer();
            this.Container = container;

            container.RegisterInitialComponents(new LocalizatorContainerSettings()
            {
                ProjectsDirectory = GetProjectsDirectory(),
                StringPartsNames = GetStringPartsNames(),
                BackupDirectory = GetBackupDirectory(),
                KeysGenerator = GetSelectedKeysGenerator()
            });

            this.configuration = this.LoadConfiguration();

            container.RegisterMainViewModel(this.configuration);

            this.userInteraction = this.Container.Resolve<IUserInteraction>();

            this.MainWindowViewModel = this.Container.Resolve<MainWindowViewModel>();
        }

        public void SaveLastProject()
        {
            var configurationFactory = this.Container.Resolve<IConfigurationFactory>();
            var serializedProject = configurationFactory.Serialize(this.MainWindowViewModel.Configuration.OriginalConfiguration);
            this.userInteraction.SaveLastProject(serializedProject);
        }

        public void ProcessSingleFile(string selectedPath)
        {
            try
            {
                this.TryProcessSingleFile(selectedPath);
                this.userInteraction.ShowMessage("FileProcessingCompletedMessage".Localize());
            }
            catch (Exception)
            {
                this.userInteraction.ShowMessage("FileProcessingErrorMessage".Localize());
            }
        }

        public void ProcessUnhandledException(Exception exception)
        {
            this.userInteraction.ShowErrorMessage(exception);
        }

        private static DocumentProcessorConfiguration CreateDefaultConfiguration()
        {
            return new DocumentProcessorConfiguration()
            {
                Prefix = "{lng:LocText",
                Suffix = "}",
                NamespaceValue = "WPFLocalizator.Resources",
                ResourceFileName = "UIResources"
            };
        }

        private static string GetBackupDirectory()
        {
            var backupDirectory = Path.Combine(GetAppDataPath(), "Veith.WPFLocalizator", "Backup");

            if (!Directory.Exists(backupDirectory))
            {
                Directory.CreateDirectory(backupDirectory);
            }

            return backupDirectory;
        }

        private static string[] GetStringPartsNames()
        {
            return ConfigurationManager.AppSettings["StringParts"].Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static string GetSelectedKeysGenerator()
        {
            return ConfigurationManager.AppSettings["KeysGenerator"];
        }

        private static string GetProjectsDirectory()
        {
            var projectsDirectory = Path.Combine(GetAppDataPath(), "Veith.WPFLocalizator", "Projects");

            if (!Directory.Exists(projectsDirectory))
            {
                Directory.CreateDirectory(projectsDirectory);
            }

            return projectsDirectory;
        }

        private static string GetAppDataPath()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return appDataPath;
        }

        private void TryProcessSingleFile(string selectedPath)
        {
            var processor = this.Container.Resolve<IDirectoriesProcessor>();
            processor.ProcessDirectory(selectedPath, this.configuration);
        }

        private DocumentProcessorConfiguration LoadConfiguration()
        {
            var configuration = CreateDefaultConfiguration();

            var lastConfigurationFile = Path.Combine(GetProjectsDirectory(), "LastProject.xml");
            if (File.Exists(lastConfigurationFile))
            {
                configuration = this.Container.Resolve<IConfigurationFactory>().CreateConfigurationForProject(lastConfigurationFile);
            }

            return configuration;
        }
    }
}
