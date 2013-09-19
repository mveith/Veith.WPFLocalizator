using System;
using Veith.WPFLocalizator.Common;

namespace Veith.WPFLocalizator.Model
{
    public class ResxResourcesDictionariesFactory : IResourcesDictionariesFactory
    {
        private readonly IFileSystemWrapper fileSystem;

        public ResxResourcesDictionariesFactory(IFileSystemWrapper fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public IResourcesDictionary CreateDictionaryFromFile(string filePath)
        {
            return new ResxResourcesDictionary(filePath, this.fileSystem);
        }
    }
}
