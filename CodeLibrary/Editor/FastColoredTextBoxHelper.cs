using CodeLibrary.Core;
using FastColoredTextBoxNS;
using GK.Template;
using MarkdownSharp;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CodeLibrary.Editor
{
    public class FastColoredTextBoxHelper : ITextBoxHelper
    {
        private CodeSnippet _StateSnippet;

        private readonly FormCodeLibrary _mainform;
        private readonly FastColoredTextBox _tb;
        private readonly TextBoxHelper _TextBoxHelper;
        private Idle _Idle = new Idle(new TimeSpan(0, 0, 2));

        private Regex _regexWildCards = new Regex("(?<=#\\[)(.*?)(?=\\]#)");
        private bool _supressTextChanged = false;

        public CodeSnippet GetStateSnippet() => _StateSnippet;

        public FastColoredTextBoxHelper(FormCodeLibrary mainform, TextBoxHelper textboxHelper)
        {
            _mainform = mainform;
            _tb = _mainform.fastColoredTextBox;
            _TextBoxHelper = textboxHelper;


            CodeInsight.Instance.Init(_mainform.listBoxInsight, _tb);
            _tb.AllowDrop = true;
            _tb.DragDrop += FastColoredTextBox_DragDrop;
            _tb.DragOver += FastColoredTextBox_DragOver;
            _tb.TextChanged += new System.EventHandler<TextChangedEventArgs>(TbCode_TextChanged);
            _tb.SelectionChanged += _tb_SelectionChanged;
            _tb.KeyDown += new KeyEventHandler(TbCode_KeyDown);
            _tb.MouseUp += new MouseEventHandler(TbCode_MouseUp);
        }

        public bool IsIdle => _Idle;

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

        public void ApplySnippetSettings()
        {
            if (_StateSnippet == null)
            {
                return;
            }

            _tb.WordWrap = _StateSnippet.Wordwrap;

            _mainform.mnuHTMLPreview.Checked = _StateSnippet.HtmlPreview;
            _mainform.splitContainerCode.Panel2Collapsed = !_StateSnippet.HtmlPreview;

            UpdateHtmlPreview();
        }

        public void BringToFront() => _tb.BringToFront();

        public void SetState(CodeSnippet snippet)
        {
            _StateSnippet = snippet;
            _supressTextChanged = true;
            _tb.BeginUpdate();

            _TextBoxHelper.SetEditorView(snippet);

            _tb.Text = snippet.Code;
            _tb.ClearUndo();
            _mainform.tbPath.Text = snippet.Path;// + $"    [C: {snippet.CreationDate},M:{snippet.CodeLastModificationDate:yyyy-MM-dd HH:mm:ss}]";
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

            _mainform.mnuWordwrap.Checked = snippet.Wordwrap;
            _mainform.mnuHTMLPreview.Checked = snippet.HtmlPreview;

            _tb.EndUpdate();

            _supressTextChanged = false;
        }

        public void SwitchHtmlPreview()
        {            
            if (_StateSnippet == null)
                return;

            _StateSnippet.HtmlPreview = !_StateSnippet.HtmlPreview;
            _mainform.mnuHTMLPreview.Checked = _StateSnippet.HtmlPreview;
            _mainform.splitContainerCode.Panel2Collapsed = !_StateSnippet.HtmlPreview;
        }

        public void Copy()
        {
            _mainform.textBoxClipboard.Text = SelectedText;
            if (!string.IsNullOrEmpty(_mainform.textBoxClipboard.Text))
                Clipboard.SetText(_mainform.textBoxClipboard.Text, TextDataFormat.Text);
            else
                Clipboard.Clear();
        }

        public void CopyWithMarkup()
        {
            _tb.Copy();
        }

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

        public string Merge(string text, CodeType targetType)
        {
            string _newText = text;
            var _matches = _regexWildCards.Matches(text);
            foreach (Match match in _matches)
            {
                CodeSnippet _snippet = CodeLib.Instance.GetByPath(match.Value);
                if (_snippet.CodeType == CodeType.Image)
                {
                    string _base64 = Convert.ToBase64String(_snippet.Blob);
                    switch (targetType)
                    {
                        case CodeType.MarkDown:
                            _newText = _newText.Replace($"#[{match.Value}]#", string.Format(@"![{0}](data:image/png;base64,{1})", _snippet.Path, _base64));
                            break;

                        default:
                            _newText = _newText.Replace($"#[{match.Value}]#", string.Format(@"<img src=""data:image/png;base64,{0}"" />", _base64));
                            break;
                    }
                }
                else
                {
                    _newText = _newText.Replace($"#[{match.Value}]#", _snippet.Code);
                }
            }

            return _newText;
        }

        public void Paste() => _tb.Paste();

        public void RefreshEditor()
        {
            string _text = _tb.Text;
            _tb.Clear();
            _tb.ClearUndo();
            _tb.Text = _text;
        }

        public bool SaveState()
        {
            bool _result = false;

            if (_StateSnippet == null)
            {
                return false;
            }
            _result = _StateSnippet.Code != _tb.Text;
            _StateSnippet.Code = _tb.Text;
            _StateSnippet.Wordwrap = _tb.WordWrap;
            _StateSnippet.CurrentLine = _tb.CurrentLineNumber();
            if (_result)
            {
                _StateSnippet.CodeLastModificationDate = DateTime.Now;
            }
            return _result;
        }

        public void SelectAll() => _tb.SelectAll();

        public void SelectLine()
        {
            Range _line = _tb.GetLine(_tb.Selection.Start.iLine);
            Place _start = new Place(0, _tb.Selection.Start.iLine);
            Place _end = new Place(_line.End.iChar, _tb.Selection.Start.iLine);
            _tb.Selection = new Range(_tb, _start, _end);
        }

        public void ShowFindDialog() => _tb.ShowFindDialog();

        public void ShowReplaceDialog() => _tb.ShowReplaceDialog();

        public bool SwitchWordWrap()
        {
            if (_StateSnippet == null)
                return false;

            _StateSnippet.Wordwrap = !_StateSnippet.Wordwrap;
            _tb.WordWrap = _StateSnippet.Wordwrap;
            _mainform.mnuWordwrap.Checked = _StateSnippet.Wordwrap;

            return _StateSnippet.Wordwrap;
        }

        public void UpdateHtmlPreview()
        {
            _mainform.webBrowser.AllowNavigation = true;

            if (!_mainform.splitContainerCode.Panel2Collapsed)
            {
                if (_StateSnippet.CodeType == CodeType.MarkDown)
                {
                    try
                    {
                        Markdown _markdown = new Markdown();
                        string _text = Merge(_tb.Text, CodeType.MarkDown);
                        _text = _markdown.Transform(_text);
                        _mainform.webBrowser.DocumentText = _text;
                    }
                    catch
                    {
                        _mainform.webBrowser.DocumentText = string.Empty;
                    }
                }
                else
                {
                    _mainform.webBrowser.DocumentText = Merge(_tb.Text, CodeType.HTML);
                }
            }
        }

        private bool DocShortCut(KeyEventArgs e)
        {
            var _snippet = CodeLib.Instance.GetByShortCut(e.KeyData).FirstOrDefault();

            if (_snippet != null)
            {
                StringTemplate stringtemplate = new StringTemplate();
                string result = stringtemplate.Format(_snippet.Code, _tb.SelectedText);
                _tb.SelectedText = result;
                _tb.Focus();
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
        }

        private void TbCode_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _mainform.mncEdit.Show(Cursor.Position.X, Cursor.Position.Y);
            }
        }

        private void TbCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateHtmlPreview();

            _Idle.Refresh();

            if (_supressTextChanged)
                return;
        }

        public string Merge()
        {
            return Merge(Text, CodeType.HTML);
        }

        private void _tb_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                _mainform.lblStart.Text = _tb.SelectionStart.ToString();
                _mainform.lblEnd.Text = (_tb.SelectionStart + _tb.SelectionLength).ToString();
                _mainform.lblLength.Text = _tb.SelectionLength.ToString();
            }
            catch { }
        }

    }
}