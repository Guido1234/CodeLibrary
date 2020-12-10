using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DevToys
{
    public sealed class CsvStreamReader : StreamReader
    {
        public CsvStreamReader(string path) : base(path, true)
        {
        }

        public CsvStreamReader(Stream stream) : base(stream, true)
        {
        }

        private enum State { First = 0, Normal = 1, Escaped = 2 }

        public bool EndOfCsvStream => (BaseStream.Position >= BaseStream.Length);

        public char Separator { get; set; } = ',';

        public string[] ReadCsvLine()
        {
            var _state = State.First;
            var _result = new List<string>();
            var _sb = new StringBuilder();

            while (true)
            {
                var _char = (char)BaseStream.ReadByte();
                if (_char == char.MaxValue || (_state == State.Normal && (_char == '\r' || _char == '\n')))
                {
                    _result.Add(_sb.ToString().Trim(new char[] { '"' }));
                    break;
                }
                if (_state == State.First)
                {
                    _state = State.Normal;
                    if (_char == '\n')
                        continue;
                }
                if (_state == State.Normal && _char == Separator)
                {
                    _result.Add(_sb.ToString().Trim(new char[] { '"' }));
                    _sb.Clear();
                    continue;
                }
                if (_char == '"')
                    _state = (_state == State.Normal) ? State.Escaped : State.Normal;

                _sb.Append(_char);
            }
            return _result.ToArray();
        }
    }
}