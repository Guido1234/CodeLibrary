using GK.Template.Attributes;
using System.ComponentModel;

namespace GK.Template.Methods
{
    [Category("Hash")]
    [FormatMethod(Name = "HashCode",
        Aliasses = "Hash",
        Example = "{0:HashCode()}")]
    [Description("Gets the HashCode for the string")]
    public sealed class MethodHashCode : MethodBase
    {
        public override string Apply(string value)
        {
            if (value == null)
                return "0";
            return value.GetHashCode().ToString();
        }
    }
}