using EditorPlugins.Core;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack
{
    [Description("Encodes selection from Base 64")]
    public class FromBase64 : IEditorPlugin
    {
        public string Category => "Encoding";
        public string DisplayName => "From Base 64";
        public Guid Id => Guid.Parse("fe576d20-face-4b22-8025-70583f9b3a8a");
        public Image Image => null;
        public bool OmitResult => false;
        public Keys ShortcutKeys => Keys.None;

        public void Apply(ISelInfo sel)
        {
            sel.SelectedText = fromBase64(sel.SelectedText);
        }

        public bool Configure()
        {
            return false;
        }

        private static string fromBase64(string text)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(text));
        }
    }
}