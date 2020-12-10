using CodeLibrary.PluginPack.Common;
using CodeLibrary.PluginPack.Forms;
using EditorPlugins.Core;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack
{
    [Description("Removes all lines not containing")]
    public class RemoveLinesNotContaining : IEditorPlugin
    {
        public string Category => "Lines";
        public string DisplayName => "Remove Lines Not Containing";
        public Guid Id => Guid.Parse("4e605db3-1807-4a6b-b818-27c1691301c2");
        public Image Image => null;
        public bool OmitResult { get; set; } = false;
        public Keys ShortcutKeys => Keys.None;

        public string NotContaining { get; set; }

        public void Apply(ISelInfo sel)
        {
            OmitResult = false;

            if (!Configure())
                OmitResult = true;

            string text = sel.SelectedText;
            string[] _lines = Utils.Lines(text);
            string[] _notContaining = Utils.Lines(NotContaining);

            StringBuilder sb = new StringBuilder();
            foreach (string line in _lines)
            {
                if (!Contains(line, _notContaining))
                    continue;

                sb.Append(line);
                sb.Append("\r\n");
            }
            sel.SelectedText = sb.ToString().TrimEnd(new char[] { '\r', '\n' });
        }

        private bool Contains(string line, string[] contains)
        {
            foreach (string item in contains)
            {
                if (line.Contains(item))
                    return true;
            }
            return false;
        }

        public bool Configure()
        {
            FormRemoveLinesContaining f = new FormRemoveLinesContaining();

            f.Containing = NotContaining;
            f.Title = "Remove Lines Containing";

            DialogResult dr = f.ShowDialog();
            if (dr == DialogResult.OK)
            {
                NotContaining = f.Containing;
                return true;
            }
            return false;
        }

    }
}