using System.Collections.Generic;

namespace Veith.WPFLocalizator.UserInteraction
{
    public interface ISelectingProjectWindow
    {
        string SelectProject(IEnumerable<string> projectsPaths);
    }
}
