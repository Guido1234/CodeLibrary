using GK.Template.Attributes;
using System.ComponentModel;

namespace GK.Template.Methods
{
    [Category("Trimming")]
    [FormatMethod(Name = "TrimLengthRight", Aliasses = "TLR")]
    [Description("Trim the current value at the right with a specified length")]
    public sealed class MethodTrimLengthRight : MethodBase
    {
        [FormatMethodParameter(Optional = false, Order = 0)]
        public int Length { get; set; }

        public override string Apply(string value)
        {
            return TrimEndByLength(value, Length);
        }

        private string TrimEndByLength(string s, int length)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            string val = s;
            int endpos = 0;
            string value = s;

            endpos = val.Length - length;
            if (endpos < 0)
                endpos = 0;

            return value.Substring(0, endpos);
        }
    }
}