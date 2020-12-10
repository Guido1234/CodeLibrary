using GK.Template.Attributes;
using System.ComponentModel;

namespace GK.Template.Methods
{
    [Category("Trimming")]
    [FormatMethod(Name = "TruncateRight", Aliasses = "TR")]
    public sealed class MethodTruncateRight : MethodBase
    {
        [FormatMethodParameter(Optional = false, Order = 0)]
        public int Length { get; set; }

        public override string Apply(string value)
        {
            return TruncateRight(value, Length);
        }

        internal static string TruncateRight(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            if (maxLength <= 0)
                return string.Empty;

            int len = maxLength;
            if ((len) > text.Length)
                len = text.Length;

            return text.Substring(0, len);
        }
    }
}