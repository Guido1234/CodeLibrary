using GK.Template.Attributes;
using System.ComponentModel;

namespace GK.Template.Methods
{
    [Category("Casing")]
    [FormatMethod(Name = "LowerCase",
        Aliasses = "LC",
        Example = "{0:LowerCase()} {0:LowerCase} {0:LC}")]
    [Description("Converts the value to lower case")]
    public sealed class MethodLowerCase : MethodBase
    {
        public override string Apply(string value)
        {
            return value.ToLower();
        }
    }
}