namespace WordCloudMVVM.Model.Word
{
    public class Word : IWord
    {
        public string Say { get; }

        public Word(string word)
        {
            Say = word;
        }
    }
}
