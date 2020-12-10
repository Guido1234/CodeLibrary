namespace GK.Template.DataCommands
{
    public class DataCommandBase
    {
        public string Data { get; set; }
        public bool IsLastCommand { get; set; }
        public string Line { get; set; }
        public string[] LineValues { get; set; }
        internal StringTemplate StringTemplate { get; set; }

        internal int TemplateNumber { get; set; }

        public virtual string Execute(StringTemplateItem template)
        {
            return template.Format(Line, Data, LineValues);
        }
    }
}