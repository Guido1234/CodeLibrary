using EditorPlugins.Core;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack
{
    [Description("Converts selection to Lowercase")]
    public class LowerCase : IEditorPlugin
    {
        public string Category => "Casing";
        public string DisplayName => "Lower Case";
        public Guid Id => Guid.Parse("376e2eb2-caf9-4478-ac7b-d0782b0b84d9");
        public Image Image => ImageResource.lowercase;
        public bool OmitResult => false;
        public Keys ShortcutKeys => (Keys.Control | Keys.L);

        public void Apply(ISelInfo sel)
        {
            sel.SelectedText = sel.SelectedText?.ToLower();
        }

        public bool Configure()
        {
            return false;
        }
    }
}