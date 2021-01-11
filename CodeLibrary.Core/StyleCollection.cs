using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
 
namespace CodeLibrary.Core
{
    public class StyleCollection
    {
        public List<RtfControlStyle> Styles = new List<RtfControlStyle>();

        private static StyleCollection _Instance;

        public static StyleCollection Instance => _Instance ?? (_Instance = new StyleCollection());

        private StyleCollection()
        {
            Styles.Add(new RtfControlStyle() { StyleName = "Header 1", FontFamily = "Arial", FontStyle = FontStyle.Bold, FontSize = 18, HorizontalAlignment = HorizontalAlignment.Left });
            Styles.Add(new RtfControlStyle() { StyleName = "Header 2", FontFamily = "Arial", FontStyle = FontStyle.Bold, FontSize = 14, HorizontalAlignment = HorizontalAlignment.Left });
            Styles.Add(new RtfControlStyle() { StyleName = "Header 3", FontFamily = "Arial", FontStyle = FontStyle.Bold, FontSize = 12, HorizontalAlignment = HorizontalAlignment.Left });
            Styles.Add(new RtfControlStyle() { StyleName = "Regular Text 1", FontFamily = "Arial", FontStyle = FontStyle.Regular, FontSize = 10, HorizontalAlignment = HorizontalAlignment.Left });
        }

        public void Deserialize(string json)
        {
            Styles.Clear();
            Styles.AddRange(Utils.FromJsonToList<RtfControlStyle>(json));
        }

        public void Load(string file)
        {
            if (!File.Exists(file))
                return;

            string _json = File.ReadAllText(file);
            Deserialize(_json);
        }

        public void Save(string file)
        {
            if (File.Exists(file))
                File.Delete(file);

            string _json = Serialize();
            File.WriteAllText(file, _json);
        }

        public string Serialize() => Utils.ToJson(Styles);
    }
}