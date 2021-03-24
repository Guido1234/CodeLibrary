using CodeLibrary.PluginPack.Common;
using CodeLibrary.PluginPack.Forms;
using EditorPlugins.Core;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack
{
    [Description("Multiple replaces within selection.")]
    public class MultiReplace : IEditorPlugin
    {
        public bool IsExtension => false;
        public string Category => "";
        public string DisplayName => "Multi Replace";
        public string Find { get; set; }
        public Guid Id => Guid.Parse("eaca17f5-0673-4a70-9f7e-4c40b22cc186");
        public Image Image => ImageResource.replace;
        public bool OmitResult { get; set; }
        public string Replace { get; set; }
        public Keys ShortcutKeys => Keys.None;

        public void Apply(ISelInfo sel)
        {
            string text = sel.SelectedText;

            OmitResult = false;

            if (!Configure())
            {
                OmitResult = true;
            }

            string[] finds = Utils.Lines(Find);
            string[] replaces = Utils.Lines(Replace);

            if (finds == null || replaces == null)
            {
                OmitResult = true;
            }

            if (finds.Length != replaces.Length)
            {
                OmitResult = true;
            }

            for (int ii = 0; ii < finds.Length; ii++)
            {
                string find = finds[ii];
                string replace = replaces[ii];
                text = text.Replace(find, replace);
            }

            OmitResult = false;
            sel.SelectedText = text;
        }

        public bool Configure()
        {
            FormMultiReplace f = new FormMultiReplace();
            f.Find = Find;
            f.Replace = Replace;

            DialogResult dr = f.ShowDialog();
            if (dr == DialogResult.OK)
            {
                Find = f.Find;
                Replace = f.Replace;
                return true;
            }
            return false;
        }
    }
}