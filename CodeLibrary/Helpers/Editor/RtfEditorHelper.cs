using CodeLibrary.Controls.Controls;
using CodeLibrary.Core;
using FastColoredTextBoxNS;
using GK.Template;
using System;
using System.Linq;
using System.Windows.Forms;

namespace CodeLibrary
{
    public class RtfEditorHelper : ITextBoxHelper
    {
        private readonly FormCodeLibrary _mainform;
        private readonly RtfControl _rtf;
        private readonly TextBoxHelper _TextBoxHelper;
        private bool _supressTextChanged = false;

        public RtfEditorHelper(FormCodeLibrary mainform, TextBoxHelper textboxHelper)
        {
            _TextBoxHelper = textboxHelper;
            _mainform = mainform;
            _rtf = _mainform.rtfEditor;

            _rtf.TextChanged += RtfEditor_TextChanged;
            _rtf.MouseUp += _rtf_MouseUp;
            _rtf.KeyDown += _rtf_KeyDown;
        }

        public ITextEditor Editor
        {
            get
            {
                return _rtf;
            }
        }

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

        public void CodeToScreen(CodeSnippet snippet)
        {
            _supressTextChanged = true;

            _TextBoxHelper.CurrentSnippet = snippet;
            _mainform.rtfEditor.ClearUndo();
            _mainform.rtfEditor.ResetText();
            _mainform.rtfEditor.Rtf = snippet.RTF;

            _mainform.rtfEditor.Zoom = Config.Zoom;

            _supressTextChanged = false;
        }

        public void Copy() => _rtf.Copy();

        public string CurrentLine()
        {
            return _rtf.CurrentLine();
        }

        public void Cut() => _rtf.Cut();

        public void Focus() => _rtf.Focus();

        public void GotoLine() => _rtf.GotoLine();

        public void GotoLine(int line) => _rtf.GotoLine(line);

        public bool GotoNextBookMark()
        {
            return false;
        }

        public bool GotoPrevBookMark()
        {
            return false;
        }

        public void Paste() => _rtf.Paste();

        public void Save()
        {
            ScreenToCode(_TextBoxHelper.CurrentSnippet);
        }

        public void ScreenToCode(CodeSnippet snippet)
        {
            if (snippet == null)
            {
                return;
            }

            snippet.RTF = _rtf.Rtf;
            snippet.Code = _rtf.Text;
        }

        public void SelectAll() => _rtf.SelectAll();

        public void SelectLine()
        {
            _rtf.SelectLine();
        }

        public void ShowFindDialog() => _rtf.ShowFindDialog();

        public void ShowReplaceDialog() => _rtf.ShowReplaceDialog();

        public bool SwitchWordWrap()
        {
            return false;
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
                _mainform.contextMenuStripPopup.Show(Cursor.Position.X, Cursor.Position.Y);
            }
        }

        private bool DocShortCut(KeyEventArgs e)
        {
            var _snippet = CodeLib.Instance.GetByShortCut(e.KeyData).FirstOrDefault();

            if (_snippet != null)
            {
                StringTemplate stringtemplate = new StringTemplate();
                string result = stringtemplate.Format(_snippet.Code, _rtf.SelectedText);
                _rtf.SelectedText = result;
                return true;
            }

            return false;
        }

        private void RtfEditor_TextChanged(object sender, EventArgs e)
        {
            if (_supressTextChanged)
                return;

            //ScreenToCode(_TextBoxHelper.CurrentSnippet);
        }
    }
}