using CodeLibrary.Controls;
using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CodeLibrary.Helpers
{
    public class BookmarkHelper
    {
        private readonly ToolStripMenuItem _BookMarkItemsMenu;
        private readonly List<CodeBookmark> _Bookmarks = new List<CodeBookmark>();
        private readonly CollectionListBox _CollectionListBoxBookmarks;
        private readonly FastColoredTextBoxHelper _FastColoredTextBoxHelper;
        private readonly CodeSnippet _Snippet;
        private readonly TreeviewHelper _TreeviewHelper;

        public BookmarkHelper(TreeviewHelper treeviewHelper, ToolStripMenuItem bookMarkItemsMenu, FastColoredTextBoxHelper fastColoredTextBoxHelper, CollectionListBox collectionListBoxBookmarks)
        {
            _Snippet = null;
            _TreeviewHelper = treeviewHelper;
            _BookMarkItemsMenu = bookMarkItemsMenu;
            _FastColoredTextBoxHelper = fastColoredTextBoxHelper;
            _FastColoredTextBoxHelper.FastColoredTextBox.BookMarkEvent += FastColoredTextBox_BookMarkEvent;
            _CollectionListBoxBookmarks = collectionListBoxBookmarks;
            _CollectionListBoxBookmarks.CollectionType = typeof(CodeBookmark);
            _CollectionListBoxBookmarks.AfterLabelEdit += CollectionListBoxBookmarks_AfterLabelEdit;
            _CollectionListBoxBookmarks.ItemSelected += CollectionListBoxBookmarks_ItemSelected;
        }

        public BookmarkHelper(TreeviewHelper treeviewHelper, CodeSnippet snippet, ToolStripMenuItem bookMarkItemsMenu, FastColoredTextBoxHelper fastColoredTextBoxHelper, CollectionListBox collectionListBoxBookmarks)
        {
            _Snippet = snippet;
            _TreeviewHelper = treeviewHelper;
            _BookMarkItemsMenu = bookMarkItemsMenu;
            _FastColoredTextBoxHelper = fastColoredTextBoxHelper;
            _FastColoredTextBoxHelper.FastColoredTextBox.BookMarkEvent += FastColoredTextBox_BookMarkEvent;
            _CollectionListBoxBookmarks = collectionListBoxBookmarks;
            _CollectionListBoxBookmarks.CollectionType = typeof(CodeBookmark);
        }

        public void BuildMenu()
        {
            _BookMarkItemsMenu.DropDownItems.Clear();
            _Bookmarks.Clear();

            if (_Snippet == null)
            {
                foreach (CodeSnippet snippet in CodeLib.Instance.Library)
                    AddSnippetBookMarks(snippet);
            }
            else
            {
                AddSnippetBookMarks(_Snippet);
            }

            _CollectionListBoxBookmarks.Refresh();
        }

        public void SetBookmark()
        {
            _FastColoredTextBoxHelper.FastColoredTextBox.BookmarkLine();
            _FastColoredTextBoxHelper.ScreenToCode(_FastColoredTextBoxHelper.CurrentSnippet);

            BuildMenu();
        }

        private void AddSnippetBookMarks(CodeSnippet snippet)
        {
            if (snippet.HasBookMarks())
            {
                foreach (CodeBookmark bm in snippet.Bookmarks)
                {
                    if (string.IsNullOrEmpty(bm.Description))
                    {
                        bm.Description = snippet.Path;
                    }
                    var _name = string.IsNullOrEmpty(bm.Description) ? $"{snippet.Path} - {bm.LineNumber}" : bm.Description;
                    var _menuItem = _BookMarkItemsMenu.DropDownItems.Add(_name);
                    _menuItem.Tag = bm;
                    _menuItem.MouseUp += MenuItem_MouseUp;
                    _Bookmarks.Add(bm);
                }
            }

            _CollectionListBoxBookmarks.SetCollection(_Bookmarks);
        }

        private void CollectionListBoxBookmarks_AfterLabelEdit(object sender, AfterLabelEditEventArgs e)
        {
            CodeBookmark _bm = (CodeBookmark)e.Tag;
            _bm.Description = e.NewLabel;
        }

        private void CollectionListBoxBookmarks_ItemSelected(object sender, CollectionListBox.CollectionListBoxEventArgs e)
        {
            CodeBookmark _bm = e.Item as CodeBookmark;
            if (_bm == null)
                return;

            GotoBm(_bm);
        }

        private void FastColoredTextBox_BookMarkEvent(object sender, BookMarkEventArgs e)
        {
            if (_FastColoredTextBoxHelper.CurrentSnippet == null)
                return;

            if (_FastColoredTextBoxHelper.CurrentSnippet.Bookmarks == null)
                _FastColoredTextBoxHelper.CurrentSnippet.Bookmarks = new List<CodeBookmark>();

            _FastColoredTextBoxHelper.CurrentSnippet.Bookmarks.Clear();

            foreach (Bookmark bookmark in _FastColoredTextBoxHelper.FastColoredTextBox.Bookmarks)
            {
                CodeBookmark codeBookmark = new CodeBookmark() { LineNumber = bookmark.LineIndex, Name = _FastColoredTextBoxHelper.CurrentSnippet.Name };
                _FastColoredTextBoxHelper.CurrentSnippet.Bookmarks.Add(codeBookmark);
            }

            BuildMenu();
        }

        private CodeBookmark GetBookMark(object sender)
        {
            CodeBookmark _bm = null;

            ToolStripItem _mi = sender as ToolStripItem;
            if (_mi != null)
            {
                _bm = _mi.Tag as CodeBookmark;
                if (_bm == null)
                    return null;
            }

            return _bm;
        }

        private void GotoBm(CodeBookmark _bm)
        {
            if (_FastColoredTextBoxHelper.CurrentSnippet.Name == _bm.Name)
            {
                _FastColoredTextBoxHelper.GotoLine(_bm.LineNumber);
                return;
            }

            _TreeviewHelper.FindNodeById(_bm.Name);
            _FastColoredTextBoxHelper.GotoLine(_bm.LineNumber);
        }

        private void MenuItem_Click(object sender, EventArgs e)
        {
            CodeBookmark _bm = GetBookMark(sender);
            if (_bm == null)
                return;

            GotoBm(_bm);
        }

        private void MenuItem_MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    MenuItem_Click(sender, e);
                    break;

                case MouseButtons.Right:
                    RenameBookMarkMenuItem(sender);
                    break;
            }
        }

        private void RenameBookMarkMenuItem(object sender)
        {
            CodeBookmark _bm = GetBookMark(sender);
            if (_bm == null)
                return;

            FormInput f = new FormInput
            {
                InputText = _bm.Description,
                Message = "Bookmark name"
            };

            var _r = f.ShowDialog();
            if (_r == DialogResult.Cancel)
                return;

            _bm.Description = f.InputText;

            BuildMenu();
        }
    }
}