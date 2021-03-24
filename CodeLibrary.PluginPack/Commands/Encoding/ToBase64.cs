using EditorPlugins.Core;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack
{
    [Description("Encodes selection to Base 64")]
    public class ToBase64 : IEditorPlugin
    {
        public bool IsExtension => false;
        public string Category => "Encoding";
        public string DisplayName => "To Base 64";
        public Guid Id => Guid.Parse("0d3dcd90-e748-413b-b462-0ce1f2b5d0a1");
        public Image Image => null;
        public bool OmitResult => false;
        public Keys ShortcutKeys => Keys.None;

        public void Apply(ISelInfo sel)
        {
            sel.SelectedText = toBase64(sel.SelectedText);
        }

        public bool Configure()
        {
            return false;
        }

        private static string toBase64(string text)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
        }
    }
}