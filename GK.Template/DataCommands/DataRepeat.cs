using GK.Template.Attributes;
using System.ComponentModel;
using System.Text;

namespace GK.Template.DataCommands
{
    [DataCommand(Name = "Repeat", Example = "Repeat(10)")]
    [Description("Repeats a template X times. index placeholders like {0} are not functional, only placeholder with function which do not relly on the index are usefull like the Random function and LineNumber.")]
    public sealed class DataCommandRepeat : DataCommandBase
    {
        [DataCommandParameter(Order = 0)]
        public int Count { get; set; }

        public override string Execute(StringTemplateItem template)
        {
            StringBuilder sb = new StringBuilder();
            for (int ii = 0; ii < Count; ii++)
            {
                template.IsLastCommand = ((ii == Count - 1) && this.IsLastCommand);
                sb.Append(template.Format(ii.ToString(), this.Data, new string[0]));
            }
            return sb.ToString();
        }
    }
}