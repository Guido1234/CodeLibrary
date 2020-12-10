using GK.Template.Attributes;
using System.ComponentModel;

namespace GK.Template.Methods
{
    [Category("Logic")]
    [FormatMethod(Name = "LineNumber",
        Aliasses = "LN",
        Example = "{0:LineNumber()} {0:LineNumber} {0:LN}")]
    [Description("Returns the Line Number")]
    public sealed class MethodLineNumber : MethodBase
    {
        public MethodLineNumber()
        {
            Step = 1;
            Start = 0;
        }

        [FormatMethodParameter(Optional = true, Order = 2)]
        public int LeadingZero { get; set; }

        [FormatMethodParameter(Optional = true, Order = 0)]
        public int Start { get; set; }

        [FormatMethodParameter(Optional = true, Order = 1)]
        public int Step { get; set; }

        public override string Apply(string value)
        {
            string value1 = (Start + (LineNumber * Step)).ToString();
            string value2 = string.Format("{0}{1}", Repeat('0', LeadingZero), value1);
            return TrimByLength(value2, Min(value1.Length, LeadingZero));
        }

        private int Min(params int[] val)
        {
            if (val.Length == 0)
                return 0;

            int currentval = int.MaxValue;

            for (int ii = 0; ii < val.Length; ii++)
                if (val[ii] < currentval)
                    currentval = val[ii];

            return currentval;
        }

        private string Repeat(char c, int length)
        {
            if (length <= 0)
                return string.Empty;

            char[] chars = new char[length];
            for (int ii = 0; ii < length; ii++)
                chars[ii] = c;

            return new string(chars);
        }

        private string TrimByLength(string s, int length)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            int endpos = 0;
            int startpos = 0;
            string value = s;

            startpos = length;
            endpos = s.Length - startpos;
            if (endpos < 0)
                endpos = 0;

            if (startpos > value.Length)
                startpos = value.Length;

            return value.Substring(startpos, endpos);
        }
    }
}