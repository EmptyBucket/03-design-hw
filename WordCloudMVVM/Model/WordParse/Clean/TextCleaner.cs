using System.Text.RegularExpressions;

namespace WordCloudMVVM.Model.WordParse.Clean
{
    public class TextCleaner : ICleaner
    {
        public string Clean(string text)
        {
            const string punctuation = "’'\\[\\],(){}⟨⟩<>:‒…!.\\‐\\-?„“«»“”‘’‹›;1234567890_\\-+=\\/|@#$%^&*\"\r\n\t";
            string punctuationPattern = $"[{punctuation}]";
            var punctuationReg = new Regex(punctuationPattern);

            const string lotSpacePattern = " {2,}";
            var lotSpaceReg = new Regex(lotSpacePattern);

            var cleanPunctuationText = punctuationReg.Replace(text, " ");
            var cleanText = lotSpaceReg.Replace(cleanPunctuationText, " ");

            return cleanText;
        }
    }
}
