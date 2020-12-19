using CodeLibrary.Helpers;
using EditorPlugins.Engine;
using GK.Template;
using GK.Template.Methods;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security;
using System.Windows.Forms;

namespace CodeLibrary
{
    public partial class FormCodeLibrary : Form
    {
        private readonly BookmarkHelper _bookmarkHelper;
        private readonly FastColoredTextBoxHelper _fastColoredTextBoxBoxHelper;
        private readonly FavoriteHelper _FavoriteHelper;
        private readonly FileHelper _fileHelper;
        private readonly TextSelectionHelper _selectionHelper;
        private readonly TreeviewHelper _treeHelper;
        private bool _exitWithoutSaving = false;
        private MainPluginHelper _PluginHelper;

        private string _preSearchSelectedId = string.Empty;

        private string _prevClipboard = string.Empty;

        public FormCodeLibrary()
        {
            InitializeComponent();
            _fileHelper = new FileHelper(treeViewLibrary, this);
            _FavoriteHelper = new FavoriteHelper(favoriteLibrariesToolStripMenuItem, _fileHelper);
            _fastColoredTextBoxBoxHelper = new FastColoredTextBoxHelper(tbCode, tbPath, this, contextMenuStripPopup, listBoxInsight);
            _selectionHelper = new TextSelectionHelper(tbCode);
            _treeHelper = new TreeviewHelper(this, _fastColoredTextBoxBoxHelper, _fileHelper);
            _fileHelper.TreeHelper = _treeHelper;
            _bookmarkHelper = new BookmarkHelper(_treeHelper, bookMarkItemsMenu, _fastColoredTextBoxBoxHelper, collectionListBoxBookmarks);

            containerLeft.Dock = DockStyle.Fill;
            timerClipboard.Enabled = false;
            timerClipboard.Interval = 100;
            timerClipboard.Tick += TimerClipboard_Tick;

            treeViewLibrary.Top = 29;
            treeViewLibrary.Left = 0;
            treeViewLibrary.Width = containerTreeview.Width;
            treeViewLibrary.Height = containerTreeview.Height - 29;

            containerCode.Location = new Point(0, 28);
            containerCode.Size = new Size(splitContainer1.Panel2.Width, splitContainer1.Panel2.Height - 52);
            containerCode.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;


            containerImage.Location = new Point(0, 28);
            containerImage.Size = new Size(splitContainer1.Panel2.Width, splitContainer1.Panel2.Height - 52);
            containerImage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            imageViewer.Dock = DockStyle.Fill;

            containerCode.BringToFront();

            tbCode.Dock = DockStyle.Fill;


            containerBookmarks.Parent = containerLeft;
            containerBookmarks.Dock = DockStyle.Fill;



            containerTreeview.BringToFront();
        }


        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAbout _frmAbout = new FormAbout();
            _frmAbout.ShowDialog(this);
        }

