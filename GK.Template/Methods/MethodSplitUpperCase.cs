using GK.Template.Attributes;
using System;
using System.ComponentModel;
using System.Text;

namespace GK.Template.Methods
{
    [Category("Casing")]
    [FormatMethod(Name = "SplitUpperCase", Aliasses = "SUC")]
    [Description("Split by UpperCase")]
    public sealed class MethodSplitUpperCase : MethodBase
    {
        public override string Apply(string value)
        {
            bool _prevUpper = false;
            StringBuilder _sb = new StringBuilder();
            char[] _chars = value.ToCharArray();
            for (int ii = 0; ii < _chars.Length; ii++)
            {
                if (char.IsUpper(_chars[ii]))
                {                    
                    if (_prevUpper == false)
                    {
                        _sb.Append(' ');
                    }
                    char _next = (_chars[Bound(ii + 1, _chars.Length - 1)]);
                    bool _nextUpper = char.IsUpper(_next);

                    if (_prevUpper == true && _nextUpper == false)
                    {
                        _sb.Append(' ');
                    }

                    _sb.Append(_chars[ii]);
                    _prevUpper = true;
                    continue;
                }
                _sb.Append(_chars[ii]);
                _prevUpper = false;
            }

            return _sb.ToString().Trim();
        }

        private int Bound(int value, int max)
        {
            if (value > max)
            {
                return max;
            }
            return value;
        }
    }


}