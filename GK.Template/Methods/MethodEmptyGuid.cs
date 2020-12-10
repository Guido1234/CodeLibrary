using GK.Template.Attributes;
using System;
using System.ComponentModel;

namespace GK.Template.Methods
{
    [Category("Guid")]
    [FormatMethod(Name = "EmptyGuid",
        Example = "{EmptyGuid()}")]
    [Description("Return an Empty Guid")]
    public sealed class MethodEmptyGuid : MethodBase
    {
        public override string Apply(string value)
        {
            return Guid.Empty.ToString();
        }
    }
}