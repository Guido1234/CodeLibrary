namespace CodeLibrary.Core
{
    public interface ITextBoxHelper
    {
        string SelectedText { get; set; }

        string Text { get; set; }

        void ApplySnippetSettings();

        void BringToFront();

        void CodeToScreen(CodeSnippet snippet);

        void Copy();

        string CurrentLine();

        void Cut();

        void Focus();

        void GotoLine();

        void GotoLine(int line);

        void Paste();

        void Save();

        void ScreenToCode(CodeSnippet snippet);

        void SelectAll();

        void SelectLine();

        void ShowFindDialog();

        void ShowReplaceDialog();

        bool SwitchWordWrap();
    }
}