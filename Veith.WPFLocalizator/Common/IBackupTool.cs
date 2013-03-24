using System.Collections.Generic;

namespace Veith.WPFLocalizator.Common
{
    public interface IBackupTool
    {
        void Backup(string directory, IEnumerable<string> resourceFilesPaths);

        void Revert(string directory, IEnumerable<string> resourceFilesPaths);

        void DeleteBackup();
    }
}
