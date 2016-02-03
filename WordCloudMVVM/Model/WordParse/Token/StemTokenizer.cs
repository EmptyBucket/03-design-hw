using System;
using System.Linq;
using NHunspell;
using WordCloudMVVM.Model.WordParse.Clean;

namespace WordCloudMVVM.Model.WordParse.Token
{
    public class StemTokenizer : Tokenizer, IDisposable
    {
        private readonly Hunspell _hunspell;

        public override string[] Tokenize(string text) =>
            base.Tokenize(text)
                .Select(word => _hunspell.Stem(word).FirstOrDefault() ?? word)
                .ToArray();

        public StemTokenizer(ICleaner cleaner, Hunspell hunspell) : base(cleaner)
        {
            _hunspell = hunspell;
        }

        public void Dispose() =>
            _hunspell.Dispose();
    }
}
