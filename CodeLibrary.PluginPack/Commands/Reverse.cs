using EditorPlugins.Core;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack
{
    [Description("Reverses the selected text.")]
    public class Reverse : IEditorPlugin
    {
        public string Category => "";
        public string DisplayName => "Reverse";
        public Guid Id => Guid.Parse("1e3bb82c-22a1-4eaa-b5b8-207e0350eaa2");
        public Image Image => ImageResource.reverse;
        public bool OmitResult => false;
        public Keys ShortcutKeys => Keys.None;

        public void Apply(ISelInfo sel)
        {
            string text = sel.SelectedText;
            char[] _arrayA = text.ToCharArray();
            char[] _arrayB = new char[_arrayA.Length];
            for (int xx = 0, yy = _arrayA.Length - 1; xx < _arrayA.Length; xx++, yy--)
            {
                _arrayB[yy] = _arrayA[xx];
            }
            sel.SelectedText = new string(_arrayB);
        }

        public bool Configure()
        {
            return false;
        }
    }
}