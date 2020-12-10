using CodeLibrary.PluginPack.Common;
using EditorPlugins.Core;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack
{
    [Description("Sorts all lines desc in selection.")]
    public class SortLinesDesc : IEditorPlugin
    {
        public string Category => "Lines";
        public string DisplayName => "Sort Lines Desc";
        public Guid Id => Guid.Parse("a56a9533-b786-4ad0-b20f-2550431f617f");
        public Image Image => ImageResource.desc;
        public bool OmitResult => false;
        public Keys ShortcutKeys => Keys.None;

        public void Apply(ISelInfo sel)
        {
            sel.SelectedText = sortLinesDesc(sel.SelectedText);
        }

        public bool Configure()
        {
            return false;
        }

        private string sortLinesDesc(string text)
        {
            string[] lines = Utils.Lines(text);
            lines = Utils.SortDesc(lines);
            return string.Join(Utils._CRLF, lines);
        }
    }
}