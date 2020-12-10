using GK.Template.Attributes;
using System.ComponentModel;

namespace GK.Template.Methods
{
    [Category("Casing")]
    [FormatMethod(Name = "CamelUpperCase",
        Aliasses = "CUC",
        Example = "{0:CamelUpperCase()}")]
    [Description("Coverts item to úpper camel case. 'word word' will be 'WordWord")]
    public sealed class MethodCamelUpperCase : MethodBase
    {
        public override string Apply(string value)
        {
            return Utils.CamelCaseUpper(value);
        }
    }
}