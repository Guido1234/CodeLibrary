using GK.Template.Attributes;
using System.ComponentModel;

namespace GK.Template.Methods
{
    [FormatMethod(Name = "Replace",
        Aliasses = "R",
        Example = "{0:Replace(\"X\",\"Y\")}\r\n{0:R(\"X\",\"Y\")}\r\n{0:Replace(\"X\",\"Y\",\"Z\", \"\")} ")]
    [Description("Find and replace within the current value")]
    public sealed class MethodReplace : MethodBase
    {
        [FormatMethodParameter(Optional = false, Order = 1)]
        [Description("Last parameter represents replace all others represents finds.")]
        public string[] Replace { get; set; }

        public override string Apply(string value)
        {
            if (Replace.Length < 2)
                return value;

            string replace = Replace[Replace.Length - 1];

            value = value.Replace("\r\n", "\\r\\n");

            for (int ii = 0; ii < Replace.Length - 1; ii++)
                value = value.Replace(Replace[ii], replace);

            return value.Replace("\\r\\n", "\r\n");
        }
    }
}