using GK.Template.Attributes;
using System.ComponentModel;

namespace GK.Template.Methods
{
    [Category("Random")]
    [FormatMethod(Name = "RandomFirstName",
        Aliasses = "FirstName",
        Example = "{RandomFirstName()} {RandomFirstName} {FirstName}")]
    [Description("Return a random First name")]
    public sealed class MethodRandomFirstName : MethodBase
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
                items = Resources.RandomResources.FirstNames.Split(new char[] { ',' });

            int rnd = RandomInt(0, items.Length - 1);
            return items[rnd];
        }
    }
}