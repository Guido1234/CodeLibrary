namespace CodeLibrary.Core
{
    public interface ITextBoxHelper
    {
        string SelectedText { get; set; }

        string Text { get; set; }

        void BringToFront();

        void CodeToScreen(CodeSnippet snippet);

        void Save();

        void Copy();

        void Cut();

        void Focus();

        void GotoLine();

        void GotoLine(int line);

        string CurrentLine();

        void Paste();

        void ScreenToCode(CodeSnippet snippet);

        void SelectAll();

        void SelectLine();

        void ShowFindDialog();

        void ShowReplaceDialog();

        bool SwitchWordWrap();
 
        void ApplySnippetSettings();

    }
}