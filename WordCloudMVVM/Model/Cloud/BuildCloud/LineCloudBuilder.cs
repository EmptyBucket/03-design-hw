using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using WordCloudMVVM.Model.Cloud.BuildCloud.Intersection;
using WordCloudMVVM.Model.Word;

namespace WordCloudMVVM.Model.Cloud.BuildCloud
{
    public class LineCloudBuilder : ICloudBuilder
    {
        private readonly IIntersectionChecker _intersectionChecher;

        public LineCloudBuilder(IIntersectionChecker intersectionChecher)
        {
            _intersectionChecher = intersectionChecher;
        }

        public Dictionary<WordStyle, Geometry> BuildWordsGeometry(IReadOnlyCollection<WordStyle> words, int imageWidth, int imageHeight, int maxFont)
        {
            var sortWords = words.OrderByDescending(word => word.FontSize);

            var geometryWords = new Dictionary<WordStyle, Geometry>();

            using (var enumeratorWords = sortWords.GetEnumerator())
            {
                var prewLineGeometry = new List<Geometry>();
                for (double coordY = 0; coordY < imageHeight;)
                {
                    var currentLineGeometry = new List<Geometry>();
                    for (double coordX = 0; coordX < imageWidth && enumeratorWords.MoveNext();)
                    {
                        var word = enumeratorWords.Current;
                        var geometryWord = GeometryExtended.GetWordGeometry(word, new Point(coordX, coordY));

                        for (double offset = 1; _intersectionChecher.CheckIntersection(geometryWord, prewLineGeometry); offset++)
                            geometryWord = GeometryExtended.GetWordGeometry(word, new Point(coordX, coordY + offset));

                        geometryWords.Add(word, geometryWord);
                        currentLineGeometry.Add(geometryWord);

                        coordX = geometryWord.GetGeometryRight() + 1;
                    }
                    coordY = currentLineGeometry.Max(geometry => geometry.GetGeometryDown());
                    prewLineGeometry = currentLineGeometry.ToList();
                }
            }

            return geometryWords;
        }
    }
}
