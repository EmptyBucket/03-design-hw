using System.IO;
using System.Text;

namespace WordCloudMVVM.Model.Read
{
    public class TxtReader : ITextReader
    {
        private readonly Encoding _encoding;

        public string ReadAll(Stream stream)
        {
            using (var reader = new StreamReader(stream, _encoding))
                return reader.ReadToEnd();
        }

        public string ReadAll(string path) =>
            File.ReadAllText(path);

        public TxtReader(Encoding encoding)
        {
            _encoding = encoding;
        }

        public TxtReader()
        {
            _encoding = Encoding.ASCII;
        }
    }
}
