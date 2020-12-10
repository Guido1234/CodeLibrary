using GK.Template.Attributes;
using System.ComponentModel;

namespace GK.Template.Methods
{
    [FormatMethod(Name = "KeepQuoted",
        Aliasses = "",
        Example = "{0:KeepQuoted()}")]
    [Description("Remove all text except for text enclosed within double quotes")]
    public sealed class MethodKeepQuoted : MethodBase
    {
        public override string Apply(string value)
        {
            return Utils.KeepQuoted(value);
        }
    }
}