using CodeLibrary.Core;
using CodeLibrary.Editor;
using CodeLibrary.Helpers;
using EditorPlugins.Engine;
using FastColoredTextBoxNS;
using GK.Template;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CodeLibrary
{
    public partial class FormCodeLibrary : Form
    {
        private readonly ClipboardMonitorHelper _clipboardMonitorHelper;
        private readonly DebugHelper _debugHelper;
        private readonly FavoriteHelper _FavoriteHelper;
        private readonly FileHelper _fileHelper;
        private readonly MenuHelper _menuHelper;
        private readonly PasswordHelper _passwordHelper;
        private readonly TextBoxHelper _textboxHelper;
        private readonly ThemeHelper _themeHelper;
        private readonly TreeviewHelper _treeHelper;
        private TextEditorContainer _CurrentEditor = new TextEditorContainer();
        private bool _exitWithoutSaving = false;
        private MainPluginHelper _PluginHelper;
        private string _preSearchSelectedId = string.Empty;

        public FormCodeLibrary()
        {
            InitializeComponent();
            DoubleBuffered = true;
            _debugHelper = new DebugHelper(this);
            _textboxHelper = new TextBoxHelper(this);
            _themeHelper = new ThemeHelper(this);
            _passwordHelper = new PasswordHelper(this);
            _fileHelper = new FileHelper(this, _debugHelper, _textboxHelper, _passwordHelper);
            _treeHelper = new TreeviewHelper(this, _textboxHelper, _fileHelper, _themeHelper);
            _fileHelper.TreeHelper = _treeHelper;
            _FavoriteHelper = new FavoriteHelper(this, _fileHelper);
            _clipboardMonitorHelper = new ClipboardMonitorHelper(this, _textboxHelper, _treeHelper);
            _menuHelper = new MenuHelper(this, _treeHelper);

            containerLeft.Dock = DockStyle.Fill;

            treeViewLibrary.Top = 29;
            treeViewLibrary.Left = 0;
            treeViewLibrary.Width = containerTreeview.Width + 1;
            treeViewLibrary.Height = containerTreeview.Height - 29;

            mncChangeType.DropDownOpening += mncChangeType_DropDownOpening;
            mnuChangeType.DropDownOpening += mnuChangeType_DropDownOpening;

            containerCode.Location = new Point(0, 28);
            containerCode.Size = new Size(splitContainerMain.Panel2.Width, splitContainerMain.Panel2.Height - 52);
            containerCode.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            containerRtfEditor.Location = new Point(0, 28);
            containerRtfEditor.Size = new Size(splitContainerMain.Panel2.Width, splitContainerMain.Panel2.Height - 52);
            containerRtfEditor.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            rtfEditor.Dock = DockStyle.Fill;

            webBrowser.Dock = DockStyle.Fill;
            webBrowser.ScriptErrorsSuppressed = true;
            webBrowser.AllowWebBrowserDrop = false;
            webBrowser.DocumentText = "";

            splitContainerCode.Dock = DockStyle.Fill;

            containerImage.Location = new Point(0, 28);
            containerImage.Size = new Size(splitContainerMain.Panel2.Width, splitContainerMain.Panel2.Height - 52);
            containerImage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            imageViewer.Dock = DockStyle.Fill;

            containerCode.BringToFront();

            fastColoredTextBox.Dock = DockStyle.Fill;

            containerTreeview.BringToFront();
        }

        public TextEditorContainer CurrentEditor
        {
            get
            {
                return _CurrentEditor;
            }
        }

        public void SaveEditor() => _textboxHelper.SaveState();

        public void SetZoom()
        {
            hScrollBarZoom.Value = Config.Zoom;
            fastColoredTextBox.Zoom = hScrollBarZoom.Value;
            rtfEditor.Zoom = hScrollBarZoom.Value;
            labelZoomPerc.Text = $"{fastColoredTextBox.Zoom}%";
        }

        internal void SetEditor(ITextEditor editor)
        {
            _CurrentEditor.Editor = editor;
        }

        private void AddNote()
        {
            if (_treeHelper.IsSystem(treeViewLibrary.SelectedNode))
                return;

            var _newNode = _treeHelper.CreateNewNodeWindowed(treeViewLibrary.SelectedNode);
            if (_newNode == null)
                return;

            treeViewLibrary.SelectedNode = _newNode;
            _textboxHelper.Focus();
        }

        private void ButtonFind_Click(object sender, EventArgs e) => FindNode();

        private void EditNodeProperties(bool keepFocus = false)
        {
            if (_treeHelper.IsSystem(treeViewLibrary.SelectedNode) || treeViewLibrary.SelectedNode == null)
                return;

            CodeSnippet _snippet = _treeHelper.FromNode(treeViewLibrary.SelectedNode);
            FormProperties _form = new FormProperties(_themeHelper) { Snippet = _snippet };
            _form.ShowDialog(this);

            CodeLib.Instance.Refresh();

            _treeHelper.RefreshCurrentTreeNode();
            if (keepFocus)
            {
                //
            }
            else
            {
                fastColoredTextBox.Focus();
            }
        }

        private void FindNode()
        {
            _preSearchSelectedId = string.IsNullOrEmpty(_treeHelper.SelectedId) ? _preSearchSelectedId : _treeHelper.SelectedId;

            Cursor.Current = Cursors.WaitCursor;
            Dictionary<string, TreeNode> _result = _fileHelper.CodeCollectionToForm(textBoxFind.Text);
            if (_result.ContainsKey(_preSearchSelectedId))
            {
                _treeHelper.SetSelectedNode(_result[_preSearchSelectedId], true);
            }

            Cursor.Current = Cursors.Default;
        }

        private void FormCodeLibrary_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_exitWithoutSaving)
                return;

            if (!string.IsNullOrEmpty(_fileHelper.CurrentFile))
                Config.LastOpenedFile = _fileHelper.CurrentFile;

            _fileHelper.SaveFile(false);

            Config.SplitterDistance = this.splitContainerMain.SplitterDistance;

            Config.Save();
        }

        private void FormCodeLibrary_Load(object sender, EventArgs e)
        {
            Enabled = false;
            splitContainerCode.Panel2Collapsed = true;
            Config.Load();

            splitContainerMain.SplitterDistance = Config.SplitterDistance;

            _themeHelper.SetTheme(Config.Theme);

            Application.DoEvents();

            rtfEditor.UpdateStyles();

            webBrowser.Document.OpenNew(true);

            tbPath.BorderStyle = BorderStyle.FixedSingle;

            if (string.IsNullOrEmpty(Config.LastOpenedFile))
                _fileHelper.NewDoc();

            _PluginHelper = new MainPluginHelper(_CurrentEditor, mnuPlugins, mncPlugins);

            if (Config.IsNewVersion())
            {
                FormAbout _frmAbout = new FormAbout();
                _frmAbout.ShowDialog(this);
            }
            Application.DoEvents();
            _fileHelper.Reload();
            _FavoriteHelper.BuildMenu();

            SetZoom();
            Enabled = true;
        }

        private void HScrollBarZoom_Scroll(object sender, ScrollEventArgs e)
        {
            fastColoredTextBox.Zoom = hScrollBarZoom.Value;
            rtfEditor.Zoom = hScrollBarZoom.Value;
            labelZoomPerc.Text = $"{fastColoredTextBox.Zoom}%";
            Config.Zoom = fastColoredTextBox.Zoom;
        }

        private void mncAdd_Click(object sender, EventArgs e) => AddNote();

        private void mncAsSelection_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(fastColoredTextBox.SelectedText))
                return;

            treeViewLibrary.SelectedNode.Text = fastColoredTextBox.SelectedText;
        }

        private void mncChangeType_DropDownOpening(object sender, EventArgs e) => _treeHelper.SetTypeMenuState();

        private void mncCopy_Click(object sender, EventArgs e) => _textboxHelper.Copy();

        private void mncCopyContentsAndMerge_Click(object sender, EventArgs e) => Clipboard.SetText(_textboxHelper.Merge());

        private void mncCopyPath_Click(object sender, EventArgs e)
        {
            string path = treeViewLibrary.SelectedNode.FullPath;
            Clipboard.SetText($"#[{path}]#");
        }

        private void mncCut_Click(object sender, EventArgs e) => _textboxHelper.Cut();

        private void mncDelete_Click(object sender, EventArgs e) => _treeHelper.DeleteSelectedNode();

        private void mncEmptyTrashcan_Click(object sender, EventArgs e) => _treeHelper.EmptyTrashcan();

        private void mncMarkImportant_Click(object sender, EventArgs e) => _treeHelper.MarkImportant();

        private void mncMoveDown_Click(object sender, EventArgs e) => _treeHelper.MoveDown();

        private void mncMoveLeft_Click(object sender, EventArgs e) => _treeHelper.MoveToLeft();

        private void mncMoveRight_Click(object sender, EventArgs e) => _treeHelper.MoveToRight();

        private void mncMoveToBottom_Click(object sender, EventArgs e) => _treeHelper.MoveToBottom();

        private void mncMoveToTop_Click(object sender, EventArgs e) => _treeHelper.MoveToTop();

        private void mncMoveUp_Click(object sender, EventArgs e) => _treeHelper.MoveUp();

        private void mncPaste_Click(object sender, EventArgs e) => _textboxHelper.Paste();

        private void mncPasteSpecial_Click(object sender, EventArgs e)
        {
            StringTemplate stringtemplate = new StringTemplate();
            string result = stringtemplate.Format(Clipboard.GetText(), _textboxHelper.SelectedText);
            _textboxHelper.SelectedText = result;
        }

        private void mncProperties_Click(object sender, EventArgs e) => EditNodeProperties();

        private void mncSelectAll_Click(object sender, EventArgs e) => _textboxHelper.SelectAll();

        private void mncSelectLine_Click(object sender, EventArgs e) => _textboxHelper.SelectLine();

        private void mncSortChildrenAscending_Click(object sender, EventArgs e) => _treeHelper.SortChildren();

        private void mncTypeCSharp_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.CSharp);

        private void mncTypeFolder_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.Folder);

        private void mncTypeHtml_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.HTML);

        private void mncTypeJS_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.JS);

        private void mncTypeLua_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.Lua);

        private void mncTypeMarkDown_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.MarkDown);

        private void mncTypeNone_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.None);

        private void mncTypePhp_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.PHP);

        private void mncTypeRtf_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.RTF);

        private void mncTypeSql_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.SQL);

        private void mncTypeTemplate_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.Template);

        private void mncTypeVB_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.VB);

        private void mncTypeXml_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.XML);

        private void mncWithMarkup_Click(object sender, EventArgs e) => _textboxHelper.CopyWithMarkup();

        private void mnuAbout_Click(object sender, EventArgs e)
        {
            FormAbout _frmAbout = new FormAbout();
            _frmAbout.ShowDialog(this);
        }

        private void mnuAdd_Click(object sender, EventArgs e) => AddNote();

        private void mnuAddCurrentToFavorite_Click(object sender, EventArgs e) => _FavoriteHelper.AddCurrentToFavorite();

        private void mnuAddDialog_Click(object sender, EventArgs e)
        {
            if (_treeHelper.IsSystem(treeViewLibrary.SelectedNode))
                return;
            var _newNode = _treeHelper.CreateNewNodeWindowedDialog(treeViewLibrary.SelectedNode);
            if (_newNode == null)
                return;

            treeViewLibrary.SelectedNode = _newNode;

            fastColoredTextBox.Focus();
        }

        private void mnuChangeType_DropDownOpening(object sender, EventArgs e) => _treeHelper.SetTypeMenuState();

        private void mnuClearPassword_Click(object sender, EventArgs e) => _passwordHelper.ClearPassWord();

        private void mnuConfigurePlugins_Click(object sender, EventArgs e)
        {
            PluginConfigurator pc = new PluginConfigurator { Plugins = _PluginHelper.Plugins };
            pc.ShowDialog();
            _PluginHelper.SaveCustomSettings();
        }

        private void mnuCopy_Click(object sender, EventArgs e) => _textboxHelper.Copy();

        private void mnuCopyContentsAndMerge_Click(object sender, EventArgs e) => Clipboard.SetText(_textboxHelper.Merge());

        private void mnuCopyPath_Click(object sender, EventArgs e)
        {
            string path = treeViewLibrary.SelectedNode.FullPath;
            Clipboard.SetText($"#[{path}]#");
        }

        private void mnuCopyWithMarkup_Click(object sender, EventArgs e) => _textboxHelper.CopyWithMarkup();

        private void mnuCut_Click(object sender, EventArgs e) => _textboxHelper.Cut();

        private void mnuDelete_Click(object sender, EventArgs e) => _treeHelper.DeleteSelectedNode();

        private void mnuDemoProject_Click(object sender, EventArgs e) => _FavoriteHelper.OpenDemo();

        private void mnuExampleLibrary_Click(object sender, EventArgs e) => _FavoriteHelper.OpenCSharpLibrary();

        private void mnuExit_Click(object sender, EventArgs e) => Close();

        private void mnuExitWithoutSaving_Click(object sender, EventArgs e)
        {
            _exitWithoutSaving = true;
            Close();
        }

        private void mnuFind_Click(object sender, EventArgs e) => _textboxHelper.ShowFindDialog();

        private void mnuHTMLPreview_Click(object sender, EventArgs e) => _textboxHelper.SwitchHtmlPreview();

        private void mnuLoad_Click(object sender, EventArgs e) => _fileHelper.OpenFile();

        private void mnuMarkDown_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.MarkDown);

        private void mnuMarkImportant_Click(object sender, EventArgs e) => _treeHelper.MarkImportant();

        private void mnuModeDark_Click(object sender, EventArgs e) => _themeHelper.DarkTheme();

        private void mnuModeHighContrast_Click(object sender, EventArgs e) => _themeHelper.HighContrastTheme();

        private void mnuModeLight_Click(object sender, EventArgs e) => _themeHelper.LightTheme();

        private void mnuMoveDown_Click(object sender, EventArgs e) => _treeHelper.MoveDown();

        private void mnuMoveLeft_Click(object sender, EventArgs e) => _treeHelper.MoveToLeft();

        private void mnuMoveRight_Click(object sender, EventArgs e) => _treeHelper.MoveToRight();

        private void mnuMoveToBottom_Click(object sender, EventArgs e) => _treeHelper.MoveToBottom();

        private void mnuMoveToTop_Click(object sender, EventArgs e) => _treeHelper.MoveToTop();

        private void mnuMoveUp_Click(object sender, EventArgs e) => _treeHelper.MoveUp();

        private void mnuNew_Click(object sender, EventArgs e) => _fileHelper.NewDoc();

        private void mnuPaste_Click(object sender, EventArgs e) => _textboxHelper.Paste();

        private void mnuPasteSpecial_Click(object sender, EventArgs e)
        {
            StringTemplate stringtemplate = new StringTemplate();
            string result = stringtemplate.Format(Clipboard.GetText(), _textboxHelper.SelectedText);
            _textboxHelper.SelectedText = result;
        }

        private void mnuPlugins_DropDownOpening(object sender, EventArgs e) => _PluginHelper.SetMenuState(mnuPlugins);

        private void mnuProperties_Click(object sender, EventArgs e) => EditNodeProperties();

        private void mnuQuickRename_Click(object sender, EventArgs e)
        {
            if (_treeHelper.IsSystem(treeViewLibrary.SelectedNode))
                return;

            ToolStripMenuItem menu = sender as ToolStripMenuItem;
            if (sender is null)
                return;

            if (treeViewLibrary.SelectedNode == null)
                return;

            treeViewLibrary.SelectedNode.Text = DateTime.Now.ToString(menu.Text);
        }

        private void mnuRemoveCurrentFromFavorite_Click(object sender, EventArgs e) => _FavoriteHelper.RemoveCurrentFromFavorite();

        private void mnuRenameAsSelectedText_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_textboxHelper.SelectedText))
                return;

            treeViewLibrary.SelectedNode.Text = _textboxHelper.SelectedText;
        }

        private void mnuReplace_Click(object sender, EventArgs e) => _textboxHelper.ShowReplaceDialog();

        private void mnuRestoreBackup_Click(object sender, EventArgs e) => _fileHelper.RestoreBackup();

        private void mnuSave_Click(object sender, EventArgs e) => _fileHelper.SaveFile(false);

        private void mnuSaveAs_Click(object sender, EventArgs e) => _fileHelper.SaveFile(true);

        private void mnuSearch_Click(object sender, EventArgs e)
        {
            FormSearch _formSearch = new FormSearch();
            DialogResult _r = _formSearch.ShowDialog();
            if (_r == DialogResult.OK)
            {
                _treeHelper.FindNodeByPath(_formSearch.SelectedPath);
            }
        }

        private void mnuSelectAll_Click(object sender, EventArgs e) => _textboxHelper.SelectAll();

        private void mnuSelectLine_Click(object sender, EventArgs e) => _textboxHelper.SelectLine();

        private void mnuSetPassword_Click(object sender, EventArgs e) => _passwordHelper.SetPassWord();

        private void mnuSetUsbKey_Click(object sender, EventArgs e) => _passwordHelper.SetUsbKey();

        private void mnuSortChildrenAscending_Click(object sender, EventArgs e) => _treeHelper.SortChildren();

        private void mnuSwitchLast2Documents_Click(object sender, EventArgs e) => _treeHelper.SwitchLastTwo();

        private void mnuTypeCSharp_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.CSharp);

        private void mnuTypeFolder_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.Folder);

        private void mnuTypeHTML_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.HTML);

        private void mnuTypeJs_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.JS);

        private void mnuTypeLua_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.Lua);

        private void mnuTypeNone_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.None);

        private void mnuTypePhp_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.PHP);

        private void mnuTypeRtf_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.RTF);

        private void mnuTypeSql_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.SQL);

        private void mnuTypeTemplate_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.Template);

        private void mnuTypeVB_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.VB);

        private void mnuTypeXML_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.XML);

        private void mnuWordwrap_Click(object sender, EventArgs e) => _textboxHelper.SwitchWordWrap();

        private void PluginsToolStripContextMenu_DropDownOpening(object sender, EventArgs e) => _PluginHelper.SetMenuState(mncPlugins);

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSettings f = new FormSettings();
            DialogResult _r = f.ShowDialog();
            if (_r == DialogResult.OK)
            {
                _themeHelper.SetTheme(Config.Theme);
            }
        }

        private void TextBoxFind_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    FindNode();
                    break;

                case Keys.Escape:
                    this.textBoxFind.Text = string.Empty;
                    FindNode();

                    break;
            }
        }

        private void treeViewLibrary_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Enter)
            {
                EditNodeProperties(true);
            }
        }
    }
}