using CodeLibrary.Core;
using DevToys;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CodeLibrary
{
    public partial class FormAddReference : Form
    {
        private FormCodeLibrary _mainform;

        public FormAddReference(FormCodeLibrary formCodeLibrary)
        {
            _mainform = formCodeLibrary;
            InitializeComponent();
            treeViewLibrary.ImageList = _mainform.imageList;
            Load += FormAddReference_Load;
            textBoxFind.KeyUp += TextBoxFind_KeyUp;
            this.GotFocus += FormAddReference_GotFocus;
        }

        public TreeNode SelectedNode { get; set; }

        public Dictionary<string, TreeNode> CodeCollectionToForm(string find, TreeView treeview)
        {
            List<TreeNode> _expandNodes = new List<TreeNode>();

            treeview.BeginUpdate();

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

            treeview.Nodes.Clear();

            foreach (CodeSnippet snippet in items)
            {
                if (string.IsNullOrEmpty(snippet.Id))
                    snippet.Id = Guid.NewGuid().ToString();

                if (snippet.Id == Constants.CLIPBOARDMONITOR)
                {
                    continue;
                }

                TreeNodeCollection parentCollection = treeview.Nodes;
                string parentPath = Utils.ParentPath(snippet.Path, '\\');
                string name = Utils.PathName(snippet.Path, '\\');

                TreeNode parent = LocalUtils.GetNodeByParentPath(treeview.Nodes, parentPath);
                if (parent != null)
                    parentCollection = parent.Nodes;

                int imageIndex = LocalUtils.GetImageIndex(snippet);

                TreeNode node = new TreeNode(name, imageIndex, imageIndex) { Name = snippet.Id };
                _foundNodes.Add(snippet.Id, node);

                parentCollection.Add(node);
            }

            if (!string.IsNullOrWhiteSpace(find))
                treeview.ExpandAll();

            treeview.EndUpdate();

            return _foundNodes;
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

        private void buttonFind_Click(object sender, EventArgs e)
        {
            FindNode();
        }

        private void dialogButton1_DialogButtonClick(object sender, Controls.DialogButton.DialogButtonClickEventArgs e)
        {
            if (e.Result == DialogResult.OK)
            {
                SelectedNode = treeViewLibrary.SelectedNode;
            }

            DialogResult = e.Result;
            Close();
        }

        private void FindNode()
        {
            Cursor.Current = Cursors.WaitCursor;
            Dictionary<string, TreeNode> _result = CodeCollectionToForm(textBoxFind.Text, treeViewLibrary);

            Cursor.Current = Cursors.Default;
        }

        private void FormAddReference_GotFocus(object sender, EventArgs e)
        {
        }

        private void FormAddReference_Load(object sender, EventArgs e)
        {
            switch (Config.Theme)
            {
                case ETheme.Light:
                    treeViewLibrary.ForeColor = Color.FromArgb(255, 0, 0, 0);
                    treeViewLibrary.BackColor = Color.FromArgb(255, 255, 255, 255);
                    break;

                case ETheme.Dark:
                    treeViewLibrary.ForeColor = Color.White;
                    treeViewLibrary.BackColor = Color.FromArgb(255, 75, 75, 75);
                    break;

                case ETheme.HighContrast:
                    treeViewLibrary.ForeColor = Color.White;
                    treeViewLibrary.BackColor = Color.FromArgb(255, 35, 35, 35);
                    break;
            }

            CodeCollectionToForm(string.Empty, treeViewLibrary);
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
    }
}