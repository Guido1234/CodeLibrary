using CodeLibrary.Core;
using DevToys;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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

        //private readonly ContextMenuStrip _contextMenuStripPopup;
        //private readonly ContextMenuStrip _contextMenuStripTrashcan;
        private readonly TextBoxHelper _textBoxHelper;

        private readonly Timer _timer = new Timer();
        private readonly TreeView _treeViewLibrary;
        private bool _BlockDrop = false;
        private string _SelectedId;
        private bool _timerTick = false;

        public TreeviewHelper(FormCodeLibrary mainform, TextBoxHelper fastColoredTextBoxHelper, FileHelper fileHelper)
        {
            _treeViewLibrary = mainform.treeViewLibrary;
            _mainform = mainform;
            _fileHelper = fileHelper;
            _textBoxHelper = fastColoredTextBoxHelper;
            _treeViewLibrary.AllowDrop = true;
            _treeViewLibrary.ItemDrag += new ItemDragEventHandler(this.TreeViewLibrary_ItemDrag);
            _treeViewLibrary.AfterSelect += new TreeViewEventHandler(this.TreeViewLibrary_AfterSelect);
            _treeViewLibrary.BeforeSelect += new TreeViewCancelEventHandler(this.TreeViewLibrary_BeforeSelect);
            _treeViewLibrary.DragDrop += new DragEventHandler(this.TreeViewLibrary_DragDrop);
            _treeViewLibrary.DragEnter += new DragEventHandler(this.TreeViewLibrary_DragEnter);
            _treeViewLibrary.DragOver += new DragEventHandler(this.TreeViewLibrary_DragOver);
            _treeViewLibrary.KeyUp += new KeyEventHandler(this.TreeViewLibrary_KeyUp);
            _treeViewLibrary.MouseUp += new MouseEventHandler(this.TreeViewLibrary_MouseUp);

            _mainform.imageViewer.ImageMouseClick += ImageViewer_ImageMouseClick;
            _mainform.setToClipboardToolStripMenuItem.Click += SetToClipboardToolStripMenuItem_Click;
            _mainform.saveImageToolStripMenuItem.Click += SaveImageToolStripMenuItem_Click;
            _mainform.copyAsBase64StringToolStripMenuItem.Click += CopyAsBase64StringToolStripMenuItem_Click;
            _mainform.copyAsHTMLIMGToolStripMenuItem.Click += CopyAsHTMLIMGToolStripMenuItem_Click;

            _mainform.mnuPasteFilelist.Click += PasteFilelistToolStripMenuItem_Click;
            _mainform.mnuPasteFilelist1.Click += PasteFilelistToolStripMenuItem_Click;

            _mainform.mnuPasteImageTool1.Click += PasteImageCompressedToolStripMenuItem_Click;
            _mainform.mnuPasteImageTool.Click += PasteImageCompressedToolStripMenuItem_Click;

            _mainform.mnuPasteImageNoCompression.Click += PasteImageNoCompressionToolStripMenuItem_Click;
            _mainform.mnuPasteImageNoCompression1.Click += PasteImageNoCompressionToolStripMenuItem_Click;

            _mainform.mnuPasteTextPerLine.Click += PasteTextPerLineToolStripMenuItem_Click;
            _mainform.mnuPasteTextPerLine1.Click += PasteTextPerLineToolStripMenuItem_Click;

            _mainform.mnuPasteText.Click += mnuPaste_Click;
            _mainform.mnuPasteText1.Click += mnuPaste_Click;

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

        public TreeNode SelectedNode => _treeViewLibrary.SelectedNode;

        public void ChangeType(TreeNode node, CodeType newType)
        {
            if (IsSystem(node))
                return;

            CodeSnippet snippet = CodeLib.Instance.Library.Get(node.Name);
            if (snippet != null)
            {
                _textBoxHelper.ChangeType(snippet, newType);
                snippet.CodeType = newType;
                int imageIndex = _fileHelper.GetImageIndex(snippet);
                node.ImageIndex = imageIndex;
                node.SelectedImageIndex = imageIndex;
            }
        }

        public CodeType CodeTypeByExtension(FileInfo file)
        {
            string _extension = file.Extension.Trim(new char[] { '.' }).ToLower();
            switch (_extension)
            {
                case "vb":
                    return CodeType.VB;

                case "cs":
                    return CodeType.CSharp;

                case "js":
                case "ts":
                case "json":
                    return CodeType.JS;

                case "txt":
                case "inf":
                case "info":
                case "nfo":
                case "md":
                    return CodeType.None;

                case "html":
                case "htm":
                    return CodeType.HTML;

                case "resx":
                case "xml":
                case "xmlt":
                case "xlt":
                case "xslt":
                    return CodeType.XML;

                case "sql":
                    return CodeType.SQL;

                case "rtf":
                    return CodeType.RTF;

                case "jpg":
                case "jpeg":
                case "png":
                case "bmp":
                    return CodeType.Image;
            }
            return CodeType.UnSuported;
        }

        public TreeNode CreateNewNode(TreeNodeCollection parent, CodeType codetype, string name, string text, string rtf)
        {
            CodeSnippet snippet = new CodeSnippet() { Code = text, CodeType = codetype, Locked = false, Name = name, RTF = rtf };
            CodeLib.Instance.Library.Add(snippet);

            int _imageIndex = 0;
            TreeNode _node = parent.Add(snippet.Name, snippet.Name, _imageIndex, _imageIndex);
            _node.Name = snippet.Id;
            ChangeType(_node, codetype);
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
            DialogResult _r = _f.ShowDialog();
            if (_r != DialogResult.OK)
            {
                return null;
            }

            string _noteName = _f.NoteName;

            int _repeat = _f.Repeat;

            TreeNode _newNode = new TreeNode();

            if (_f.Root)
            {
                if (_f.DefaultParent)
                {
                    var _parentSNippet = CodeLib.Instance.Library.Get(_noteName);
                    _parentSNippet.DefaultChildCodeTypeEnabled = true;
                    _parentSNippet.DefaultChildCodeType = _f.SelectedType;
                }
                for (int ii = 0; ii < _repeat; ii++)
                {
                    _newNode = CreateNewRootNode(_f.SelectedType, string.Format(_noteName, DateTime.Now, ii + 1), string.Empty);
                }

                return _newNode;
            }

            if (_f.DefaultParent)
            {
                var _parentSNippet = CodeLib.Instance.Library.Get(parent.Name);
                _parentSNippet.DefaultChildCodeTypeEnabled = true;
                _parentSNippet.DefaultChildCodeType = _f.SelectedType;
            }
            for (int ii = 0; ii < _repeat; ii++)
                _newNode = CreateNewNode(parent.Nodes, _f.SelectedType, string.Format(_noteName, DateTime.Now, ii + 1), string.Empty, string.Empty);

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
            GetAllChildIds(_fileHelper.TrashcanNode, ref _ids);

            _fileHelper.TrashcanNode.Nodes.Clear();

            CodeLib.Instance.Library.RemoveRange(_ids);

            if (CodeLib.Instance.Library.Count == 2)
            {
                CreateNewNode(_treeViewLibrary.Nodes, CodeType.Folder, Constants.SNIPPETS, "", "");
            }

            SetLibraryMenuState();
        }

        public bool FindNodeById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return false;

            foreach (TreeNode node in _treeViewLibrary.Nodes)
            {
                if (node.Name.Equals(id))
                {
                    _treeViewLibrary.SelectedNode = node;
                    //SetSelectedNode(node);
                    SetLibraryMenuState();
                    return true;
                }
                bool b = FindNodeById(id, node);
                if (b)
                    return true;
            }
            return false;
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

        public CodeSnippet FromNode(TreeNode node) => CodeLib.Instance.Library.Get(node.Name);

        public void GetAllChildIds(TreeNode node, ref List<string> ids)
        {
            foreach (TreeNode child in node.Nodes)
            {
                ids.Add(child.Name);
                GetAllChildIds(child, ref ids);
            }
        }

        public string GetDefaultCode(TreeNode node, string defaultDefault, int level, ref int nodecount)
        {
            if (level == 0)
            {
                nodecount = node.Nodes.Count;
            }

            CodeSnippet _snippet = FromNode(node);
            if (!string.IsNullOrEmpty(_snippet.DefaultChildCode))
                return string.Format(_snippet.DefaultChildCode, DateTime.Now, nodecount);

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
                return string.Format(_snippet.DefaultChildName, DateTime.Now, nodecount);

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

        public TreeNode GetRootNode(TreeNode node)
        {
            TreeNode root = node;
            if (root == null)
                return node;

            while (root.Parent != null)
            {
                if (root.Parent == null)
                    return root;

                root = root.Parent;
            }

            return root;
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

            CodeSnippet snippet = CodeLib.Instance.Library.Get(node.Name);
            return (snippet.CodeType == CodeType.System && snippet.Id == Constants.CLIPBOARDMONITOR);
        }

        public bool IsImage(TreeNode node)
        {
            if (node == null)
                return false;

            CodeSnippet snippet = CodeLib.Instance.Library.Get(node.Name);
            return (snippet.CodeType == CodeType.Image);
        }

        public bool IsInTrashcan(TreeNode node)
        {
            if (node == null)
                return false;

            TreeNode root = GetRootNode(node);
            return IsTrashcan(root);
        }

        public bool IsSystem(TreeNode node)
        {
            if (node == null)
                return false;

            CodeSnippet snippet = CodeLib.Instance.Library.Get(node.Name);
            return snippet.CodeType == CodeType.System;
        }

        public bool IsTrashcan(TreeNode node)
        {
            if (node == null)
                return false;

            CodeSnippet snippet = CodeLib.Instance.Library.Get(node.Name);
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

        public void MoveDown()
        {
            if (IsSystem(_treeViewLibrary.SelectedNode))
                return;

            if (_treeViewLibrary.SelectedNode != null)
                _treeViewLibrary.SelectedNode.MoveDown();
        }

        public void MoveUp()
        {
            if (IsSystem(_treeViewLibrary.SelectedNode))
                return;

            if (_treeViewLibrary.SelectedNode != null)
                _treeViewLibrary.SelectedNode.MoveUp();
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

        public void RefreshCurrentTreeNode()
        {
            RefreshNode(_treeViewLibrary.SelectedNode);
            SetLibraryMenuState();
        }

        public void RefreshNode(TreeNode node)
        {
            if (IsSystem(node))
                return;

            CodeSnippet snippet = CodeLib.Instance.Library.Get(node.Name);
            if (snippet != null)
            {
                int imageIndex = _fileHelper.GetImageIndex(snippet);
                node.ImageIndex = imageIndex;
                node.SelectedImageIndex = imageIndex;
                _textBoxHelper.ChangeType(snippet, snippet.CodeType);
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
                    _mainform.containerCode.Visible = true;
                    _mainform.containerImage.Visible = false;
                    _mainform.containerRtfEditor.Visible = false;
                    _textBoxHelper.CodeToScreen(_snippet);
                    _treeViewLibrary.SelectedNode = node;
                    _mainform.containerInfoBar.Visible = true;

                    SetLibraryMenuState();
                    break;

                case CodeType.RTF:
                    _mainform.containerRtfEditor.Visible = true;
                    _mainform.containerCode.Visible = false;
                    _mainform.containerImage.Visible = false;
                    _textBoxHelper.CodeToScreen(_snippet);
                    _treeViewLibrary.SelectedNode = node;
                    _mainform.containerInfoBar.Visible = true;
                    SetLibraryMenuState();

                    break;
            }
            _textBoxHelper.ApplySettings();
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

            _mainform.mnuType2CSharp.Checked = (_type == CodeType.CSharp);
            _mainform.mnuType2Folder.Checked = (_type == CodeType.Folder);
            _mainform.mnuType2Html.Checked = (_type == CodeType.HTML);
            _mainform.mnuType2JS.Checked = (_type == CodeType.JS);
            _mainform.mnuType2Lua.Checked = (_type == CodeType.Lua);
            _mainform.mnuType2None.Checked = (_type == CodeType.None);
            _mainform.mnuType2Php.Checked = (_type == CodeType.PHP);
            _mainform.mnuType2Rtf.Checked = (_type == CodeType.RTF);
            _mainform.mnuType2Sql.Checked = (_type == CodeType.SQL);
            _mainform.mnuType2Template.Checked = (_type == CodeType.Template);
            _mainform.mnuType2VB.Checked = (_type == CodeType.VB);
            _mainform.mnuType2Xml.Checked = (_type == CodeType.XML);

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

        private static byte[] ConvertImageToByteArray(Image imageToConvert)
        {
            byte[] Ret;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    imageToConvert.Save(ms, ImageFormat.Jpeg);
                    Ret = ms.ToArray();
                }
            }
            catch (Exception) { throw; }
            return Ret;
        }

        private static byte[] ConvertImageToByteArray(Image imageToConvert, long quality)
        {
            ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
            Encoder myEncoder = Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            myEncoderParameters.Param[0] = new EncoderParameter(myEncoder, quality);
            ImageCodecInfo.GetImageEncoders();

            byte[] Ret;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    imageToConvert.Save(ms, jgpEncoder, myEncoderParameters);
                    Ret = ms.ToArray();
                }
            }
            catch (Exception) { throw; }
            return Ret;
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        private void AddFiles(TreeNode targetNode, string[] filenames)
        {
            List<string> _filenames = filenames.ToList();
            _filenames.Sort();

            foreach (string filename in _filenames)
            {
                FileInfo _file = new FileInfo(filename);
                var _type = CodeTypeByExtension(_file);

                switch (_type)
                {
                    case CodeType.Image:
                        byte[] _imageData = File.ReadAllBytes(filename);
                        AddImageNode(targetNode, _imageData, _file.Name);
                        break;

                    case CodeType.CSharp:
                    case CodeType.HTML:
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
                        CreateNewNode(targetNode.Nodes, _type, _file.Name, _text, _text);
                        break;

                    case CodeType.System:
                    case CodeType.UnSuported:
                        break;
                }
            }
        }

        private void AddImageNode(TreeNode parentNode, Image image, string name)
        {
            byte[] _imageData = ConvertImageToByteArray(image, 33L);
            AddImageNode(parentNode, _imageData, name);
        }

        private void AddImageNode(TreeNode parentNode, byte[] _imageData, string name)
        {
            CodeSnippet snippet = new CodeSnippet() { Code = "", CodeType = CodeType.Image, Locked = false, Name = name, Blob = _imageData };
            CodeLib.Instance.Library.Add(snippet);

            int _imageIndex = 0;
            TreeNode _node = parentNode.Nodes.Add(snippet.Name, snippet.Name, _imageIndex, _imageIndex);
            _node.Name = snippet.Id;
            ChangeType(_node, CodeType.Image);
        }

        private void AddImageNodeNoCompression(TreeNode parentNode, Image image, string name)
        {
            byte[] _imageData = ConvertImageToByteArray(image);
            AddImageNode(parentNode, _imageData, name);
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

        private void CopyAsBase64StringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CodeSnippet _snippet = CodeLib.Instance.Library.Get(_SelectedId);
            string _base64 = Convert.ToBase64String(_snippet.Blob);
            Clipboard.SetText(_base64);
        }

        private void CopyAsHTMLIMGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CodeSnippet _snippet = CodeLib.Instance.Library.Get(_SelectedId);
            string _base64 = Convert.ToBase64String(_snippet.Blob);
            Clipboard.SetText(string.Format(@"<img src=""data:image/png;base64,{0}"" />", _base64));
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
                _mainform.contextMenuImage.Show(Cursor.Position.X, Cursor.Position.Y);
            }
        }

        private void mnuPaste_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                CreateNewNode(_treeViewLibrary.SelectedNode.Nodes, CodeType.None, "New Note", Clipboard.GetText(), "");
            }
        }

        private List<TreeNode> ParentPath(TreeNode node)
        {
            List<TreeNode> parents = new List<TreeNode>();
            while (node.Parent != null)
            {
                node = node.Parent;
                if (node != null)
                    parents.Add(node);
            }
            return parents;
        }

        private void PasteClipBoardEachLine()
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

        private void PasteClipBoardFileList()
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

        private void PasteFilelistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsFileDropList())
            {
                PasteClipBoardFileList();
            }
        }

        private void PasteImageCompressedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                AddImageNode(_treeViewLibrary.SelectedNode, Clipboard.GetImage(), "image");
            }
        }

        private void PasteImageNoCompressionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                AddImageNodeNoCompression(_treeViewLibrary.SelectedNode, Clipboard.GetImage(), "image");
            }
        }

        private void PasteTextPerLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                PasteClipBoardEachLine();
            }
        }

        private void SaveImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog _dialog = new SaveFileDialog();
            _dialog.Filter = "JPEG Image|*.jpg";
            DialogResult _result = _dialog.ShowDialog();
            if (_result == DialogResult.OK)
            {
                string _filename = _dialog.FileName;
                byte[] _bytes = ConvertImageToByteArray(_mainform.imageViewer.Image, 100L);
                File.WriteAllBytes(_filename, _bytes);
            }
        }

        private void SetLibraryMenuState()
        {
            _mainform.mnuAdd.Enabled = (!IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode));
            _mainform.mnuAdd1.Enabled = (!IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode));
            _mainform.mnuAddDialog.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);

            _mainform.mnuRemove.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsInTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);
            _mainform.mnuRemove1.Enabled = !IsInTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);

            _mainform.mnuChangeType.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsImage(_treeViewLibrary.SelectedNode);
            _mainform.mnuChangeType1.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsImage(_treeViewLibrary.SelectedNode);

            _mainform.mnuQuickRename.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsImage(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);
            _mainform.mnuQuickRename1.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsImage(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);

            _mainform.mnuMarkImportant.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);
            _mainform.mnuMarkImportant1.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);

            _mainform.mnuCopyPath.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);
            _mainform.mnuCopyPath1.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);

            _mainform.mnuMoveUp.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);
            _mainform.mnuMoveUp1.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);

            _mainform.mnMoveDown.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);
            _mainform.mnuMoveDown1.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);

            _mainform.mnuClipboard.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);
            _mainform.mnuClipboard1.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);

            _mainform.mnuProperties.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsImage(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);
            _mainform.mnuProperties1.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsImage(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);

            _mainform.mnuSearch.Enabled = !IsTrashcan(_treeViewLibrary.SelectedNode) && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);

            _mainform.mnuPasteFilelist.Visible = !IsTrashcan(_treeViewLibrary.SelectedNode) && Clipboard.ContainsFileDropList() && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);
            _mainform.mnuPasteFilelist1.Visible = !IsTrashcan(_treeViewLibrary.SelectedNode) && Clipboard.ContainsFileDropList() && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);

            _mainform.mnuPasteImageTool.Visible = !IsTrashcan(_treeViewLibrary.SelectedNode) && Clipboard.ContainsImage() && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);
            _mainform.mnuPasteImageTool1.Visible = !IsTrashcan(_treeViewLibrary.SelectedNode) && Clipboard.ContainsImage() && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);

            _mainform.mnuPasteImageNoCompression.Visible = !IsTrashcan(_treeViewLibrary.SelectedNode) && Clipboard.ContainsImage() && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);
            _mainform.mnuPasteImageNoCompression1.Visible = !IsTrashcan(_treeViewLibrary.SelectedNode) && Clipboard.ContainsImage() && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);

            _mainform.mnuPasteTextPerLine.Visible = !IsTrashcan(_treeViewLibrary.SelectedNode) && Clipboard.ContainsText() && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);
            _mainform.mnuPasteTextPerLine1.Visible = !IsTrashcan(_treeViewLibrary.SelectedNode) && Clipboard.ContainsText() && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);

            _mainform.mnuPasteText.Visible = !IsTrashcan(_treeViewLibrary.SelectedNode) && Clipboard.ContainsText() && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);
            _mainform.mnuPasteText1.Visible = !IsTrashcan(_treeViewLibrary.SelectedNode) && Clipboard.ContainsText() && !IsClipBoardMonitor(_treeViewLibrary.SelectedNode);
        }

        private void SetToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetImage(_mainform.imageViewer.Image);
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

                    List<TreeNode> parents = ParentPath(node);

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
            SetSelectedNode(e.Node, true);
        }

        private void TreeViewLibrary_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_SelectedId))
                return;

            CodeSnippet _snippet = CodeLib.Instance.Library.Get(_SelectedId);
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

        private void TreeViewLibrary_KeyUp(object sender, KeyEventArgs e)
        {
            SetLibraryMenuState();

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
                    CreateNewNode(_treeViewLibrary.SelectedNode.Nodes, CodeType.None, "New Note", Clipboard.GetText(), "");
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
                    _mainform.contextMenuStripTrashcan.Show(Cursor.Position.X, Cursor.Position.Y);
                    return;
                }
                if (IsClipBoardMonitor(_treeViewLibrary.SelectedNode))
                {
                    _mainform.contextMenuClipboardMonitor.Show(Cursor.Position.X, Cursor.Position.Y);
                    return;
                }

                _mainform.contextMenuStripLibrary.Show(Cursor.Position.X, Cursor.Position.Y);
            }
        }
    }
}