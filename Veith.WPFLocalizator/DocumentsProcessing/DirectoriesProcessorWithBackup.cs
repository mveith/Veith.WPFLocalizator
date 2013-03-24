using System.Collections.Generic;
using Veith.WPFLocalizator.Common;

namespace Veith.WPFLocalizator.DocumentsProcessing
{
    public class DirectoriesProcessorWithBackup : IDirectoriesProcessor
    {
        private readonly IDirectoriesProcessor baseProcessor;
        private readonly IBackupTool backupTool;

        private string directory;
        private DocumentProcessorConfiguration configuration;

        public DirectoriesProcessorWithBackup(IDirectoriesProcessor baseProcessor, IBackupTool backupTool)
        {
            this.baseProcessor = baseProcessor;
            this.backupTool = backupTool;
        }

        public void ProcessDirectory(string directory, DocumentProcessorConfiguration configuration)
        {
            this.directory = directory;
            this.configuration = configuration;

            this.backupTool.Backup(this.directory, this.configuration.ResourceFilesPaths);

            try
            {
                this.Process();
            }
            catch (System.Exception)
            {
                this.backupTool.Revert(this.directory, this.configuration.ResourceFilesPaths);
                throw;
            }
            finally
            {
                this.backupTool.DeleteBackup();
            }
        }

        private void Process()
        {
            this.baseProcessor.ProcessDirectory(this.directory, this.configuration);
        }
    }
}