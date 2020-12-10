using DevToys;
using GK.Template.Attributes;
using System.ComponentModel;

namespace GK.Template.Methods
{
    [FormatMethod(Name = "HtmlDecode",
        Aliasses = "",
        Example = "{0:HtmlDecode()}")]
    [Description("Decodes Html Characters")]
    public sealed class MethodHtmlDecode : MethodBase
    {
        public override string Apply(string value)
        {
            return HtmlChar.HtmlSmartEncode(value, HtmlChar.HtmlEncoding.AsciiEncoding);
        }
    }
}