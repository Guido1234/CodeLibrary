using GK.Template.Attributes;
using System;
using System.ComponentModel;
using System.Text;

namespace GK.Template.Methods
{
    [Category("Base 64")]
    [FormatMethod(Name = "ToBase64",
        Aliasses = "",
        Example = "{0:ToBase64()}")]
    [Description("Encodes Base64 text")]
    public sealed class MethodToBase64 : MethodBase
    {
        public static string ToBase64(string text)
        {
            return Convert.ToBase64String(Encoding.Default.GetBytes(text));
        }

        public override string Apply(string value)
        {
            return ToBase64(value);
        }
    }
}