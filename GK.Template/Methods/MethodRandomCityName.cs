using GK.Template.Attributes;
using System.ComponentModel;

namespace GK.Template.Methods
{
    [Category("Random")]
    [FormatMethod(Name = "RandomCityName",
        Aliasses = "CityName",
        Example = "{RandomCityName()} {CityName()}")]
    [Description("Return a random City Name")]
    public sealed class MethodRandomCityName : MethodBase
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
                items = Resources.RandomResources.CityNames.Split(new char[] { ',' });

            int rnd = RandomInt(0, items.Length - 1);
            return items[rnd];
        }
    }
}