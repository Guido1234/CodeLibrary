using CodeLibrary.PluginPack.Common;
using CodeLibrary.PluginPack.Forms;
using EditorPlugins.Core;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack
{
    [Description("Trims all lines in selection.")]
    public class TrimLines : IEditorPlugin
    {
        public bool IsExtension => false;
        public string Category => "Lines";
        public string DisplayName => "Trim Lines";
        public Guid Id => Guid.Parse("633403f3-d687-4892-82dc-eae43c670fa5");
        public Image Image => null;
        public bool OmitResult { get; set; } = false;
        public Keys ShortcutKeys => Keys.None;
        public string TrimChars { get; set; }
        public bool TrimEnd { get; set; }

        public bool TrimStart { get; set; }

        public void Apply(ISelInfo sel)
        {
            string text = sel.SelectedText;

            OmitResult = false;

            if (!Configure())
                OmitResult = true;

            string[] _lines = Utils.Lines(text);

            for (int ii = 0; ii < _lines.Length; ii++)
            {
                if (!string.IsNullOrWhiteSpace(TrimChars))
                {
                    char[] trimChars = TrimChars.ToCharArray();

                    if (TrimEnd && TrimStart)
                        _lines[ii] = _lines[ii].Trim(trimChars);

                    if (TrimEnd)
                        _lines[ii] = _lines[ii].TrimEnd(trimChars);

                    if (TrimStart)
                        _lines[ii] = _lines[ii].TrimStart(trimChars);
                }
                else
                {
                    if (TrimEnd && TrimStart)
                        _lines[ii] = _lines[ii].Trim();

                    if (TrimEnd)
                        _lines[ii] = _lines[ii].TrimEnd();

                    if (TrimStart)
                        _lines[ii] = _lines[ii].TrimStart();
                }
            }

            sel.SelectedText = string.Join(Utils._CRLF, _lines);
        }

        public bool Configure()
        {
            FormTrimLines f = new FormTrimLines();
            f.TrimEnd = TrimEnd;
            f.TrimStart = TrimStart;
            f.TrimChars = TrimChars;

            DialogResult dr = f.ShowDialog();
            if (dr == DialogResult.OK)
            {
                TrimEnd = f.TrimEnd;
                TrimStart = f.TrimStart;
                TrimChars = f.TrimChars;
                return true;
            }
            return false;
        }
    }
}