using NSubstitute;
using Veith.WPFLocalizator.Configuration;
using Veith.WPFLocalizator.DocumentsProcessing;
using Veith.WPFLocalizator.UserInteraction;
using Veith.WPFLocalizator.ViewModel;

namespace Veith.WPFLocalizator.UnitTests.ViewModel
{
    public abstract class MainWindowViewModelTestsBase
    {
        protected readonly string directoryPath = "DIRECTORY PATH";

        protected MainWindowViewModel viewModel;
        protected IUserInteraction userInteraction;
        protected IConfigurationFactory configurationFactory;
        protected DocumentProcessorConfiguration configuration;

        protected IDirectoriesProcessor processor;

        protected void TestInitialize()
        {
            this.userInteraction = Substitute.For<IUserInteraction>();
            this.configurationFactory = Substitute.For<IConfigurationFactory>();
            this.configuration = new DocumentProcessorConfiguration()
            {
                Prefix = "AA",
                Suffix = "BB",
                ResourceFileName = "CC",
                NamespaceValue = "DD"
            };

            this.processor = Substitute.For<IDirectoriesProcessor>();

            this.viewModel = new MainWindowViewModel(
                processor,
                this.userInteraction,
                this.configurationFactory,
                this.configuration)
                {
                    SelectedDirectory = this.directoryPath
                };
        }

    }
}
