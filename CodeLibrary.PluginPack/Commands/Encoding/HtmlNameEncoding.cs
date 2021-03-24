using DevToys;
using EditorPlugins.Core;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack.Commands.Encoding
{
    public class HtmlNameEncoding : IEditorPlugin
    {
        public bool IsExtension => false;
        public string Category => "Encoding";
        public string DisplayName => "Html Name Encoding";
        public Guid Id => Guid.Parse("f3e31c0d-497d-4873-915d-05456c8fb7aa");
        public Image Image => null;
        public bool OmitResult => false;
        public Keys ShortcutKeys => Keys.None;

        public void Apply(ISelInfo sel)
        {
            sel.SelectedText = execute(sel.SelectedText);
        }

        public bool Configure()
        {
            return false;
        }

        private static string execute(string text)
        {
            return HtmlChar.HtmlSmartEncode(text, HtmlChar.HtmlEncoding.HtmlNameEncoding);
        }
    }
}