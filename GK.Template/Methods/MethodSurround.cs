using GK.Template.Attributes;
using System;
using System.ComponentModel;
using static GK.Template.Utils;

namespace GK.Template.Methods
{
    [FormatMethod(Name = "Surround", Aliasses = "")]
    [Description("Surround a value with a Postfix and/or Prefix or set a default when the value is empty. these surrounding can be applied depending on modes, the modes can be first|last|middle|default. These modes are usefull when you want to create a comma separated string and ignore the last item.")]
    public sealed class MethodSurround : MethodBase
    {
        [FormatMethodParameter(Optional = false, Order = 3)]
        public string Default { get; set; }

        [FormatMethodParameter(Optional = false, Order = 2)]
        [Description("first|last|middle|default")]
        public string Modes { get; set; }

        [FormatMethodParameter(Optional = false, Order = 1)]
        public string Postfix { get; set; }

        [FormatMethodParameter(Optional = false, Order = 0)]
        public string Prefix { get; set; }

        public override string Apply(string value)
        {
            if (string.IsNullOrEmpty(Modes))
            {
                if (string.IsNullOrEmpty(value))
                    value = string.Empty;
                return string.Format("{0}{1}{2}", Prefix, value, Postfix);
            }

            if (Utils.Contains(Modes, ContainsMode.All, StringComparison.OrdinalIgnoreCase, "default"))
            {
                if (string.IsNullOrEmpty(value))
                    return Default;
            }
            if (Utils.Contains(Modes, ContainsMode.All, StringComparison.OrdinalIgnoreCase, "first"))
            {
                if (this.IsFirstLine)
                    return string.Format("{0}{1}{2}", Prefix, value, Postfix);
            }
            if (Utils.Contains(Modes, ContainsMode.All, StringComparison.OrdinalIgnoreCase, "last"))
            {
                if (this.IsLastLine)
                    return string.Format("{0}{1}{2}", Prefix, value, Postfix);
            }
            if (Utils.Contains(Modes, ContainsMode.All, StringComparison.OrdinalIgnoreCase, "middle"))
            {
                if ((!this.IsLastLine && !this.IsFirstLine) || (this.IsLastLine && this.IsFirstLine))
                    return string.Format("{0}{1}{2}", Prefix, value, Postfix);
            }

            return value;
        }
    }
}