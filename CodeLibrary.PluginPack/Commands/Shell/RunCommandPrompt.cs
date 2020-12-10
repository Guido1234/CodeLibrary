using EditorPlugins.Core;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack
{
    [Description("")]
    public class RunCommandPrompt : IEditorPlugin
    {
        public string Category => "Shell";
        public string DisplayName => "Run Command Prompt";
        public Guid Id => Guid.Parse("64207bbf-0b15-4f4b-8363-e7b2201ef277");
        public Image Image => null;
        public bool OmitResult => true;
        public Keys ShortcutKeys => Keys.None;

        public void Apply(ISelInfo sel)
        {
            System.Diagnostics.Process.Start("CMD.exe", sel.SelectedText);
        }

        public bool Configure()
        {
            return false;
        }
    }
}