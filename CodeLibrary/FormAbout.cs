using CodeLibrary.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CodeLibrary
{
    public partial class FormAbout : Form
    {
        private readonly string _Content = @"
Version 1.8:
-   Paste in Treeviewer (Ctrl-V) 
    -   FileList: inserts all files below current note.
    -   Text: Note contents contains clipboard contents.
    -   Image: Paste image into Treeviewer, Image is displayed instead of code editor.

-   Paste in Treeviewer (Ctrl-Shift-V): Splits text in lines and creates a note for every line.

-   Drag / Drop into treeviewer supports images (jpg / png / bmp)


Version 1.7:
-   Fixed Version number bug.

-   Library Search (Search global in all documents)

-   Assign shortcut keys to note, Note can be used as a template (Paste Special).
    Shortcut keys can be set in the Note properties window.

-   Added Note properties window.

-   Removed fontsize / added zoom control.

-   F12 swtiches between last two documents

-   Added 2 plugins: Remove Lines Containing, Remove Lines Not Containing

-   Added KeepQuoted to C# plugins: remove all text except for text enclosed within double qoutes.

-   Added a Template function KeepQouted.

Version 1.6:

-   Clipboard monitor
    (Auto paste clipboard contents in current document when the clipboard contents changes.)

-   Option to remove current from Favorite Library.

-   Favorite Library shortcuts automatically have a shortcut key (Ctrl-F1 - Ctrl-F12)

-   Bookmarks overview.

-   Drop files into browser.

-   Drop file into editor.

-   In memmory password uses SecureString.

-   Keep selection after clearing search.

-   Message when opening non library file (or wrong password) instead of C# error message.

-   Remind Last line on note.

-   New Plugin: Encoding\Import as base64

Version 1.5:

-   Favorite Libraries Menu
    (Switching Favorite Library always saves implicit.)

-   Note Defaults.
    create default text / title for Childs.

-   Template Type.
    Templates are used for Paste Special.
    (Template Type not required for Paste Special)

-   Bookmarks

-   High Contrast Mode

-   Paste Special
    --------------------------------------------
    Use formatting on clipboard to merge with selected Text.

    Exampe 1
    --------

    Clipboard contents:
    ""{0}"",

    Selected Text:
    Id
    Name

    CTRL-T Result:
    ""Id"",""Name"",

    Exampe 2
    --------

    Clipboard content:
    private {0} {1:CamelCaseLower};

    public {0} {1:CamelCaseUpper}
    {
        get
        {
            return {0} {1:CamelCaseLower};
        }
        set
        {
            {1:CamelCaseLower} = value;
        }
    }

    Selected text:
    int;Id
    string;Name

    Result:
    int;Id
    string;Name

    Note 1: Each line in selected text will be seen as a record!
    Note 2: Formatting is not the same as C# string formatting.

    0:MethodAsciiValue()

";

        public FormAbout()
        {
            InitializeComponent();
        }

        private void DialogButton_DialogButtonClick(object sender, DialogButton.DialogButtonClickEventArgs e)
        {
            Close();
        }

        private void TbCode_Load(object sender, EventArgs e)
        {
            tbCode.Language = FastColoredTextBoxNS.Language.Custom;
            tbCode.SelectedText = _Content;
            tbCode.IndentBackColor = Color.FromArgb(255, 35, 35, 35);
            tbCode.BackColor = Color.FromArgb(255, 10, 10, 10);
            tbCode.CaretColor = Color.White;
            tbCode.ForeColor = Color.LightGray;
            tbCode.SelectionColor = Color.Red;
            tbCode.LineNumberColor = Color.LightSeaGreen;
            tbCode.HighContrastStyle();
            tbCode.WordWrap = true;
            tbCode.GotoLine(0);
        }
    }
}