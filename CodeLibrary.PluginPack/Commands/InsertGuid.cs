using EditorPlugins.Core;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack
{
    [Description("Insert a new Guid.")]
    public class InsertGuid : IEditorPlugin
    {
        public string Category => "";
        public string DisplayName => "Insert Guid";
        public Guid Id => Guid.Parse("88ba6eae-fa89-4275-8f92-020fbcf78a12");
        public Image Image => null;
        public bool OmitResult => false;
        public Keys ShortcutKeys => Keys.None;

        public void Apply(ISelInfo sel)
        {
            sel.SelectedText = Guid.NewGuid().ToString();
        }

        public bool Configure()
        {
            return false;
        }
    }
}