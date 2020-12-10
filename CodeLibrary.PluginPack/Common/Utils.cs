using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeLibrary.PluginPack.Common
{
    internal static class Utils
    {
        /// <summary>
        /// \
        /// </summary>
        public const string _BACKSLASH = "\\";

        /// <summary>
        /// \r = CR (Carriage Return)
        /// </summary>
        public const string _CR = "\r";

        /// <summary>
        /// \n\r = CR + LF (Carriage Return + Line Feed)
        /// </summary>
        public const string _CRLF = "\r\n";

        /// <summary>
        /// "
        /// </summary>
        public const string _DOUBLEQUOTE = "\"";

        /// <summary>
        /// >
        /// </summary>
        public const string _GT = ">";

        /// <summary>
        ///  \n = LF (Line Feed)
        /// </summary>
        public const string _LF = "\n";

        /// <summary>
        /// &lt;
        /// </summary>
        public const string _LT = "<";

        /// <summary>
        /// "
        /// </summary>
        public const string _SINGLEQUOTE = "'";

        /// <summary>
        /// /
        /// </summary>
        public const string _SLASH = "/";

        /// <summary>
        /// Space
        /// </summary>
        public const string _SPACE = " ";

        /// <summary>
        /// \t = Tab
        /// </summary>
        public const string _TAB = "\t";

        private static Random _random = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// L force Int64
        /// M force Decimal
        /// D force Double
        /// F force Float/Single
        /// B force Byte
        /// I force Int16
        ///
        /// Default conversion chain: Guid, Boolean, Int32, Int64, Single, Double, Decimal, TimeSpan, DateTime, DateTimeOffset, String.
        /// Byte, Int16 only when enforced.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static object ConvertImplicit(string s)
        {
            bool _succes = false;

            if (s.EndsWith("L"))
            {
                _succes = Int64.TryParse(s.TrimEnd(new char[] { 'L' }), out Int64 _Int64L);
                if (_succes)
                    return _Int64L;
            }
            if (s.EndsWith("M"))
            {
                _succes = Decimal.TryParse(s.TrimEnd(new char[] { 'M' }), out Decimal _DecimalL);
                if (_succes)
                    return _DecimalL;
            }
            if (s.EndsWith("D"))
            {
                _succes = Double.TryParse(s.TrimEnd(new char[] { 'D' }), out Double _DoubleL);
                if (_succes)
                    return _DoubleL;
            }
            if (s.EndsWith("F"))
            {
                _succes = Single.TryParse(s.TrimEnd(new char[] { 'F' }), out Single _SingleL);
                if (_succes)
                    return _SingleL;
            }
            if (s.EndsWith("B"))
            {
                _succes = Byte.TryParse(s.TrimEnd(new char[] { 'B' }), out Byte _ByteL);
                if (_succes)
                    return _ByteL;
            }
            if (s.EndsWith("I"))
            {
                _succes = Int16.TryParse(s.TrimEnd(new char[] { 'I' }), out Int16 _Int16L);
                if (_succes)
                    return _Int16L;
            }

            _succes = Guid.TryParse(s, out Guid _Guid);
            if (_succes)
                return _Guid;

            _succes = Boolean.TryParse(s, out Boolean _Boolean);
            if (_succes)
                return _Boolean;

            _succes = Int32.TryParse(s, out Int32 _Int32);
            if (_succes)
                return _Int32;

            _succes = Int64.TryParse(s, out Int64 _Int64);
            if (_succes)
                return _Int64;

            _succes = Single.TryParse(s, out Single _Single);
            if (_succes)
                return _Single;

            _succes = Double.TryParse(s, out Double _Double);
            if (_succes)
                return _Double;

            _succes = Decimal.TryParse(s, out Decimal _Decimal);
            if (_succes)
                return _Decimal;

            _succes = TimeSpan.TryParse(s, out TimeSpan _TimeSpan);
            if (_succes)
                return _TimeSpan;

            _succes = DateTime.TryParse(s, out DateTime _DateTime);
            if (_succes)
                return _DateTime;

            _succes = DateTimeOffset.TryParse(s, out DateTimeOffset _DateTimeOffset);
            if (_succes)
                return _DateTimeOffset;

            return s;
        }

        public static string[] Lines(string s)
        {
            if (string.IsNullOrEmpty(s))
                return new string[0];

            s = NormalizeLineBreaks(s);

            return s.Split(new string[] { _CRLF }, StringSplitOptions.None);
        }

        public static string NormalizeLineBreaks(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            string var = s;

            var = var.Replace(_CRLF, "@!rn!@");
            var = var.Replace(_LF, _CRLF);
            var = var.Replace(_CRLF, "@!rn!@");
            var = var.Replace(_CR, _CRLF);
            var = var.Replace("@!rn!@", _CRLF);
            return var;
        }

        public static int Random(int min, int max)
        {
            if (min >= max)
                return 0;

            return _random.Next(min, max);
        }

        /// <summary>
        /// Shuffles a collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> Shuffle<T>(List<T> list)
        {
            List<T> result = new List<T>();
            result.AddRange(list);
            for (int ii = 0; ii < result.Count; ii++)
            {
                T swap = result[ii];
                int random = _random.Next(0, list.Count - 1);
                result[ii] = result[random];
                result[random] = swap;
            }
            return result;
        }

        public static string[] SortAsc(string[] array)
        {
            if (array == null)
            {
                return new string[0];
            }

            // sort variables based on length longest variables first to prevent half variable replace.
            IEnumerable<string> items = from item in array
                                        orderby item ascending
                                        select item;
            return items.ToArray();
        }

        public static string[] SortDesc(string[] array)
        {
            if (array == null)
            {
                return new string[0];
            }

            // sort variables based on length longest variables first to prevent half variable replace.
            IEnumerable<string> items = from item in array
                                        orderby item descending
                                        select item;
            return items.ToArray();
        }

        public static IEnumerable<string> SplitEscaped(string s, char separator, char escape)
        {
            if (string.IsNullOrEmpty(s))
                yield return string.Empty;

            if (!s.Contains(separator.ToString()))
                yield return s;

            Func<int, int, int, int> _bound = (value, lowbound, highbound) => (value > highbound) ? highbound : (value < lowbound) ? lowbound : value;

            StringBuilder _partBuilder = new StringBuilder();
            char[] _textCharArray = s.ToCharArray();
            char _charPrev = ' ';
            bool _isEscaped = false;

            for (int ii = 0; ii < _textCharArray.Length; ii++)
            {
                bool _skip = false;
                char _charCurrent = _textCharArray[ii];

                _isEscaped = (_isEscaped == false && _charCurrent == escape && _charPrev != '\\') ? true : (_charCurrent == escape && _charPrev != '\\') ? false : _isEscaped;

                if (_isEscaped == false && _charCurrent == separator)
                {
                    yield return _partBuilder.ToString();
                    _partBuilder = new StringBuilder();
                    _skip = true;
                }
                if (_charCurrent == '\\' && _textCharArray[_bound(ii + 1, 0, _textCharArray.Length - 1)] == escape)
                {
                    _charPrev = _charCurrent;
                    _skip = true;
                }
                if (!_skip)
                {
                    _charPrev = _charCurrent;
                    _partBuilder.Append(_charCurrent);
                }
            }
            yield return _partBuilder.ToString();
        }

        public static string StringFormatImplicit(string s, params string[] values)
        {
            object[] _objValues = new object[values.Length];
            for (int ii = 0; ii < _objValues.Length; ii++)
            {
                _objValues[ii] = ConvertImplicit(values[ii]);
            }
            return string.Format(s, _objValues);
        }
    }
}