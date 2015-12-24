﻿using System.Collections.Generic;
using System.Linq;
using WordCloudMVVM.Model.WordParse;

namespace WordCloudMVVM
{
    public class WordCountParser : IWordWeightParser
    {
        private readonly ITokenizer mTokenizer;

        public WordWeight[] Parse(string text)
        {
            string[] primaryWords = mTokenizer.Tokenize(text).ToArray();
            HashSet<string> uniquePrimaryWords = GetAllUniqueWords(primaryWords);

            Dictionary<string, int> dictCountUniqueWords = uniquePrimaryWords
                .ToDictionary(item => item, item => 0);

            foreach (var item in primaryWords)
                dictCountUniqueWords[item]++;

            return dictCountUniqueWords
                .Select(CountWord => new WordWeight(CountWord.Key, CountWord.Value))
                .ToArray();
        }

        public HashSet<string> GetAllUniqueWords(IReadOnlyCollection<string> words) =>
            new HashSet<string>(words);

        public WordCountParser(ITokenizer tokenizer)
        {
            mTokenizer = tokenizer;
        }
    }
}
