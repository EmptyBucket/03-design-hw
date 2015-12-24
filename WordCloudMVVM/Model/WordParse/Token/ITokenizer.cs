using System.Collections.Generic;

namespace WordCloudMVVM.Model.WordParse
{
    public interface ITokenizer
    {
        string[] Tokenize(string text);
    }
}
