using GK.Template.Attributes;
using System.ComponentModel;

namespace GK.Template.Methods
{
    [Category("Trimming")]
    [FormatMethod(Name = "Trim")]
    [Description("Trim the current value.")]
    public sealed class MethodTrim : MethodBase
    {
        public MethodTrim()
        {
        }

        public override string Apply(string value)
        {
            return value.Trim();
        }
    }
}