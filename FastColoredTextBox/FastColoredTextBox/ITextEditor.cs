using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastColoredTextBoxNS
{
    public  interface ITextEditor
    {
        void Cut();

        void Copy();

        void Paste();

        void ShowFindDialog();

        void ShowReplaceDialog();

        void Undo();

        void GotoLine(int line);

        void SelectAll();

        void SelectLine();

        int Zoom { get; set; }

        string SelectedText { get; set; }

        string Text { get; set; }

        string CurrentLine();

    }
}
