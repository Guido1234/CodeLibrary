using CodeLibrary.Core;
using CodeLibrary.Editor;
using CodeLibrary.Extensions;
using CodeLibrary.Helpers;
using DevToys;
using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CodeLibrary
{
    public class TreeviewHelper
    {
        private readonly FileHelper _fileHelper;
        private readonly FixedQueue<TreeNode> _LastTwo = new FixedQueue<TreeNode>(2);
        private readonly FormCodeLibrary _mainform;
        private readonly TextBoxHelper _textBoxHelper;
        private readonly ThemeHelper _themeHelper;
        private readonly Timer _timer = new Timer();
        private readonly TreeView _treeViewLibrary;
        private bool _BlockDrop = false;
        private string _SelectedId;
        private bool _timerTick = false;
        private int _upodating = 0;
        private CodeType DialogSelectedCodeType = CodeType.None;

        public TreeviewHelper(FormCodeLibrary mainform, TextBoxHelper textBoxHelper, FileHelper fileHelper, ThemeHelper themeHelper)
        {
            _treeViewLibrary = mainform.treeViewLibrary;
            _mainform = mainform;
            _fileHelper = fileHelper;
            _textBoxHelper = textBoxHelper;
            _themeHelper = themeHelper;
            _treeViewLibrary.AllowDrop = true;
            _treeViewLibrary.ItemDrag += new ItemDragEventHandler(this.TreeViewLibrary_ItemDrag);
            _treeViewLibrary.AfterSelect += new TreeViewEventHandler(this.TreeViewLibrary_AfterSelect);
            _treeViewLibrary.BeforeSelect += new TreeViewCancelEventHandler(this.TreeViewLibrary_BeforeSelect);
            _treeViewLibrary.DragDrop += new DragEventHandler(this.TreeViewLibrary_DragDrop);
            _treeViewLibrary.DragEnter += new DragEventHandler(this.TreeViewLibrary_DragEnter);
            _treeViewLibrary.DragOver += new DragEventHandler(this.TreeViewLibrary_DragOver);
            _treeViewLibrary.KeyDown += new KeyEventHandler(this.TreeViewLibrary_KeyDown);
            _treeViewLibrary.MouseUp += new MouseEventHandler(this.TreeViewLibrary_MouseUp);
            _treeViewLibrary.BeforeExpand += _treeViewLibrary_BeforeExpand;

            _mainform.imageViewer.ImageMouseClick += ImageViewer_ImageMouseClick;

            _timer.Interval = 1000;
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        public string SelectedId
        {
            get
            {
                return _SelectedId;
            }
            set
            {
                _SelectedId = value;
                _fileHelper.SelectedId = _SelectedId;
            }
        }

        public TreeNode SelectedNode
        {
            get
            {
                return _treeViewLibrary.SelectedNode;
            }
            set
            {
                _treeViewLibrary.SelectedNode = value;
            }
        }

        public void AddImageNode(TreeNode parentNode, Image image, string name)
        {
            byte[] _imageData = image.ConvertImageToByteArray(33L);
            AddImageNode(parentNode, _imageData, name);
        }

        public void AddImageNodeNoCompression(TreeNode parentNode, Image image, string name)
        {
            byte[] _imageData = image.ConvertImageToByteArray();
            AddImageNode(parentNode, _imageData, name);
        }

        public void BeginUpdate()
        {
            _treeViewLibrary.BeginUpdate();
            _upodating++;
        }

        public void ChangeType(TreeNode node, CodeType newType)
        {
            if (IsSystem(node))
                return;

            CodeSnippet snippet = CodeLib.Instance.GetById(node.Name);

            if (snippet != null)
            {
                CodeType _oldType = snippet.CodeType;

                if (newType == CodeType.RTF || _oldType == CodeType.RTF)
                {
                    if (_oldType != CodeType.RTF)
                    {
                        FastColoredTextBox _fb = new FastColoredTextBox();
                        _fb.Language = LocalUtils.CodeTypeToLanguage(_oldType);
                        _fb.Text = snippet.Code;
                        _fb.Refresh();
                    }

                    // Copy with Markup

                    RichTextBox _richTextBox = new RichTextBox();

                    _themeHelper.RichTextBoxTheme(_richTextBox);

                    if (newType == CodeType.RTF && _oldType != CodeType.RTF)
                    {
                        _richTextBox.Text = snippet.Code;
                        snippet.RTF = _richTextBox.Rtf;
                    }
                    else if (_oldType == CodeType.RTF && newType != CodeType.RTF)
                    {
                        _richTextBox.Rtf = snippet.RTF;
                        snippet.Code = _richTextBox.Text;
                        snippet.RTF = string.Empty;
                    }
                }
                _textBoxHelper.ChangeView(newType);
                snippet.CodeType = newType;
                _textBoxHelper.SetStateNoSave(snippet);
                int imageIndex = _fileHelper.GetImageIndex(snippet);
                node.ImageIndex = imageIndex;
                node.SelectedImageIndex = imageIndex;
            }
        }

        public TreeNode CreateNewNode(TreeNodeCollection parent, CodeType codetype, string name, string text, string rtf)
        {
            CodeSnippet snippet = new CodeSnippet() { Code = text, CodeType = codetype, Locked = false, Name = name, RTF = rtf };
            if (snippet.CodeType == CodeType.HTML || snippet.CodeType == CodeType.MarkDown)
            {
                snippet.HtmlPreview = true;
            }
            CodeLib.Instance.Library.Add(snippet);

            int _imageIndex = _fileHelper.GetImageIndex(snippet);

            TreeNode _node = parent.Add(snippet.Name, snippet.Name, _imageIndex, _imageIndex);
            _node.Name = snippet.Id;
            CodeLib.Instance.AddNodeIndexer(_node);

            return _node;
        }

        public TreeNode CreateNewNodeWindowed(TreeNode parent)
        {
            if (parent == null || HasDefaultChildCodeTypeEnabled(parent) == false)
            {
                return CreateNewNodeWindowedDialog(parent);
            }
            else
            {
                int x = 0, y = 0;
                CodeType _defaultCodeType = GetDefaultCodeType(parent, CodeType.None);
                string defaultName = GetDefaultName(parent, "New Note", 0, ref x);
                string defaultCode = GetDefaultCode(parent, string.Empty, 0, ref y);
                string defaultRtf = GetDefaultRtf(parent, string.Empty, 0, ref y);
                TreeNode _result = CreateNewNode(parent.Nodes, _defaultCodeType, defaultName, defaultCode, defaultRtf);
                return _result;
            }
        }

        public TreeNode CreateNewNodeWindowedDialog(TreeNode parent)
        {
            FormAddNote _f = new FormAddNote();
            _f.SelectedType = DialogSelectedCodeType;

            DialogResult _r = _f.ShowDialog();
            if (_r != DialogResult.OK)
            {
                return null;
            }

            DialogSelectedCodeType = _f.SelectedType;

            string _noteName = _f.NoteName;

            int _repeat = _f.Repeat;

            TreeNode _newNode = new TreeNode();

            if (_f.Root)
            {
                if (_f.DefaultParent)
                {
                    var _parentSNippet = CodeLib.Instance.GetById(_noteName);
                    _parentSNippet.DefaultChildCodeTypeEnabled = true;
                    _parentSNippet.DefaultChildCodeType = _f.SelectedType;
                }
                for (int ii = 0; ii < _repeat; ii++)
                {
                    _newNode = CreateNewRootNode(_f.SelectedType, string.Format(_noteName, DateTime.Now, ii + 1), string.Empty);
                }

                CodeLib.Instance.AddNodeIndexer(_newNode);
                return _newNode;
            }

            if (_f.DefaultParent)
            {
                var _parentSNippet = CodeLib.Instance.GetById(parent.Name);
                _parentSNippet.DefaultChildCodeTypeEnabled = true;
                _parentSNippet.DefaultChildCodeType = _f.SelectedType;
            }
            for (int ii = 0; ii < _repeat; ii++)
            {
                _newNode = CreateNewNode(parent.Nodes, _f.SelectedType, string.Format(_noteName, DateTime.Now, ii + 1), string.Empty, string.Empty);
            }
            CodeLib.Instance.AddNodeIndexer(_newNode);
            return _newNode;
        }

        public TreeNode CreateNewRootNode(CodeType codetype, string name, string text) => CreateNewNode(_treeViewLibrary.Nodes, codetype, name, text, string.Empty);

        public void DeleteSelectedNode()
        {
            if (_treeViewLibrary.SelectedNode == null)
                return;

            if (IsSystem(_treeViewLibrary.SelectedNode))
                return;

            RemoveNode(_treeViewLibrary.SelectedNode, false);

            CodeLib.Instance.BuildNodeIndexer(_treeViewLibrary);

            SetLibraryMenuState();
        }

        public void EmptyTrashcan()
        {
            DialogResult _dialogResult = MessageBox.Show("Are you sure?", "Sure?", MessageBoxButtons.YesNo);
            if (_dialogResult != DialogResult.Yes)
                return;

            List<string> _ids = new List<string>();
            _fileHelper.TrashcanNode.GetAllChildNames(ref _ids);

            _fileHelper.TrashcanNode.Nodes.Clear();

            CodeLib.Instance.Library.RemoveRange(_ids);

            if (CodeLib.Instance.Library.Count == 2)
            {
                CreateNewNode(_treeViewLibrary.Nodes, CodeType.Folder, Constants.SNIPPETS, "", "");
            }

            SetLibraryMenuState();
        }

        public void EndUpdate()
        {
            _treeViewLibrary.EndUpdate();
            _upodating--;
            if (_upodating < 0)
            {
                _upodating = 0;
            }
        }

        public bool FindNodeByPath(string fullpath)
        {
            if (string.IsNullOrEmpty(fullpath))
                return false;

            foreach (TreeNode node in _treeViewLibrary.Nodes)
            {
                if (node.FullPath.Equals(fullpath, StringComparison.InvariantCultureIgnoreCase))
                {
                    _treeViewLibrary.SelectedNode = node;
                    SetLibraryMenuState();
                    return true;
                }
                bool b = FindNodeByPath(fullpath, node);
                if (b)
                    return true;
            }
            return false;
        }

        public CodeSnippet FromNode(TreeNode node) => CodeLib.Instance.GetById(node.Name);

        public string GetDefaultCode(TreeNode node, string defaultDefault, int level, ref int nodecount)
        {
            if (level == 0)
            {
                nodecount = node.Nodes.Count;
            }

            CodeSnippet _snippet = FromNode(node);
            if (!string.IsNullOrEmpty(_snippet.DefaultChildCode))
            {
                try
                {
                    return string.Format(_snippet.DefaultChildCode, DateTime.Now, nodecount);
                }
                catch
                {
                    return _snippet.DefaultChildCode;
                }
            }

            if (node.Parent == null)
                return defaultDefault;

            return GetDefaultCode(node.Parent, defaultDefault, level++, ref nodecount);
        }

        public CodeType GetDefaultCodeType(TreeNode node, CodeType defaultDefault)
        {
            CodeSnippet _snippet = FromNode(node);
            if (_snippet.DefaultChildCodeTypeEnabled)
                return _snippet.DefaultChildCodeType;

            if (node.Parent == null)
                return defaultDefault;

            return GetDefaultCodeType(node.Parent, defaultDefault);
        }

        public string GetDefaultName(TreeNode node, string defaultDefault, int level, ref int nodecount)
        {
            if (level == 0)
            {
                nodecount = node.Nodes.Count;
            }

            CodeSnippet _snippet = FromNode(node);
            if (!string.IsNullOrEmpty(_snippet.DefaultChildName))
            {
                try
                {
                    return string.Format(_snippet.DefaultChildName, DateTime.Now, nodecount);
                }
                catch
                {
                    return _snippet.DefaultChildName;
                }
            }

            if (node.Parent == null)
                return defaultDefault;

            return GetDefaultName(node.Parent, defaultDefault, level++, ref nodecount);
        }

        public string GetDefaultRtf(TreeNode node, string defaultDefault, int level, ref int nodecount)
        {
            if (level == 0)
            {
                nodecount = node.Nodes.Count;
            }

            CodeSnippet _snippet = FromNode(node);
            if (!string.IsNullOrEmpty(_snippet.DefaultChildRtf))
                return _snippet.DefaultChildRtf;

            if (node.Parent == null)
                return defaultDefault;

            return GetDefaultRtf(node.Parent, defaultDefault, level++, ref nodecount);
        }

        public bool HasDefaultChildCodeTypeEnabled(TreeNode node)
        {
            CodeSnippet _snippet = FromNode(node);
            if (_snippet.DefaultChildCodeTypeEnabled)
                return _snippet.DefaultChildCodeTypeEnabled;

            if (node.Parent == null)
                return false;

            return HasDefaultChildCodeTypeEnabled(node.Parent);
        }

        public bool IsClipBoardMonitor(TreeNode node)
        {
            if (node == null)
                return false;

            CodeSnippet snippet = CodeLib.Instance.GetById(node.Name);
            return (snippet.CodeType == CodeType.System && snippet.Id == Constants.CLIPBOARDMONITOR);
        }

        public bool IsImage(TreeNode node)
        {
            if (node == null)
                return false;

            CodeSnippet snippet = CodeLib.Instance.GetById(node.Name);
            return (snippet.CodeType == CodeType.Image);
        }

        public bool IsInTrashcan(TreeNode node)
        {
            if (node == null)
                return false;

            var root = node.GetRootNode();
            return IsTrashcan(root);
        }

        public bool IsSystem(TreeNode node)
        {
            if (node == null)
                return false;

            CodeSnippet snippet = CodeLib.Instance.GetById(node.Name);
            return snippet.CodeType == CodeType.System;
        }

        public bool IsTrashcan(TreeNode node)
        {
            if (node == null)
                return false;

            CodeSnippet snippet = CodeLib.Instance.GetById(node.Name);
            return (snippet.CodeType == CodeType.System && snippet.Id == Constants.TRASHCAN);
        }

        public void MarkImportant()
        {
            if (IsSystem(_treeViewLibrary.SelectedNode))
                return;

            if (_treeViewLibrary.SelectedNode == null)
                return;

            CodeSnippet _snippet = FromNode(_treeViewLibrary.SelectedNode);
            _snippet.Important = !_snippet.Important;

            ChangeType(_treeViewLibrary.SelectedNode, _snippet.CodeType);
            SetLibraryMenuState();
        }

        public bool MergeAllowed(TreeNode node)
        {
            if (node == null)
                return false;

            CodeSnippet snippet = CodeLib.Instance.GetById(node.Name);
            switch (snippet.CodeType)
            {
                case CodeType.Image:
                case CodeType.System:
                case CodeType.UnSuported:
                case CodeType.RTF:
                    return false;
            }
            return true;
        }

        public void MoveDown()
        {
            if (IsSystem(_treeViewLibrary.SelectedNode))
                return;

            if (_treeViewLibrary.SelectedNode != null)
            {
                BeginUpdate();
                _treeViewLibrary.SelectedNode.MoveDown();
                EndUpdate();
            }
        }

        public void MoveToBottom()
        {
            if (IsSystem(_treeViewLibrary.SelectedNode))
                return;

            if (_treeViewLibrary.SelectedNode != null)
            {
                BeginUpdate();
                _treeViewLibrary.SelectedNode.MoveToBottom();
                EndUpdate();
            }
        }

        public void MoveToLeft()
        {
            if (IsSystem(_treeViewLibrary.SelectedNode))
                return;

            if (_treeViewLibrary.SelectedNode != null)
            {
                BeginUpdate();
                _treeViewLibrary.SelectedNode.MoveLeft();
                EndUpdate();
            }
        }

        public void MoveToRight()
        {
            if (IsSystem(_treeViewLibrary.SelectedNode))
                return;

            if (_treeViewLibrary.SelectedNode != null)
            {
                BeginUpdate();
                _treeViewLibrary.SelectedNode.MoveRight();
                EndUpdate();
            }
        }

        public void MoveToTop()
        {
            if (IsSystem(_treeViewLibrary.SelectedNode))
                return;

            if (_treeViewLibrary.SelectedNode != null)
            {
                BeginUpdate();
                _treeViewLibrary.SelectedNode.MoveToTop();
                EndUpdate();
            }
        }

        public void MoveUp()
        {
            if (IsSystem(_treeViewLibrary.SelectedNode))
                return;

            if (_treeViewLibrary.SelectedNode != null)
            {
                BeginUpdate();
                _treeViewLibrary.SelectedNode.MoveUp();
                EndUpdate();
            }
        }

        public void NoteIcon()
        {
            CodeSnippet _snippet = FromNode(_treeViewLibrary.SelectedNode);

            if (_snippet == null)
            {
                return;
            }
            CodeType _type = _snippet.CodeType;

            switch (_type)
            {
                case CodeType.CSharp:
                    _mainform.pctType.Image = _mainform.mnuTypeCSharp.Image;
                    break;

                case CodeType.HTML:
                    _mainform.pctType.Image = _mainform.mnuTypeHTML.Image;
                    break;

                case CodeType.JS:
                    _mainform.pctType.Image = _mainform.mnuTypeJs.Image;
                    break;

                case CodeType.Lua:
                    _mainform.pctType.Image = _mainform.mnuTypeLua.Image;
                    break;

                case CodeType.None:
                    _mainform.pctType.Image = _mainform.mnuTypeNone.Image;
                    break;

                case CodeType.MarkDown:
                    _mainform.pctType.Image = _mainform.mnuTypeNone.Image;
                    break;

                case CodeType.PHP:
                    _mainform.pctType.Image = _mainform.mnuTypePhp.Image;
                    break;

                case CodeType.RTF:
                    _mainform.pctType.Image = _mainform.mnuTypeRtf.Image;
                    break;

                case CodeType.SQL:
                    _mainform.pctType.Image = _mainform.mnuTypeSql.Image;
                    break;

                case CodeType.XML:
                    _mainform.pctType.Image = _mainform.mnuTypeXML.Image;
                    break;

                case CodeType.Template:
                    _mainform.pctType.Image = _mainform.mnuTypeTemplate.Image;
                    break;

                default:
                    _mainform.pctType.Image = null;
                    break;
            }
        }

        public void PasteClipBoardEachLine()
        {
            string _text = Clipboard.GetText();

            string[] _lines = Utils.SplitLines(_text);
            foreach (string line in _lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    CreateNewNode(_treeViewLibrary.SelectedNode.Nodes, CodeType.None, line, "", "");
                }
            }
        }

        public void PasteClipBoardFileList()
        {
            List<string> items = new List<string>();
            foreach (string s in Clipboard.GetFileDropList())
            {
                items.Add(s);
            }
            if (items.Count > 0)
            {
                AddFiles(_treeViewLibrary.SelectedNode, items.ToArray());
            }
        }

        public void RefreshCurrentTreeNode()
        {
            RefreshNode(_treeViewLibrary.SelectedNode);
            SetLibraryMenuState();
        }

        public void RefreshNode(TreeNode node)
        {
            if (IsSystem(node))
                return;

            CodeSnippet snippet = CodeLib.Instance.GetById(node.Name);
            if (snippet != null)
            {
                int imageIndex = _fileHelper.GetImageIndex(snippet);
                node.ImageIndex = imageIndex;
                node.SelectedImageIndex = imageIndex;
                _textBoxHelper.SetStateNoSave(snippet);
                _textBoxHelper.ChangeView(snippet.CodeType);
                _textBoxHelper.ApplySettings();
            }
        }

        public void RemoveNode(TreeNode node, bool permanent)
        {
            if (node == null)
                return;

            _treeViewLibrary.Nodes.Remove(node);

            if (permanent)
                CodeLib.Instance.Library.Remove(node.Name);
            else
                _fileHelper.TrashcanNode.Nodes.Add(node);

            SetLibraryMenuState();
        }

        public void SetSelectedNode(TreeNode node, bool setHistory)
        {
            CodeSnippet _snippet = FromNode(node);
            SelectedId = node.Name;

            if (setHistory)
                _LastTwo.Add(node);

            switch (_snippet.CodeType)
            {
                case CodeType.Image:
                    _mainform.containerImage.Visible = true;
                    _mainform.containerCode.Visible = false;
                    _mainform.imageViewer.setImage(_snippet.Blob);
                    _mainform.containerInfoBar.Visible = false;
                    _mainform.containerRtfEditor.Visible = false;
                    break;

                case CodeType.CSharp:
                case CodeType.Folder:
                case CodeType.HTML:
                case CodeType.JS:
                case CodeType.Lua:
                case CodeType.None:
                case CodeType.PHP:
                case CodeType.SQL:
                case CodeType.System:
                case CodeType.Template:
                case CodeType.VB:
                case CodeType.XML:
                case CodeType.MarkDown:
                    _mainform.containerCode.Visible = true;
                    _mainform.containerImage.Visible = false;
                    _mainform.containerRtfEditor.Visible = false;
                    _textBoxHelper.SetState(_snippet);
                    _treeViewLibrary.SelectedNode = node;
                    _mainform.containerInfoBar.Visible = true;

                    SetLibraryMenuState();
                    break;

                case CodeType.RTF:
                    _mainform.containerRtfEditor.Visible = true;
                    _mainform.containerCode.Visible = false;
                    _mainform.containerImage.Visible = false;
                    _textBoxHelper.SetState(_snippet);
                    _treeViewLibrary.SelectedNode = node;
                    _mainform.containerInfoBar.Visible = true;
                    SetLibraryMenuState();

                    break;
            }
            if (_snippet.CodeType != CodeType.Image)
            {
                _textBoxHelper.ApplySettings();
            }

            NoteIcon();
        }

        public void SetTypeMenuState()
        {
            CodeSnippet _snippet = FromNode(_treeViewLibrary.SelectedNode);

            if (_snippet == null)
            {
                return;
            }
            CodeType _type = _snippet.CodeType;

            _mainform.mnuMarkDown.Checked = (_type == CodeType.MarkDown);
            _mainform.mncTypeMarkDown.Checked = (_type == CodeType.MarkDown);

            _mainform.mncTypeCSharp.Checked = (_type == CodeType.CSharp);
            _mainform.mncTypeFolder.Checked = (_type == CodeType.Folder);
            _mainform.mncTypeHtml.Checked = (_type == CodeType.HTML);
            _mainform.mncTypeJS.Checked = (_type == CodeType.JS);
            _mainform.mncTypeLua.Checked = (_type == CodeType.Lua);
            _mainform.mncTypeNone.Checked = (_type == CodeType.None);
            _mainform.mncTypePhp.Checked = (_type == CodeType.PHP);
            _mainform.mncTypeRtf.Checked = (_type == CodeType.RTF);
            _mainform.mncTypeSql.Checked = (_type == CodeType.SQL);
            _mainform.mncTypeTemplate.Checked = (_type == CodeType.Template);
            _mainform.mncTypeVB.Checked = (_type == CodeType.VB);
            _mainform.mncTypeXml.Checked = (_type == CodeType.XML);

            _mainform.mnuTypeCSharp.Checked = (_type == CodeType.CSharp);
            _mainform.mnuTypeFolder.Checked = (_type == CodeType.Folder);
            _mainform.mnuTypeHTML.Checked = (_type == CodeType.HTML);
            _mainform.mnuTypeJs.Checked = (_type == CodeType.JS);
            _mainform.mnuTypeLua.Checked = (_type == CodeType.Lua);
            _mainform.mnuTypeNone.Checked = (_type == CodeType.None);
            _mainform.mnuTypePhp.Checked = (_type == CodeType.PHP);
            _mainform.mnuTypeRtf.Checked = (_type == CodeType.RTF);
            _mainform.mnuTypeSql.Checked = (_type == CodeType.SQL);
            _mainform.mnuTypeTemplate.Checked = (_type == CodeType.Template);
            _mainform.mnuTypeVB.Checked = (_type == CodeType.VB);
            _mainform.mnuTypeXML.Checked = (_type == CodeType.XML);
        }

        public void SortChildren()
        {
            switch (Config.SortMode)
            {
                case ESortMode.Alphabetic:
                    SortChildrenAscending();
                    break;

                case ESortMode.AlphabeticGrouped:
                    SortChildrenAscendingGrouped();
                    break;
            }
        }

        public void SortChildrenAscending()
        {
            if (IsSystem(_treeViewLibrary.SelectedNode))
                return;

            var _node = _treeViewLibrary.SelectedNode;
            if (_node != null)
            {
                BeginUpdate();

                List<TreeNode> _sort = new List<TreeNode>();

                foreach (TreeNode item in _node.Nodes)
                {
                    _sort.Add(item);
                }

                _node.Nodes.Clear();

                foreach (var item in _sort.OrderBy(p => p.Text))
                {
                    _node.Nodes.Add(item);
                }

                _treeViewLibrary.SelectedNode = _node;
                EndUpdate();
            }
        }

        public void SortChildrenAscendingGrouped()
        {
            if (IsSystem(_treeViewLibrary.SelectedNode))
                return;

            var _node = _treeViewLibrary.SelectedNode;
            if (_node != null)
            {
                BeginUpdate();

                List<TreeNode> _sort1 = new List<TreeNode>();
                List<TreeNode> _sort2 = new List<TreeNode>();
                List<TreeNode> _sort3 = new List<TreeNode>();

                foreach (TreeNode item in _node.Nodes)
                {
                    CodeSnippet snippet = CodeLib.Instance.GetById(item.Name);
                    if (snippet.Important)
                    {
                        _sort1.Add(item);
                    }
                }

                foreach (TreeNode item in _node.Nodes)
                {
                    CodeSnippet snippet = CodeLib.Instance.GetById(item.Name);
                    if (snippet.CodeType == CodeType.Folder && !snippet.Important)
                    {
                        _sort2.Add(item);
                    }
                }

                foreach (TreeNode item in _node.Nodes)
                {
                    CodeSnippet snippet = CodeLib.Instance.GetById(item.Name);
                    if (snippet.CodeType != CodeType.Folder && !snippet.Important)
                    {
                        _sort3.Add(item);
                    }
                }

                _node.Nodes.Clear();

                foreach (var item in _sort1.OrderBy(p => p.Text))
                {
                    _node.Nodes.Add(item);
                }
                foreach (var item in _sort2.OrderBy(p => p.Text))
                {
                    _node.Nodes.Add(item);
                }
                foreach (var item in _sort3.OrderBy(p => p.Text))
                {
                    _node.Nodes.Add(item);
                }
                _treeViewLibrary.SelectedNode = _node;
                EndUpdate();
            }
        }

        public void SwitchLastTwo()
        {
            if (_LastTwo.Full)
            {
                var _node = _LastTwo.Next();
                if (_node == null)
                    return;

                SetSelectedNode(_node, false);
            }
        }





        private void _treeViewLibrary_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
        }

        private void AddFiles(TreeNode targetNode, string[] filenames)
        {
            List<string> _filenames = filenames.ToList();
            _filenames.Sort();

            foreach (string filename in _filenames)
            {
                FileInfo _file = new FileInfo(filename);
                var _type = LocalUtils.CodeTypeByExtension(_file);

                switch (_type)
                {
                    case CodeType.Image:
                        byte[] _imageData = File.ReadAllBytes(filename);
                        AddImageNode(targetNode, _imageData, _file.Name);
                        break;

                    case CodeType.CSharp:
                    case CodeType.HTML:
                    case CodeType.MarkDown:
                    case CodeType.JS:
                    case CodeType.Lua:
                    case CodeType.PHP:
                    case CodeType.VB:
                    case CodeType.None:
                    case CodeType.SQL:
                    case CodeType.XML:
                    case CodeType.Template:
                    case CodeType.RTF:
                        string _text = File.ReadAllText(filename);
                        CreateNewNode(targetNode.Nodes, _type, _file.Name, _text, _text); // ## LET OP
                        break;

                    case CodeType.System:
                    case CodeType.UnSuported:
                        break;
                }
            }
        }

        private void AddImageNode(TreeNode parentNode, byte[] _imageData, string name)
        {
            CodeSnippet snippet = new CodeSnippet() { Code = "", CodeType = CodeType.Image, Locked = false, Name = name, Blob = _imageData };
            CodeLib.Instance.Library.Add(snippet);

            int _imageIndex = _fileHelper.GetImageIndex(snippet);
            TreeNode _node = parentNode.Nodes.Add(snippet.Name, snippet.Name, _imageIndex, _imageIndex);
            _node.Name = snippet.Id;
            CodeLib.Instance.AddNodeIndexer(_node);
        }

        // Determine whether one node is a parent
        // or ancestor of a second node.
        private bool ContainsNode(TreeNode node1, TreeNode node2)
        {
            // Check the parent node of the second node.
            if (node2 == null) return false;
            if (node2.Parent == null) return false;
            if (node2.Parent.Equals(node1)) return true;

            // If the parent node is not null or equal to the first node,
            // call the ContainsNode method recursively using the parent of
            // the second node.
            return ContainsNode(node1, node2.Parent);
        }

        private bool FindNodeById(string id, TreeNode parent)
        {
            foreach (TreeNode node in parent.Nodes)
            {
                if (node.Name.Equals(id))
                {
                    _treeViewLibrary.SelectedNode = node;
                    SetLibraryMenuState();
                    return true;
                }
                bool b = FindNodeById(id, node);
                if (b)
                    return true;
            }
            return false;
        }

        private bool FindNodeByPath(string fullpath, TreeNode parent)
        {
            foreach (TreeNode node in parent.Nodes)
            {
                if (node.FullPath.Equals(fullpath, StringComparison.InvariantCultureIgnoreCase))
                {
                    _treeViewLibrary.SelectedNode = node;
                    SetLibraryMenuState();
                    return true;
                }
                bool b = FindNodeByPath(fullpath, node);
                if (b)
                    return true;
            }
            return false;
        }

        private void ImageViewer_ImageMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _mainform.mncImage.Show(Cursor.Position.X, Cursor.Position.Y);
            }
        }

        private void SetLibraryMenuState()
        {
            _mainform.mnuCopyContentsAndMerge.Enabled = MergeAllowed(_treeViewLibrary.SelectedNode);
            _mainform.mncCopyContentsAndMerge.Enabled = MergeAllowed(_treeViewLibrary.SelectedNode);

            _mainform.mnuAdd.Enabled = (!IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode));
            _mainform.mncAdd.Enabled = (!IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode));
            _mainform.mnuAddDialog.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);

            _mainform.mnuDelete.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsInTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);
            _mainform.mncDelete.Enabled = !IsInTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);

            _mainform.mncChangeType.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsImage(_treeViewLibrary.SelectedNode);
            _mainform.mnuChangeType.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsImage(_treeViewLibrary.SelectedNode);

            _mainform.mnuQuickRename.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsImage(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);
            _mainform.mncQuickRename.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsImage(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);

            _mainform.mnuMarkImportant.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);
            _mainform.mncMarkImportant.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);

            _mainform.mnuCopyPath.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);
            _mainform.mncCopyPath.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);

            _mainform.mncSortChildrenAscending.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode) && TreeViewExtensions.ParentCount(_treeViewLibrary.SelectedNode) > 1 && _treeViewLibrary.SelectedNode.Nodes.Count > 1;
            _mainform.mnuSortChildrenAscending.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode) && TreeViewExtensions.ParentCount(_treeViewLibrary.SelectedNode) > 1 && _treeViewLibrary.SelectedNode.Nodes.Count > 1;

            _mainform.mncMoveLeft.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode) && _treeViewLibrary.SelectedNode.Parent != null;
            _mainform.mnuMoveLeft.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode) && _treeViewLibrary.SelectedNode.Parent != null;

            _mainform.mncMoveRight.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode) && _treeViewLibrary.SelectedNode.PrevNode != null;
            _mainform.mnuMoveRight.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode) && _treeViewLibrary.SelectedNode.PrevNode != null;

            _mainform.mncMoveUp.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode) && TreeViewExtensions.ParentCount(_treeViewLibrary.SelectedNode) > 1;
            _mainform.mnuMoveUp.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode) && TreeViewExtensions.ParentCount(_treeViewLibrary.SelectedNode) > 1;

            _mainform.mncMoveDown.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode) && TreeViewExtensions.ParentCount(_treeViewLibrary.SelectedNode) > 1;
            _mainform.mnuMoveDown.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode) && TreeViewExtensions.ParentCount(_treeViewLibrary.SelectedNode) > 1;

            _mainform.mncMoveToTop.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode) && TreeViewExtensions.ParentCount(_treeViewLibrary.SelectedNode) > 1;
            _mainform.mnuMoveToTop.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode) && TreeViewExtensions.ParentCount(_treeViewLibrary.SelectedNode) > 1;

            _mainform.mncMoveToBottom.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode) && TreeViewExtensions.ParentCount(_treeViewLibrary.SelectedNode) > 1;
            _mainform.mnuMoveToBottom.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode) && TreeViewExtensions.ParentCount(_treeViewLibrary.SelectedNode) > 1;

            _mainform.mncClipboard.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);
            _mainform.mnuClipboard.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);

            _mainform.mnuProperties.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsImage(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);
            _mainform.mncProperties.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsImage(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);

            _mainform.mnuSearch.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);

            _mainform.mncPasteFilelist.Visible = !IsTrashcan(_treeViewLibrary.SelectedNode) && Clipboard.ContainsFileDropList() && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);
            _mainform.mnuPasteFilelist.Visible = !IsTrashcan(_treeViewLibrary.SelectedNode) && Clipboard.ContainsFileDropList() && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);

            _mainform.mnuPasteImage.Visible = !IsTrashcan(_treeViewLibrary.SelectedNode) && Clipboard.ContainsImage() && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);
            _mainform.mncPasteImage.Visible = !IsTrashcan(_treeViewLibrary.SelectedNode) && Clipboard.ContainsImage() && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);

            _mainform.mncPasteImageNoCompression.Visible = !IsTrashcan(_treeViewLibrary.SelectedNode) && Clipboard.ContainsImage() && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);
            _mainform.mnuPasteImageNoCompression.Visible = !IsTrashcan(_treeViewLibrary.SelectedNode) && Clipboard.ContainsImage() && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);

            _mainform.mncPasteTextPerLine.Visible = !IsTrashcan(_treeViewLibrary.SelectedNode) && Clipboard.ContainsText() && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);
            _mainform.mnuPasteTextPerLine.Visible = !IsTrashcan(_treeViewLibrary.SelectedNode) && Clipboard.ContainsText() && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);

            _mainform.mncPasteText.Visible = !IsTrashcan(_treeViewLibrary.SelectedNode) && Clipboard.ContainsText() && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);
            _mainform.mnuPasteText.Visible = !IsTrashcan(_treeViewLibrary.SelectedNode) && Clipboard.ContainsText() && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _timerTick = !_timerTick;
            IEnumerable<CodeSnippet> snippets = CodeLib.Instance.GetByAlarmActive();
            foreach (CodeSnippet snippet in snippets)
            {
                if (snippet.AlarmDate < DateTime.Now)
                {
                    TreeNode node = CodeLib.Instance.NodeIndexer.Get(snippet.Id); // Find Node
                    if (node == null)
                        continue;

                    List<TreeNode> parents = node.ParentPath();

                    if (_timerTick)
                    {
                        node.ForeColor = Color.White;
                        foreach (TreeNode parent in parents)
                            parent.ForeColor = Color.White;
                    }
                    else
                    {
                        node.ForeColor = Color.Red;
                        foreach (TreeNode parent in parents)
                            parent.ForeColor = Color.Red;
                    }
                }
            }
        }

        private void TreeViewLibrary_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (_upodating > 0)
            {
                return;
            }
            SetSelectedNode(e.Node, true);

            _mainform.SetZoom();
        }

        private void TreeViewLibrary_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (_upodating > 0)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(_SelectedId))
                return;

            CodeSnippet _snippet = CodeLib.Instance.GetById(_SelectedId);
            if (_snippet != null)
                _snippet.CurrentLine = _textBoxHelper.FastColoredTextBox.CurrentLineNumber();
        }

        private void TreeViewLibrary_DragDrop(object sender, DragEventArgs e)
        {
            // Retrieve the client coordinates of the drop location.
            Point targetPoint = _treeViewLibrary.PointToClient(new Point(e.X, e.Y));

            // Retrieve the node at the drop location.
            TreeNode targetNode = _treeViewLibrary.GetNodeAt(targetPoint);

            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                string[] filenames = (string[])(e.Data.GetData(DataFormats.FileDrop, false));
                AddFiles(targetNode, filenames);

                return;
            }

            // Retrieve the node that was dragged.
            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));

            // Confirm that the node at the drop location is not
            // the dragged node or a descendant of the dragged node.
            if (!draggedNode.Equals(targetNode) && !ContainsNode(draggedNode, targetNode))
            {
                // If it is a move operation, remove the node from its current
                // location and add it to the node at the drop location.
                if (e.Effect == DragDropEffects.Move)
                {
                    draggedNode.Remove();
                    if (targetNode != null)
                    {
                        targetNode.Nodes.Add(draggedNode);
                    }
                    else
                    {
                        _treeViewLibrary.Nodes.Add(draggedNode);
                    }
                }

                // If it is a copy operation, clone the dragged node
                // and add it to the node at the drop location.
                else if (e.Effect == DragDropEffects.Copy)
                {
                    // targetNode.Nodes.Add((TreeNode)draggedNode.Clone());
                    // Cloning not supported.
                }

                // Expand the node at the location
                // to show the dropped node.
                if (targetNode != null)
                {
                    targetNode.Expand();
                }
                SetLibraryMenuState();
            }
        }

        // Set the target drop effect to the effect
        // specified in the ItemDrag event handler.
        private void TreeViewLibrary_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;

            if (_treeViewLibrary.SelectedNode != null)
            {
                var _snippet = FromNode(_treeViewLibrary.SelectedNode);
                Console.WriteLine(_snippet.Path);
                if (_snippet.CodeType == CodeType.System)
                {
                    e.Effect = DragDropEffects.None;
                    _BlockDrop = true;
                }
                else
                {
                    _BlockDrop = false;
                }
            }

            SetLibraryMenuState();
        }

        // Select the node under the mouse pointer to indicate the
        // expected drop location.
        private void TreeViewLibrary_DragOver(object sender, DragEventArgs e)
        {
            // Retrieve the client coordinates of the mouse position.
            Point targetPoint = _treeViewLibrary.PointToClient(new Point(e.X, e.Y));

            // Select the node at the mouse position.
            _treeViewLibrary.SelectedNode = _treeViewLibrary.GetNodeAt(targetPoint);

            if (_treeViewLibrary.SelectedNode != null)
            {
                var _snippet = FromNode(_treeViewLibrary.SelectedNode);
                Console.WriteLine(_snippet.Path);
                if (_snippet.CodeType == CodeType.System && _snippet.Name != Constants.TRASHCAN || _BlockDrop)
                {
                    e.Effect = DragDropEffects.None;
                }
                else
                {
                    e.Effect = DragDropEffects.Move;
                }
            }

            SetLibraryMenuState();
        }

        private void TreeViewLibrary_ItemDrag(object sender, ItemDragEventArgs e)
        {
            // Move the dragged node when the left mouse button is used.
            if (e.Button == MouseButtons.Left)
            {
                _mainform.DoDragDrop(e.Item, DragDropEffects.Move);
            }

            // Copy the dragged node when the right mouse button is used.
            else if (e.Button == MouseButtons.Right)
            {
                _mainform.DoDragDrop(e.Item, DragDropEffects.Copy);
            }
            SetLibraryMenuState();
        }

        private void TreeViewLibrary_KeyDown(object sender, KeyEventArgs e)
        {
            SetLibraryMenuState();

            if (e.KeyCode == Keys.Up && e.Control && e.Shift)
            {
                MoveToTop();
                return;
            }

            if (e.KeyCode == Keys.Down && e.Control && e.Shift)
            {
                MoveToBottom();
                return;
            }

            if (e.KeyCode == Keys.Up && e.Control)
            {
                MoveUp();
                return;
            }

            if (e.KeyCode == Keys.Down && e.Control)
            {
                MoveDown();
                return;
            }

            if (e.KeyCode == Keys.Left && e.Control)
            {
                MoveToLeft();
                return;
            }

            if (e.KeyCode == Keys.Right && e.Control)
            {
                MoveToRight();
                return;
            }

            if (e.KeyValue == 113)
            {
                _treeViewLibrary.SelectedNode.BeginEdit();
                return;
            }
            if (e.KeyCode == Keys.Delete && e.Shift)
            {
                RemoveNode(_treeViewLibrary.SelectedNode, true);
                return;
            }
            if (e.KeyCode == Keys.V && e.Control && e.Shift)
            {
                if (Clipboard.ContainsText())
                {
                    PasteClipBoardEachLine();
                }
            }

            if (e.KeyCode == Keys.V && e.Control)
            {
                if (Clipboard.ContainsImage())
                {
                    AddImageNode(_treeViewLibrary.SelectedNode, Clipboard.GetImage(), "image");
                }
                if (Clipboard.ContainsText())
                {
                    TreeNode _node = CreateNewNode(_treeViewLibrary.SelectedNode.Nodes, CodeType.None, "New Note", Clipboard.GetText(), "");
                    CodeLib.Instance.AddNodeIndexer(_node);
                }
                if (Clipboard.ContainsFileDropList())
                {
                    PasteClipBoardFileList();
                }
                if (Clipboard.ContainsAudio())
                {
                }
                return;
            }

            if (e.KeyCode == Keys.Delete)
            {
                DeleteSelectedNode();
            }
        }

        private void TreeViewLibrary_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _treeViewLibrary.SelectedNode = _treeViewLibrary.GetNodeAt(e.Location);
                if (_treeViewLibrary.SelectedNode == null)
                    return;

                SetLibraryMenuState();

                if (IsTrashcan(_treeViewLibrary.SelectedNode))
                {
                    _mainform.mncTrashcan.Show(Cursor.Position.X, Cursor.Position.Y);
                    return;
                }
                if (IsClipBoardMonitor(_treeViewLibrary.SelectedNode))
                {
                    _mainform.mncClipboardMonitor.Show(Cursor.Position.X, Cursor.Position.Y);
                    return;
                }

                _mainform.mncLibrary.Show(Cursor.Position.X, Cursor.Position.Y);
            }
        }
    }
}