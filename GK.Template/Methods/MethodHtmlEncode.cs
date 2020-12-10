using DevToys;
using GK.Template.Attributes;
using System.ComponentModel;

namespace GK.Template.Methods
{
    [FormatMethod(Name = "HtmlEncode",
        Aliasses = "",
        Example = "{0:HtmlEncode()}")]
    [Description("Encodes unencoded Html Characters")]
    public sealed class MethodHtmlEncode : MethodBase
    {
        public override string Apply(string value)
        {
            return HtmlChar.HtmlSmartEncode(value, HtmlChar.HtmlEncoding.HtmlNameEncoding);
        }
    }
}