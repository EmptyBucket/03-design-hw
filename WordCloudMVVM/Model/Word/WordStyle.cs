using System.Windows.Media;

namespace WordCloudMVVM.Model.Word
{
    public class WordStyle : WordFontSize
    {
        public Color Color { get; }

        public WordStyle(string word, int fontSize, Color color) : base(word, fontSize)
        {
            Color = color;
        }
    }
}
