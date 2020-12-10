using CodeLibrary.PluginPack.Common;
using EditorPlugins.Core;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack
{
    [Description("Sorts all lines asc in selection.")]
    public class SortLinesAsc : IEditorPlugin
    {
        public string Category => "Lines";
        public string DisplayName => "Sort Lines Asc";
        public Guid Id => Guid.Parse("8b54e23d-dacc-41a8-a094-6afdcdf26b87");
        public Image Image => ImageResource.asc;
        public bool OmitResult => false;
        public Keys ShortcutKeys => Keys.None;

        public void Apply(ISelInfo sel)
        {
            sel.SelectedText = sortLinesAsc(sel.SelectedText);
        }

        public bool Configure()
        {
            return false;
        }

        private string sortLinesAsc(string text)
        {
            string[] lines = Utils.Lines(text);
            lines = Utils.SortAsc(lines);
            return string.Join(Utils._CRLF, lines);
        }
    }
}