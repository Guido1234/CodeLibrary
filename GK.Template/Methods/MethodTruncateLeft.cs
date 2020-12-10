using GK.Template.Attributes;
using System.ComponentModel;

namespace GK.Template.Methods
{
    [Category("Trimming")]
    [FormatMethod(Name = "TruncateLeft", Aliasses = "TL")]
    public sealed class MethodTruncateLeft : MethodBase
    {
        [FormatMethodParameter(Optional = false, Order = 0)]
        public int Length { get; set; }

        public static string TruncateLeft(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            if (maxLength <= 0)
                return string.Empty;

            int len = maxLength;

            if ((len) > text.Length)
                len = text.Length;

            string srev = MethodReverse.Reverse(text);
            srev = srev.Substring(0, len);
            return MethodReverse.Reverse(srev);
        }

        public override string Apply(string value)
        {
            return TruncateLeft(value, Length);
        }
    }
}