using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NHunspell;
using WordCloudMVVM.Model.WordParse.Clean;
using WordCloudMVVM.Model.WordParse.Token;

namespace WordCloudTest
{
    [TestClass]
    public class StemTokenizerTest
    {
        private readonly StemTokenizer _stemTokenizer;
        private readonly string _text;

        public StemTokenizerTest()
        {
            _text = "свойственны дворе свете";
            var textCleaner = Mock.Of<ICleaner>(cleaner => cleaner.Clean(It.IsAny<string>()) == _text);

            var pathHunspellDict = Path.Combine(Environment.CurrentDirectory, "HunspellDictionary", "ru_RU.dic");
            var pathHunspellAff = Path.Combine(Environment.CurrentDirectory, "HunspellDictionary", "ru_RU.aff");

            var hunspell = new Hunspell(pathHunspellAff, pathHunspellDict);

            _stemTokenizer = new StemTokenizer(textCleaner, hunspell);
        }

        [TestMethod]
        public void Text_GetAllPrimaryWord_PrimaryWord()
        {
            var exceptWord = new[] { "свойственный", "свет", "двор" };
            var actualWord = _stemTokenizer.Tokenize(_text);
            Assert.IsTrue(exceptWord.All(word => actualWord.Contains(word)));
        }
    }
}
