namespace WordCloudMVVM.Model.WordParse.Token
{
    public interface ITokenizer
    {
        string[] Tokenize(string text);
    }
}
