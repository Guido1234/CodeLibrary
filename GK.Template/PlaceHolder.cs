namespace GK.Template
{
    /// <summary>
    /// Represents part of the template
    /// </summary>
    public class PlaceHolder
    {
        public PlaceHolder()
        {
            Text = string.Empty;
        }

        public PlaceHolder(string[] templates, string text)
        {
            Templates = templates;
            Text = text;
        }

        public string[] Templates { get; set; }

        public string Text { get; set; }

        public override string ToString()
        {
            return string.Format("[{0}] {1}", this.GetType().Name, Text);
        }
    }
}