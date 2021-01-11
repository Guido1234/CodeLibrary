namespace FastColoredTextBoxNS
{
    public interface ITextEditor
    {
        string SelectedText { get; set; }

        string Text { get; set; }

        int Zoom { get; set; }

        void Copy();

        string CurrentLine();

        void Cut();

        void GotoLine(int line);

        void Paste();

        void SelectAll();

        void SelectLine();

        void ShowFindDialog();

        void ShowReplaceDialog();

        void Undo();
    }
}