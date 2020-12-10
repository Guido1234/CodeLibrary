using GK.Template.Attributes;
using System;
using System.ComponentModel;

namespace GK.Template.Methods
{
    [Category("Guid")]
    [FormatMethod(Name = "Guid", Example = "{Guid()}")]
    public sealed class MethodGuid : MethodBase
    {
        public override string Apply(string value)
        {
            return Guid.NewGuid().ToString();
        }
    }
}