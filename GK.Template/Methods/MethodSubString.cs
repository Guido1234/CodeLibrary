using GK.Template.Attributes;
using System.ComponentModel;

namespace GK.Template.Methods
{
    [FormatMethod(Name = "SubString", Aliasses = "")]
    [Description("Get a substring from the current value")]
    public sealed class MethodSubString : MethodBase
    {
        [FormatMethodParameter(Optional = false, Order = 1)]
        public int Length { get; set; }

        [FormatMethodParameter(Optional = false, Order = 0)]
        public int Start { get; set; }

        public static string SubString(string s, int index, int length)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            string val = s;
            if (val == null)
                return string.Empty;

            if (length <= 0)
                return string.Empty;

            if (index > s.Length)
                return string.Empty;

            if (index < 0)
            {
                if ((index + length) > 0)
                {
                    length = (index + length);
                    index = 0;
                }
                else
                {
                    return string.Empty;
                }
            }
            int len = length;
            string value = s;

            if ((index + len) > s.Length)
                len = s.Length - index;

            val = s.Substring(index, len);
            return val;
        }

        public override string Apply(string value)
        {
            return SubString(value, Start, Length);
        }
    }
}