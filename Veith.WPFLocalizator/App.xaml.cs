using System.Windows;
using Veith.WPFLocalizator.ViewModel;

namespace Veith.WPFLocalizator
{
    public partial class App : Application
    {
        private ApplicationViewModel viewModel;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.viewModel = new ApplicationViewModel();
            this.viewModel.Initialize();

            this.ProcessStartupArgs(e.Args);

            this.DispatcherUnhandledException += (s, ea) => this.viewModel.ProcessUnhandledException(ea.Exception);

            var window = new MainWindow()
            {
                DataContext = this.viewModel.MainWindowViewModel
            };

            window.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            try
            {
                this.viewModel.SaveLastProject();

                this.viewModel.Container.Dispose();
            }
            catch (System.Exception)
            {
            }
        }

        private void ProcessStartupArgs(string[] startupArgs)
        {
            if (startupArgs.Length > 0)
            {
                var selectedPath = startupArgs[0];

                if (!string.IsNullOrEmpty(selectedPath) && System.IO.File.Exists(selectedPath))
                {
                    this.viewModel.ProcessSingleFile(selectedPath);

                    this.Shutdown();
                }
            }
        }
    }
}
