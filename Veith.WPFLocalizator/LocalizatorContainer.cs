﻿using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Veith.WPFLocalizator.Common;
using Veith.WPFLocalizator.Configuration;
using Veith.WPFLocalizator.DocumentsLoading;
using Veith.WPFLocalizator.DocumentsParsing;
using Veith.WPFLocalizator.DocumentsProcessing;
using Veith.WPFLocalizator.DocumentsSerializing;
using Veith.WPFLocalizator.Model;
using Veith.WPFLocalizator.UserInteraction;
using Veith.WPFLocalizator.View;
using Veith.WPFLocalizator.ViewModel;

namespace Veith.WPFLocalizator
{
    public class LocalizatorContainer : WindsorContainer
    {
        public void RegisterInitialComponents(
            string projectsDirectory,
            string[] stringPartsNames,
            string backupDirectory)
        {
            this.Register(
                Component.For<IFileSystemWrapper>().ImplementedBy<FileSystemWrapper>().LifestyleSingleton(),
                Component.For<IDocumentsLoader>().ImplementedBy<DocumentsLoader>(),
                Component.For<IDocumentParser>().ImplementedBy<XAMLDocumentParser>(),
                Component.For<IConfigurationFactory>().ImplementedBy<ConfigurationFactory>(),
                Component.For<IUserEditingKeysWindow>().ImplementedBy<UserEditingKeysWindowOpener>(),
                Component.For<ISelectingProjectWindow>().ImplementedBy<SelectingProjectWindowOpener>(),
                Component.For<INameSelectingWindow>().ImplementedBy<NameSelectingWindowOpenener>(),
                Component.For<ProjectsTools>().DependsOn(new
                {
                    projectsDirectory = projectsDirectory
                }),
                Component.For<IUserInteraction>().ImplementedBy<DefaultUserInteraction>().LifestyleSingleton(),
                Component.For<IResourcesKeysGenerator>().ImplementedBy<DefaultResourcesKeysGenerator>(),
                Component.For<IKeysForPartsGenerator>().ImplementedBy<KeysForPartsGenerator>(),
                Component.For<IDocumentProcessor>().ImplementedBy<DocumentProcessor>().DependsOn(new
                {
                    stringPartsNames = stringPartsNames
                }),
                Component.For<IDocumentSerializer>().ImplementedBy<DefaultDocumentSerializer>(),
                Component.For<IResourcesDictionariesFactory>().ImplementedBy<ResxResourcesDictionariesFactory>(),
                Component.For<IBackupTool>().ImplementedBy<BackupTool>().DependsOn(new { configuration = new BackupConfiguration(backupDirectory, new[] { "xaml" }) }),
                Component.For<IDirectoriesProcessor>().ImplementedBy<DirectoriesProcessorWithBackup>(),
                Component.For<IDirectoriesProcessor>().ImplementedBy<DirectoriesProcessor>(),
                Component.For<DocumentsTools>(),
                Component.For<DocumentInfoFactory>());
        }

        public void RegisterMainViewModel(DocumentProcessorConfiguration configuration)
        {
            this.Register(Component.For<MainWindowViewModel>().DependsOn(new { configuration = configuration }));
        }
    }
}
