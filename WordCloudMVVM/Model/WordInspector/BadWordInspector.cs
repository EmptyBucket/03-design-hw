using System;
using System.Collections.Generic;
using System.IO;

namespace WordCloudMVVM.Model.WordInspector
{
    public class BadWordInspector : IWordInspector
    {
        private readonly HashSet<string> _badWords;

        public BadWordInspector(Stream stream)
        {
            using (var reader = new StreamReader(stream))
                _badWords = new HashSet<string>(
                    reader
                    .ReadToEnd()
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
        }

        public BadWordInspector(string path)
        {
            _badWords = new HashSet<string>(File.ReadAllLines(path));
        }

        public bool IsBad(string word) =>
            _badWords.Contains(word);
    }
}
