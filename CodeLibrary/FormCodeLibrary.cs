using CodeLibrary.Core;
using CodeLibrary.Helpers;
using EditorPlugins.Engine;
using FastColoredTextBoxNS;
using GK.Template;
using GK.Template.Methods;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security;
using System.Windows.Forms;

namespace CodeLibrary
{
    public partial class FormCodeLibrary : Form
    {
        private readonly ClipboardMonitorHelper _clipboardMonitorHelper;
        private readonly DebugHelper _debugHelper;
        private readonly FavoriteHelper _FavoriteHelper;
        private readonly FileHelper _fileHelper;
        private readonly TextBoxHelper _textboxHelper;
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
            _fileHelper = new FileHelper(treeViewLibrary, this, _debugHelper);
            _FavoriteHelper = new FavoriteHelper(favoriteLibrariesToolStripMenuItem, _fileHelper);
            _textboxHelper = new TextBoxHelper(this);
            _treeHelper = new TreeviewHelper(this, _textboxHelper, _fileHelper);
            _clipboardMonitorHelper = new ClipboardMonitorHelper(this, _textboxHelper, _treeHelper);

            _fileHelper.TreeHelper = _treeHelper;

            containerLeft.Dock = DockStyle.Fill;

            treeViewLibrary.Top = 29;
            treeViewLibrary.Left = 0;
            treeViewLibrary.Width = containerTreeview.Width;
            treeViewLibrary.Height = containerTreeview.Height - 29;

            mnuChangeType.DropDownOpening += MnuChangeType_DropDownOpening;
            mnuChangeType1.DropDownOpening += MnuChangeType2_DropDownOpening;

            containerCode.Location = new Point(0, 28);
            containerCode.Size = new Size(splitContainer1.Panel2.Width, splitContainer1.Panel2.Height - 52);
            containerCode.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            containerRtfEditor.Location = new Point(0, 28);
            containerRtfEditor.Size = new Size(splitContainer1.Panel2.Width, splitContainer1.Panel2.Height - 52);
            containerRtfEditor.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            rtfEditor.Dock = DockStyle.Fill;

            webBrowser.Dock = DockStyle.Fill;
            webBrowser.ScriptErrorsSuppressed = true;
            webBrowser.AllowWebBrowserDrop = false;
            webBrowser.DocumentText = "";

            splitContainerCode.Dock = DockStyle.Fill;

            containerImage.Location = new Point(0, 28);
            containerImage.Size = new Size(splitContainer1.Panel2.Width, splitContainer1.Panel2.Height - 52);
            containerImage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            imageViewer.Dock = DockStyle.Fill;

            containerCode.BringToFront();

            tbCode.Dock = DockStyle.Fill;

            containerTreeview.BringToFront();
        }

        public TextEditorContainer CurrentEditor
        {
            get
            {
                return _CurrentEditor;
            }
        }

        public void SaveEditor() => _textboxHelper.Save();

        public void SetZoom()
        {
            hScrollBarZoom.Value = Config.Zoom;
            tbCode.Zoom = hScrollBarZoom.Value;
            rtfEditor.Zoom = hScrollBarZoom.Value;
            labelZoomPerc.Text = $"{tbCode.Zoom}%";
        }

        internal void SetEditor(ITextEditor editor)
        {
            _CurrentEditor.Editor = editor;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAbout _frmAbout = new FormAbout();
            _frmAbout.ShowDialog(this);
        }

        private void AddCurrentToFavoriteToolStripMenuItem_Click(object sender, EventArgs e) => _FavoriteHelper.AddCurrentToFavorite();

        private void AddNote()
        {
            if (_treeHelper.IsSystem(treeViewLibrary.SelectedNode))
                return;

            var _newNode = _treeHelper.CreateNewNodeWindowed(treeViewLibrary.SelectedNode);
            if (_newNode == null)
                return;

            treeViewLibrary.SelectedNode = _newNode;
            tbCode.Focus();
        }

