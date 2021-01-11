using EditorPlugins.Core;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack
{
    [Description("Removes all text except for text enclosed in quotes.")]
    public class KeepQuoted : IEditorPlugin
    {
        public string Category => "C#";
        public string DisplayName => "Keep Quoted";
        public Guid Id => Guid.Parse("04a9843c-bbd5-4252-860a-ce46e664bdbd");
        public Image Image => null;
        public bool OmitResult => false;
        public Keys ShortcutKeys => Keys.None;

        public void Apply(ISelInfo sel)
        {
            sel.SelectedText = Parse(sel.SelectedText);
        }

        public bool Configure()
        {
            return false;
        }

        private string Parse(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            StringBuilder _sb = new StringBuilder();

            char[] _chars = text.ToCharArray();
            char _prevChar = (char)0;

            bool _withinQuotes = false;

            foreach (char c in _chars)
            {
                if (_withinQuotes == false && c == '"' && _prevChar != '\\')
                {
                    _withinQuotes = true;
                    continue;
                }
                if (_withinQuotes == true && c == '"' && _prevChar != '\\')
                {
                    _withinQuotes = false;
                    continue;
                }

                if (_withinQuotes)
                {
                    _sb.Append(c);
                }
                _prevChar = c;
            }

            return _sb.ToString();
        }
    }
}