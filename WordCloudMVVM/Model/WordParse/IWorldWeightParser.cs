using WordCloudMVVM.Model.Word;

namespace WordCloudMVVM.Model.WordParse
{
    public interface IWordWeightParser
    {
        WordWeight[] Parse(string text);
    }
}
