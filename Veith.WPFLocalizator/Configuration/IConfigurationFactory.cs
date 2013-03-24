using Veith.WPFLocalizator.DocumentsProcessing;

namespace Veith.WPFLocalizator.Configuration
{
    public interface IConfigurationFactory
    {
        DocumentProcessorConfiguration CreateConfigurationForProject(string projectFilePath);

        string Serialize(DocumentProcessorConfiguration configuration);
    }
}