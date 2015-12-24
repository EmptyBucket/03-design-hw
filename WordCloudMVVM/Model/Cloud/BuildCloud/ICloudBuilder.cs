using System.Collections.Generic;
using System.Windows.Media;

namespace WordCloudMVVM.Model.Cloud.BuildCloud
{
    public interface ICloudBuilder
    {
        Dictionary<WordStyle, Geometry> BuildWordsGeometry(IReadOnlyCollection<WordStyle> words, int imageWidth, int imageHeight, int maxFont);
    }
}
