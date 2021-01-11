namespace CodeLibrary.Controls
{
    public class Range
    {
        public int End => Start + Length;
        public int Length { get; set; }
        public int Start { get; set; }
    }
}