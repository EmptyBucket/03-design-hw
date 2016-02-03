using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordCloudMVVM.Model.WordInspector;

namespace WordCloudTest
{
    [TestClass]
    public class BadWordInspectorTest
    {
        private readonly BadWordInspector _badWordInspector;
        private readonly string _badWords;

        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public BadWordInspectorTest()
        {
            _badWords = "о он его него ему нему его им ним нём она её".Replace(" ", Environment.NewLine);

            using (var memoryStream = GenerateStreamFromString(_badWords))
                _badWordInspector = new BadWordInspector(memoryStream);
        }

        [TestMethod]
        public void TextBadAndGoodWord_IsBad_TextGoodWord()
        {
            var lines = _badWords
                .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();
            lines.Add("пошел");
            const string exceptWord = "пошел";
            var actualWord = lines.Where(line => !_badWordInspector.IsBad(line)).ToList();
            Assert.IsTrue(actualWord.Count == 1);
            Assert.AreEqual(exceptWord, actualWord[0]);
        }
    }
}
