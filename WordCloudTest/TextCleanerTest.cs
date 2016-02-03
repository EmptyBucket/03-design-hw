using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordCloudMVVM.Model.WordParse.Clean;

namespace WordCloudTest
{
    [TestClass]
    public class TextCleanerTest
    {
        private readonly TextCleaner _cleaner;

        public TextCleanerTest()
        {
            _cleaner = new TextCleaner();
        }

        [TestMethod]
        public void DirtyText_Clear_CleanText()
        {
            const string dirtyText = "’'[],(){}⟨⟩<>:‒…!.‐-?„“«»“”‘’‹qwe›;1234567890_-+=/|@#$%^&*\"\r\n\t";
            var cleanText = _cleaner.Clean(dirtyText);
            const string exceptCleanText = " qwe ";
            Assert.AreEqual(exceptCleanText, cleanText);
        }
    }
}
