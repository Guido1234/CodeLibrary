using CodeLibrary.Core;
using CodeLibrary.Helpers;
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
        private readonly DebugHelper _DebugHelper;
        private readonly FormCodeLibrary _mainform;
        private readonly TreeView _treeViewLibrary;
        private readonly TextBoxHelper _textBoxHelper;
        private String _AutoSaveFileName = string.Empty;
        private string _Find = string.Empty;
        private DateTime _lastAutoSavedDate = new DateTime();
        private DateTime _lastOpenedDate = DateTime.Now;
        private Cursor _PrevCursor;
        private int _updating = 0;

        public FileHelper(FormCodeLibrary mainform, DebugHelper debugHelper, TextBoxHelper textBoxHelper)
        {
            _DebugHelper = debugHelper;
            _mainform = mainform;
            _treeViewLibrary = _mainform.treeViewLibrary;
            _textBoxHelper = textBoxHelper;

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

            List<TreeNode> _expandNodes = new List<TreeNode>();

            _treeViewLibrary.BeginUpdate();

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

            _treeViewLibrary.Nodes.Clear();

            foreach (CodeSnippet snippet in items)
            {
                if (string.IsNullOrEmpty(snippet.Id))
                    snippet.Id = Guid.NewGuid().ToString();

                TreeNodeCollection parentCollection = _treeViewLibrary.Nodes;
                string parentPath = Utils.ParentPath(snippet.Path, '\\');
                string name = Utils.PathName(snippet.Path, '\\');

                TreeNode parent = GetNodeByParentPath(_treeViewLibrary.Nodes, parentPath);
                if (parent != null)
                    parentCollection = parent.Nodes;

                int imageIndex = GetImageIndex(snippet);

                TreeNode node = new TreeNode(name, imageIndex, imageIndex) { Name = snippet.Id };
                _foundNodes.Add(snippet.Id, node);

                parentCollection.Add(node);

                if (snippet.Id == Constants.TRASHCAN)
                    TrashcanNode = node;

                if (snippet.Important)
                    _treeViewLibrary.SelectedNode = node;

                if (snippet.Expanded)
                    _expandNodes.Add(node);
            }
            foreach (TreeNode node in _expandNodes)
            {
                node.Expand();
            }

            CodeLib.Instance.BuildNodeIndexer(_treeViewLibrary);

            if (!string.IsNullOrWhiteSpace(find))
                _treeViewLibrary.ExpandAll();

            _treeViewLibrary.EndUpdate();

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

            CodeLib.Instance.BuildNodeIndexer(_treeViewLibrary);
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

            FormToCodeCollection(_treeViewLibrary.Nodes);
            if (_treeViewLibrary.SelectedNode != null)
                collection.LastSelected = _treeViewLibrary.SelectedNode.FullPath;

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
                case CodeType.MarkDown:
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
            _AutoSaveFileName = null;
            
            CodeLib.Instance.New();
            CodeCollectionToForm(string.Empty);
            TreeHelper.FindNodeByPath("Snippets");
        }

        public void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "json Files (*.json)|*.json|All Files (*.*)|*.*",
                InitialDirectory = Config.LastOpenedDir
            };
            if (openFileDialog.ShowDialog(_mainform) == DialogResult.OK)
            {
                string filename = openFileDialog.FileName;
                OpenFile(filename);
            }
        }

        public void OpenFile(string filename)
        {
            BeginUpdate();
            bool _succes = false;
            CodeSnippetCollection _collection = ReadCollection(filename, Password, ref _succes);
            if (_succes == false)
            {
                _collection = ReadCollectionOld(filename, Password, ref _succes);
            }

            if (_succes == false)
            {
                MessageBox.Show($"Could not open the file '{filename}'", "Error opening file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                NewDoc();
                EndUpdate();
                return;
            }

            CodeLib.Instance.Load(_collection);

            if (!CodeLib.Instance.Library.ContainsKey(Constants.TRASHCAN))
            {
                CodeLib.Instance.Library.Add(CodeSnippet.TrashcanSnippet());
            }

            if (!CodeLib.Instance.Library.ContainsKey(Constants.CLIPBOARDMONITOR))
            {
                CodeLib.Instance.Library.Add(CodeSnippet.ClipboardMonitorSnippet());
            }

            CodeCollectionToForm(string.Empty);

            EndUpdate();

            TreeHelper.FindNodeByPath(_collection.LastSelected);

            Config.LastOpenedFile = filename;
            FileInfo fi = new FileInfo(filename);
            Config.LastOpenedDir = fi.Directory.FullName;

            CurrentFile = filename;
            CodeLib.Instance.Changed = false;
            _lastOpenedDate = DateTime.Now;
            SetTitle();
        }

        public void Reload()
        {
            BeginUpdate();

            if (Config.LastOpenedFile != null)
                if (Utils.IsFileOrDirectory(Config.LastOpenedFile) == Utils.FileOrDirectory.File)
                    OpenFile(Config.LastOpenedFile);

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

        public void RestoreBackup(string file)
        {
            if (string.IsNullOrEmpty(file))
            {
                return;
            }

            FormBackupRestore _f = new FormBackupRestore(file);
            var _result = _f.ShowDialog();
            if (_result == DialogResult.OK)
            {
                CurrentFile = file;
                LoadBackup(_f.Selected.Path);
            }
        }

        public void SaveFile(bool saveas)
        {
            _textBoxHelper.SaveState();

            string _selectedfile = CurrentFile;

            if (string.IsNullOrEmpty(_selectedfile))
            {
                saveas = true;
            }

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

            
            CurrentFile = _selectedfile;
            _lastOpenedDate = DateTime.Now;
            SetTitle();
            
            CodeSnippetCollection _collection = new CodeSnippetCollection { LastSaved = _lastOpenedDate };

            FormToCodeCollection(_treeViewLibrary.Nodes);
            if (_treeViewLibrary.SelectedNode != null)
            {
                _collection.LastSelected = _treeViewLibrary.SelectedNode.FullPath;
            }

            _mainform.SaveEditor();
            CodeLib.Instance.Save(_collection);


            BackupHelper backupHelper = new BackupHelper(CurrentFile);
            backupHelper.Backup();


            Save(_collection, _selectedfile);
        }

        private static CodeSnippetCollection ReadCollectionOld(string filename, SecureString password, ref bool succes)
        {
            succes = true;
            string _data = File.ReadAllText(filename, Encoding.Default);
            try
            {
                if (password != null)
                {
                    _data = StringCipher.Decrypt(_data, password);
                }
            }
            catch
            {
                MessageBox.Show($"Could not decrypt: '{filename}' with the current password! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) ;
                succes = false;
                return null;
            }
            try
            {
                CodeSnippetCollection _collection = Utils.FromJson<CodeSnippetCollection>(_data);
                _collection.FromBase64();
                if (_collection.Items == null)
                {
                    succes = false;
                    return null;
                }
                succes = true;
                return _collection;
            }
            catch
            {
                succes = false;
                return null;
            }
        }

        private static CodeSnippetCollection TryDecrypt(string data, SecureString password, ref bool succes)
        {
            try
            {
                data = Utils.FromBase64(data);
                data = StringCipher.Decrypt(data, password);
                CodeSnippetCollection _collection = Utils.FromJson<CodeSnippetCollection>(data);
                _collection.FromBase64();
                succes = true;
                return _collection;
            }
            catch 
            { 
            }
            succes = false;
            return null;
        }


        private CodeSnippetCollection ReadCollection(string filename, SecureString password, ref bool succes)
        {
            succes = true;
            string _fileData = string.Empty;
            FileContainer _container = new FileContainer();

            try
            {
                _fileData = File.ReadAllText(filename, Encoding.Default);
                _container = Utils.FromJson<FileContainer>(_fileData);
            }
            catch
            {
                succes = false;
                return null;
            }

            if (_container.Encrypted)
            {
                // Decrypt with given password.
                if (password == null)
                {
                    goto setPassword;
                }

            retryPassword:
                CodeSnippetCollection _result = TryDecrypt(_container.Data, password, ref succes);
                if (succes)
                {
                    Password = password;
                    _mainform.ShowKey();
                    return _result;
                }

                setPassword:
                FormSetPassword _formSet = new FormSetPassword();
                DialogResult _dg = _formSet.ShowDialog();
                if (_dg == DialogResult.OK)
                {
                    password = _formSet.Password;
                    goto retryPassword;
                }     
                else
                {
                    succes = false;
                    return null;
                }
            }
            else
            {
                try
                {
                    CodeSnippetCollection _collection = Utils.FromJson<CodeSnippetCollection>(Utils.FromBase64(_container.Data));
                    _collection.FromBase64();
                    Password = null;
                    _mainform.ShowKey();
                    succes = true;
                    return _collection;
                }
                catch
                {
                    succes = false;
                    return null;
                }
            }

            return null;
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
                    return CodeType.None;

                case "md":
                    return CodeType.MarkDown;

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


        private void AutoSaveFile()
        {
            string _fileName = GetAutoSaveFileName();

            CodeSnippetCollection _collection = new CodeSnippetCollection { LastSaved = _lastOpenedDate };

            FormToCodeCollection(_treeViewLibrary.Nodes);
            if (_treeViewLibrary.SelectedNode != null)
                _collection.LastSelected = _treeViewLibrary.SelectedNode.FullPath;

            _mainform.SaveEditor();
            CodeLib.Instance.Save(_collection);

            Save(_collection, _fileName);
        }

        private void AutoSaveTimer_Tick(object sender, EventArgs e)
        {
            if (!_textBoxHelper.IsIdle)
            {
                return;
            }

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
                    OpenFile(file);

            Config.LastOpenedFile = _lastOpened;
            CurrentFile = _lastOpened;
            CodeLib.Instance.Changed = false;
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

            string _base64Json = Utils.ToBase64(_json);

            FileContainer _fileContainer = new FileContainer() { Version = Config.CurrentVersion().ToString(), Encrypted = (Password != null), Data = _base64Json };
            string _json2 = Utils.ToJson(_fileContainer);

            try
            {
                File.WriteAllText(fileName, _json2);
                CodeLib.Instance.Changed = false;
            }
            catch (UnauthorizedAccessException ua)
            {
                MessageBox.Show(_mainform, $"Access to file '{fileName}' denied!.", "File error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (Exception e)
            {
                return;
            }
        }

        private void SetTitle()
        {
            _mainform.Text = $"Code Library ( {CurrentFile} )";
        }
    }
}