using GK.Template.Attributes;
using System.ComponentModel;
using System.Text;

namespace GK.Template.Methods
{
    [Category("Random")]
    [FormatMethod(Name = "LoremIpsum",
        Aliasses = "LI",
        Example = "{0:LoremIpsum(50)} {0:LI(100)}")]
    [Description("Returns random Lorum Ipsum generated text.\r\n\r\nSample\r\nsapien sapien risus maximus vestibulum blandit Pellentesque interdum tellus nulla odio morbi potenti Ut elementum Etiam, aliquam litora nisi odio")]
    public sealed class MethodLoremIpsum : MethodBase
    {
        private static string[] words;

        public MethodLoremIpsum()
        {
            words = new string[] { "a", "ac", "accumsan", "ad", "adipiscing", "Aenean", "aliquam", "Aliquam", "aliquet", "amet", "ante", "aptent", "arcu", "at", "auctor", "augue", "bibendum", "blandit", "Class", "commodo", "condimentum", "congue", "consectetur", "consequat", "conubia", "convallis", "Cras", "cubilia", "Curabitur", "Curae;", "cursus", "dapibus", "diam", "dictum", "dictumst", "dignissim", "dis", "dolor", "Donec", "dui", "Duis", "efficitur", "egestas", "eget", "eleifend", "elementum", "elit", "enim", "erat", "eros", "est", "et", "Etiam", "eu", "euismod", "ex", "facilisi", "facilisis", "fames", "faucibus", "felis", "fermentum", "feugiat", "finibus", "fringilla", "Fusce", "gravida", "habitant", "habitasse", "hac", "hendrerit", "himenaeos", "iaculis", "id", "imperdiet", "in", "In", "inceptos", "Integer", "interdum", "Interdum", "ipsum", "justo", "lacinia", "lacus", "laoreet", "lectus", "leo", "libero", "ligula", "litora", "lobortis", "lorem", "Lorem", "luctus", "Maecenas", "magna", "magnis", "malesuada", "massa", "mattis", "mauris", "Mauris", "maximus", "metus", "mi", "molestie", "mollis", "montes", "morbi", "Morbi", "mus", "Nam", "nascetur", "natoque", "nec", "neque", "netus", "nibh", "nisi", "nisl", "non", "nostra", "nulla", "Nulla", "Nullam", "nunc", "Nunc", "odio", "orci", "Orci", "ornare", "parturient", "pellentesque", "Pellentesque", "penatibus", "per", "pharetra", "Phasellus", "placerat", "platea", "porta", "porttitor", "posuere", "potenti", "Praesent", "pretium", "primis", "Proin", "pulvinar", "purus", "quam", "quis", "Quisque", "rhoncus", "ridiculus", "risus", "rutrum", "sagittis", "sapien", "scelerisque", "sed", "Sed", "sem", "semper", "senectus", "sit", "sociosqu", "sodales", "sollicitudin", "suscipit", "Suspendisse", "taciti", "tellus", "tempor", "tempus", "tincidunt", "torquent", "tortor", "tristique", "turpis", "ullamcorper", "ultrices", "ultricies", "urna", "ut", "Ut", "varius", "vehicula", "vel", "velit", "venenatis", "vestibulum", "Vestibulum", "vitae", "Vivamus", "viverra", "volutpat", "vulputate" };
        }

        [Description("Number of words to use.")]
        [FormatMethodParameter(Optional = true, Order = 0)]
        public int WordCount { get; set; }

        public override string Apply(string value)
        {
            StringBuilder sb = new StringBuilder();
            string s = string.Empty;
            bool nextUp = false;

            for (int ii = 0; ii < WordCount; ii++)
            {
                string word = words[Utils.RandomInt(0, words.Length - 1)];
                if (nextUp)
                {
                    char[] chars = word.ToCharArray();
                    chars[0] = char.ToUpper(word[0]);
                    word = new string(chars);
                }
                else
                {
                    sb.Append(word);
                }
                nextUp = false;
                switch (Utils.RandomInt(0, 20))
                {
                    case 5:
                        sb.Append(", ");
                        break;

                    case 15:
                        sb.Append(". ");
                        nextUp = true;
                        break;

                    default:
                        sb.Append(" ");
                        break;
                }
            }
            return sb.ToString();
        }
    }
}