using CodeLibrary.PluginPack.Common;
using EditorPlugins.Core;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack
{
    [Description("Removes all empty lines from selection")]
    public class RemoveEmptyLines : IEditorPlugin
    {
        public string Category => "Lines";
        public string DisplayName => "Remove Empty Lines";
        public Guid Id => Guid.Parse("c5e4695e-9c5c-4615-b22f-dfd8eb50bf86");
        public Image Image => null;
        public bool OmitResult => false;
        public Keys ShortcutKeys => Keys.None;

        public void Apply(ISelInfo sel)
        {
            string text = sel.SelectedText;
            string[] _lines = Utils.Lines(text);
            StringBuilder sb = new StringBuilder();
            foreach (string line in _lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                sb.Append(line);
                sb.Append("\r\n");
            }
            sel.SelectedText = sb.ToString().TrimEnd(new char[] { '\r', '\n' });
        }

        public bool Configure()
        {
            return false;
        }
    }
}