using FastColoredTextBoxNS;
using GK.Template;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CodeLibrary
{
    public class FastColoredTextBoxHelper
    {
        private readonly ContextMenuStrip _contextMenuStripPopup;
        private readonly FastColoredTextBox _fastColoredTextBox;
        private readonly FormCodeLibrary _mainform;
        private readonly TextBox _textboxPath;
        private CodeSnippet _currentSnippet = null;
        private CycleList<CodeSnippet> _cycleSnippets = null;
        private string _cycleText = null;
        private bool initSnippet = false;

        public FastColoredTextBoxHelper(FastColoredTextBox fastColoredTextBox, TextBox textboxPath, FormCodeLibrary mainform, ContextMenuStrip contextMenuStripPopup, ListBox listBoxInsight)
        {
            _fastColoredTextBox = fastColoredTextBox;
            _mainform = mainform;
            _textboxPath = textboxPath;
            _contextMenuStripPopup = contextMenuStripPopup;

            CodeInsight.Instance.Init(listBoxInsight, fastColoredTextBox);

            _fastColoredTextBox.AllowDrop = true;
            _fastColoredTextBox.DragDrop += FastColoredTextBox_DragDrop;
            _fastColoredTextBox.DragOver += FastColoredTextBox_DragOver;
            _fastColoredTextBox.TextChanged += new System.EventHandler<TextChangedEventArgs>(TbCode_TextChanged);
            _fastColoredTextBox.KeyDown += new KeyEventHandler(TbCode_KeyDown);
            _fastColoredTextBox.MouseUp += new MouseEventHandler(TbCode_MouseUp);
        }

        public CodeSnippet CurrentSnippet
        {
            get
            {
                return _currentSnippet;
            }
        }

        public FastColoredTextBox FastColoredTextBox
        {
            get
            {
                return _fastColoredTextBox;
            }
        }

        public string SelectedText
        {
            get
            {
                return _fastColoredTextBox.SelectedText;
            }
            set
            {
                _fastColoredTextBox.SelectedText = value;
            }
        }

        public string Text
        {
            get
            {
                return _fastColoredTextBox.Text;
            }
            set
            {
                _fastColoredTextBox.Text = value;
            }
        }

        public void BringToFront()
        {
            _fastColoredTextBox.BringToFront();
        }

        public void ChangeType(CodeSnippet snippet, CodeType newtype)
        {
            if (snippet.CodeType != newtype)
                CodeToScreen(snippet);

            SetEditorCodeType(newtype);
        }

        public void CodeToScreen(CodeSnippet snippet)
        {
            initSnippet = true;
            _currentSnippet = snippet;
            SetEditorCodeType(snippet.CodeType);
            _fastColoredTextBox.Text = _currentSnippet.Code;
            _fastColoredTextBox.ClearUndo();
            _textboxPath.Text = _currentSnippet.Path;
            _fastColoredTextBox.WordWrap = _currentSnippet.Wordwrap;
            _fastColoredTextBox.SelectionStart = 0;
            _fastColoredTextBox.SelectionLength = 0;
            _fastColoredTextBox.ScrollControlIntoView(_fastColoredTextBox);

            _fastColoredTextBox.Bookmarks.Clear();
            if (snippet.Bookmarks != null)
            {
                foreach (CodeBookmark bm in snippet.Bookmarks)
                {
                    _fastColoredTextBox.Bookmarks.Add(new Bookmark(_fastColoredTextBox, bm.Name, bm.LineNumber));
                }
            }

            int _lines = _fastColoredTextBox.LinesCount;
            try
            {
                if (_lines > _currentSnippet.CurrentLine)
                    _fastColoredTextBox.GotoLine(_currentSnippet.CurrentLine);
            }
            catch { }

            initSnippet = false;
            _mainform.wordwrapToolStripMenuItem.Checked = _currentSnippet.Wordwrap;
        }

        public void Copy() => _fastColoredTextBox.Copy();

        public void Cut() => _fastColoredTextBox.Cut();

        public void Focus() => _fastColoredTextBox.Focus();

        public void GotoLine(int line) => _fastColoredTextBox.GotoLine(line);

        public bool GotoNextBookMark() => _fastColoredTextBox.GotoNextBookmark();

        public bool GotoPrevBookMark() => _fastColoredTextBox.GotoPrevBookmark();

        public void Paste() => _fastColoredTextBox.Paste();

        public void ScreenToCode(CodeSnippet snippet)
        {
            snippet.Code = _fastColoredTextBox.Text;
            snippet.Wordwrap = _fastColoredTextBox.WordWrap;
            snippet.CurrentLine = _fastColoredTextBox.CurrentLineNumber();
        }

        public void SelectAll() => _fastColoredTextBox.SelectAll();

        public void SetEditorCodeType(CodeType type)
        {
            switch (type)
            {
                case CodeType.Template:
                    CodeInsight.Instance.SetInsightHandler(new TemplateCodeInsightHandler());
                    _fastColoredTextBox.Language = FastColoredTextBoxNS.Language.CSharp;
                    break;

                case CodeType.CSharp:
                    CodeInsight.Instance.SetInsightHandler(null);
                    _fastColoredTextBox.Language = FastColoredTextBoxNS.Language.CSharp;
                    break;

                case CodeType.Folder:
                    CodeInsight.Instance.SetInsightHandler(null);
                    _fastColoredTextBox.Language = FastColoredTextBoxNS.Language.Custom;
                    break;

                case CodeType.SQL:
                    CodeInsight.Instance.SetInsightHandler(null);
                    _fastColoredTextBox.Language = FastColoredTextBoxNS.Language.SQL;
                    break;

                case CodeType.VB:
                    CodeInsight.Instance.SetInsightHandler(null);
                    _fastColoredTextBox.Language = FastColoredTextBoxNS.Language.VB;
                    break;

                case CodeType.None:
                    CodeInsight.Instance.SetInsightHandler(null);
                    _fastColoredTextBox.Language = FastColoredTextBoxNS.Language.Custom;
                    break;

                case CodeType.HTML:
                    CodeInsight.Instance.SetInsightHandler(null);
                    _fastColoredTextBox.Language = FastColoredTextBoxNS.Language.HTML;

                    break;
            }
        }

        public void SetWordWrap()
        {
            if (_currentSnippet == null)
                return;

            _currentSnippet.Wordwrap = !_currentSnippet.Wordwrap;
            _fastColoredTextBox.WordWrap = _currentSnippet.Wordwrap;
            _mainform.wordwrapToolStripMenuItem.Checked = _currentSnippet.Wordwrap;
        }

        public void ShowFindDialog() => _fastColoredTextBox.ShowFindDialog();

        public void ShowReplaceDialog() => _fastColoredTextBox.ShowReplaceDialog();

        private bool DocShortCut(KeyEventArgs e)
        {
            Console.WriteLine(e.KeyData);
            var _snippet = CodeLib.Instance.GetByShortCut(e.KeyData).FirstOrDefault();

            if (_snippet != null)
            {
                StringTemplate stringtemplate = new StringTemplate();
                string result = stringtemplate.Format(_snippet.Code, _fastColoredTextBox.SelectedText);
                _fastColoredTextBox.SelectedText = result;
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

        private void InitCycleSnippets()
        {
            _cycleSnippets = new CycleList<CodeSnippet>();
            _cycleSnippets.AddRange(CodeLib.Instance.GetCyclicShortCuts());
            _cycleText = _fastColoredTextBox.SelectedText;
        }

        private void ResetCycleSnippets()
        {
            _cycleSnippets = null;
            _cycleText = null;
        }

        private void TbCode_KeyDown(object sender, KeyEventArgs e)
        {
            FastColoredTextBox tb = sender as FastColoredTextBox;

            if (DocShortCut(e))
                return;

            if (string.IsNullOrEmpty(tb.SelectedText))
                return;

            ResetCycleSnippets();

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
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                _contextMenuStripPopup.Show(Cursor.Position.X, Cursor.Position.Y);
            }
        }

        private void TbCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (initSnippet)
                return;

            ScreenToCode(_currentSnippet);
        }
    }
}