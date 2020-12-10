using GK.Template.Attributes;
using System.ComponentModel;

namespace GK.Template.Methods
{
    [FormatMethod(Name = "Reverse",
        Aliasses = "Rev",
        Example = "{0:Reverse()} {0:Reverse} {0:Rev}")]
    [Description("Reverses the value.")]
    public sealed class MethodReverse : MethodBase
    {
        public override string Apply(string value)
        {
            return Reverse(value);
        }

        internal static string Reverse(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] source = s.ToCharArray();
            char[] target = new char[source.Length];
            for (int ii = 0; ii < source.Length; ii++)
            {
                target[(source.Length - 1) - ii] = source[ii];
            }
            return new string(target);
        }
    }
}