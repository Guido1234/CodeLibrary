using CodeLibrary.Core;
using FastColoredTextBoxNS;

namespace CodeLibrary
{
    public class TextBoxHelper
    {
        private readonly FormCodeLibrary _mainform;
        private ITextBoxHelper _ActiveTextBoxHelper;
        private FastColoredTextBoxHelper _FastColoredTextBoxHelper;
        private RtfEditorHelper _RtfEditorHelper;

        public TextBoxHelper(FormCodeLibrary mainform)
        {
            _mainform = mainform;
            _RtfEditorHelper = new RtfEditorHelper(_mainform, this);
            _FastColoredTextBoxHelper = new FastColoredTextBoxHelper(_mainform, this);
        }

        public FastColoredTextBox FastColoredTextBox
        {
            get
            {
                return _FastColoredTextBoxHelper.FastColoredTextBox;
            }
        }

        public bool IsIdle => _RtfEditorHelper.IsIdle && _FastColoredTextBoxHelper.IsIdle;

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

        public void ApplySettings() => _ActiveTextBoxHelper.ApplySnippetSettings();

        public void BringToFront() => _ActiveTextBoxHelper.BringToFront();

        public void ChangeView(CodeType newtype)
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
            SetEditorCodeType(newtype);
        }

        public void Copy() => _ActiveTextBoxHelper.Copy();

        public void CopyWithMarkup() => _FastColoredTextBoxHelper.CopyWithMarkup();

        public void Cut() => _ActiveTextBoxHelper.Cut();

        public void CutWithMarkup() => _FastColoredTextBoxHelper.Cut();

        public void Focus() => _ActiveTextBoxHelper.Focus();

        public void GotoLine(int line) => _ActiveTextBoxHelper.GotoLine(line);

        public string Merge() => _ActiveTextBoxHelper.Merge();

        public void Paste() => _ActiveTextBoxHelper.Paste();

        public bool SaveState()
        {
            if (_ActiveTextBoxHelper == null)
                return false;

            bool _changed = _ActiveTextBoxHelper.SaveState();
            if (_changed == true)
            {
                CodeLib.Instance.Changed = true;
            }
            return _changed;
        }

        public void SelectAll() => _ActiveTextBoxHelper.SelectAll();

        public void SelectLine() => _ActiveTextBoxHelper.SelectLine();

        public void SetEditorView(CodeSnippet snippet)
        {
            bool _htmlpreview = false;
            if (snippet != null)
            {
                _htmlpreview = snippet.HtmlPreview;
            }
            _mainform.splitContainerCode.Panel2Collapsed = (snippet.CodeType == CodeType.RTF) ? true : !_htmlpreview;
            SetEditorView(snippet.CodeType);
        }

