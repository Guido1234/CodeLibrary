using CodeLibrary.PluginPack.Forms;
using EditorPlugins.Core;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack
{
    [Description("Insert current Date and Time.\r\nRight click menu to configure.")]
    public class InsertDateTime : IEditorPlugin
    {
        public bool IsExtension => false;
        public string Category => "";
        public string DateFormat { get; set; } = "dd-MM-yyyy HH:mm";
        public string DisplayName => "Insert DateTime";
        public Guid Id => Guid.Parse("7b0cf2ca-fd9a-462b-abfc-137e0ce0717f");
        public Image Image => ImageResource.clock;
        public bool OmitResult => false;
        public Keys ShortcutKeys => Keys.None;

        public void Apply(ISelInfo sel)
        {
            sel.SelectedText = DateTime.Now.ToString(DateFormat);
        }

        public bool Configure()
        {
            FormDateFormat f = new FormDateFormat();
            f.DateFormat = DateFormat;

            DialogResult dr = f.ShowDialog();
            if (dr == DialogResult.OK)
            {
                DateFormat = f.DateFormat;
                return true;
            }
            return false;
        }
    }
}