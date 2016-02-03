using System;
using WordCloudMVVM.Model.WordParse.Clean;

namespace WordCloudMVVM.Model.WordParse.Token
{
    public class Tokenizer : ITokenizer
    {
        private readonly ICleaner _cleaner;

        public virtual string[] Tokenize(string text)
        {
            return _cleaner
                .Clean(text)
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public Tokenizer(ICleaner cleaner)
        {
            _cleaner = cleaner;
        }
    }
}
