using CodeLibrary.Controls.Controls;
using CodeLibrary.Core;
using FastColoredTextBoxNS;
using GK.Template;
using System;
using System.Linq;
using System.Windows.Forms;

namespace CodeLibrary.Editor
{
    public class RtfEditorHelper : ITextBoxHelper
    {
        private readonly FormCodeLibrary _mainform;
        private readonly RtfControl _rtf;
        private readonly TextBoxHelper _TextBoxHelper;
        private Idle _Idle = new Idle(new TimeSpan(0, 0, 2));
        private CodeSnippet _StateSnippet;
        private bool _supressTextChanged = false;

        public RtfEditorHelper(FormCodeLibrary mainform, TextBoxHelper textboxHelper)
        {
            _TextBoxHelper = textboxHelper;
            _mainform = mainform;
            _rtf = _mainform.rtfEditor;

            _rtf.TextChanged += RtfEditor_TextChanged;
            _rtf.MouseUp += _rtf_MouseUp;
            _rtf.KeyDown += _rtf_KeyDown;
            _rtf.RichTextConrol.SelectionChanged += RichTextConrol_SelectionChanged;
        }

        public ITextEditor Editor
        {
            get
            {
                return _rtf;
            }
        }

        public bool IsIdle => _Idle;

        public string SelectedText
        {
            get
            {
                return _rtf.SelectedText;
            }
            set
            {
                _rtf.SelectedText = value;
            }
        }

        public string Text
        {
            get
            {
                return _rtf.Text;
            }
            set
            {
                _rtf.Text = value;
            }
        }

        public void ApplySnippetSettings()
        {
        }

        public void BringToFront() => _rtf.BringToFront();

        public void Copy() => _rtf.Copy();

        public string CurrentLine() => _rtf.CurrentLine();

        public void Cut() => _rtf.Cut();

        public void Focus() => _rtf.Focus();

        public CodeSnippet GetStateSnippet() => _StateSnippet;

        public void GotoLine() => _rtf.GotoLine();

        public void GotoLine(int line) => _rtf.GotoLine(line);

        public bool GotoNextBookMark() => false;

        public bool GotoPrevBookMark() => false;

        public string Merge() => string.Empty;

        public void Paste() => _rtf.Paste();

        public bool SaveState()
        {
            bool _result = false;
            if (_StateSnippet == null)
            {
                return false;
            }

            _StateSnippet.SetRtf(_rtf.Rtf, out _result);
            _StateSnippet.SetCode(_rtf.Text, out _result);
            if (_result)
            {
                _StateSnippet.CodeLastModificationDate = DateTime.Now;
            }

            _StateSnippet.RTFOwnTheme = _mainform.rtfEditor.OwnTheme;
            if (_StateSnippet.RTFOwnTheme)
            {
                _StateSnippet.RTFTheme = _mainform.rtfEditor.Theme;
            }

            return _result;
        }

        public void SelectAll() => _rtf.SelectAll();

        public void SelectLine() => _rtf.SelectLine();

        public void SetState(CodeSnippet snippet)
        {
            _supressTextChanged = true;

            _StateSnippet = snippet;
            _mainform.tbPath.Text = snippet.GetPath();
            _mainform.rtfEditor.ClearUndo();
            _mainform.rtfEditor.ResetText();

            _mainform.rtfEditor.OwnTheme = snippet.RTFOwnTheme;
            if (_mainform.rtfEditor.OwnTheme)
            {
                _mainform.rtfEditor.SetOwnTheme(snippet.RTFTheme);
            }
            else
            {
                _mainform.rtfEditor.Theme = Config.Theme;
            }

            _mainform.rtfEditor.Rtf = snippet.GetRTF();

            _mainform.rtfEditor.Zoom = Config.Zoom;

            _supressTextChanged = false;
        }

        public void ShowFindDialog() => _rtf.ShowFindDialog();

        public void ShowReplaceDialog() => _rtf.ShowReplaceDialog();

        public void SwitchHtmlPreview()
        {
        }

        public bool SwitchWordWrap() => false;

        public void UpdateHtmlPreview()
        {
        }

        private void _rtf_KeyDown(object sender, KeyEventArgs e)
        {
            RtfControl tb = sender as RtfControl;

            if (DocShortCut(e))
                return;

            if (e.KeyValue == 71 && e.Control)
            {
                _rtf.GotoLine();
                e.Handled = true;
                return;
            }

            if (string.IsNullOrEmpty(tb.SelectedText))
                return;

            if (e.KeyValue == 222 && e.Shift)
            {
                tb.SelectedText = string.Format("\"{0}\"", tb.SelectedText);
                e.Handled = true;
                return;
            }
            if ((e.KeyValue == 57 || e.KeyValue == 48) && e.Shift)
            {
                tb.SelectedText = string.Format("({0})", tb.SelectedText);
                e.Handled = true;
                return;
            }
            if ((e.KeyValue == 219 || e.KeyValue == 221) && e.Shift)
            {
                tb.SelectedText = string.Format("{{{0}}}", tb.SelectedText);
                e.Handled = true;
                return;
            }
            if (e.KeyValue == 219 || e.KeyValue == 221)
            {
                tb.SelectedText = string.Format("[{0}]", tb.SelectedText);
                e.Handled = true;
                return;
            }
            if ((e.KeyValue == 188 || e.KeyValue == 190) && e.Shift)
            {
                tb.SelectedText = string.Format("<{0}>", tb.SelectedText);
                e.Handled = true;
                return;
            }
            if (e.KeyValue == 222)
            {
                tb.SelectedText = string.Format("'{0}'", tb.SelectedText);
                e.Handled = true;
                return;
            }
        }

        private void _rtf_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _mainform.mncEdit.Show(Cursor.Position.X, Cursor.Position.Y);
            }
        }

        private bool DocShortCut(KeyEventArgs e)
        {
            var _snippet = CodeLib.Instance.CodeSnippets.GetByShortCut(e.KeyData).FirstOrDefault();

            if (_snippet != null)
            {
                StringTemplate stringtemplate = new StringTemplate();
                string result = stringtemplate.Format(_snippet.GetCode(), _rtf.SelectedText);
                _rtf.SelectedText = result;
                _rtf.Focus();
                return true;
            }

            return false;
        }

        private void RichTextConrol_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                _mainform.lblStart.Text = _rtf.RichTextConrol.SelectionStart.ToString();
                _mainform.lblEnd.Text = (_rtf.RichTextConrol.SelectionStart + _rtf.RichTextConrol.SelectionLength).ToString();
                _mainform.lblLength.Text = _rtf.RichTextConrol.SelectionLength.ToString();
            }
            catch
            { }
        }

        private void RtfEditor_TextChanged(object sender, EventArgs e)
        {
            _Idle.Refresh();

            if (_supressTextChanged)
                return;

            CodeLib.Instance.Changed = true;
        }
    }
}