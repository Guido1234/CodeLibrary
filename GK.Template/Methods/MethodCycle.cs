using GK.Template.Attributes;
using System.ComponentModel;

namespace GK.Template.Methods
{
    [Category("Logic")]
    [FormatMethod(Name = "Cycle",
        Example = "{0:Cycle(\"A\",\"B\")}")]
    [Description("Cycle to the switches for each row")]
    public sealed class MethodCycle : MethodBase
    {
        private int cycle = 0;

        [FormatMethodParameter(Optional = false, Order = 0)]
        public string[] Switches { get; set; }

        public override string Apply(string value)
        {
            if (cycle >= Switches.Length)
                cycle = 0;

            string ret = Utils.ArrayValue(Switches, cycle);
            cycle++;
            return ret;
        }
    }
}