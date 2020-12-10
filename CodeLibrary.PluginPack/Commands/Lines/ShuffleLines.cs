using CodeLibrary.PluginPack.Common;
using EditorPlugins.Core;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack
{
    [Description("Shuffles all lines in selection.")]
    public class ShuffleLines : IEditorPlugin
    {
        public string Category => "Lines";
        public string DisplayName => "Shuffle Lines";
        public Guid Id => Guid.Parse("e9e3d1d5-ed28-4192-a62c-fc8d04dc9042");
        public Image Image => null;
        public bool OmitResult => false;
        public Keys ShortcutKeys => Keys.None;

        public void Apply(ISelInfo sel)
        {
            string text = sel.SelectedText;
            var lines = Utils.Lines(text).ToList();
            var shuffled = Utils.Shuffle(lines);
            sel.SelectedText = string.Join(Utils._CRLF, shuffled.ToArray());
        }

        public bool Configure()
        {
            return false;
        }
    }
}