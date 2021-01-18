namespace CodeLibrary.Core
{
    public interface ITextBoxHelper
    {
        CodeSnippet GetStateSnippet();

        string Merge();

        string SelectedText { get; set; }

        string Text { get; set; }

        void ApplySnippetSettings();

        void BringToFront();

        void SetState(CodeSnippet snippet);

        void Copy();

        string CurrentLine();

        void Cut();

        void Focus();

        void GotoLine();

        void GotoLine(int line);

        void Paste();
        void SaveState();

        void SelectAll();

        void SelectLine();

        void ShowFindDialog();

        void ShowReplaceDialog();

        bool SwitchWordWrap();

        void UpdateHtmlPreview();
    }
}