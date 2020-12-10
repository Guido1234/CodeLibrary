using EditorPlugins.Core;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack
{
    [Description("Converts selection to uppercase")]
    public class UpperCase : IEditorPlugin
    {
        public string Category => "Casing";
        public string DisplayName => "Upper Case";
        public Guid Id => Guid.Parse("bc9c306c-66a0-48f9-8bc6-93f1c3121929");
        public Image Image => ImageResource.uppercase;
        public bool OmitResult => false;
        public Keys ShortcutKeys => (Keys.Control | Keys.U);

        public void Apply(ISelInfo sel)
        {
            sel.SelectedText = sel.SelectedText?.ToUpper();
        }

        public bool Configure()
        {
            return false;
        }
    }
}