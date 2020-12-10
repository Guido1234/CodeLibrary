using CodeLibrary.PluginPack.Common;
using CodeLibrary.PluginPack.Forms;
using EditorPlugins.Core;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack.Commands.Lines
{
    [Description(@"Apply StringFormat function to selection.
1. Selection is treated as Multi/SingleLine Comma Separated Data.
2. Clipboard contains your Formatting.

EXAMPLE:

Clipboard contents:
""{0:X} | {1}""

Selection:
100,200
200,300

On StringForm:
""64 | 200""
""C8 | 300""

Numbers will be implicitly converted in following order:
Guid, Boolean, Int32, Int64, Single, Double, Decimal, TimeSpan, DateTime, DateTimeOffset, String.
Byte, Int16 only when enforced.

Enforce number types:
L force Int64 ( 223L )
M force Decimal ( 223M )
D force Double  ( 223D )
F force Float/Single ( 56F )
B force Byte ( 10B )
I force Int16 ( 100I )
")]
    public class StringFormatLines : IEditorPlugin
    {
        public string Category => "Lines";
        public string DisplayName => "String Format";
        public Guid Id => Guid.Parse("6ac93162-f593-4a0a-9e5d-342de850752e");
        public Image Image => null;
        public bool OmitResult => false;
        public Keys ShortcutKeys => Keys.None;

        public string SplitterChar { get; set; } = ",";

        public void Apply(ISelInfo sel)
        {
            sel.SelectedText = Format(sel.SelectedText);
        }

        public bool Configure()
        {
            FormStringFormat f = new FormStringFormat();
            f.SplitterChar = SplitterChar.ToString();

            DialogResult dr = f.ShowDialog();
            if (dr == DialogResult.OK)
            {
                if (f.SplitterChar.Length > 0)
                    SplitterChar = f.SplitterChar;
                else
                    SplitterChar = ",";
                return true;
            }
            return false;
        }

        private string Format(string text)
        {
            string _clipboard = Clipboard.GetText();
            if (string.IsNullOrWhiteSpace(_clipboard))
                return text;

            string[] lines = Utils.Lines(text);

            char _splitterchar = ',';
            if (SplitterChar.Length == 1)
                _splitterchar = SplitterChar[0];

            for (int ii = 0; ii < lines.Length; ii++)
            {
                string[] _items = Utils.SplitEscaped(lines[ii], _splitterchar, '"').ToArray();
                lines[ii] = Utils.StringFormatImplicit(_clipboard, _items);
            }

            return string.Join(Utils._CRLF, lines);
        }
    }
}