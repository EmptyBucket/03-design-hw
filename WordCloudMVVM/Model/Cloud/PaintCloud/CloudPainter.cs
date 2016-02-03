using System.Collections.Generic;
using System.Windows.Media;
using WordCloudMVVM.Model.Cloud.BuildCloud;
using WordCloudMVVM.Model.Word;

namespace WordCloudMVVM.Model.Cloud.PaintCloud
{
    public class CloudPainter : ICloudPainter
    {
        private readonly ICloudBuilder _cloudPainter;

        public CloudPainter(ICloudBuilder clouPainter)
        {
            _cloudPainter = clouPainter;
        }

        protected DrawingImage DrawingImage(Dictionary<WordStyle, Geometry> geomWordDict)
        {
            var visual = new DrawingVisual();
            using (var drawingContext = visual.RenderOpen())
                foreach (var item in geomWordDict)
                    drawingContext.DrawGeometry(new SolidColorBrush(item.Key.Color), null, item.Value);
            return new DrawingImage(visual.Drawing);
        }

        public DrawingImage DrawCloudWord(IReadOnlyCollection<WordStyle> words, int imageWidht, int imageHeight, int maxFont) =>
            DrawingImage(_cloudPainter.BuildWordsGeometry(words, imageWidht, imageHeight, maxFont));
    }
}