        public void SetEditorView(CodeType type)
        {
            switch (type)
            {
                case CodeType.Template:
                    CodeInsight.Instance.SetInsightHandler(new TemplateCodeInsightHandler());
                    _mainform.fastColoredTextBox.Language = FastColoredTextBoxNS.Language.CSharp;

                    _mainform.containerCode.Visible = true;
                    _mainform.containerRtfEditor.Visible = false;
                    break;

                case CodeType.CSharp:
                    CodeInsight.Instance.SetInsightHandler(null);
                    _mainform.fastColoredTextBox.Language = FastColoredTextBoxNS.Language.CSharp;
                    _mainform.containerCode.Visible = true;
                    _mainform.containerRtfEditor.Visible = false;

                    break;

                case CodeType.Folder:
                    CodeInsight.Instance.SetInsightHandler(null);
                    _mainform.fastColoredTextBox.Language = FastColoredTextBoxNS.Language.Custom;
                    _mainform.containerCode.Visible = true;
                    _mainform.containerRtfEditor.Visible = false;

                    break;

                case CodeType.SQL:
                    CodeInsight.Instance.SetInsightHandler(null);
                    _mainform.fastColoredTextBox.Language = FastColoredTextBoxNS.Language.SQL;
                    _mainform.containerCode.Visible = true;
                    _mainform.containerRtfEditor.Visible = false;

                    break;

                case CodeType.VB:
                    CodeInsight.Instance.SetInsightHandler(null);
                    _mainform.fastColoredTextBox.Language = FastColoredTextBoxNS.Language.VB;
                    _mainform.containerCode.Visible = true;
                    _mainform.containerRtfEditor.Visible = false;
                    break;

                case CodeType.None:
                    CodeInsight.Instance.SetInsightHandler(null);
                    _mainform.fastColoredTextBox.Language = FastColoredTextBoxNS.Language.Custom;
                    _mainform.containerCode.Visible = true;
                    _mainform.containerRtfEditor.Visible = false;
                    break;

                case CodeType.HTML:
                    CodeInsight.Instance.SetInsightHandler(null);
                    _mainform.fastColoredTextBox.Language = FastColoredTextBoxNS.Language.HTML;
                    _mainform.containerCode.Visible = true;
                    _mainform.containerRtfEditor.Visible = false;
                    break;

                case CodeType.MarkDown:
                    CodeInsight.Instance.SetInsightHandler(null);
                    _mainform.fastColoredTextBox.Language = FastColoredTextBoxNS.Language.Custom;
                    _mainform.containerCode.Visible = true;
                    _mainform.containerRtfEditor.Visible = false;
                    break;

                case CodeType.RTF:
                    CodeInsight.Instance.SetInsightHandler(null);
                    _mainform.fastColoredTextBox.Language = FastColoredTextBoxNS.Language.Custom;
                    _mainform.splitContainerCode.Panel2Collapsed = true;
                    _mainform.containerCode.Visible = false;
                    _mainform.containerRtfEditor.Visible = true;
                    break;

                case CodeType.XML:
                    CodeInsight.Instance.SetInsightHandler(null);
                    _mainform.fastColoredTextBox.Language = FastColoredTextBoxNS.Language.XML;
                    _mainform.containerCode.Visible = true;
                    _mainform.containerRtfEditor.Visible = false;
                    break;

                case CodeType.JS:
                    CodeInsight.Instance.SetInsightHandler(null);
                    _mainform.fastColoredTextBox.Language = FastColoredTextBoxNS.Language.JS;
                    _mainform.containerCode.Visible = true;
                    _mainform.containerRtfEditor.Visible = false;
                    break;

                case CodeType.PHP:
                    CodeInsight.Instance.SetInsightHandler(null);
                    _mainform.fastColoredTextBox.Language = FastColoredTextBoxNS.Language.PHP;
                    _mainform.containerCode.Visible = true;
                    _mainform.containerRtfEditor.Visible = false;
                    break;

                case CodeType.Lua:
                    CodeInsight.Instance.SetInsightHandler(null);
                    _mainform.fastColoredTextBox.Language = FastColoredTextBoxNS.Language.Lua;
                    _mainform.containerCode.Visible = true;
                    _mainform.containerRtfEditor.Visible = false;
                    break;
            }
            UpdateHtmlPreview();
        }

        public void SetState(CodeSnippet snippet)
        {
            SaveState();

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

            _ActiveTextBoxHelper.SetState(snippet);
        }

        public void SetStateNoSave(CodeSnippet snippet)
        {
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

            _ActiveTextBoxHelper.SetState(snippet);
        }

        public void ShowFindDialog() => _ActiveTextBoxHelper.ShowFindDialog();

        public void ShowReplaceDialog() => _ActiveTextBoxHelper.ShowReplaceDialog();

        public void SwitchHtmlPreview() => _ActiveTextBoxHelper.SwitchHtmlPreview();

        public bool SwitchWordWrap() => _ActiveTextBoxHelper.SwitchWordWrap();

        public void UpdateHtmlPreview() => _ActiveTextBoxHelper.UpdateHtmlPreview();

        private void SetEditorCodeType(CodeType type)
        {
            if (type == CodeType.RTF)
            {
                _ActiveTextBoxHelper = _RtfEditorHelper;
                _mainform.CurrentEditor.Editor = _RtfEditorHelper.Editor;
                SetEditorView(type);
            }
            else
            {
                _ActiveTextBoxHelper = _FastColoredTextBoxHelper;
                _mainform.CurrentEditor.Editor = _FastColoredTextBoxHelper.Editor;
                SetEditorView(type);
                _FastColoredTextBoxHelper.RefreshEditor();
            }
        }
    }
}