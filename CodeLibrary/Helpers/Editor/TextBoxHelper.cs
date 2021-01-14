using CodeLibrary.Core;
using FastColoredTextBoxNS;

namespace CodeLibrary
{ 
    public class TextBoxHelper
    {
        private readonly FormCodeLibrary _mainform;
        private ITextBoxHelper _ActiveTextBoxHelper;
        private CodeSnippet _currentSnippet = null;
        private FastColoredTextBoxHelper _FastColoredTextBoxHelper;
        private RtfEditorHelper _RtfEditorHelper;

        public TextBoxHelper(FormCodeLibrary mainform)
        {
            _mainform = mainform;
            _RtfEditorHelper = new RtfEditorHelper(mainform, this);
            _FastColoredTextBoxHelper = new FastColoredTextBoxHelper(mainform, this);
        }

        public CodeSnippet CurrentSnippet
        {
            get
            {
                return _currentSnippet;
            }
            set
            {
                _currentSnippet = value;
            }
        }

        public FastColoredTextBox FastColoredTextBox
        {
            get
            {
                return _FastColoredTextBoxHelper.FastColoredTextBox;
            }
        }

        public string SelectedText
        {
            get
            {
                return _ActiveTextBoxHelper.SelectedText;
            }
            set
            {
                _ActiveTextBoxHelper.SelectedText = value;
            }
        }

        public string Text
        {
            get
            {
                return _ActiveTextBoxHelper.Text;
            }
            set
            {
                _ActiveTextBoxHelper.Text = value;
            }
        }

        public void ApplySettings()
        {
            _ActiveTextBoxHelper.ApplySnippetSettings();
        }

        public void BringToFront() => _ActiveTextBoxHelper.BringToFront();

        public void ChangeType(CodeSnippet snippet, CodeType newtype)
        {
            if (newtype == CodeType.RTF)
            {
                _ActiveTextBoxHelper = _RtfEditorHelper;
                _mainform.CurrentEditor.Editor = _RtfEditorHelper.Editor;
            }
            else
            {
                _ActiveTextBoxHelper = _FastColoredTextBoxHelper;
                _mainform.CurrentEditor.Editor = _FastColoredTextBoxHelper.Editor;
            }

            snippet.CodeType = newtype;
            SetEditorCodeType(newtype);
        }

        public void CodeToScreen(CodeSnippet snippet)
        {
            ScreenToCode(CurrentSnippet);

            if (snippet.CodeType == CodeType.RTF)
            {
                _ActiveTextBoxHelper = _RtfEditorHelper;
                _mainform.CurrentEditor.Editor = _RtfEditorHelper.Editor;
            }
            else
            {
                _ActiveTextBoxHelper = _FastColoredTextBoxHelper;
                _mainform.CurrentEditor.Editor = _FastColoredTextBoxHelper.Editor;
            }

            _ActiveTextBoxHelper.CodeToScreen(snippet);
        }

        public void Copy() => _ActiveTextBoxHelper.Copy();

        public void CopyWithMarkup()
        {
            if (CurrentSnippet.CodeType != CodeType.RTF)
            {
                _FastColoredTextBoxHelper.CopyWithMarkup();
            }
        }

        public void Cut() => _ActiveTextBoxHelper.Cut();

        public void CutWithMarkup()
        {
            if (CurrentSnippet.CodeType != CodeType.RTF)
            {
                _FastColoredTextBoxHelper.Cut();
            }
        }

        public void Focus() => _ActiveTextBoxHelper.Focus();

        public void GotoLine(int line) => _ActiveTextBoxHelper.GotoLine(line);

        public void Paste() => _ActiveTextBoxHelper.Paste();

        public void Save()
        {
            if (_ActiveTextBoxHelper == null)
                return;

            _ActiveTextBoxHelper.Save();
        }

        public void ScreenToCode(CodeSnippet snippet)
        {
            if (_ActiveTextBoxHelper == null)
                return;

            _ActiveTextBoxHelper.ScreenToCode(snippet);
        }

        public void SelectAll() => _ActiveTextBoxHelper.SelectAll();

        public void SelectLine() => _ActiveTextBoxHelper.SelectLine();

        public void ShowFindDialog() => _ActiveTextBoxHelper.ShowFindDialog();

        public void ShowReplaceDialog() => _ActiveTextBoxHelper.ShowReplaceDialog();

        public void SwitchHtmlPreview()
        {
            if (_currentSnippet == null)
                return;

            _currentSnippet.HtmlPreview = !_currentSnippet.HtmlPreview;
            _mainform.hTMLPreviewToolStripMenuItem.Checked = _currentSnippet.HtmlPreview;
            _mainform.splitContainerCode.Panel2Collapsed = !_currentSnippet.HtmlPreview;
        }

        public bool SwitchWordWrap()
        {
            return _ActiveTextBoxHelper.SwitchWordWrap();
        }

        public void UpdateHtmlPreview()
        {
            if (_currentSnippet == null)
                return;

            _ActiveTextBoxHelper.UpdateHtmlPreview();
        }

        private void SetEditorCodeType(CodeType type)
        {
            if (type == CodeType.RTF)
            {
                _ActiveTextBoxHelper = _RtfEditorHelper;
                _mainform.CurrentEditor.Editor = _RtfEditorHelper.Editor;
            }
            else
            {
                _ActiveTextBoxHelper = _FastColoredTextBoxHelper;
                _mainform.CurrentEditor.Editor = _FastColoredTextBoxHelper.Editor;
            }

            _FastColoredTextBoxHelper.SetEditorCodeType(type);
            _FastColoredTextBoxHelper.RefreshEditor();
        }

        public string Merge()
        {
            return _ActiveTextBoxHelper.Merge();
        }
    }
}