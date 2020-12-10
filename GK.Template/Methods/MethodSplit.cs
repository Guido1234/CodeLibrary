using GK.Template.Attributes;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GK.Template.Methods
{
    [FormatMethod(Name = "Split",
        Example = "{0:Split(\",\", 1, true)}\r\n{0:Split(\",\", 0, false, '\"')}\r\n\r\nData:\r\n\"aaa, bbb\",ccc")]
    [Description("Splits an item an returns the item by the given Index.")]
    public sealed class MethodSplit : MethodBase
    {
        [FormatMethodParameter(Optional = true, Order = 4)]
        [Description("Splitter will not be applied between escapes")]
        public string EscapeEnd { get; set; }

        [FormatMethodParameter(Optional = true, Order = 3)]
        [Description("Splitter will not be applied between escapes, if only EscapeStart supplied, it will be the escape end as well.")]
        public string EscapeStart { get; set; }

        [FormatMethodParameter(Optional = false, Order = 1)]
        [Description("Index of splitted item to return.")]
        public int Index { get; set; }

        [FormatMethodParameter(Optional = false, Order = 2)]
        [Description("Determines whether the index is in reverse (last item = 0)")]
        public bool Reverse { get; set; }

        [FormatMethodParameter(Optional = false, Order = 0)]
        [Description("Separator to use")]
        public string Separator { get; set; }

        public override string Apply(string value)
        {
            string[] items = null;

            if (!string.IsNullOrEmpty(EscapeStart) && !string.IsNullOrEmpty(EscapeEnd))
            {
                items = SplitEscaped(value, Separator[0], EscapeStart[0], EscapeEnd[0]);
            }
            if (!string.IsNullOrEmpty(EscapeStart))
            {
                if (items == null)
                    items = SplitEscaped(value, Separator[0], EscapeStart[0], EscapeStart[0]);
            }

            if (items == null)
                items = value.Split(new char[] { Separator[0] }, System.StringSplitOptions.None);

            if (Index >= items.Length || Index < 0)
                return string.Empty;

            if (!Reverse)
                return items[Index];
            else
                return items[items.Length - 1 - Index];
        }

        private static string[] SplitEscaped(string s, char separator, char escapeStart, char escapeEnd)
        {
            if (string.IsNullOrEmpty(s))
                return new string[0];

            var _result = new List<string>();
            var _partBuilder = new StringBuilder();
            var _textCharArray = s.ToCharArray();
            var _escaped = false;
            var _prevChar = (char)0;

            for (int ii = 0; ii < _textCharArray.Length; ii++)
            {
                char _charCurrent = _textCharArray[ii];
                if (_charCurrent == escapeStart && _prevChar != '\\')
                    _escaped = true;

                if (_charCurrent == escapeEnd && _prevChar != '\\')
                    _escaped = false;

                if (_charCurrent == separator && _escaped == false)
                {
                    _result.Add(_partBuilder.ToString());
                    _partBuilder = new StringBuilder();
                    continue;
                }

                _partBuilder.Append(_charCurrent);
                _prevChar = _charCurrent;
            }

            _result.Add(_partBuilder.ToString());
            return _result.ToArray();
        }
    }
}