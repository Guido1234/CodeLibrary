using CodeLibrary.Core;
using DevToys;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Windows.Forms;

namespace CodeLibrary
{
    public class FileHelper
    {
        private readonly int _AutoSaveMinutes = 1;
        private readonly Timer _autoSaveTimer = new Timer();
        private readonly FormCodeLibrary _mainform;
        private readonly TreeView _treeview;
        private String _AutoSaveFileName = string.Empty;
        private string _Find = string.Empty;
        private DateTime _lastAutoSavedDate = new DateTime();
        private DateTime _lastOpenedDate = DateTime.Now;
        private Cursor _PrevCursor;
        private int _updating = 0;

        public FileHelper(TreeView treeview, FormCodeLibrary mainform)
        {
            _treeview = treeview;
            _mainform = mainform;
            _lastAutoSavedDate = DateTime.Now;
            _autoSaveTimer.Interval = 1000;
            _autoSaveTimer.Tick += AutoSaveTimer_Tick;
            _autoSaveTimer.Start();
        }

        public TreeNode ClipBoardMonitorNode { get; set; }

        public string CurrentFile { get; set; }

        public bool IsUpdating => _updating > 0;

        public SecureString Password { get; set; }

        public string SelectedId { get; set; }

        public TreeNode TrashcanNode { get; set; }

        public TreeviewHelper TreeHelper { get; set; }

        public void BeginUpdate()
        {
            if (_updating == 0)
            {
                _PrevCursor = _mainform.Cursor;
            }
            _updating++;
            _mainform.UseWaitCursor = true;
            _mainform.Cursor = Cursors.WaitCursor;
        }

        public Dictionary<string, TreeNode> CodeCollectionToForm(string find)
        {
            _Find = find;

            _treeview.BeginUpdate();

            List<CodeSnippet> items = new List<CodeSnippet>();

            if (string.IsNullOrWhiteSpace(find))
            {
                items = CodeLib.Instance.Library.OrderBy(p => p.Order).OrderBy(p => Utils.SplitPath(p.Path, '\\').Length).ToList();
            }
            else
            {
                items = FindNodes(find).OrderBy(p => p.Order).OrderBy(p => Utils.SplitPath(p.Path, '\\').Length).ToList();
            }
            Dictionary<string, TreeNode> _foundNodes = new Dictionary<string, TreeNode>();

            _treeview.Nodes.Clear();

            foreach (CodeSnippet snippet in items)
            {
                if (string.IsNullOrEmpty(snippet.Id))
                    snippet.Id = Guid.NewGuid().ToString();

                TreeNodeCollection parentCollection = _treeview.Nodes;
                string parentPath = Utils.ParentPath(snippet.Path, '\\');
                string name = Utils.PathName(snippet.Path, '\\');

                TreeNode parent = GetNodeByParentPath(_treeview.Nodes, parentPath);
                if (parent != null)
                    parentCollection = parent.Nodes;

                int imageIndex = GetImageIndex(snippet);

                TreeNode node = new TreeNode(name, imageIndex, imageIndex) { Name = snippet.Id };
                _foundNodes.Add(snippet.Id, node);

                parentCollection.Add(node);

                if (snippet.Id == Constants.TRASHCAN)
                    TrashcanNode = node;

                if (snippet.Important)
                    _treeview.SelectedNode = node;
            }

            CodeLib.Instance.BuildNodeIndexer(_treeview);

            if (!string.IsNullOrWhiteSpace(find))
                _treeview.ExpandAll();

            _treeview.EndUpdate();

            return _foundNodes;
        }

