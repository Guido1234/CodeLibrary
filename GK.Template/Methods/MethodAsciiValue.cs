using GK.Template.Attributes;
using System.ComponentModel;

namespace GK.Template.Methods
{
    [FormatMethod(Name = "AsciiValue",
        Aliasses = "Ascii",
        Example = "{0:AsciiValue()}")]
    [Description("Returns ascii value for first letter.")]
    public sealed class MethodAsciiValue : MethodBase
    {
        public override string Apply(string value)
        {
            char[] ch = value.ToCharArray();
            if (ch.Length == 0)
                return string.Empty;

            return ((int)ch[0]).ToString();
        }
    }
}