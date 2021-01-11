using CodeLibrary.Controls;
using CodeLibrary.Core;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CodeLibrary
{
    public partial class FormAbout : Form
    {
        private readonly string _Content = @"
Version 2.0:

NEW:
-   Rtf document type + Rtf Editor

-   Add New Dialog box
    -   Add New always shows dialog.
    -   Assign defaults in note properties to supress this behaviour.

-   Backup Manager

CHANGES:
-   Clipboard monitor now has an own Note, clipboard monitor can be activated and deactivated in it's context menu.
    the clipboard monitor will only paste changes within this note.

-   Removed Default Note Settings

-   Removed Bookmark functionality

-   Re-designed Note properties window.

-   Reorganized Library Menu

-   Reorganized Edit Menu

-   Reorganized File Menu

-   Removed themes from dialog windows (only main screen uses themes).

-   Note links #[Note Path]# will be merged in Html Preview. (links created by [Copy Path] )

BUG FIXES:
-   Syntax Highlight not working on first note.

-   HtmlPreview window not properly initialized.

-   Wordwrap not applied.

-   Syntax Highlight not applied directly on Change Type

-   Template insight dropdown unusable small.

-   Tree icon does not change when changing type.

-   Errors / Situations in Empty project while.

-   Error while switching to deleted favorite.

-   Drag drop system notes should not be allowed.

----------------------------------------------------------------------
Version 1.9:
-   Added Clipboard menu to Note context menu item.
    -   Menu items depending on clipboard content.
    -   Added paste image without compression.

-   Image context menu (rightclick on image itself)
    -   Added 'copy as Base64 String'
    -   Added 'copy as Html IMG'

-   Image can be moved around.

-   Adding notes beneath images.

-   Images default orginal size.

-   Html Preview window for Html / Xml

----------------------------------------------------------------------
Version 1.8:
-   Paste in Treeviewer (Ctrl-V)
    -   FileList: inserts all files below current note.
    -   Text: Note contents contains clipboard contents.
    -   Image: Paste image into Treeviewer, Image is displayed instead of code editor.

-   Paste in Treeviewer (Ctrl-Shift-V): Splits text in lines and creates a note for every line.

-   Drag / Drop into treeviewer supports images (jpg / png / bmp)

----------------------------------------------------------------------
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

----------------------------------------------------------------------
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

----------------------------------------------------------------------
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
            lbTitle.Text = $"Code Library V{Config.CurrentVersion().ToString() }";
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