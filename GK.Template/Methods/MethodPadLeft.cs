using GK.Template.Attributes;
using System.ComponentModel;

namespace GK.Template.Methods
{
    [FormatMethod(Name = "PadLeft",
        Aliasses = "PL",
        Example = "{0:PadLeft(4, \"0\")} {0:PL(4, \"0\")}")]
    [Description("Add a number of padding characters to the left of value.")]
    public sealed class MethodPadLeft : MethodBase
    {
        public MethodPadLeft()
        {
            PaddingChar = ' ';
        }

        [FormatMethodParameter(Optional = false, Order = 0)]
        public int Length { get; set; }

        [FormatMethodParameter(Optional = true, Order = 1)]
        public char PaddingChar { get; set; }

        public override string Apply(string value)
        {
            return value.PadLeft(Length, PaddingChar);
        }
    }
}