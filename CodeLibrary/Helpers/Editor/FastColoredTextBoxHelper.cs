using CodeLibrary.Core;
using FastColoredTextBoxNS;
using GK.Template;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CodeLibrary
{
    public class FastColoredTextBoxHelper : ITextBoxHelper
    {
        private readonly FormCodeLibrary _mainform;
        private readonly FastColoredTextBox _tb;
        private readonly TextBoxHelper _TextBoxHelper;

        private Regex _regexWildCards = new Regex("(?<=#\\[)(.*?)(?=\\]#)");
        private bool _supressTextChanged = false;


        public FastColoredTextBoxHelper(FormCodeLibrary mainform, TextBoxHelper textboxHelper)
        {
            _tb = mainform.tbCode;
            _mainform = mainform;
            _TextBoxHelper = textboxHelper;

            CodeInsight.Instance.Init(_mainform.listBoxInsight, _tb);
            _tb.AllowDrop = true;
            _tb.DragDrop += FastColoredTextBox_DragDrop;
            _tb.DragOver += FastColoredTextBox_DragOver;
            _tb.TextChanged += new System.EventHandler<TextChangedEventArgs>(TbCode_TextChanged);
            _tb.KeyDown += new KeyEventHandler(TbCode_KeyDown);
            _tb.MouseUp += new MouseEventHandler(TbCode_MouseUp);
        }


        public ITextEditor Editor
        {
            get
            {
                return _tb;
            }
        }

        public FastColoredTextBox FastColoredTextBox
        {
            get
            {
                return _tb;
            }
        }

        public string SelectedText
        {
            get
            {
                return _tb.SelectedText;
            }
            set
            {
                _tb.SelectedText = value;
            }
        }

        public string Text
        {
            get
            {
                return _tb.Text;
            }
            set
            {
                _tb.Text = value;
            }
        }

        public void BringToFront() => _tb.BringToFront();

        public void CodeToScreen(CodeSnippet snippet)
        {
            _supressTextChanged = true;
            _tb.BeginUpdate();

            SetEditorCodeType(snippet.CodeType);

            _tb.Text = snippet.Code;
            _tb.ClearUndo();
            _mainform.tbPath.Text = snippet.Path;
            _tb.WordWrap = snippet.Wordwrap;
            _tb.SelectionStart = 0;
            _tb.SelectionLength = 0;
            _tb.ScrollControlIntoView(_tb);


            int _lines = _tb.LinesCount;
            try
            {
                if (_lines > snippet.CurrentLine)
                    _tb.GotoLine(snippet.CurrentLine);
            }
            catch { }

            _mainform.wordwrapToolStripMenuItem.Checked = snippet.Wordwrap;
            _mainform.hTMLPreviewToolStripMenuItem.Checked = snippet.HtmlPreview;

            _tb.EndUpdate();

            _TextBoxHelper.CurrentSnippet = snippet;

            _supressTextChanged = false;
        }

        public void Save()
        {
            ScreenToCode(_TextBoxHelper.CurrentSnippet);
        }

        public void Copy()
        {
            _mainform.textBoxClipboard.Text = SelectedText;
            if (!string.IsNullOrEmpty(_mainform.textBoxClipboard.Text))
                Clipboard.SetText(_mainform.textBoxClipboard.Text, TextDataFormat.Text);
            else
                Clipboard.Clear();
        }

        public void CopyWithMarkup() => _tb.Copy();

        public string CurrentLine()
        {
            Range _line = _tb.GetLine(_tb.Selection.Start.iLine);
            return _line.Text;
        }

        public void Cut()
        {
            _mainform.textBoxClipboard.Text = SelectedText;
            SelectedText = string.Empty;
            if (!string.IsNullOrEmpty(_mainform.textBoxClipboard.Text))
                Clipboard.SetText(_mainform.textBoxClipboard.Text, TextDataFormat.Text);
            else
                Clipboard.Clear();
        }

        public void CutWithMarkup() => _tb.Cut();

        public void Focus() => _tb.Focus();

        public void GotoLine() => _tb.GotoLine();

        public void GotoLine(int line) => _tb.GotoLine(line);


        public string Merge(string text)
        {
            string _newText = text;
            var _matches = _regexWildCards.Matches(text);
            foreach (Match match in _matches)
            {
                CodeSnippet _snippet = CodeLib.Instance.GetByPath(match.Value);
                if (_snippet.CodeType == CodeType.Image)
                {
                    string _base64 = Convert.ToBase64String(_snippet.Blob);
                    _newText = _newText.Replace($"#[{match.Value}]#", string.Format(@"<img src=""data:image/png;base64,{0}"" />", _base64));
                }
                else
                {
                    _newText = _newText.Replace($"#[{match.Value}]#", _snippet.Code);
                }
            }

            return _newText;
        }

        public void Paste() => _tb.Paste();

        public void ScreenToCode(CodeSnippet snippet)
        {
            if (snippet == null)
                return;

            snippet.Code = _tb.Text;
            snippet.Wordwrap = _tb.WordWrap;
            snippet.CurrentLine = _tb.CurrentLineNumber();
        }

        public void SelectAll() => _tb.SelectAll();

        public void SelectLine()
        {
            Range _line = _tb.GetLine(_tb.Selection.Start.iLine);
            Place _start = new Place(0, _tb.Selection.Start.iLine);
            Place _end = new Place(_line.End.iChar, _tb.Selection.Start.iLine);
            _tb.Selection = new Range(_tb, _start, _end);
        }

        public void RefreshEditor()
        {
            string _text = _tb.Text;
            _tb.Clear();
            _tb.ClearUndo();
            _tb.Text = _text;
        }

        public void SetEditorCodeType(CodeType type)
        {

            bool _htmlpreview = false;
            if (_TextBoxHelper.CurrentSnippet != null)
            {
                _htmlpreview = _TextBoxHelper.CurrentSnippet.HtmlPreview;
            }
            switch (type)
            {
                case CodeType.Template:
                    CodeInsight.Instance.SetInsightHandler(new TemplateCodeInsightHandler());
                    _tb.Language = FastColoredTextBoxNS.Language.CSharp;
                    _mainform.splitContainerCode.Panel2Collapsed = !_htmlpreview;
                    _mainform.containerCode.Visible = true;
                    _mainform.containerRtfEditor.Visible = false;
                    break;

                case CodeType.CSharp:
                    CodeInsight.Instance.SetInsightHandler(null);
                    _tb.Language = FastColoredTextBoxNS.Language.CSharp;
                    _mainform.splitContainerCode.Panel2Collapsed = !_htmlpreview;
                    _mainform.containerCode.Visible = true;
                    _mainform.containerRtfEditor.Visible = false;

                    break;

                case CodeType.Folder:
                    CodeInsight.Instance.SetInsightHandler(null);
                    _tb.Language = FastColoredTextBoxNS.Language.Custom;
                    _mainform.splitContainerCode.Panel2Collapsed = !_htmlpreview;
                    _mainform.containerCode.Visible = true;
                    _mainform.containerRtfEditor.Visible = false;

                    break;

                case CodeType.SQL:
                    CodeInsight.Instance.SetInsightHandler(null);
                    _tb.Language = FastColoredTextBoxNS.Language.SQL;
                    _mainform.splitContainerCode.Panel2Collapsed = !_htmlpreview;
                    _mainform.containerCode.Visible = true;
                    _mainform.containerRtfEditor.Visible = false;

                    break;

                case CodeType.VB:
                    CodeInsight.Instance.SetInsightHandler(null);
                    _tb.Language = FastColoredTextBoxNS.Language.VB;
                    _mainform.splitContainerCode.Panel2Collapsed = !_htmlpreview;
                    _mainform.containerCode.Visible = true;
                    _mainform.containerRtfEditor.Visible = false;
                    break;

                case CodeType.None:
                    CodeInsight.Instance.SetInsightHandler(null);
                    _tb.Language = FastColoredTextBoxNS.Language.Custom;
                    _mainform.splitContainerCode.Panel2Collapsed = !_htmlpreview;
                    _mainform.containerCode.Visible = true;
                    _mainform.containerRtfEditor.Visible = false;
                    break;

                case CodeType.HTML:
                    CodeInsight.Instance.SetInsightHandler(null);
                    _tb.Language = FastColoredTextBoxNS.Language.HTML;
                    _mainform.splitContainerCode.Panel2Collapsed = !_htmlpreview;
                    _mainform.containerCode.Visible = true;
                    _mainform.containerRtfEditor.Visible = false;
                    break;

                case CodeType.RTF:
                    CodeInsight.Instance.SetInsightHandler(null);
                    _tb.Language = FastColoredTextBoxNS.Language.Custom;
                    _mainform.splitContainerCode.Panel2Collapsed = true;
                    _mainform.containerCode.Visible = false;
                    _mainform.containerRtfEditor.Visible = true;
                    break;

                case CodeType.XML:
                    CodeInsight.Instance.SetInsightHandler(null);
                    _tb.Language = FastColoredTextBoxNS.Language.XML;
                    _mainform.splitContainerCode.Panel2Collapsed = !_htmlpreview;
                    _mainform.containerCode.Visible = true;
                    _mainform.containerRtfEditor.Visible = false;
                    break;

                case CodeType.JS:
                    CodeInsight.Instance.SetInsightHandler(null);
                    _tb.Language = FastColoredTextBoxNS.Language.JS;
                    _mainform.splitContainerCode.Panel2Collapsed = !_htmlpreview;
                    _mainform.containerCode.Visible = true;
                    _mainform.containerRtfEditor.Visible = false;
                    break;

                case CodeType.PHP:
                    CodeInsight.Instance.SetInsightHandler(null);
                    _tb.Language = FastColoredTextBoxNS.Language.PHP;
                    _mainform.splitContainerCode.Panel2Collapsed = !_htmlpreview;
                    _mainform.containerCode.Visible = true;
                    _mainform.containerRtfEditor.Visible = false;
                    break;

                case CodeType.Lua:
                    CodeInsight.Instance.SetInsightHandler(null);
                    _tb.Language = FastColoredTextBoxNS.Language.Lua;
                    _mainform.splitContainerCode.Panel2Collapsed = !_htmlpreview;
                    _mainform.containerCode.Visible = true;
                    _mainform.containerRtfEditor.Visible = false;
                    break;
            }
          
        }

        public bool SwitchWordWrap()
        {
            if (_TextBoxHelper.CurrentSnippet == null)
                return false;

            _TextBoxHelper.CurrentSnippet.Wordwrap = !_TextBoxHelper.CurrentSnippet.Wordwrap;
            _tb.WordWrap = _TextBoxHelper.CurrentSnippet.Wordwrap;
            _mainform.wordwrapToolStripMenuItem.Checked = _TextBoxHelper.CurrentSnippet.Wordwrap;

            return _TextBoxHelper.CurrentSnippet.Wordwrap;
        }

        public void ApplySnippetSettings()
        {
            if (_TextBoxHelper.CurrentSnippet == null)
                return;

            _tb.WordWrap = _TextBoxHelper.CurrentSnippet.Wordwrap;

            _mainform.hTMLPreviewToolStripMenuItem.Checked = _TextBoxHelper.CurrentSnippet.HtmlPreview;
            _mainform.splitContainerCode.Panel2Collapsed = !_TextBoxHelper.CurrentSnippet.HtmlPreview;

            UpdateHtmlPreview();
        }

        public void ShowFindDialog() => _tb.ShowFindDialog();

        public void ShowReplaceDialog() => _tb.ShowReplaceDialog();


        private bool DocShortCut(KeyEventArgs e)
        {
            var _snippet = CodeLib.Instance.GetByShortCut(e.KeyData).FirstOrDefault();

            if (_snippet != null)
            {
                StringTemplate stringtemplate = new StringTemplate();
                string result = stringtemplate.Format(_snippet.Code, _tb.SelectedText);
                _tb.SelectedText = result;
                return true;
            }

            return false;
        }

        private void FastColoredTextBox_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                string[] filenames = (string[])(e.Data.GetData(DataFormats.FileDrop, false));
                if (filenames.Length >= 1)
                {
                    string _text = File.ReadAllText(filenames[0]);
                    Text = _text;
                }
            }
        }

        private void FastColoredTextBox_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void TbCode_KeyDown(object sender, KeyEventArgs e)
        {
            FastColoredTextBox tb = sender as FastColoredTextBox;

            if (DocShortCut(e))
                return;

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

             //ScreenToCode(_TextBoxHelper.CurrentSnippet);

        }

        private void TbCode_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _mainform.contextMenuStripPopup.Show(Cursor.Position.X, Cursor.Position.Y);
            }
        }

        private void UpdateHtmlPreview()
        {
            if (!_mainform.splitContainerCode.Panel2Collapsed)
            {
                _mainform.webBrowser.DocumentText = Merge(_tb.Text);
            }
        }

        private void TbCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateHtmlPreview();

            if (_supressTextChanged)
                return;

             //ScreenToCode(_TextBoxHelper.CurrentSnippet); 
        }
    }
}