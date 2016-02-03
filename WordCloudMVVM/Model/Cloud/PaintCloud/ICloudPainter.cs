using System.Collections.Generic;
using System.Windows.Media;
using WordCloudMVVM.Model.Word;

namespace WordCloudMVVM.Model.Cloud.PaintCloud
{
    public interface ICloudPainter
    {
        DrawingImage DrawCloudWord(IReadOnlyCollection<WordStyle> words, int imageWidht, int imageHeight, int maxFont);
    }
}
