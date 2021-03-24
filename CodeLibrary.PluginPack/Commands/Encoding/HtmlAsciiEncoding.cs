using DevToys;
using EditorPlugins.Core;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack.Commands.Encoding
{
    public class HtmlAsciiEncoding : IEditorPlugin
    {
        public bool IsExtension => false;
        public string Category => "Encoding";
        public string DisplayName => "Html Ascii Encoding";
        public Guid Id => Guid.Parse("22947a2f-e3cd-4f59-b8ea-bc30495d0e61");
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
            return HtmlChar.HtmlSmartEncode(text, HtmlChar.HtmlEncoding.AsciiEncoding);
        }
    }
}