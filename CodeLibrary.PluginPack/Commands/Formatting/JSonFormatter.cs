using CodeLibrary.PluginPack.Common;
using EditorPlugins.Core;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack
{
    [Description("Indents and formats JSON")]
    public class JSonFormatter : IEditorPlugin
    {
        public string Category => "Formatting";
        public string DisplayName => "JSon Formatter";
        public Guid Id => Guid.Parse("db389199-1a82-40d1-8d0e-1ff55228b4df");
        public Image Image => null;
        public bool OmitResult => false;
        public Keys ShortcutKeys => Keys.None;

        public bool IsExtension => false;

        public void Apply(ISelInfo sel)
        {
            sel.SelectedText = JsonHelper.FormatJson(sel.SelectedText);
        }

        public bool Configure()
        {
            return false;
        }
    }
}