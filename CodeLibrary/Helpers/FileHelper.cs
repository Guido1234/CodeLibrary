using CodeLibrary.Core;
using CodeLibrary.Editor;
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
        private readonly PasswordHelper _passwordHelper;
        private readonly StateIconHelper _StateIconHelper;
        private readonly TextBoxHelper _textBoxHelper;
        private readonly TreeView _treeViewLibrary;
        private String _AutoSaveFileName = string.Empty;
        private string _Find = string.Empty;
        private DateTime _lastAutoSavedDate = new DateTime();
        private DateTime _lastOpenedDate = DateTime.Now;
        private Cursor _PrevCursor;
        private int _updating = 0;

        public FileHelper(FormCodeLibrary mainform, DebugHelper debugHelper, TextBoxHelper textBoxHelper, PasswordHelper passwordHelper, StateIconHelper stateIconHelper)
        {
            _StateIconHelper = stateIconHelper;
            _DebugHelper = debugHelper;
            _mainform = mainform;
            _treeViewLibrary = _mainform.treeViewLibrary;
            _textBoxHelper = textBoxHelper;
            _passwordHelper = passwordHelper;
            CodeLib.Instance.ChangeStateChanged += Instance_ChangeStateChanged;

            _lastAutoSavedDate = DateTime.Now;
            _autoSaveTimer.Interval = 1000;
            _autoSaveTimer.Tick += AutoSaveTimer_Tick;
            _autoSaveTimer.Start();
        }

        public TreeNode ClipBoardMonitorNode { get; set; }

        public string CurrentFile { get; set; }

        public bool IsUpdating => _updating > 0;

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

            TreeHelper.BeginUpdate();

            List<CodeSnippet> items = new List<CodeSnippet>();

            if (string.IsNullOrWhiteSpace(find))
            {
                items = CodeLib.Instance.CodeSnippets.OrderBy(p => p.Order).OrderBy(p => Utils.SplitPath(p.Path, '\\').Length).ToList();
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
                if (snippet.CodeType == CodeType.ReferenceLink)
                {
                    var _refSnippet = CodeLib.Instance.CodeSnippets.Get(snippet.ReferenceLinkId);
                    name = Utils.PathName(_refSnippet.Path, '\\');
                }

                TreeNode parent = LocalUtils.GetNodeByParentPath(_treeViewLibrary.Nodes, parentPath);
                if (parent != null)
                    parentCollection = parent.Nodes;

                int imageIndex = LocalUtils.GetImageIndex(snippet);

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

            CodeLib.Instance.TreeNodes.Add(_treeViewLibrary);

            if (!string.IsNullOrWhiteSpace(find))
                _treeViewLibrary.ExpandAll();

            TreeHelper.EndUpdate();

            return _foundNodes;
        }

        public DialogResult DiscardChangesDialog()
        {
            DialogResult _dialogResult = DialogResult.Yes;
            if (CodeLib.Instance.Changed)
            {
                _dialogResult = MessageBox.Show(_mainform, "Changes have not been saved, are you sure?", "File not saved", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            }
            return _dialogResult;
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
            DictionaryList<CodeSnippet, string> _items = CodeLib.Instance.CodeSnippets.Where(p => LocalUtils.LastPart(p.Path).ToLower().Contains(find.ToLower())).ToDictionaryList(p => p.Id);
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
            _passwordHelper.UsbKeyId = null;
            _passwordHelper.Password = null;
            _AutoSaveFileName = null;
            _lastAutoSavedDate = new DateTime();
            _lastOpenedDate = DateTime.Now;

            CodeLib.Instance.New();
            CodeCollectionToForm(string.Empty);
            TreeHelper.FindNodeByPath("Snippets");
            _passwordHelper.ShowKey();
            SetTitle();
        }

        public void OpenFile()
        {
            if (DiscardChangesDialog() == DialogResult.No)
            {
                return;
            }

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
            CodeSnippetCollection _collection = ReadCollection(filename, _passwordHelper.Password, out _succes);
            if (_succes == false)
            {
                _collection = ReadCollectionOld(filename, _passwordHelper.Password, out _succes);
            }

            if (_succes == false)
            {
                MessageBox.Show($"Could not open the file '{filename}'", "Error opening file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                NewDoc();
                EndUpdate();
                return;
            }

            CodeLib.Instance.Load(_collection);

            if (!CodeLib.Instance.CodeSnippets.ContainsKey(Constants.TRASHCAN))
            {
                CodeLib.Instance.CodeSnippets.Add(CodeSnippet.TrashcanSnippet());
            }

            if (!CodeLib.Instance.CodeSnippets.ContainsKey(Constants.CLIPBOARDMONITOR))
            {
                CodeLib.Instance.CodeSnippets.Add(CodeSnippet.ClipboardMonitorSnippet());
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
                {
                    OpenFile(Config.LastOpenedFile);
                }

            EndUpdate();
        }

        public void RestoreBackup()
        {
            FormBackupRestore _f = new FormBackupRestore(CurrentFile);
            var _result = _f.ShowDialog();
            if (_result == DialogResult.OK)
            {
                CurrentFile = _f.CurrentFile;
                Config.LastOpenedFile = CurrentFile;
                SetTitle();
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

            SecureString _securepw = null;
            if (!string.IsNullOrEmpty(_passwordHelper.UsbKeyId))
            {
                bool _canceled = false;
                byte[] _key = _passwordHelper.GetUsbKey(_passwordHelper.UsbKeyId, false, out _canceled);
                if (_canceled)
                {
                    return;
                }
                _securepw = StringCipher.ToSecureString(Utils.ByteArrayToString(_key));
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

            Save(_collection, _selectedfile, _securepw);
        }

        internal void ShowIcon()
        {
            _StateIconHelper.Changed = CodeLib.Instance.Changed;
        }

        private static CodeSnippetCollection ReadCollectionOld(string filename, SecureString password, out bool succes)
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
                MessageBox.Show($"Could not decrypt: '{filename}' with the current password! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private static CodeSnippetCollection TryDecrypt(string data, SecureString password, out bool succes)
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
            catch (Exception e)
            {
            }
            succes = false;
            return null;
        }

        private void AutoSaveFile()
        {
            string _fileName = GetAutoSaveFileName();

            CodeSnippetCollection _collection = new CodeSnippetCollection
            {
                LastSaved = _lastOpenedDate,
            };

            SecureString _securepw = null;
            if (!string.IsNullOrEmpty(_passwordHelper.UsbKeyId))
            {
                bool _canceled = false;
                byte[] _key = _passwordHelper.GetUsbKey(_passwordHelper.UsbKeyId, true, out _canceled);
                if (_canceled)
                {
                    return;
                }
                _securepw = StringCipher.ToSecureString(Utils.ByteArrayToString(_key));
            }

            FormToCodeCollection(_treeViewLibrary.Nodes);
            if (_treeViewLibrary.SelectedNode != null)
                _collection.LastSelected = _treeViewLibrary.SelectedNode.FullPath;

            _mainform.SaveEditor();
            CodeLib.Instance.Save(_collection);

            Save(_collection, _fileName, _securepw);
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
                CodeSnippet _snippet = CodeLib.Instance.CodeSnippets.Get(node.Name);

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
                CodeSnippet _snippet = CodeLib.Instance.CodeSnippets.Get(node.Name);

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
                CodeSnippet _item = CodeLib.Instance.CodeSnippets.GetByPath(_parentPath);
                if (_item != null)
                {
                    _result.Add(_item);
                }
            }
            return _result;
        }

        private void Instance_ChangeStateChanged(object sender, EventArgs e)
        {
            ShowIcon();
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

        private CodeSnippetCollection ReadCollection(string filename, SecureString password, out bool succes)
        {
            string usbKeyId = null;
            succes = true;
            string _fileData = string.Empty;
            SecureString _usbKeyPassword = null;
            FileContainer _container = new FileContainer();

            try
            {
                _fileData = File.ReadAllText(filename, Encoding.Default);
                _container = Utils.FromJson<FileContainer>(_fileData);
                usbKeyId = _container.UsbKeyId;
            }
            catch
            {
                succes = false;
                return null;
            }

            if (_container.Encrypted)
            {
                if (!string.IsNullOrEmpty(_container.UsbKeyId))
                {
                    bool _canceled;
                    usbKeyId = _container.UsbKeyId;

                    byte[] _key = _passwordHelper.GetUsbKey(_container.UsbKeyId, false, out _canceled);
                    if (_canceled)
                    {
                        succes = false;
                        usbKeyId = null;
                        return null;
                    }
                    _usbKeyPassword = StringCipher.ToSecureString(Utils.ByteArrayToString(_key));

                    CodeSnippetCollection _result1 = TryDecrypt(_container.Data, _usbKeyPassword, out succes);
                    if (succes)
                    {
                        _passwordHelper.Password = null;
                        _passwordHelper.UsbKeyId = usbKeyId;
                        _passwordHelper.ShowKey();
                        return _result1;
                    }
                    else
                    {
                        MessageBox.Show(_mainform, $"Could not open file {filename}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _passwordHelper.Password = null;
                        _passwordHelper.UsbKeyId = null;
                        _passwordHelper.ShowKey();
                    }
                }

                // Decrypt with given password.
                if (password == null)
                {
                    goto setPassword;
                }

            retryPassword:
                CodeSnippetCollection _result = TryDecrypt(_container.Data, password, out succes);
                if (succes)
                {
                    _passwordHelper.Password = password;
                    _passwordHelper.UsbKeyId = null;
                    _passwordHelper.ShowKey();
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
                    _passwordHelper.Password = null;
                    _passwordHelper.UsbKeyId = null;
                    _passwordHelper.ShowKey();
                    succes = true;
                    return _collection;
                }
                catch
                {
                    succes = false;
                    return null;
                }
            }
        }

        private void Save(CodeSnippetCollection collection, string fileName, SecureString usbKeyPW)
        {
            collection.ToBase64();
            string _json = Utils.ToJson(collection);

            if (_passwordHelper.Password != null)
            {
                _json = StringCipher.Encrypt(_json, _passwordHelper.Password);
            }

            if (usbKeyPW != null)
            {
                _json = StringCipher.Encrypt(_json, usbKeyPW);
            }

            collection.FromBase64();

            string _base64Json = Utils.ToBase64(_json);

            FileContainer _fileContainer = new FileContainer()
            {
                Version = Config.CurrentVersion().ToString(),
                Encrypted = (_passwordHelper.Password != null) || !string.IsNullOrEmpty(_passwordHelper.UsbKeyId),
                Data = _base64Json,
                UsbKeyId = _passwordHelper.UsbKeyId
            };

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