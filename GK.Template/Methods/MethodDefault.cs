using GK.Template.Attributes;
using System.ComponentModel;

namespace GK.Template.Methods
{
    [Category("Logic")]
    [FormatMethod(Name = "Default",
        Aliasses = "",
        Example = "{0:Default(\"unknown\")}")]
    [Description("Set a default for empty values")]
    public sealed class MethodDefault : MethodBase
    {
        [FormatMethodParameter(Optional = false, Order = 0)]
        [Description("Default value to use when empty.")]
        public string DefaultValue { get; set; }

        public override string Apply(string value)
        {
            if (string.IsNullOrEmpty(value))
                return DefaultValue;

            return value;
        }
    }
}