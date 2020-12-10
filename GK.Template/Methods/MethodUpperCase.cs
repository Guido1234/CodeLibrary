using GK.Template.Attributes;
using System.ComponentModel;

namespace GK.Template.Methods
{
    [Category("Casing")]
    [FormatMethod(Name = "UpperCase", Aliasses = "UC")]
    [Description("Convert the value to upper case")]
    public sealed class MethodUpperCase : MethodBase
    {
        public override string Apply(string value)
        {
            return value.ToUpper();
        }
    }
}