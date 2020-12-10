using GK.Template.Attributes;
using System.ComponentModel;

namespace GK.Template.Methods
{
    public enum IfMode
    {
        IsOneOf = 1,
        NotIsOneOf = 2,
        ContainsOneOf = 3,
        NotContainsOneOf = 4
    }

    [Category("Logic")]
    [FormatMethod(Name = "IF", Example = "{0:IF(\"a\", \"b\"),Skip(\"Equals\", \"a\", \"c\", \"d\")}")]
    [Description("When criteria is met following formatters will be applied, if criteria is not met, formatters will be applied till IF.")]
    public sealed class MethodIF : MethodBase
    {
        public MethodIF()
        {
        }

        [FormatMethodParameter(Optional = false, Order = 1)]
        public string[] Match { get; set; }

        [FormatMethodParameter(Optional = false, Order = 0)]
        public IfMode Mode { get; set; }

        public override string Apply(string value)
        {
            bool match = false;

            switch (Mode)
            {
                case IfMode.IsOneOf:
                    foreach (string m in Match)
                    {
                        if (m.Equals(value))
                            match = true;
                    }

                    if (!match)
                        return null;

                    break;

                case IfMode.NotIsOneOf:

                    break;

                case IfMode.ContainsOneOf:

                    break;

                case IfMode.NotContainsOneOf:

                    break;
            }

            return value;
        }
    }
}