        public void CodeCollectionToForm(TreeNode root, DictionaryList<CodeSnippet, string> library)
        {
            string _basepath = root.FullPath;

            root.Nodes.Clear();
            List<CodeSnippet> items = library.OrderBy(p => p.Order).OrderBy(p => Utils.SplitPath(p.Path, '\\').Length).ToList();
            //List<CodeSnippet> items = CodeLib.Instance.Library.OrderBy(p => PathUtility.SplitPath(p.Path, '\\').Length).ToList();
            int _x = 0;

            foreach (CodeSnippet snippet in items)
            {
                _x++;
                // #TODO sort on: snippet.Order

                TreeNodeCollection parentCollection = root.Nodes;

                // string _extendedpath = Path.Combine(_basepath.TrimEnd(new char[] { '\\' }), snippet.Path.TrimEnd(new char[] { '\\' }));

                string _extendedpath = Utils.CombinePath(_basepath, snippet.Path);

                string _parentPath = Utils.ParentPath(_extendedpath, '\\');
                string _name = Utils.PathName(_extendedpath, '\\');

                TreeNode parent = GetNodeByParentPath(root.Nodes, _parentPath);
                if (parent != null)
                    parentCollection = parent.Nodes;

                int imageIndex = GetImageIndex(snippet);

                TreeNode node = new TreeNode(_name, imageIndex, imageIndex) { Name = snippet.Id };

                parentCollection.Add(node);
            }

            CodeLib.Instance.BuildNodeIndexer(_treeview);
        }

        public void EndUpdate()
        {
            _updating--;
            if (_updating <= 0)
            {
                _mainform.Cursor = _PrevCursor;
                _mainform.UseWaitCursor = false;
            }
        }

        public List<CodeSnippet> FindNodes(string find)
        {
            DictionaryList<CodeSnippet, string> _items = CodeLib.Instance.Library.Where(p => LastPart(p.Path).ToLower().Contains(find.ToLower())).ToDictionaryList(p => p.Id);
            _items.RegisterLookup("PATH", p => p.Path);

            DictionaryList<CodeSnippet, string> _paths = new DictionaryList<CodeSnippet, string>(p => p.Path);
            foreach (CodeSnippet item in _items)
            {
                List<CodeSnippet> _parents = GetParents(item.Path);

                foreach (CodeSnippet parent in _parents)
                {
                    if (!_paths.ContainsKey(parent.Path) && (_items.Lookup("PATH", parent.Path).FirstOrDefault() == null))
                        _paths.Add(parent);
                }
            }

            _items.AddRange(_paths);
            return _items.ToList();
        }

        public void FormToCodeLib()
        {
            CodeSnippetCollection collection = new CodeSnippetCollection { LastSaved = _lastOpenedDate };

            FormToCodeCollection(_treeview.Nodes);
            if (_treeview.SelectedNode != null)
                collection.LastSelected = _treeview.SelectedNode.FullPath;

            _mainform.SaveEditor();
            CodeLib.Instance.Save(collection);
        }

        public int GetImageIndex(CodeSnippet snippet)
        {
            if (snippet.Important)
                return 2;

            if (snippet.CodeType == CodeType.System && snippet.Id == Constants.TRASHCAN)
                return 3;

            if (snippet.CodeType == CodeType.System && snippet.Id == Constants.CLIPBOARDMONITOR)
                return 11;

            if (snippet.AlarmActive)
                return 5;

            return GetImageIndex(snippet.CodeType);
        }

        public int GetImageIndex(CodeType type)
        {
            switch (type)
            {
                case CodeType.Template:
                    return 1;

                case CodeType.CSharp:
                case CodeType.HTML:
                case CodeType.VB:
                case CodeType.JS:
                case CodeType.PHP:
                case CodeType.XML:
                case CodeType.Lua:
                case CodeType.None:
                case CodeType.RTF:
                case CodeType.SQL:
                    return 1;

                case CodeType.Folder:
                    return 0;

                case CodeType.Image:
                    return 10;
            }
            return 0;
        }

        public TreeNode GetNodeByParentPath(TreeNodeCollection collection, string path)
        {
            foreach (TreeNode node in collection)
            {
                if (node.FullPath.Equals(path))
                    return node;
            }
            foreach (TreeNode node in collection)
            {
                TreeNode subnode = GetNodeByParentPath(node.Nodes, path);
                if (subnode != null)
                    return subnode;
            }
            return null;
        }

