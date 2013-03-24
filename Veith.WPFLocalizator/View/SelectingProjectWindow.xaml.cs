using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Veith.WPFLocalizator.UserInteraction;

namespace Veith.WPFLocalizator.View
{
    public partial class SelectingProjectWindow : Window
    {
        public SelectingProjectWindow(IEnumerable<string> projectsPaths)
        {
            this.InitializeComponent();

            this.Projects = projectsPaths.Select(p => new Project() { Path = p }).ToArray();

            this.DataContext = this;
        }

        public IEnumerable<Project> Projects { get; private set; }

        public Project SelectedProject { get; set; }

        public string SelectedProjectPath
        {
            get
            {
                return this.SelectedProject != null ? this.SelectedProject.Path : default(string);
            }
        }

        private void SelectButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    public class Project
    {
        public string Path { get; set; }

        public string Name
        {
            get
            {
                return System.IO.Path.GetFileNameWithoutExtension(this.Path);
            }
        }
    }

    public class SelectingProjectWindowOpener : ISelectingProjectWindow
    {
        public string SelectProject(IEnumerable<string> projectsPaths)
        {
            var window = new SelectingProjectWindow(projectsPaths);
            window.UpdateOwner();

            if (window.ShowDialog() == true)
            {
                return window.SelectedProjectPath;
            }

            return null;
        }
    }
}