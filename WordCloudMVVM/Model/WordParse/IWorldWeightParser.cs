namespace WordCloudMVVM
{
    public interface IWordWeightParser
    {
        WordWeight[] Parse(string text);
    }
}
