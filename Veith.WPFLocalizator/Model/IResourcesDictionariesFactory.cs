namespace Veith.WPFLocalizator.Model
{
    public interface IResourcesDictionariesFactory
    {
        IResourcesDictionary CreateDictionaryFromFile(string filePath);
    }
}