        public void ImportFile()
        {
            BeginUpdate();

            OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "text Files (*.txt)|*.json|All Files (*.*)|*.*" };
            if (openFileDialog.ShowDialog(_mainform) == DialogResult.OK)
            {
                string filename = openFileDialog.FileName;
                string text = File.ReadAllText(filename);
                FileInfo fi = new FileInfo(filename);
                CodeType codetype = CodeType.None;
                switch (fi.Extension.ToLower())
                {
                    case "cs":
                        codetype = CodeType.CSharp;
                        break;

                    case "vb":
                        codetype = CodeType.VB;
                        break;

                    case "txt":
                        codetype = CodeType.None;
                        break;

                    case "html":
                    case "htm":
                        codetype = CodeType.HTML;
                        break;

                    case "tsql":
                    case "sql":
                        codetype = CodeType.SQL;
                        break;

                    case "xml":
                    case "xmlt":
                        codetype = CodeType.XML;
                        break;

                    case "js":
                        codetype = CodeType.JS;
                        break;

                    case "php":
                        codetype = CodeType.PHP;
                        break;

                    case "lua":
                        codetype = CodeType.Lua;
                        break;

                    case "rtf":
                        codetype = CodeType.RTF;
                        break;
                }
                TreeHelper.CreateNewRootNode(codetype, fi.Name, text);
            }

            EndUpdate();
        }

        public bool IsOverwritingNewerFile(string filename)
        {
            if (File.Exists(filename))
                return false;

            DateTime _lastSaved = File.GetLastWriteTime(filename);

            if (_lastSaved > _lastOpenedDate)
                return true;

            return false;
        }

        public void NewDoc()
        {
            CurrentFile = null;
            CodeLib.Instance.New();
            CodeCollectionToForm(string.Empty);
        }

        public void OpenFile() => OpenFile(null);

