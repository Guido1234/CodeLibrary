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
    [Description("Removes all containing items.")]
    public class RemoveLinesContaining : IEditorPlugin
    {
        public bool IsExtension => false;
        public string Category => "Lines";
        public string Containing { get; set; }
        public string DisplayName => "Remove Lines Containing";
        public Guid Id => Guid.Parse("bfcb4446-67a6-4023-878b-29da1d47f8bc");
        public Image Image => null;
        public bool OmitResult { get; set; } = false;
        public Keys ShortcutKeys => Keys.None;

        public void Apply(ISelInfo sel)
        {
            OmitResult = false;

            if (!Configure())
                OmitResult = true;

            string text = sel.SelectedText;
            string[] _lines = Utils.Lines(text);
            string[] _containing = Utils.Lines(Containing);

            StringBuilder sb = new StringBuilder();
            foreach (string line in _lines)
            {
                if (Contains(line, _containing))
                    continue;

                sb.Append(line);
                sb.Append("\r\n");
            }
            sel.SelectedText = sb.ToString().TrimEnd(new char[] { '\r', '\n' });
        }

        //Range _line = _fastColoredTextBox.GetLine(_fastColoredTextBox.Selection.Start.iLine);
        //Place _start = new Place(0, _fastColoredTextBox.Selection.Start.iLine);
        //Place _end = new Place(_line.End.iChar, _fastColoredTextBox.Selection.Start.iLine);
        //_fastColoredTextBox.Selection = new Range(_fastColoredTextBox, _start, _end);

        public bool Configure()
        {
            FormRemoveLinesContaining f = new FormRemoveLinesContaining();

            f.Containing = Containing;
            f.Title = "Remove Lines Containing";

            DialogResult dr = f.ShowDialog();
            if (dr == DialogResult.OK)
            {
                Containing = f.Containing;
                return true;
            }
            return false;
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
    }
}