using GK.Template.Attributes;
using System.ComponentModel;

namespace GK.Template.Methods
{
    [FormatMethod(Name = "Calculate",
        Aliasses = "Calc,Eval,Evaluate",
        Example = "{0:Eval()}")]
    [Description("Calculate")]
    public sealed class MethodCalc : MethodBase
    {
        public override string Apply(string value)
        {
            return Utils.EvaluateExpression(value).ToString();
        }
    }
}