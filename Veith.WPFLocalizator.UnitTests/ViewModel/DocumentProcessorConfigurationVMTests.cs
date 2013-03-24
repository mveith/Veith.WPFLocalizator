using Microsoft.VisualStudio.TestTools.UnitTesting;
using Veith.WPFLocalizator.DocumentsProcessing;
using Veith.WPFLocalizator.ViewModel;

namespace Veith.WPFLocalizator.UnitTests.Model
{
    [TestClass]
    public class DocumentProcessorConfigurationVMTests
    {
        [TestMethod]
        public void SampleIsCreatedFromActualConfiguration()
        {
            var configuration = new DocumentProcessorConfigurationViewModel(new DocumentProcessorConfiguration() { Prefix = "{prefix", Suffix = "}", NamespaceValue = "My.Namespace", ResourceFileName = "ResourcesFile" });

            Assert.AreEqual("{prefix My.Namespace:ResourcesFile:VALUE}", configuration.Sample);
        }
    }
}
