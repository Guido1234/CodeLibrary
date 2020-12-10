using GK.Template.Attributes;
using System.ComponentModel;
using System.Text;

namespace GK.Template.Methods
{
    [Category("Random")]
    [FormatMethod(Name = "RandomBSN",
        Aliasses = "",
        Example = "{RandomBSN()}")]
    [Description("Return a random valid (11 proof) Burger Service Number")]
    public sealed class MethodRandomBSN : MethodBase
    {
        private static int seed = System.DateTime.Now.Millisecond;

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
            return randomSOFINumber();
        }

        private string randomSOFINumber()
        {
            // (9 * A) +(8 * B) +(7 * C) +(6 * D) +(5 * E) +(4 * F) +(3 * G) +(2 * H) +(-1 * I)
            int result = 1;
            int[] vector = new int[] { 9, 8, 7, 6, 5, 4, 3, 2, -1 };
            int[] digits = new int[9];

            while (result != 0)
            {
                for (int ii = 0; ii < digits.Length; ii++)
                    digits[ii] = RandomInt(0, 9);

                int value = 0;
                for (int ii = 0; ii < digits.Length; ii++)
                    value = value + (vector[ii] * digits[ii]);
                result = value % 11;
            }

            StringBuilder sb = new StringBuilder();
            for (int ii = 0; ii < digits.Length; ii++)
                sb.Append(digits[ii].ToString());

            return sb.ToString();
        }
    }
}