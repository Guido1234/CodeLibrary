namespace CodeLibrary.Core
{
    public class CodeBookmark
    {
        public string Description { get; set; }
        public int LineNumber { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Description} - {LineNumber}";
        }
    }
}