using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using Veith.WPFLocalizator.Configuration;
using Veith.WPFLocalizator.DocumentsProcessing;
using Veith.WPFLocalizator.Infrastructure;
using Veith.WPFLocalizator.UserInteraction;

namespace Veith.WPFLocalizator.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IUserInteraction userInteraction;
        private readonly IConfigurationFactory configurationFactory;
        private readonly IDirectoriesProcessor processor;

        private DocumentProcessorConfigurationViewModel configuration;

        public MainWindowViewModel(
            IDirectoriesProcessor processor,
            IUserInteraction userInteraction,
            IConfigurationFactory configurationFactory,
            DocumentProcessorConfiguration configuration)
        {
            this.processor = processor;
            this.userInteraction = userInteraction;
            this.configurationFactory = configurationFactory;

            this.configuration = new DocumentProcessorConfigurationViewModel(configuration);

            this.ProcessSelectedDirectoryCommand = new RelayCommand(this.ProcessSelectedDirectory);
            this.SelectDirectoryCommand = new RelayCommand(this.SelectDirectory);
            this.AddResourcesDictionariesCommand = new RelayCommand(this.AddResourcesDictionaries);
            this.RemoveResourcesDictionaryCommand = new RelayCommand(this.RemoveSelectedResourcesFilePath, this.IsSelectedResourcesFilePath);
            this.OpenProjectCommand = new RelayCommand(this.OpenProject);
            this.SaveProjectCommand = new RelayCommand(this.SaveProject);

            this.LanguagesViewModel = new LanguagesViewModel();
        }

        public string SelectedDirectory
        {
            get
            {
                return this.configuration.SelectedDirectory;
            }

            set
            {
                this.configuration.SelectedDirectory = value;
                this.RaisePropertyChanged(() => this.SelectedDirectory);
            }
        }

        public ICommand ProcessSelectedDirectoryCommand { get; protected set; }

        public ICommand SelectDirectoryCommand { get; protected set; }

        public ICommand AddResourcesDictionariesCommand { get; protected set; }

        public ICommand RemoveResourcesDictionaryCommand { get; protected set; }

        public ICommand OpenProjectCommand { get; protected set; }

        public ICommand SaveProjectCommand { get; protected set; }

        public LanguagesViewModel LanguagesViewModel { get; private set; }

        public ObservableCollection<string> ResourceFilesPaths
        {
            get
            {
                return this.configuration.ResourceFilesPaths;
            }
        }

        public DocumentProcessorConfigurationViewModel Configuration
        {
            get
            {
                return this.configuration;
            }
        }

        public string SelectedResourceFile { get; set; }

        public string SelectedProjectName { get; protected set; }

        private void ProcessSelectedDirectory()
        {
            try
            {
                this.processor.ProcessDirectory(this.SelectedDirectory, this.configuration.OriginalConfiguration);

                this.userInteraction.ShowMessage("ProcessingCompletedMessage".Localize());
            }
            catch (System.Exception e)
            {
                this.userInteraction.ShowErrorMessage(e);
            }
        }

        private void SelectDirectory()
        {
            this.SelectedDirectory = this.userInteraction.SelectDirectory();
        }

        private void AddResourcesDictionaries()
        {
            var filesPath = this.userInteraction.SelectFiles();

            foreach (var filePath in filesPath)
            {
                if (!string.IsNullOrEmpty(filePath))
                {
                    this.ResourceFilesPaths.Add(filePath);
                }
            }
        }

        private bool IsSelectedResourcesFilePath()
        {
            return this.ResourceFilesPaths.Contains(this.SelectedResourceFile);
        }

        private void RemoveSelectedResourcesFilePath()
        {
            this.ResourceFilesPaths.Remove(this.SelectedResourceFile);
        }

        private void OpenProject()
        {
            var selectedProject = this.userInteraction.SelectProject();

            if (selectedProject == null)
            {
                return;
            }

            var configurationForProject = this.configurationFactory.CreateConfigurationForProject(selectedProject);

            this.configuration = new DocumentProcessorConfigurationViewModel(configurationForProject);

            this.SelectedProjectName = Path.GetFileNameWithoutExtension(selectedProject);

            this.RaisePropertyChanged(() => this.Configuration);
            this.RaisePropertyChanged(() => this.SelectedDirectory);
            this.RaisePropertyChanged(() => this.ResourceFilesPaths);
            this.RaisePropertyChanged(() => this.SelectedProjectName);
        }

        private void SaveProject()
        {
            var serializedConfiguration = this.configurationFactory.Serialize(this.configuration.OriginalConfiguration);

            var savedProjectName = this.userInteraction.SaveProject(serializedConfiguration, this.SelectedProjectName);

            if (savedProjectName == null)
            {
                return;
            }

            this.SelectedProjectName = savedProjectName;

            this.RaisePropertyChanged(() => this.SelectedProjectName);
        }
    }
}