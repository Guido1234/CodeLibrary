using EditorPlugins.Core;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack
{
    [Description("Opens Link Location, URL or any other application for specific link. \r\nif nothing is selected, current line will be used.")]
    public class OpenLink : IEditorPlugin
    {
        public string Category => "Shell";
        public string DisplayName => "Open Link";
        public Guid Id => Guid.Parse("d85e28a0-f1b1-4803-825e-992f8116d09b");
        public Image Image => null;
        public bool OmitResult => false;
        public Keys ShortcutKeys => (Keys.Control | Keys.O);

        public void Apply(ISelInfo sel)
        {
            if (string.IsNullOrEmpty(sel.SelectedText))
            {
                string _line = sel.CurrentLine.Trim();
                if (string.IsNullOrEmpty(_line))
                    System.Diagnostics.Process.Start("http://google.com");
                else
                    System.Diagnostics.Process.Start(_line);
            }

            System.Diagnostics.Process.Start(sel.SelectedText);
        }

        public bool Configure()
        {
            return false;
        }
    }
}