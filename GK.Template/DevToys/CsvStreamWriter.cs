using System.IO;
using System.Text;

namespace DevToys
{
    public sealed class CsvStreamWriter : StreamWriter
    {
        public CsvStreamWriter(string path) : base(path)
        {
        }

        public CsvStreamWriter(Stream stream) : base(stream)
        {
        }

        public char Separator { get; set; } = ',';

        public void WriteCsvLine(params string[] values)
        {
            var _sb = new StringBuilder().Append((BaseStream.Position > 0) ? "\r\n" : "");
            for (int ii = 0; ii < values.Length; ii++)
                _sb.Append(Esc(values[ii] ?? "")).Append(Separator);
            _sb.Length--;
            BaseStream.Write(Encoding.Default.GetBytes(_sb.ToString()), 0, _sb.Length);
        }

        private string Esc(string s) => (s.IndexOfAny(new char[] { '\r', '\n', '"', Separator }) == -1) ? s : $"\"{s.Replace("\"", "\"\"")}\"";
    }
}