        public void OpenFile(TreeNode rootnode)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "json Files (*.json)|*.json|All Files (*.*)|*.*",
                InitialDirectory = Config.LastOpenedDir
            };
            if (openFileDialog.ShowDialog(_mainform) == DialogResult.OK)
            {
                string filename = openFileDialog.FileName;
                OpenFile(filename, rootnode);
            }
        }

        public void OpenFile(string filename, TreeNode rootnode)
        {
            BeginUpdate();
            CodeSnippetCollection _collection = ReadCollection(filename, Password);

            if (_collection == null)
            {
                MessageBox.Show($"Could not open the file '{filename}'", "Error opening file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EndUpdate();
                return;
            }

            if (rootnode != null)
            {
                DictionaryList<CodeSnippet, string> library = new DictionaryList<CodeSnippet, string>(p => p.Id);
                CodeLib.Import(_collection, library, true);
                CodeCollectionToForm(rootnode, library);
                CodeLib.Instance.Import(_collection);
                EndUpdate();
                return;
            }

            CodeLib.Instance.Load(_collection);

            if (rootnode == null && !CodeLib.Instance.Library.ContainsKey(Constants.TRASHCAN))
                CodeLib.Instance.Library.Add(CodeSnippet.TrashcanSnippet());

            if (rootnode == null && !CodeLib.Instance.Library.ContainsKey(Constants.CLIPBOARDMONITOR))
                CodeLib.Instance.Library.Add(CodeSnippet.ClipboardMonitorSnippet());

            //var snippet = CodeLib.Instance.Library.Where(p => p.Name == "TEMP").FirstOrDefault();
            //snippet.CodeType = CodeType.None;

            CodeCollectionToForm(string.Empty);

            EndUpdate();

            TreeHelper.FindNodeByPath(_collection.LastSelected);

            if (Password == null)
                Config.LastOpenedFile = filename;

            FileInfo fi = new FileInfo(filename);

            if (Password == null)
                Config.LastOpenedDir = fi.Directory.FullName;

            CurrentFile = filename;
            _lastOpenedDate = DateTime.Now;
            SetTitle();
        }

        public void Reload()
        {
            BeginUpdate();

            if (Config.LastOpenedFile != null)
                if (Utils.IsFileOrDirectory(Config.LastOpenedFile) == Utils.FileOrDirectory.File)
                    OpenFile(Config.LastOpenedFile, null);

            EndUpdate();
        }

        public void RestoreBackup()
        {
            if (string.IsNullOrEmpty(CurrentFile))
            {
                return;
            }

            FormBackupRestore _f = new FormBackupRestore(CurrentFile);
            var _result = _f.ShowDialog();
            if (_result == DialogResult.OK)
            {
                LoadBackup(_f.Selected.Path);
            }
        }

        public void SaveFile(bool saveas) => SaveFile(saveas, null);

        public void SaveFile(bool saveas, TreeNode rootnode)
        {
            string _selectedfile = CurrentFile;

            if (string.IsNullOrEmpty(_selectedfile))
                saveas = true || rootnode != null;

            if (saveas == true)
            {
                SaveFileDialog d = new SaveFileDialog { Filter = "json Files (*.json)|*.json|All Files (*.*)|*.*" };
                DialogResult dr = d.ShowDialog();
                if (dr == DialogResult.Cancel)
                    return;

                _selectedfile = d.FileName;
            }

            if (IsOverwritingNewerFile(_selectedfile))
            {
                FormOverwrite f = new FormOverwrite();
                f.ShowDialog();
                switch (f.DlgResult)
                {
                    case OverwriteMode.Cancel:
                        return;

                    case OverwriteMode.Overwrite:
                        break;

                    case OverwriteMode.Reload:
                        Reload();
                        return;
                }
            }

            if (rootnode == null)
            {
                CurrentFile = _selectedfile;
                _lastOpenedDate = DateTime.Now;
                SetTitle();
            }

            CodeSnippetCollection _collection = new CodeSnippetCollection { LastSaved = _lastOpenedDate };

            if (rootnode == null)
            {
                FormToCodeCollection(_treeview.Nodes);
                if (_treeview.SelectedNode != null)
                    _collection.LastSelected = _treeview.SelectedNode.FullPath;

                _mainform.SaveEditor();
                CodeLib.Instance.Save(_collection);
            }
            else
            {
                FormToCodeCollection(rootnode.Nodes, rootnode);
                List<string> ids = new List<string>();
                GetIds(rootnode.Nodes, rootnode, ref ids);
                IEnumerable<CodeSnippet> _snippets = CodeLib.Instance.Library.GetRange(ids);
                _collection.Items.AddRange(_snippets);
            }

            if (rootnode == null)
            {
                BackupHelper backupHelper = new BackupHelper(CurrentFile);
                backupHelper.Backup();
            }

            Save(_collection, _selectedfile);
        }

        private static CodeSnippetCollection ReadCollection(string filename, SecureString password)
        {
            string _data = File.ReadAllText(filename, Encoding.Default);
            if (password != null)
                _data = StringCipher.Decrypt(_data, password);
            try
            {
                CodeSnippetCollection _collection = Utils.FromJson<CodeSnippetCollection>(_data);
                _collection.FromBase64();
                return _collection;
            }
            catch
            {
                return null;
            }
        }

        private void AutoSaveFile()
        {
            string _fileName = GetAutoSaveFileName();

            CodeSnippetCollection _collection = new CodeSnippetCollection { LastSaved = _lastOpenedDate };

            FormToCodeCollection(_treeview.Nodes);
            if (_treeview.SelectedNode != null)
                _collection.LastSelected = _treeview.SelectedNode.FullPath;

            _mainform.SaveEditor();
            CodeLib.Instance.Save(_collection);

            Save(_collection, _fileName);
        }

        private void AutoSaveTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan _elapsed = DateTime.Now - _lastAutoSavedDate;
            if (_elapsed.TotalMinutes > _AutoSaveMinutes)
            {
                _lastAutoSavedDate = DateTime.Now;
                AutoSaveFile();
            }
        }

        private void FormToCodeCollection(TreeNodeCollection nodes)
        {
            int _order = 0;
            foreach (TreeNode node in nodes)
            {
                CodeSnippet _snippet = CodeLib.Instance.Library.Get(node.Name);

                _snippet.Path = node.FullPath;
                _snippet.Name = node.Name;

                if (string.IsNullOrWhiteSpace(_Find))
                    _snippet.Order = _order;

                _order++;

                if (_snippet.CodeType == CodeType.System && _snippet.Id == Constants.TRASHCAN)
                    _snippet.Order = -2;

                if (_snippet.CodeType == CodeType.System && _snippet.Id == Constants.CLIPBOARDMONITOR)
                    _snippet.Order = -1;

                FormToCodeCollection(node.Nodes);
            }
        }

        private void FormToCodeCollection(TreeNodeCollection nodes, TreeNode root)
        {
            string _rootpath = $"##_{root.FullPath.TrimEnd(new char[] { '\\' })}";
            int _order = 0;
            foreach (TreeNode node in nodes)
            {
                CodeSnippet _snippet = CodeLib.Instance.Library.Get(node.Name);

                string _fullpath = $"##_{node.FullPath}";
                string _path = _fullpath.Replace(_rootpath, string.Empty).TrimStart(new char[] { '\\' });

                _snippet.Path = _path;
                _snippet.Name = node.Name;
                if (string.IsNullOrWhiteSpace(_Find))
                    _snippet.Order = _order;

                _order++;

                if (_snippet.CodeType == CodeType.System && _snippet.Id == Constants.TRASHCAN)
                    _snippet.Order = -2;

                if (_snippet.CodeType == CodeType.System && _snippet.Id == Constants.CLIPBOARDMONITOR)
                    _snippet.Order = -1;

                FormToCodeCollection(node.Nodes, root);
            }
        }

        private string GetAutoSaveFileName()
        {
            if (string.IsNullOrEmpty(_AutoSaveFileName) && string.IsNullOrEmpty(CurrentFile))
            {
                string _autoSaveDefaultPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                _AutoSaveFileName = Path.Combine(_autoSaveDefaultPath, $"{Guid.NewGuid()}_AutoSave.json");
                return _AutoSaveFileName;
            }
            if (string.IsNullOrEmpty(_AutoSaveFileName))
            {
                FileInfo _fileInfo = new FileInfo(CurrentFile);
                _AutoSaveFileName = Path.Combine(_fileInfo.Directory.FullName, $"{_fileInfo.Name}_AutoSave.json");
            }
            return _AutoSaveFileName;
        }

        private void GetIds(TreeNodeCollection nodes, TreeNode root, ref List<string> ids)
        {
            foreach (TreeNode node in nodes)
            {
                ids.Add(node.Name);
                GetIds(node.Nodes, root, ref ids);
            }
        }

        private List<CodeSnippet> GetParents(string path)
        {
            List<CodeSnippet> _result = new List<CodeSnippet>();
            string[] items = Utils.SplitPath(path, '\\');
            for (int ii = 0; ii < items.Length - 1; ii++)
            {
                string _parentPath = items[ii];
                CodeSnippet _item = CodeLib.Instance.Library.Lookup(Constants.LOOKUP_PATH, _parentPath).FirstOrDefault();
                if (_item != null)
                {
                    _result.Add(_item);
                }
            }
            return _result;
        }

        private string LastPart(string path)
        {
            int ii = path.IndexOf('\\');
            if (ii < 0)
                return path;

            return path.Substring(ii, path.Length - ii);
        }

        private void LoadBackup(string file)
        {
            string _lastOpened = Config.LastOpenedFile;

            BeginUpdate();

            if (file != null)
                if (Utils.IsFileOrDirectory(file) == Utils.FileOrDirectory.File)
                    OpenFile(file, null);

            Config.LastOpenedFile = _lastOpened;
            CurrentFile = _lastOpened;
            SetTitle();
            EndUpdate();
        }

        private void Save(CodeSnippetCollection collection, string fileName)
        {
            collection.ToBase64();
            string _json = Utils.ToJson(collection);

            if (Password != null)
            {
                _json = StringCipher.Encrypt(_json, Password);
            }

            collection.FromBase64();

            //string _base64Json = Utils.ToBase64(_json);

            //FileContainer _fileContainer = new FileContainer() { Version = Program.VersionNumber.ToString(), IsEncrypted = _encrypted, Data = _base64Json };
            //string _json2 = Utils.ToJson(_fileContainer);

            File.WriteAllText(fileName, _json);
        }

        private void SetTitle()
        {
            _mainform.Text = $"Code Library ( {CurrentFile} )";
        }
    }
}