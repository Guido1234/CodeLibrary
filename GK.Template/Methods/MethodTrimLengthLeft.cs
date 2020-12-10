using GK.Template.Attributes;
using System.ComponentModel;

namespace GK.Template.Methods
{
    [Category("Trimming")]
    [FormatMethod(Name = "TrimLengthLeft", Aliasses = "TLL")]
    [Description("Trim the current value at the left with a specified length")]
    public sealed class MethodTrimLengthLeft : MethodBase
    {
        [FormatMethodParameter(Optional = false, Order = 0)]
        public int Length { get; set; }

        public static string TrimByLength(string value, int length)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            int endpos = 0;
            int startpos = 0;

            startpos = length;
            endpos = value.Length - startpos;
            if (endpos < 0)
                endpos = 0;

            if (startpos > value.Length)
                startpos = value.Length;

            return value.Substring(startpos, endpos);
        }

        public override string Apply(string value)
        {
            return TrimByLength(value, Length);
        }
    }
}