        private void addNoteDialogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_treeHelper.IsSystem(treeViewLibrary.SelectedNode))
                return;
            var _newNode = _treeHelper.CreateNewNodeWindowedDialog(treeViewLibrary.SelectedNode);
            if (_newNode == null)
                return;

            treeViewLibrary.SelectedNode = _newNode;
            tbCode.Focus();
        }

        private void AddToolStripMenuItem_Click(object sender, EventArgs e) => AddNote();

        private void addToolStripMenuItem_Click_1(object sender, EventArgs e) => AddNote();

        private void asSelectedTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_textboxHelper.SelectedText))
                return;

            treeViewLibrary.SelectedNode.Text = _textboxHelper.SelectedText;
        }

        private void asSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbCode.SelectedText))
                return;

            treeViewLibrary.SelectedNode.Text = tbCode.SelectedText;
        }

        private void ButtonFind_Click(object sender, EventArgs e) => FindNode();

        private void ClearPassWord()
        {
            _fileHelper.Password = null;
            tbPath.BackColor = SystemColors.ButtonFace;
        }

        private void ClearPasswordToolStripMenuItem_Click(object sender, EventArgs e) => ClearPassWord();

        private void ConfigurePluginsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PluginConfigurator pc = new PluginConfigurator { Plugins = _PluginHelper.Plugins };
            pc.ShowDialog();
            _PluginHelper.SaveCustomSettings();
        }

        private void copyPathToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string path = treeViewLibrary.SelectedNode.FullPath;
            Clipboard.SetText($"#[{path}]#");
        }

        private void copyPathToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            string path = treeViewLibrary.SelectedNode.FullPath;
            Clipboard.SetText($"#[{path}]#");
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e) => _textboxHelper.Copy();

        private void CopyToolStripMenuItem1_Click(object sender, EventArgs e) => _textboxHelper.Copy();

        private void CopyWithMarkupToolStripMenuItem_Click(object sender, EventArgs e) => _textboxHelper.CopyWithMarkup();

        private void CopyWithMarkupToolStripMenuItem1_Click(object sender, EventArgs e) => _textboxHelper.CopyWithMarkup();

        private void CToolStripMenuItem_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.CSharp);

        private void CToolStripMenuItem1_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.CSharp);

        private void CutToolStripMenuItem_Click(object sender, EventArgs e) => _textboxHelper.Cut();

        private void CutToolStripMenuItem1_Click(object sender, EventArgs e) => _textboxHelper.Cut();

        private void DarkToolStripMenuItem_Click(object sender, EventArgs e) => DarkTheme();

        private void demoProjectToolStripMenuItem_Click(object sender, EventArgs e) => _FavoriteHelper.OpenDemo();

        private void EditNodeProperties()
        {
            if (_treeHelper.IsSystem(treeViewLibrary.SelectedNode))
                return;

            CodeSnippet _snippet = _treeHelper.FromNode(treeViewLibrary.SelectedNode);
            FormProperties _form = new FormProperties { Snippet = _snippet };
            _form.ShowDialog(this);

            CodeLib.Instance.Refresh();

            _treeHelper.RefreshCurrentTreeNode();

            tbCode.Focus();
        }

        private void EmptyTrashcanToolStripMenuItem_Click(object sender, EventArgs e) => _treeHelper.EmptyTrashcan();

        private void EvalualteExpressionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MethodCalc methodCalc = new MethodCalc();
            _textboxHelper.SelectedText = methodCalc.Apply(_textboxHelper.SelectedText);
        }

        private void EvaluateExpressionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MethodCalc methodCalc = new MethodCalc();
            _textboxHelper.SelectedText = methodCalc.Apply(_textboxHelper.SelectedText);
        }

        private void EvaluateExpressionToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MethodCalc methodCalc = new MethodCalc();
            _textboxHelper.SelectedText = _textboxHelper.SelectedText + " = " + methodCalc.Apply(_textboxHelper.SelectedText);
        }

        private void EvaluateExpressionToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            MethodCalc methodCalc = new MethodCalc();
            _textboxHelper.SelectedText = _textboxHelper.SelectedText + " = " + methodCalc.Apply(_textboxHelper.SelectedText);
        }

        private void exampleLibraryToolStripMenuItem_Click(object sender, EventArgs e) => _FavoriteHelper.OpenCSharpLibrary();

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e) => Close();

        private void ExitWithoutSavingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _exitWithoutSaving = true;
            Close();
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

        private void FindToolStripMenuItem_Click(object sender, EventArgs e) => _textboxHelper.ShowFindDialog();

        private void FolderToolStripMenuItem_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.Folder);

        private void FolderToolStripMenuItem1_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.Folder);

        private void FormCodeLibrary_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_exitWithoutSaving)
                return;

            if (_fileHelper.Password == null)
            {
                if (!string.IsNullOrEmpty(_fileHelper.CurrentFile))
                    Config.LastOpenedFile = _fileHelper.CurrentFile;
            }

            _fileHelper.SaveFile(false);

            Config.Save();
        }

        private void FormCodeLibrary_Load(object sender, EventArgs e)
        {
            this.Enabled = false;
            splitContainerCode.Panel2Collapsed = true;
            Config.Load();

            if (Config.HighContrastMode)
                HighContrastTheme();
            else if (Config.DarkMode)
                DarkTheme();
            else
                LightTheme();

            Application.DoEvents();

            rtfEditor.UpdateStyles();

            webBrowser.Document.OpenNew(true);

            tbPath.BorderStyle = BorderStyle.FixedSingle;

            if (string.IsNullOrEmpty(Config.LastOpenedFile))
                _fileHelper.NewDoc();

            _PluginHelper = new MainPluginHelper(_CurrentEditor, pluginsToolStripEditMenuItem, pluginsToolStripContextMenu);

            if (Config.IsNewVersion())
            {
                FormAbout _frmAbout = new FormAbout();
                _frmAbout.ShowDialog(this);
            }
            Application.DoEvents();
            _fileHelper.Reload();
            _FavoriteHelper.BuildMenu();

            SetZoom();
            this.Enabled = true;
        }

        private void HighContrastToolStripMenuItem_Click(object sender, EventArgs e) => HighContrastTheme();

        private void HScrollBarZoom_Scroll(object sender, ScrollEventArgs e)
        {
            tbCode.Zoom = hScrollBarZoom.Value;
            rtfEditor.Zoom = hScrollBarZoom.Value;
            labelZoomPerc.Text = $"{tbCode.Zoom}%";
            Config.Zoom = tbCode.Zoom;
        }

        private void hTMLPreviewToolStripMenuItem_Click(object sender, EventArgs e) => _textboxHelper.SwitchHtmlPreview();

        private void HTMLToolStripMenuItem_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.HTML);

        private void HTMLToolStripMenuItem1_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.HTML);

        private void InsertDateTimeToolStripMenuItem_Click(object sender, EventArgs e) => _textboxHelper.SelectedText = DateTime.Now.ToString();

        private void InsertDateTimeToolStripMenuItem1_Click(object sender, EventArgs e) => _textboxHelper.SelectedText = DateTime.Now.ToString();

        private void InsertGuidToolStripMenuItem_Click(object sender, EventArgs e) => _textboxHelper.SelectedText = Guid.NewGuid().ToString();

        private void InsertGuidToolStripMenuItem1_Click(object sender, EventArgs e) => _textboxHelper.SelectedText = Guid.NewGuid().ToString();

        private void jSToolStripMenuItem_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.JS);

        private void jSToolStripMenuItem1_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.JS);

        private void LightToolStripMenuItem_Click(object sender, EventArgs e) => LightTheme();

        private void LoadToolStripMenuItem_Click(object sender, EventArgs e) => _fileHelper.OpenFile();

        private void luaToolStripMenuItem_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.Lua);

        private void luaToolStripMenuItem1_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.Lua);

        private void MarkImportantToolStripMenuItem_Click(object sender, EventArgs e) => _treeHelper.MarkImportant();

        private void MarkImportantToolStripMenuItem1_Click(object sender, EventArgs e) => _treeHelper.MarkImportant();

        private void MnuChangeType_DropDownOpening(object sender, EventArgs e) => _treeHelper.SetTypeMenuState();

        private void MnuChangeType2_DropDownOpening(object sender, EventArgs e) => _treeHelper.SetTypeMenuState();

        #region themes

        private void DarkTheme()
        {
            Config.DarkMode = true;
            Config.HighContrastMode = false;
            darkToolStripMenuItem.Checked = true;
            lightToolStripMenuItem.Checked = false;
            highContrastToolStripMenuItem.Checked = false;

            textBoxFind.BackColor = Color.FromArgb(255, 40, 40, 40);
            textBoxFind.ForeColor = Color.LightYellow;
            buttonFind.BackColor = Color.FromArgb(255, 100, 100, 100);
            buttonFind.ForeColor = Color.White;

            menuStrip1.ForeColor = Color.White;
            menuStrip1.BackColor = Color.FromArgb(255, 100, 100, 100);

            mainToolStripMenuItem.BackColor = Color.FromArgb(255, 100, 100, 100);
            ForeColor = Color.White;
            BackColor = Color.FromArgb(255, 100, 100, 100);
            treeViewLibrary.ForeColor = Color.White;
            treeViewLibrary.BackColor = Color.FromArgb(255, 75, 75, 75);
            rtfEditor.Theme = RtfTheme.Dark;

            tbCode.IndentBackColor = Color.FromArgb(255, 75, 75, 75);
            tbCode.BackColor = Color.FromArgb(255, 40, 40, 40);
            tbCode.CaretColor = Color.White;
            tbCode.ForeColor = Color.LightGray;
            tbCode.SelectionColor = Color.Red;
            tbCode.LineNumberColor = Color.LightSeaGreen;
            tbPath.ForeColor = Color.White;
            tbPath.BackColor = Color.FromArgb(255, 100, 100, 100);
            pictureBox1.BackColor = Color.FromArgb(255, 100, 100, 100);
            containerTreeview.BackColor = Color.FromArgb(255, 75, 75, 75);
            containerLeft.BackColor = Color.FromArgb(255, 75, 75, 75);

            containerImage.BackColor = Color.FromArgb(255, 75, 75, 75);
            containerCode.BackColor = Color.FromArgb(255, 75, 75, 75);
            containerRtfEditor.BackColor = Color.FromArgb(255, 75, 75, 75);

            containerInfoBar.BackColor = Color.FromArgb(255, 75, 75, 75);
            label2.ForeColor = Color.FromArgb(255, 255, 255, 255);
            label4.ForeColor = Color.FromArgb(255, 255, 255, 255);
            lblStart.ForeColor = Color.FromArgb(255, 255, 255, 255);
            lblEnd.ForeColor = Color.FromArgb(255, 255, 255, 255);
            labelZoomPerc.ForeColor = Color.FromArgb(255, 255, 255, 255);

            imageViewer.BackColor = Color.FromArgb(255, 0, 0, 0);

            tbCode.DarkStyle();
            tbCode.Refresh();
        }

        private void HighContrastTheme()
        {
            Config.DarkMode = true;
            Config.HighContrastMode = true;
            darkToolStripMenuItem.Checked = false;
            lightToolStripMenuItem.Checked = false;
            highContrastToolStripMenuItem.Checked = true;

            textBoxFind.BackColor = Color.FromArgb(255, 10, 10, 10);
            textBoxFind.ForeColor = Color.LightYellow;
            buttonFind.BackColor = Color.FromArgb(255, 60, 60, 60);
            buttonFind.ForeColor = Color.White;

            menuStrip1.ForeColor = Color.White;
            menuStrip1.BackColor = Color.FromArgb(255, 60, 60, 60);

            mainToolStripMenuItem.BackColor = Color.FromArgb(255, 60, 60, 60);
            ForeColor = Color.White;
            BackColor = Color.FromArgb(255, 100, 100, 100);
            treeViewLibrary.ForeColor = Color.White;
            treeViewLibrary.BackColor = Color.FromArgb(255, 35, 35, 35);
            tbCode.IndentBackColor = Color.FromArgb(255, 35, 35, 35);
            rtfEditor.Theme = RtfTheme.HighContrast;

            tbCode.BackColor = Color.FromArgb(255, 10, 10, 10);
            tbCode.CaretColor = Color.White;
            tbCode.ForeColor = Color.LightGray;
            tbCode.SelectionColor = Color.Red;
            tbCode.LineNumberColor = Color.LightSeaGreen;
            tbPath.ForeColor = Color.White;
            tbPath.BackColor = Color.FromArgb(255, 60, 60, 60);
            pictureBox1.BackColor = Color.FromArgb(255, 60, 60, 60);
            containerTreeview.BackColor = Color.FromArgb(255, 35, 35, 35);
            containerLeft.BackColor = Color.FromArgb(255, 35, 35, 35);

            containerInfoBar.BackColor = Color.FromArgb(255, 35, 35, 35);

            containerImage.BackColor = Color.FromArgb(255, 35, 35, 35);
            containerCode.BackColor = Color.FromArgb(255, 35, 35, 35);
            containerRtfEditor.BackColor = Color.FromArgb(255, 35, 35, 35);

            label2.ForeColor = Color.FromArgb(255, 255, 255, 255);
            label4.ForeColor = Color.FromArgb(255, 255, 255, 255);
            lblStart.ForeColor = Color.FromArgb(255, 255, 255, 255);
            lblEnd.ForeColor = Color.FromArgb(255, 255, 255, 255);
            labelZoomPerc.ForeColor = Color.FromArgb(255, 255, 255, 255);

            imageViewer.BackColor = Color.FromArgb(255, 0, 0, 0);

            tbCode.HighContrastStyle();
            tbCode.Refresh();
        }

        private void LightTheme()
        {
            Config.DarkMode = false;
            Config.HighContrastMode = false;
            darkToolStripMenuItem.Checked = false;
            lightToolStripMenuItem.Checked = true;
            highContrastToolStripMenuItem.Checked = false;

            textBoxFind.BackColor = Color.White;
            textBoxFind.ForeColor = Color.Black;
            buttonFind.BackColor = SystemColors.ButtonFace;
            buttonFind.ForeColor = Color.Black;

            menuStrip1.ForeColor = Color.FromArgb(255, 0, 0, 0);
            menuStrip1.BackColor = Color.FromArgb(255, 240, 240, 240);
            ForeColor = Color.FromArgb(255, 0, 0, 0);
            BackColor = Color.FromArgb(255, 240, 240, 240);
            treeViewLibrary.ForeColor = Color.FromArgb(255, 0, 0, 0);
            treeViewLibrary.BackColor = Color.FromArgb(255, 255, 255, 255);
            tbCode.IndentBackColor = Color.FromArgb(255, 255, 255, 255);
            rtfEditor.Theme = RtfTheme.Light;

            tbCode.BackColor = Color.FromArgb(255, 255, 255, 255);
            tbCode.ForeColor = Color.Black;
            tbCode.CaretColor = Color.White;
            tbCode.SelectionColor = Color.FromArgb(50, 0, 0, 255);
            tbCode.LineNumberColor = Color.FromArgb(255, 0, 128, 128);
            tbPath.ForeColor = Color.Black;
            tbPath.BackColor = Color.FromArgb(255, 240, 240, 240);
            pictureBox1.BackColor = Color.FromArgb(255, 100, 100, 100);

            contextMenuStripLibrary.BackColor = SystemColors.ButtonFace;
            containerTreeview.BackColor = Color.FromArgb(255, 255, 255, 255);

            containerTreeview.BackColor = Color.FromArgb(255, 255, 255, 255);
            containerLeft.BackColor = Color.FromArgb(255, 255, 255, 255);

            containerImage.BackColor = Color.FromArgb(255, 255, 255, 255);
            containerCode.BackColor = Color.FromArgb(255, 255, 255, 255);
            containerRtfEditor.BackColor = Color.FromArgb(255, 255, 255, 255);

            containerInfoBar.BackColor = Color.FromArgb(255, 255, 255, 255);
            label2.ForeColor = Color.FromArgb(255, 0, 0, 0);
            label4.ForeColor = Color.FromArgb(255, 0, 0, 0);
            lblStart.ForeColor = Color.FromArgb(255, 0, 0, 0);
            lblEnd.ForeColor = Color.FromArgb(255, 0, 0, 0);
            labelZoomPerc.ForeColor = Color.FromArgb(255, 0, 0, 0);

            imageViewer.BackColor = Color.FromArgb(255, 125, 125, 125);

            tbCode.LightStyle();
            tbCode.Refresh();
        }

        #endregion themes

        private void mnuMarkDown_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.MarkDown);

        private void mnuMarkDown1_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.MarkDown);

        private void MoveDownToolStripMenuItem_Click(object sender, EventArgs e) => _treeHelper.MoveDown();

        private void MoveDownToolStripMenuItem1_Click(object sender, EventArgs e) => _treeHelper.MoveDown();

        private void MoveUpToolStripMenuItem_Click(object sender, EventArgs e) => _treeHelper.MoveUp();

        private void MoveUpToolStripMenuItem1_Click(object sender, EventArgs e) => _treeHelper.MoveUp();

        private void MultiSelectToolStripMenuItem_Click(object sender, EventArgs e) => this.treeViewLibrary.CheckBoxes = !this.treeViewLibrary.CheckBoxes;

        private void NewToolStripMenuItem_Click(object sender, EventArgs e) => _fileHelper.NewDoc();

        private void NoneToolStripMenuItem_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.None);

        private void NoneToolStripMenuItem1_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.None);

        private void PasteSpecialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringTemplate stringtemplate = new StringTemplate();
            string result = stringtemplate.Format(Clipboard.GetText(), _textboxHelper.SelectedText);
            _textboxHelper.SelectedText = result;
        }

        private void PasteSpecialToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            StringTemplate stringtemplate = new StringTemplate();
            string result = stringtemplate.Format(Clipboard.GetText(), _textboxHelper.SelectedText);
            _textboxHelper.SelectedText = result;
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e) => _textboxHelper.Paste();

        private void PasteToolStripMenuItem1_Click(object sender, EventArgs e) => _textboxHelper.Paste();

        private void pHPToolStripMenuItem_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.PHP);

        private void pHPToolStripMenuItem1_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.PHP);

        private void PluginsToolStripContextMenu_DropDownOpening(object sender, EventArgs e) => _PluginHelper.SetMenuState(pluginsToolStripContextMenu);

        private void PluginsToolStripEditMenuItem_DropDownOpening(object sender, EventArgs e) => _PluginHelper.SetMenuState(pluginsToolStripEditMenuItem);

        private void PropertiesToolStripMenuItem_Click(object sender, EventArgs e) => EditNodeProperties();

        private void propertiesToolStripMenuItem1_Click(object sender, EventArgs e) => EditNodeProperties();

        private void QuickRename(object sender, EventArgs e)
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

        private void RemoveCurrentFromFavoriteToolStripMenuItem_Click(object sender, EventArgs e) => _FavoriteHelper.RemoveCurrentFromFavorite();

        private void RemoveToolStripMenuItem_Click(object sender, EventArgs e) => _treeHelper.DeleteSelectedNode();

        private void RemoveToolStripMenuItem1_Click(object sender, EventArgs e) => _treeHelper.DeleteSelectedNode();

        private void RenameNoteToSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbCode.SelectedText))
                return;

            treeViewLibrary.SelectedNode.Text = tbCode.SelectedText;
        }

        private void ReplaceToolStripMenuItem_Click(object sender, EventArgs e) => _textboxHelper.ShowReplaceDialog();

        private void restoreBackupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _fileHelper.RestoreBackup();
        }

        private void rTFToolStripMenuItem_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.RTF);

        private void rTFToolStripMenuItem1_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.RTF);

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e) => _fileHelper.SaveFile(true);

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e) => _fileHelper.SaveFile(false);

        private void SearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSearch _formSearch = new FormSearch();
            DialogResult _r = _formSearch.ShowDialog();
            if (_r == DialogResult.OK)
            {
                _treeHelper.FindNodeByPath(_formSearch.SelectedPath);
            }
        }

        private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e) => _textboxHelper.SelectAll();

        private void SelectAllToolStripMenuItem1_Click(object sender, EventArgs e) => _textboxHelper.SelectAll();

        private void SelectLineToolStripMenuItem_Click(object sender, EventArgs e) => _textboxHelper.SelectLine();

        private void SetPassWord(SecureString password)
        {
            _fileHelper.Password = password;
            tbPath.BackColor = Color.DarkGray;
        }

        private void SetPasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSetPassword f = new FormSetPassword();
            DialogResult result = f.ShowDialog();

            if (result == DialogResult.OK)
                SetPassWord(f.Password);
        }

        private void SortChildrenToolStripMenuItem_Click(object sender, EventArgs e) => treeViewLibrary.Sort();

        private void SortChildrenToolStripMenuItem1_Click(object sender, EventArgs e) => treeViewLibrary.Sort();

        private void SQLToolStripMenuItem_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.SQL);

        private void SQLToolStripMenuItem1_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.SQL);

        private void switchLast2DocumentsToolStripMenuItem1_Click(object sender, EventArgs e) => _treeHelper.SwitchLastTwo();

        private void TbCode_SelectionChanged(object sender, EventArgs e)
        {
            lblStart.Text = tbCode.Selection.Start.ToString();
            lblEnd.Text = tbCode.Selection.End.ToString();
        }

        private void TemplateToolStripMenuItem_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.Template);

        private void TemplateToolStripMenuItem1_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.Template);

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

        private void ToolStripButtonCopy_Click(object sender, EventArgs e) => _textboxHelper.CopyWithMarkup();

        private void ToolStripButtonCut_Click(object sender, EventArgs e) => _textboxHelper.CutWithMarkup();

        private void ToolStripButtonOpenFile_Click(object sender, EventArgs e) => _fileHelper.OpenFile();

        private void ToolStripButtonPaste_Click(object sender, EventArgs e) => _textboxHelper.Paste();

        private void ToolStripButtonSaveFile_Click(object sender, EventArgs e) => _fileHelper.SaveFile(false);

        private void ToolStripMenuItem35_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbCode.SelectedText))
                return;

            treeViewLibrary.SelectedNode.Text = tbCode.SelectedText;
        }

        private void ToolStripMenuItem8_Click(object sender, EventArgs e) => _textboxHelper.SelectLine();

        private void UpperCaseToolStripMenuItem_Click(object sender, EventArgs e) => _textboxHelper.SelectedText = _textboxHelper.SelectedText.ToUpper();

        private void UpperCaseToolStripMenuItem1_Click(object sender, EventArgs e) => _textboxHelper.SelectedText = _textboxHelper.SelectedText.ToUpper();

        private void VBToolStripMenuItem_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.VB);

        private void VBToolStripMenuItemChangeType_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.VB);

        private void WordwrapToolStripMenuItem_Click(object sender, EventArgs e) => _textboxHelper.SwitchWordWrap();

        private void xMLToolStripMenuItem_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.XML);

        private void xMLToolStripMenuItem1_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.XML);

        private void xToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var item in CodeLib.Instance.Library)
            {
                item.CreationDate = DateTime.MinValue.ToString("yyyyMMdd hh:nn"); 
            }
        }

        private void munCopyContentsAndMerge_Click(object sender, EventArgs e) => Clipboard.SetText(_textboxHelper.Merge());

        private void munCopyContentsAndMerge1_Click(object sender, EventArgs e) => Clipboard.SetText( _textboxHelper.Merge());

    }
}