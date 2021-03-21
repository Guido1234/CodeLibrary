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
----------------------------------------------------------------------
Version 2.x:
-   Improved TreeView behavior: changed behavior of the short cut keys - they now respond to KeyDown instead of the KeyUp event for better response

Version 2.2:
-   Added Setting Screen
    -   Sort Mode
    -   Theme

-   New: Manage Favorites

-   State Icons (right corner)
    -   Debug Mode
    -   Document Changed
    -   Password / USB locked
    -   Clipboard monitor on

-   Exit without saving warning dialog.

-   Browse file in Restore Backup

-   USBKey.
    Use a usb flash drive as a passkey container instead of using a password.
    -   There can be only one usb flash drive marked as a USBKey inserted in the usb ports.
    -   When creating an usb key drive, there can only be one usb drive in the ports.
    -   When an USBKey is created multiple files can be created using this usb flash drive.
    -   Each new file protected with a USBKey has an unique identifier which matches the keyfiles in: [Drive]:\EncrKeys
    -   You will never be prompted for a password, only to insert the USBKey flash drive when it's not presents or the required key does not exist.
    -   The USBKey is required for Saving and for loading, keys are not kept in memory and are read from the drive when needed.

-   Improved TreeView Load.

-   Improved Sort ASC, Important sorted first, Folders sorted second then other notes.

-   New database field 'CodeLastModificationDate'.
    This field is set when the 'Code' field changes.
    This field is currently only shown in the search results.

-   Added new commands to Library management (treeview):

        - Sort children ascending (A-Z).
            This command sorts the children of the selected node in ascending order.

        - MoveLeft
            This moves the selected item to the left.
            This meaning it will be decoupled from to parent and it will be put just after parent (as a Next node)

        - MoveRight
            This moves the selected item to the right.
            This meains it will be put as a child under the previous node and the last item.

-   Added some shortcuts to Library management (in treeview) and dialogs
        - Ctrl + Enter in TreeView
            This will open the properties dialog for the selected node
        - Esc key in Property dialog
            This will close the form

FIXED:
-   Paste text per line misses last one

-   Treeview paste text applies text to current note.

-   AddImageNode ChangeType

-   HTML Preview

-   CreationDate contained incorrect formatting for seconds containing ':nn'.
    The is changed in the getter changed to ':00:00'.
    Please note that the field is still incorrect in the contents of file (Json)
    until the the Code Snippet is changed and saved again.

-   Removed Shortcut keys from Library Ordering Menu.

-   In treeview shortcutkeys:
    -   Ctrl-Up         Move up
    -   Ctrl-Down       Move Down
    -   Ctrl-Shft-Up    Move to Top
    -   Ctrl-Shft-Down  Move to Bottom
    -   Ctrl-Left       Move left
    -   Ctrl-Right      Move right

----------------------------------------------------------------------
Version 2.1:

-   New file container.

-   Ask for password on encrypted file container. (only for new container or saved container).
    When a file without password is opened, the password will be reset.

-   Encrypted files are reminded as well, password dialog will show on opening.

-   Move to Top / Move to Bottom

-   Bugfixes

Version 2.0:

NEW:
-   Rtf document type + Rtf Editor

-   MarkDowm document type
    -   Html Preview will translate markdown script.

-   Add New Dialog box
    -   Add New always shows dialog.
    -   Assign defaults in note properties to supress this behaviour.

-   Backup Manager

-   Plugin: Markdown to HTML

-   [Copy Contents and Merge] option in clipboard context menu. (merges all #[Note Path]# tags)

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

-   Save Image => GDI+ Error message

-   Error, open file with last selection an Image

-   Error, Default child code containing wrong format

-   Error, Default child name containing wrong format

-   Error, Access Denied

-   New Document did not clear the editor.

-   Html Preview not shown on first open.

-   Disabled Javascript errors in webbrowser.

-   Default OK button for Properties window and Addnew dialog

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

----------------------------------------------------------------------
General Public Licence

This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.

Programming: Guido Kleijer
";

        public FormAbout()
        {
            InitializeComponent();
            CancelButton = dialogButton.buttonOk;
            AcceptButton = dialogButton.buttonOk;
        }

        private void DialogButton_DialogButtonClick(object sender, DialogButton.DialogButtonClickEventArgs e)
        {
            Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://sourceforge.net/u/guidok915");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/PavelTorgashov/FastColoredTextBox");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Guido1234/CodeLibrary");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/StackExchange/MarkdownSharp");
        }

        private void TbCode_Load(object sender, EventArgs e)
        {
            lbTitle.Text = $"Code Library";
            lblVersion.Text = $"V{Config.CurrentVersion().ToString() }";
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