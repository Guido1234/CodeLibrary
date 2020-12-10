using GK.Template.Attributes;
using System.ComponentModel;

namespace GK.Template.Methods
{
    [Category("Random")]
    [FormatMethod(Name = "RandomLastName",
        Aliasses = "LastName",
        Example = "{RandomLastName()} {LastName}")]
    [Description("Return a random Last Name")]
    public sealed class MethodRandomLastName : MethodBase
    {
        private static string[] items = new string[0];

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
            if (items.Length == 0)
                items = Resources.RandomResources.LastNames.Split(new char[] { ',' });

            int rnd = RandomInt(0, items.Length - 1);
            return items[rnd];
        }
    }
}