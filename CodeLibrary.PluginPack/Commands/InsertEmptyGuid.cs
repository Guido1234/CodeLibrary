using EditorPlugins.Core;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack
{
    [Description("Insert an empty Guid.")]
    public class InsertEmptyGuid : IEditorPlugin
    {
        public bool IsExtension => false;
        public string Category => "";

        public string DisplayName => "Insert Empty Guid";

        public Guid Id => Guid.Parse("7622652a-f12a-43d4-b3dc-d059a66256c0");

        public Image Image => null;

        public bool OmitResult => false;

        public Keys ShortcutKeys => Keys.None;

        public void Apply(ISelInfo sel)
        {
            sel.SelectedText = Guid.Empty.ToString();
        }

        public bool Configure()
        {
            return false;
        }
    }
}