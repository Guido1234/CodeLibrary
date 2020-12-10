using GK.Template.Attributes;
using System.ComponentModel;

namespace GK.Template.Methods
{
    [Category("Random")]
    [FormatMethod(Name = "RandomItem",
        Aliasses = "RndItem",
        Example = "{RandomItem(\"true\", \"false\")}  {RndItem(\"true\", \"false\")}")]
    [Description("Randomly returns one of the items")]
    public sealed class MethodRandomItem : MethodBase
    {
        private static int seed = System.DateTime.Now.Millisecond;

        [FormatMethodParameter(Optional = false, Order = 0)]
        public string[] Items { get; set; }

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
            if (Items == null)
                return string.Empty;

            if (Items.Length == 0)
                return string.Empty;

            int ii = RandomInt(0, Items.Length - 1);
            return Items[ii];
        }
    }
}