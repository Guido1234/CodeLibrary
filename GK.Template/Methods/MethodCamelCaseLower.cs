using GK.Template.Attributes;
using System.ComponentModel;

namespace GK.Template.Methods
{
    [Category("Casing")]
    [FormatMethod(Name = "CamelLowerCase",
        Aliasses = "CLC",
        Example = "{0:CamelLowerCase()}")]
    [Description("Coverts item to úpper camel case. 'word word' will be 'WordWord")]
    public sealed class MethodCamelCaseLower : MethodBase
    {
        public override string Apply(string value)
        {
            return Utils.CamelCaseLower(value);
        }
    }
}