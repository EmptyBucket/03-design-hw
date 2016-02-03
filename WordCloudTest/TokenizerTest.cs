using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WordCloudMVVM.Model.WordParse.Clean;
using WordCloudMVVM.Model.WordParse.Token;

namespace WordCloudTest
{
    [TestClass]
    public class TokenizerTest
    {
        private readonly Tokenizer _tokenizer;

        public TokenizerTest()
        {
            const string clearText = "которые свойственны состаревшемуся в свете и при дворе значительному человеку";
            var textCleaner = Mock.Of<ICleaner>(cleaner => cleaner.Clean(It.IsAny<string>()) == clearText);

            _tokenizer = new Tokenizer(textCleaner);
        }

        [TestMethod]
        public void Text_Tokenize_AllWord()
        {
            const string text = "которые,  свойственны  .состаревшемуся  в,  свете  и . при  дворе  ,значительному человеку";
            var actualWord = _tokenizer.Tokenize(text);
            var exceptWord = new[] { "которые", "свойственны", "состаревшемуся", "в", "свете", "и", "при", "дворе", "значительному", "человеку" };
            Assert.IsTrue(exceptWord.All(word => actualWord.Contains(word)));
        }
    }
}
