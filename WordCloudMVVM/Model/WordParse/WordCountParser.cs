using System.Collections.Generic;
using System.Linq;
using WordCloudMVVM.Model.Word;
using WordCloudMVVM.Model.WordParse.Token;

namespace WordCloudMVVM.Model.WordParse
{
    public class WordCountParser : IWordWeightParser
    {
        private readonly ITokenizer _tokenizer;

        public WordWeight[] Parse(string text)
        {
            var primaryWords = _tokenizer.Tokenize(text).ToArray();
            var uniquePrimaryWords = GetAllUniqueWords(primaryWords);

            var dictCountUniqueWords = uniquePrimaryWords
                .ToDictionary(item => item, item => 0);

            foreach (var item in primaryWords)
                dictCountUniqueWords[item]++;

            return dictCountUniqueWords
                .Select(countWord => new WordWeight(countWord.Key, countWord.Value))
                .ToArray();
        }

        public HashSet<string> GetAllUniqueWords(IReadOnlyCollection<string> words) =>
            new HashSet<string>(words);

        public WordCountParser(ITokenizer tokenizer)
        {
            _tokenizer = tokenizer;
        }
    }
}
