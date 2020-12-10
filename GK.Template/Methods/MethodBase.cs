namespace GK.Template.Methods
{
    public abstract class MethodBase
    {
        public MethodBase()
        {
            SkipData = false;
        }

        public bool IsFirstLine
        {
            get
            {
                return (LineNumber == 0);
            }
        }

        public bool IsLastLine { get; set; }

        /// <summary>
        /// Indicates current line number
        /// </summary>
        internal int LineNumber { get; set; }

        /// <summary>
        /// Indicates Data should be skipped.
        /// </summary>
        internal bool SkipData { get; set; }

        internal string[] Templates { get; set; }

        public virtual string Apply(string value)
        {
            return value;
        }
    }
}