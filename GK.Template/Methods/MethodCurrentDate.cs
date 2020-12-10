using GK.Template.Attributes;
using System;
using System.ComponentModel;

namespace GK.Template.Methods
{
    [FormatMethod(Name = "CurrentDate")]
    [Description("Returns the Current date.")]
    public sealed class MethodCurrentDate : MethodBase
    {
        [FormatMethodParameter(Optional = false, Order = 0)]
        public string Format { get; set; }

        public override string Apply(string value)
        {
            return DateTime.Now.ToString(Format);
        }
    }
}