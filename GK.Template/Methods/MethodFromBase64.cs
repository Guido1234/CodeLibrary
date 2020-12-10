using GK.Template.Attributes;
using System;
using System.ComponentModel;
using System.Text;

namespace GK.Template.Methods
{
    [Category("Base 64")]
    [FormatMethod(Name = "FromBase64",
        Aliasses = "",
        Example = "{0:FromBase64()}")]
    [Description("Decodes Base64 text")]
    public sealed class MethodFromBase64 : MethodBase
    {
        public static string FromBase64(string text)
        {
            byte[] bytes = Convert.FromBase64String(text);
            return Encoding.Default.GetString(bytes);
        }

        public override string Apply(string value)
        {
            return FromBase64(value);
        }
    }
}