        private void AddAndPasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeViewLibrary.SelectedNode == null)
                return;

            this.treeViewLibrary.SelectedNode = _treeHelper.CreateNewNode(treeViewLibrary.SelectedNode);
            tbCode.Focus();
            tbCode.Paste();
        }

        private void AddCurrentToFavoriteToolStripMenuItem_Click(object sender, EventArgs e) => _FavoriteHelper.AddCurrentToFavorite();

        private void AddFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeViewLibrary.SelectedNode != null)
            {
                this.treeViewLibrary.SelectedNode = _treeHelper.CreateNewFolderNode(treeViewLibrary.SelectedNode);
                tbCode.Focus();
                return;
            }
            this.treeViewLibrary.SelectedNode = _treeHelper.CreateNewRootFolderNode();
            tbCode.Focus();
        }

        private void AddFolderToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (_treeHelper.IsSystem(treeViewLibrary.SelectedNode))
                return;

            if (treeViewLibrary.SelectedNode != null)
            {
                this.treeViewLibrary.SelectedNode = _treeHelper.CreateNewFolderNode(treeViewLibrary.SelectedNode);
                tbCode.Focus();
                return;
            }
            this.treeViewLibrary.SelectedNode = _treeHelper.CreateNewRootFolderNode();
            tbCode.Focus();
        }

        private void AddFolderToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            this.treeViewLibrary.SelectedNode = _treeHelper.CreateNewRootNode(CodeType.Folder, "New Folder", string.Empty);
            tbCode.Focus();
        }

        private void AddRootToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeViewLibrary.SelectedNode = _treeHelper.CreateNewRootNode();
            tbCode.Focus();
        }

        private void AddTempToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_fileHelper.TempNode != null)
            {
                treeViewLibrary.SelectedNode = _treeHelper.CreateNewNode(_fileHelper.TempNode);
                tbCode.Focus();
                return;
            }
        }

        private void AddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_treeHelper.IsSystem(treeViewLibrary.SelectedNode))
                return;

            if (treeViewLibrary.SelectedNode != null)
            {
                treeViewLibrary.SelectedNode = _treeHelper.CreateNewNode(treeViewLibrary.SelectedNode);
                tbCode.Focus();
                return;
            }
            this.treeViewLibrary.SelectedNode = _treeHelper.CreateNewRootNode();
            tbCode.Focus();
        }

        private void AddToolStripMenuItem2_Click(object sender, EventArgs e) => _bookmarkHelper.SetBookmark();

        private void bookmarksToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            containerBookmarks.BringToFront();
        }

        private void browserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            containerTreeview.BringToFront();
        }

        private void ButtonFind_Click(object sender, EventArgs e)
        {
            FindNode();
        }

        private void ClearPassWord()
        {
            _fileHelper.Password = null;
            tbPath.BackColor = SystemColors.ButtonFace;
        }

        private void ClearPasswordToolStripMenuItem_Click(object sender, EventArgs e) => ClearPassWord();

        private void ClipboardMonitorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clipboardMonitorToolStripMenuItem.Checked = !clipboardMonitorToolStripMenuItem.Checked;
            timerClipboard.Enabled = clipboardMonitorToolStripMenuItem.Checked;
        }

        private void ConfigurePluginsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PluginConfigurator pc = new PluginConfigurator { Plugins = _PluginHelper.Plugins };
            pc.ShowDialog();
            _PluginHelper.SaveCustomSettings();
        }

        private void ContainerTreeview_Click(object sender, EventArgs e)
        {
        }

        private void ContextMenuStripLibrary_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        { }

        private void CopyPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = treeViewLibrary.SelectedNode.FullPath;
            Clipboard.SetText($"#[{path}]#");
        }

        private void CopyPathToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string path = treeViewLibrary.SelectedNode.FullPath;
            Clipboard.SetText($"#[{path}]#");
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxClipboard.Text = _fastColoredTextBoxBoxHelper.SelectedText;
            if (!string.IsNullOrEmpty(textBoxClipboard.Text))
                Clipboard.SetText(textBoxClipboard.Text, TextDataFormat.Text);
            else
                Clipboard.Clear();
        }

        private void CopyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            textBoxClipboard.Text = _fastColoredTextBoxBoxHelper.SelectedText;
            if (!string.IsNullOrEmpty(textBoxClipboard.Text))
                Clipboard.SetText(textBoxClipboard.Text, TextDataFormat.Text);
            else
                Clipboard.Clear();
        }

        private void CopyWithMarkupToolStripMenuItem_Click(object sender, EventArgs e) => _fastColoredTextBoxBoxHelper.Copy();

        private void CopyWithMarkupToolStripMenuItem1_Click(object sender, EventArgs e) => _fastColoredTextBoxBoxHelper.Copy();

        private void CToolStripMenuItem_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.CSharp);

        private void CToolStripMenuItem1_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.CSharp);

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxClipboard.Text = _fastColoredTextBoxBoxHelper.SelectedText;
            _fastColoredTextBoxBoxHelper.SelectedText = string.Empty;
            if (!string.IsNullOrEmpty(textBoxClipboard.Text))
                Clipboard.SetText(textBoxClipboard.Text, TextDataFormat.Text);
            else
                Clipboard.Clear();
        }

        private void CutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            textBoxClipboard.Text = _fastColoredTextBoxBoxHelper.SelectedText;
            _fastColoredTextBoxBoxHelper.SelectedText = string.Empty;
            if (!string.IsNullOrEmpty(textBoxClipboard.Text))
                Clipboard.SetText(textBoxClipboard.Text, TextDataFormat.Text);
            else
                Clipboard.Clear();
        }


        private void DarkToolStripMenuItem_Click(object sender, EventArgs e) => DarkTheme();

        private void EditNodeDefaults()
        {
            if (_treeHelper.IsSystem(treeViewLibrary.SelectedNode))
                return;

            CodeSnippet _snippet = _treeHelper.FromNode(treeViewLibrary.SelectedNode);
            FormDefaults _form = new FormDefaults
            {
                DefaultCode = _snippet.DefaultChildCode,
                DefaultName = _snippet.DefaultChildName,
                DefaultCodeType = _snippet.DefaultChildCodeType,
                DefaultCodeTypeEnabled = _snippet.DefaultChildCodeTypeEnabled
            };
            DialogResult _result = _form.ShowDialog(this);

            if (_result == DialogResult.OK)
            {
                _snippet.DefaultChildCode = _form.DefaultCode;
                _snippet.DefaultChildName = _form.DefaultName;
                _snippet.DefaultChildCodeType = _form.DefaultCodeType;
                _snippet.DefaultChildCodeTypeEnabled = _form.DefaultCodeTypeEnabled;
            }
            tbCode.Focus();
        }

        private void EmptyTrashcanToolStripMenuItem_Click(object sender, EventArgs e) => _treeHelper.EmptyTrashcan();

        private void EvalualteExpressionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MethodCalc methodCalc = new MethodCalc();
            _fastColoredTextBoxBoxHelper.SelectedText = methodCalc.Apply(_fastColoredTextBoxBoxHelper.SelectedText);
        }

        private void EvaluateExpressionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MethodCalc methodCalc = new MethodCalc();
            _fastColoredTextBoxBoxHelper.SelectedText = methodCalc.Apply(_fastColoredTextBoxBoxHelper.SelectedText);
        }

        private void EvaluateExpressionToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MethodCalc methodCalc = new MethodCalc();
            _fastColoredTextBoxBoxHelper.SelectedText = _fastColoredTextBoxBoxHelper.SelectedText + " = " + methodCalc.Apply(_fastColoredTextBoxBoxHelper.SelectedText);
        }

        private void EvaluateExpressionToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            MethodCalc methodCalc = new MethodCalc();
            _fastColoredTextBoxBoxHelper.SelectedText = _fastColoredTextBoxBoxHelper.SelectedText + " = " + methodCalc.Apply(_fastColoredTextBoxBoxHelper.SelectedText);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e) => Close();

        private void ExitWithoutSavingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _exitWithoutSaving = true;
            Close();
        }

        private void ExportLibraryToolStripMenuItem_Click(object sender, EventArgs e)
        { }

        private void ExportLibraryToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (treeViewLibrary.SelectedNode != null)
                _fileHelper.SaveFile(true, treeViewLibrary.SelectedNode);
        }

        private void ExportToTextFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_treeHelper.IsSystem(treeViewLibrary.SelectedNode))
                return;

            if (treeViewLibrary.SelectedNode == null)
                return;

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "Export text file",
                FileName = treeViewLibrary.SelectedNode.Text
            };
            DialogResult _dr = saveFileDialog.ShowDialog();
            if (_dr != DialogResult.OK)
                return;

            CodeSnippet snippet = _treeHelper.FromNode(treeViewLibrary.SelectedNode);
            File.WriteAllText(saveFileDialog.FileName, snippet.Code);
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

        private void FindToolStripMenuItem_Click(object sender, EventArgs e) => _fastColoredTextBoxBoxHelper.ShowFindDialog();

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
            Config.Load();
            _fileHelper.Reload();
            _FavoriteHelper.BuildMenu();
            _bookmarkHelper.BuildMenu();
            SetZoom();

            if (Config.HighContrastMode)
                HighContrastTheme();
            else if (Config.DarkMode)
                DarkTheme();
            else
                LightTheme();

            tbPath.BorderStyle = BorderStyle.FixedSingle;

            if (string.IsNullOrEmpty(_fileHelper.CurrentFile))
                _fileHelper.NewDoc();

            _PluginHelper = new MainPluginHelper(tbCode, pluginsToolStripEditMenuItem, pluginsToolStripContextMenu);

            if (Config.IsNewVersion())
            {
                FormAbout _frmAbout = new FormAbout();
                _frmAbout.ShowDialog(this);
            }
        }

        private void GoToSiteToolStripMenuItem_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start("https://sourceforge.net/u/guidok915");



        private void HighContrastToolStripMenuItem_Click(object sender, EventArgs e) => HighContrastTheme();

        private void HTMLToolStripMenuItem_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.HTML);

        private void HTMLToolStripMenuItem1_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.HTML);

        private void ImportFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_treeHelper.IsSystem(treeViewLibrary.SelectedNode))
                return;

            if (treeViewLibrary.SelectedNode == null)
                return;

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Import text or similar file.",
                Multiselect = true
            };
            DialogResult _dr = openFileDialog.ShowDialog();
            if (_dr != DialogResult.OK)
                return;

            TreeNode _lastNode = null;
            this.treeViewLibrary.BeginUpdate();
            foreach (string filename in openFileDialog.FileNames)
            {
                FileInfo fi = new FileInfo(filename);
                string text = File.ReadAllText(fi.FullName);
                _lastNode = _treeHelper.CreateNewNode(treeViewLibrary.SelectedNode.Nodes, CodeType.CSharp, fi.Name, text);
            }
            if (_lastNode != null)
                this.treeViewLibrary.SelectedNode = _lastNode;

            this.treeViewLibrary.EndUpdate();
            tbCode.Focus();
        }

        private void ImportLibraryToolStripMenuItem_Click(object sender, EventArgs e)
        { }

        private void ImportLibraryToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (treeViewLibrary.SelectedNode != null)
                _fileHelper.OpenFile(treeViewLibrary.SelectedNode);
        }

        private void ImportToolStripMenuItem_Click(object sender, EventArgs e) => _fileHelper.ImportFile();

        private void InsertDateTimeToolStripMenuItem_Click(object sender, EventArgs e) => _fastColoredTextBoxBoxHelper.SelectedText = DateTime.Now.ToString();

        private void InsertDateTimeToolStripMenuItem1_Click(object sender, EventArgs e) => _fastColoredTextBoxBoxHelper.SelectedText = DateTime.Now.ToString();

        private void InsertGuidToolStripMenuItem_Click(object sender, EventArgs e) => _fastColoredTextBoxBoxHelper.SelectedText = Guid.NewGuid().ToString();

        private void InsertGuidToolStripMenuItem1_Click(object sender, EventArgs e) => _fastColoredTextBoxBoxHelper.SelectedText = Guid.NewGuid().ToString();

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
            containerBookmarks.BackColor = Color.FromArgb(255, 75, 75, 75); ;
            containerLeft.BackColor = Color.FromArgb(255, 75, 75, 75);
            
            containerInfoBar.BackColor = Color.FromArgb(255, 75, 75, 75);
            label2.ForeColor = Color.FromArgb(255, 255, 255, 255);
            label4.ForeColor = Color.FromArgb(255, 255, 255, 255);
            lblStart.ForeColor = Color.FromArgb(255, 255, 255, 255);
            lblEnd.ForeColor = Color.FromArgb(255, 255, 255, 255);
            labelZoomPerc.ForeColor = Color.FromArgb(255, 255, 255, 255);

            imageViewer.BackColor = Color.FromArgb(255, 0, 0, 0);

            collectionListBoxBookmarks.DarkTheme();

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
            tbCode.BackColor = Color.FromArgb(255, 10, 10, 10);
            tbCode.CaretColor = Color.White;
            tbCode.ForeColor = Color.LightGray;
            tbCode.SelectionColor = Color.Red;
            tbCode.LineNumberColor = Color.LightSeaGreen;
            tbPath.ForeColor = Color.White;
            tbPath.BackColor = Color.FromArgb(255, 60, 60, 60);
            pictureBox1.BackColor = Color.FromArgb(255, 60, 60, 60);
            containerTreeview.BackColor = Color.FromArgb(255, 35, 35, 35);
            containerBookmarks.BackColor = Color.FromArgb(255, 35, 35, 35);
            containerLeft.BackColor = Color.FromArgb(255, 35, 35, 35);
            
            containerInfoBar.BackColor = Color.FromArgb(255, 35, 35, 35);
            label2.ForeColor = Color.FromArgb(255, 255, 255, 255);
            label4.ForeColor = Color.FromArgb(255, 255, 255, 255);
            lblStart.ForeColor = Color.FromArgb(255, 255, 255, 255);
            lblEnd.ForeColor = Color.FromArgb(255, 255, 255, 255);
            labelZoomPerc.ForeColor = Color.FromArgb(255, 255, 255, 255);

            imageViewer.BackColor = Color.FromArgb(255, 0, 0, 0);

            collectionListBoxBookmarks.HighContrastTheme();

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
            containerBookmarks.BackColor = Color.FromArgb(255, 255, 255, 255);
            containerLeft.BackColor = Color.FromArgb(255, 255, 255, 255);
            
            containerInfoBar.BackColor = Color.FromArgb(255, 255, 255, 255);
            label2.ForeColor = Color.FromArgb(255, 0, 0, 0);
            label4.ForeColor = Color.FromArgb(255, 0, 0, 0);
            lblStart.ForeColor = Color.FromArgb(255, 0, 0, 0);
            lblEnd.ForeColor = Color.FromArgb(255, 0, 0, 0);
            labelZoomPerc.ForeColor = Color.FromArgb(255, 0, 0, 0);

            imageViewer.BackColor = Color.FromArgb(255, 125, 125, 125); 

            collectionListBoxBookmarks.HighContrastTheme();

            tbCode.LightStyle();
            tbCode.Refresh();
        }

        private void LightToolStripMenuItem_Click(object sender, EventArgs e) => LightTheme();

        private void LoadToolStripMenuItem_Click(object sender, EventArgs e) => _fileHelper.OpenFile();

        private void LowerCaseToolStripMenuItem_Click(object sender, EventArgs e) => _selectionHelper.SelectedText = _selectionHelper.SelectedText.ToLower();

        private void LowerCaseToolStripMenuItem1_Click(object sender, EventArgs e) => _selectionHelper.SelectedText = _selectionHelper.SelectedText.ToLower();

        private void MainToolStripMenuItem_Click(object sender, EventArgs e)
        { }

        private void MarkImportantToolStripMenuItem_Click(object sender, EventArgs e) => _treeHelper.MarkImportant();

        private void MarkImportantToolStripMenuItem1_Click(object sender, EventArgs e) => _treeHelper.MarkImportant();

        private void MoveDownToolStripMenuItem_Click(object sender, EventArgs e) => _treeHelper.MoveDown();

        private void MoveDownToolStripMenuItem1_Click(object sender, EventArgs e) => _treeHelper.MoveDown();

        private void MoveUpToolStripMenuItem_Click(object sender, EventArgs e) => _treeHelper.MoveUp();

        private void MoveUpToolStripMenuItem1_Click(object sender, EventArgs e) => _treeHelper.MoveUp();

        private void MultiSelectToolStripMenuItem_Click(object sender, EventArgs e) => this.treeViewLibrary.CheckBoxes = !this.treeViewLibrary.CheckBoxes;

        private void NewToolStripMenuItem_Click(object sender, EventArgs e) => _fileHelper.NewDoc();

        private void NodeDefaultsToolStripMenuItem_Click(object sender, EventArgs e) => EditNodeDefaults();

        private void NodeDefaultsToolStripMenuItem1_Click(object sender, EventArgs e) => EditNodeDefaults();

        private void NoneToolStripMenuItem_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.None);

        private void NoneToolStripMenuItem1_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.None);

        private void PasteSpecialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringTemplate stringtemplate = new StringTemplate();
            string result = stringtemplate.Format(Clipboard.GetText(), _fastColoredTextBoxBoxHelper.SelectedText);
            _fastColoredTextBoxBoxHelper.SelectedText = result;
        }

        private void PasteSpecialToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            StringTemplate stringtemplate = new StringTemplate();
            string result = stringtemplate.Format(Clipboard.GetText(), _fastColoredTextBoxBoxHelper.SelectedText);
            _fastColoredTextBoxBoxHelper.SelectedText = result;
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e) => _fastColoredTextBoxBoxHelper.Paste();

        private void PasteToolStripMenuItem1_Click(object sender, EventArgs e) => _fastColoredTextBoxBoxHelper.Paste();

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            _fileHelper.FormToCodeLib();

            string text = _fastColoredTextBoxBoxHelper.Text;

            foreach (CodeSnippet snippet in CodeLib.Instance.Library)
            {
                string path = $"#[{snippet.Path}]#";
                if (text.Contains(path))
                    text = text.Replace(path, snippet.Code);
            }

            Clipboard.SetText(text);
        }

        private void PluginsToolStripContextMenu_DropDownOpening(object sender, EventArgs e) => _PluginHelper.SetMenuState(pluginsToolStripContextMenu);

        private void PluginsToolStripEditMenuItem_Click(object sender, EventArgs e)
        { }

        private void PluginsToolStripEditMenuItem_DropDownOpening(object sender, EventArgs e) => _PluginHelper.SetMenuState(pluginsToolStripEditMenuItem);

        private void PreviousNoteToolStripMenuItem_Click(object sender, EventArgs e)
        { }

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

        private void QuickRenameToolStripMenuItem_Click(object sender, EventArgs e)
        { }

        private void RemoveCurrentFromFavoriteToolStripMenuItem_Click(object sender, EventArgs e) => _FavoriteHelper.RemoveCurrentFromFavorite();

        private void RemoveToolStripMenuItem_Click(object sender, EventArgs e) => _treeHelper.DeleteSelectedNode();

        private void RemoveToolStripMenuItem1_Click(object sender, EventArgs e) => _treeHelper.DeleteSelectedNode();

        private void RenameNoteToSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbCode.SelectedText))
                return;

            treeViewLibrary.SelectedNode.Text = tbCode.SelectedText;
        }

        private void ReplaceToolStripMenuItem_Click(object sender, EventArgs e) => _fastColoredTextBoxBoxHelper.ShowReplaceDialog();

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e) => _fileHelper.SaveFile(true);

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e) => _fileHelper.SaveFile(false);

        private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e) => _fastColoredTextBoxBoxHelper.SelectAll();

        private void SelectAllToolStripMenuItem1_Click(object sender, EventArgs e) => _fastColoredTextBoxBoxHelper.SelectAll();

        private void SelectLineToolStripMenuItem_Click(object sender, EventArgs e) => _selectionHelper.SelectLine();

        private void SetAlarmToolStripMenuItem_Click(object sender, EventArgs e) => _treeHelper.SetAlarm();

        private void SetZoom()
        {
            hScrollBarZoom.Value = Config.Zoom;
            tbCode.Zoom = hScrollBarZoom.Value;
            labelZoomPerc.Text = $"{tbCode.Zoom}%";
        }

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

        private void SplitDoc()
        {
            FormSplitter f = new FormSplitter();
            DialogResult dr = f.ShowDialog();
            if (dr == DialogResult.Cancel)
                return;

            _treeHelper.SplitDocument(this.treeViewLibrary.SelectedNode, tbCode.Text, f.Splitter);
        }

        private void SplitDocumentToolStripMenuItem_Click(object sender, EventArgs e) => SplitDoc();

        private void SplitDocumentToolStripMenuItem1_Click(object sender, EventArgs e) => SplitDoc();

        private void SQLToolStripMenuItem_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.SQL);

        private void SQLToolStripMenuItem1_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.SQL);

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

        private void TimerClipboard_Tick(object sender, EventArgs e)
        {
            string _text = Clipboard.GetText();
            if (string.IsNullOrWhiteSpace(_text))
                return;

            if (_prevClipboard.Equals(_text))
                return;

            tbCode.SelectedText = _text + "\r\n";

            _prevClipboard = _text;
        }

        private void ToolStripButton6_Click(object sender, EventArgs e)
        {
            if (treeViewLibrary.SelectedNode != null)
                _treeHelper.CreateNewNode(this.treeViewLibrary.SelectedNode);
            else
                _treeHelper.CreateNewRootNode();
            _fastColoredTextBoxBoxHelper.Focus();
        }

        private void ToolStripButtonCopy_Click(object sender, EventArgs e) => _fastColoredTextBoxBoxHelper.Copy();

        private void ToolStripButtonCut_Click(object sender, EventArgs e) => _fastColoredTextBoxBoxHelper.Cut();

        private void ToolStripButtonOpenFile_Click(object sender, EventArgs e) => _fileHelper.OpenFile();

        private void ToolStripButtonPaste_Click(object sender, EventArgs e) => _fastColoredTextBoxBoxHelper.Paste();

        private void ToolStripButtonSaveFile_Click(object sender, EventArgs e) => _fileHelper.SaveFile(false);

        private void ToolStripLabel1_Click(object sender, EventArgs e)
        { }

        private void ToolStripMenuItem35_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbCode.SelectedText))
                return;

            treeViewLibrary.SelectedNode.Text = tbCode.SelectedText;
        }

        private void ToolStripMenuItem39_Click(object sender, EventArgs e) => _treeHelper.SetAlarm();

        private void ToolStripMenuItem8_Click(object sender, EventArgs e) => _selectionHelper.SelectLine();


        private void UpperCaseToolStripMenuItem_Click(object sender, EventArgs e) => _fastColoredTextBoxBoxHelper.SelectedText = _fastColoredTextBoxBoxHelper.SelectedText.ToUpper();

        private void UpperCaseToolStripMenuItem1_Click(object sender, EventArgs e) => _fastColoredTextBoxBoxHelper.SelectedText = _fastColoredTextBoxBoxHelper.SelectedText.ToUpper();

        private void VBToolStripMenuItem_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.VB);

        private void VBToolStripMenuItemChangeType_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.VB);

        private void WordwrapToolStripMenuItem_Click(object sender, EventArgs e) => _fastColoredTextBoxBoxHelper.SetWordWrap();

        private void HScrollBarZoom_Scroll(object sender, ScrollEventArgs e)
        {
            tbCode.Zoom = hScrollBarZoom.Value;
            labelZoomPerc.Text = $"{tbCode.Zoom}%";
            Config.Zoom = tbCode.Zoom;
        }

        private void SwitchLast2DocumentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _treeHelper.SwitchLastTwo();
        }

        private void PropertiesToolStripMenuItem_Click(object sender, EventArgs e) => EditNodeProperties();

        private void PropertiesToolStripMenuItem1_Click(object sender, EventArgs e) => EditNodeProperties();

        private void EditNodeProperties()
        {
            if (_treeHelper.IsSystem(treeViewLibrary.SelectedNode))
                return;

            CodeSnippet _snippet = _treeHelper.FromNode(treeViewLibrary.SelectedNode);
            FormProperties _form = new FormProperties { SelectedObject = _snippet };

            _form.ShowDialog(this);

            CodeLib.Instance.Refresh();

            _fastColoredTextBoxBoxHelper.SetWordWrap();
            
            tbCode.Focus();
        }

        private void SearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSearch _formSearch = new FormSearch();
            DialogResult _r = _formSearch.ShowDialog();
            if (_r == DialogResult.OK)
            {
                _treeHelper.FindNodeByPath(_formSearch.SelectedPath);
            }
        }

        private void changeTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void changeTypeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
        }

        private void xMLToolStripMenuItem_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.XML);

        private void jSToolStripMenuItem_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.JS);

        private void pHPToolStripMenuItem_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.PHP);

        private void luaToolStripMenuItem_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.Lua);

        private void xMLToolStripMenuItem1_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.XML);

        private void jSToolStripMenuItem1_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.JS);

        private void pHPToolStripMenuItem1_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.PHP);

        private void luaToolStripMenuItem1_Click(object sender, EventArgs e) => _treeHelper.ChangeType(treeViewLibrary.SelectedNode, CodeType.Lua);

        private void containerImage_Click(object sender, EventArgs e)
        {

        }
    }
}