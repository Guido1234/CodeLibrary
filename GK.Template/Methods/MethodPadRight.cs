using GK.Template.Attributes;
using System.ComponentModel;

namespace GK.Template.Methods
{
    [FormatMethod(Name = "PadRight",
        Aliasses = "PR",
        Example = "{0:PadRight(4, \"0\")} {0:PR(4, \"0\")}")]
    [Description("Add a number of padding characters to the right of value.")]
    public sealed class MethodPadRight : MethodBase
    {
        public MethodPadRight()
        {
            PaddingChar = ' ';
        }

        [FormatMethodParameter(Optional = false, Order = 0)]
        public int Length { get; set; }

        [FormatMethodParameter(Optional = true, Order = 1)]
        public char PaddingChar { get; set; }

        public override string Apply(string value)
        {
            return value.PadRight(Length, PaddingChar);
        }
    }
}