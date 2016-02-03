using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WordCloudMVVM.Model.Word;
using WordCloudMVVM.Model.WordParse;
using WordCloudMVVM.Model.WordParse.Token;

namespace WordCloudTest
{
    [TestClass]
    public class WordCountParserTest
    {
        private readonly WordCountParser _parser;

        public WordCountParserTest()
        {
            var exceptedTokenize = new[]
            {
                "свойственный",
                "состаревшемуся",
                "двор",
                "свет",
                "и",
                "пря",
                "двор"
            };
            var stemTokenizer = Mock.Of<ITokenizer>(tokenizer => tokenizer.Tokenize(It.IsAny<string>()) == exceptedTokenize);

            _parser = new WordCountParser(stemTokenizer);
        }

        [TestMethod]
        public void TextNewLineWord_Parse_CorrectEnumWordWeight()
        {
            var newLine = Environment.NewLine;
            string textNewLineWord = $"свойственны{newLine}состаревшемуся{newLine}дворе{newLine}свете{newLine}и{newLine}при{newLine}дворе";

            var wordWeightEnum = _parser.Parse(textNewLineWord);
            var except = new WordWeight("свойственный", 1);
            var actual = wordWeightEnum.First(wordWeight => wordWeight.Say == "свойственный");
            Assert.AreEqual(except.Say, actual.Say);
            Assert.AreEqual(except.Weight, actual.Weight);
            except = new WordWeight("свет", 1);
            actual = wordWeightEnum.First(wordWeight => wordWeight.Say == "свет");
            Assert.AreEqual(except.Say, actual.Say);
            Assert.AreEqual(except.Weight, actual.Weight);
            except = new WordWeight("двор", 2);
            actual = wordWeightEnum.First(wordWeight => wordWeight.Say == "двор");
            Assert.AreEqual(except.Say, actual.Say);
            Assert.AreEqual(except.Weight, actual.Weight);
        }

        [TestMethod]
        public void TextNewLineWord_Parse_CorrectCoutElement()
        {
            var newLine = Environment.NewLine;
            string textNewLineWord = $"свойственны{newLine}состаревшемуся{newLine}дворе{newLine}свете{newLine}и{newLine}при{newLine}дворе";
            var wordWeightEnum = _parser.Parse(textNewLineWord);
            var actualCount = wordWeightEnum.Length;
            Assert.AreEqual(6, actualCount);
        }

        [TestMethod]
        public void TextLiterature_Parse_CorrectCoutElement()
        {
            const string literText = "которые,./[]  свойственны 1234567890-=<>?:  ;\',.состаревшемуся  @&%(дворе  свете !@#$%^&*( + )()!и  при  дворе";

            var wordWeightEnum = _parser.Parse(literText);
            var actualCount = wordWeightEnum.Length;
            Assert.AreEqual(6, actualCount);
        }

        [TestMethod]
        public void TextLiterature_Parse_CorrectEnumWordWeight()
        {
            const string literText = "которые,./[]  свойственны 1234567890-=<>?:  ;\',.состаревшемуся  @&%(дворе  свете !@#$%^&*( + )()!и  при  дворе";
            var wordWeightEnum = _parser.Parse(literText);
            var except = new WordWeight("свойственный", 1);
            var actual = wordWeightEnum.First(wordWeight => wordWeight.Say == "свойственный");
            Assert.AreEqual(except.Say, actual.Say);
            Assert.AreEqual(except.Weight, actual.Weight);
            except = new WordWeight("свет", 1);
            actual = wordWeightEnum.First(wordWeight => wordWeight.Say == "свет");
            Assert.AreEqual(except.Say, actual.Say);
            Assert.AreEqual(except.Weight, actual.Weight);
            except = new WordWeight("двор", 2);
            actual = wordWeightEnum.First(wordWeight => wordWeight.Say == "двор");
            Assert.AreEqual(except.Say, actual.Say);
            Assert.AreEqual(except.Weight, actual.Weight);
        }

        [TestMethod]
        public void Text_GetUniqueWord_UniqueWord()
        {
            var text = new[] { "свойственны", "дворе", "свойственны", "дворе", "свойственны" };
            var exceptUniqueWord = new HashSet<string> { "дворе", "свойственны" };
            var actualUniqueWord = _parser.GetAllUniqueWords(text);
            Assert.IsTrue(exceptUniqueWord.All(word => actualUniqueWord.Contains(word)));
            Assert.AreEqual(actualUniqueWord.Count, exceptUniqueWord.Count);
        }
    }
}
