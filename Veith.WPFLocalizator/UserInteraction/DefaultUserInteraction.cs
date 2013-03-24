using System;
using System.Collections.Generic;
using System.IO;
using Veith.WPFLocalizator.Common;
using Veith.WPFLocalizator.Model;
using Veith.WPFLocalizator.View;

namespace Veith.WPFLocalizator.UserInteraction
{
    public class DefaultUserInteraction : IUserInteraction
    {
        private readonly IUserEditingKeysWindow userEditingKeysWindow;
        private readonly IFileSystemWrapper fileSystem;
        private readonly ProjectsTools projectsTools;

        public DefaultUserInteraction(
            IUserEditingKeysWindow userEditingKeysWindow,
            IFileSystemWrapper fileSystem,
            ProjectsTools projectsTool)
        {
            this.userEditingKeysWindow = userEditingKeysWindow;
            this.fileSystem = fileSystem;
            this.projectsTools = projectsTool;
        }

        public void UserEditingKeys(IEnumerable<KeyAndValueItem> items, string fileName)
        {
            this.userEditingKeysWindow.ShowWindow(items, fileName);
        }

        public string SelectDirectory()
        {
            return this.fileSystem.SelectDirectory();
        }

        public IEnumerable<string> SelectFiles()
        {
            return this.fileSystem.SelectFiles();
        }

        public void ShowMessage(string message)
        {
            MessageBoxWindow.Show("UserInteractionMessageTitle".Localize(), message);
        }

        public void ShowErrorMessage(Exception exception)
        {
            var errorMessage = string.Empty;

            if (exception.InnerException != null)
            {
                errorMessage = string.Format(
                    "{2} {0} ({1}).",
                    GetExceptionText(exception),
                    GetExceptionText(exception.InnerException),
                    "UserInteractionErrorMessage".Localize());
            }
            else
            {
                errorMessage = string.Format("{1} {0}.", GetExceptionText(exception), "UserInteractionErrorMessage".Localize());
            }

            MessageBoxWindow.Show("UserInteractionErrorTitle".Localize(), errorMessage);
        }

        public string SelectProject()
        {
            var projectFiles = this.fileSystem.GetFilesWithExtensionInDirectory(this.projectsTools.ProjectsDirectory, "xml");

            return this.projectsTools.SelectingProjectWindow.SelectProject(projectFiles);
        }

        public string SaveProject(string serializedConfiguration, string actualProjectName)
        {
            var projectName = this.projectsTools.NameSelectingWindow.GetName(actualProjectName);

            if (projectName == null)
            {
                return null;
            }

            this.SaveProjectWithName(serializedConfiguration, projectName);

            return projectName;
        }

        public void SaveLastProject(string serializedConfiguration)
        {
            this.SaveProjectWithName(serializedConfiguration, "LastProject");
        }

        public void SaveProjectWithName(string serializedConfiguration, string projectName)
        {
            this.fileSystem.SaveFileContent(Path.Combine(this.projectsTools.ProjectsDirectory, string.Format("{0}.xml", projectName)), serializedConfiguration);
        }

        private static string GetExceptionText(Exception exception)
        {
            if (exception == null)
            {
                return string.Empty;
            }

            return string.Format("{0}[{1}]", exception.Message, exception.GetType().Name);
        }
    }

    public class ProjectsTools
    {
        public ProjectsTools(string projectsDirectory)
        {
            this.ProjectsDirectory = projectsDirectory;
        }

        public string ProjectsDirectory { get; private set; }

        public ISelectingProjectWindow SelectingProjectWindow { get; set; }

        public INameSelectingWindow NameSelectingWindow { get; set; }
    }
}
