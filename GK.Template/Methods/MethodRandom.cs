using GK.Template.Attributes;
using System.ComponentModel;

namespace GK.Template.Methods
{
    [Category("Random")]
    [FormatMethod(Name = "Random",
        Aliasses = "Rnd",
        Example = "{Random(0,100)} {Rnd(0,100)}")]
    [Description("Return a random number between Min and Max")]
    public sealed class MethodRandom : MethodBase
    {
        private static int seed = System.DateTime.Now.Millisecond;

        [FormatMethodParameter(Optional = true, Order = 2)]
        public int LeadingZero { get; set; }

        [FormatMethodParameter(Optional = false, Order = 1)]
        public int Max { get; set; }

        [FormatMethodParameter(Optional = false, Order = 0)]
        public int Min { get; set; }

        public static int RandomInt(int min, int max)
        {
            if (min >= max)
            {
                return 0;
            }
            System.Random rnd = new System.Random(seed);
            seed = rnd.Next();
            return rnd.Next(min, max);
        }

        public override string Apply(string value)
        {
            string value1 = RandomInt(Min, Max).ToString();
            string value2 = string.Format("{0}{1}", Repeat('0', LeadingZero), value1);
            return TrimByLength(value2, Minimal(value1.Length, LeadingZero));
        }

        private int Minimal(params int[] val)
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