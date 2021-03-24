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
    [Description("Surrounds all lines with prefix and postfix in selection.")]
    public class SurroundLines : IEditorPlugin
    {
        public bool IsExtension => false;
        public string Category => "Lines";
        public string DisplayName => "Surround Lines";
        public Guid Id => Guid.Parse("bbc6fe3c-b845-40d3-ae86-46cd0fccdadb");
        public Image Image => null;
        public bool OmitResult { get; set; } = false;
        public string Postfix { get; set; }
        public string Prefix { get; set; }
        public Keys ShortcutKeys => Keys.None;

        public void Apply(ISelInfo sel)
        {
            string text = sel.SelectedText;
            OmitResult = false;

            if (!Configure())
                OmitResult = true;

            StringBuilder _sb = new StringBuilder();
            string[] _lines = Utils.Lines(text);

            for (int ii = 0; ii < _lines.Length; ii++)
            {
                _sb.Append(Prefix);
                _sb.Append(_lines[ii]);
                _sb.Append(Postfix);
                if (ii < _lines.Length - 1)
                    _sb.Append(Utils._CRLF);
            }

            sel.SelectedText = _sb.ToString();
        }

        public bool Configure()
        {
            FormSurroundLines f = new FormSurroundLines();

            f.Postfix = Postfix;
            f.Prefix = Prefix;

            DialogResult dr = f.ShowDialog();
            if (dr == DialogResult.OK)
            {
                Postfix = f.Postfix;
                Prefix = f.Prefix;
                return true;
            }
            return false;
        }
    }
}