using CodeLibrary.Core;
using CodeLibrary.Extensions;
using System;
using System.Linq;
using System.Windows.Forms;

namespace CodeLibrary
{
    public partial class FormAddNote : Form
    {
        public FormAddNote()
        {
            InitializeComponent();
            Load += FormAddNote_Load;
            AcceptButton = dialogButton1.buttonOk;
            tbName.TextChanged += TbName_TextChanged;
            cbRoot.CheckedChanged += CbRoot_CheckedChanged;
            this.GotFocus += FormAddNote_GotFocus;
        }

        public bool DefaultParent { get; set; }

        public string NoteName { get; set; }

        public TreeNode ParentNode { get; set; }

        public int Repeat { get; set; } = 1;

        public bool Root { get; set; }

        public CodeType SelectedType { get; set; }

        private void CbRoot_CheckedChanged(object sender, EventArgs e)
        {
            dialogButton1.buttonOk.Enabled = !NameExists();
        }

        private void dialogButton1_DialogButtonClick(object sender, Controls.DialogButton.DialogButtonClickEventArgs e)
        {
            if (e.Result == DialogResult.OK)
            {
                if (listViewTypes.SelectedItems.Count > 0)
                {
                    NoteName = tbName.Text;
                    Root = cbRoot.Checked;
                    DefaultParent = cbDefaultParent.Checked;
                    Repeat = (int)nuRepeat.Value;

                    string _name = listViewTypes.SelectedItems[0].Name;
                    switch (_name)
                    {
                        case "Folder":
                            SelectedType = CodeType.Folder;
                            break;

                        case "C#":
                            SelectedType = CodeType.CSharp;
                            break;

                        case "Sql":
                            SelectedType = CodeType.SQL;
                            break;

                        case "Text":
                            SelectedType = CodeType.None;
                            break;

                        case "Visual Basic":
                            SelectedType = CodeType.VB;
                            break;

                        case "Lua":
                            SelectedType = CodeType.Lua;
                            break;

                        case "Xml":
                            SelectedType = CodeType.XML;
                            break;

                        case "PHP":
                            SelectedType = CodeType.PHP;
                            break;

                        case "HTML":
                            SelectedType = CodeType.HTML;
                            break;

                        case "Rich Text":
                            SelectedType = CodeType.RTF;
                            break;

                        case "Markdown":
                            SelectedType = CodeType.MarkDown;
                            break;

                        case "Template":
                            SelectedType = CodeType.Template;
                            break;
                    }
                }
            }

            DialogResult = e.Result;
            Close();
        }

        private void FormAddNote_GotFocus(object sender, EventArgs e)
        {
            tbName.SelectAll();
            tbName.Focus();
        }

        private void FormAddNote_Load(object sender, EventArgs e)
        {
            cbRoot.Checked = Root;
            listViewTypes.Items.Add(new ListViewItem() { Selected = SelectedType == CodeType.Folder, ImageKey = "folder", Name = "Folder", Text = "Folder" });
            listViewTypes.Items.Add(new ListViewItem() { Selected = SelectedType == CodeType.CSharp, ImageKey = "c#", Name = "C#", Text = "C#" });
            listViewTypes.Items.Add(new ListViewItem() { Selected = SelectedType == CodeType.None, ImageKey = "txt", Name = "Text", Text = "Text" });
            listViewTypes.Items.Add(new ListViewItem() { Selected = SelectedType == CodeType.SQL, ImageKey = "sql", Name = "Sql", Text = "Sql" });
            listViewTypes.Items.Add(new ListViewItem() { Selected = SelectedType == CodeType.VB, ImageKey = "vb", Name = "Visual Basic", Text = "Visual Basic" });
            listViewTypes.Items.Add(new ListViewItem() { Selected = SelectedType == CodeType.Lua, ImageKey = "lua", Name = "Lua", Text = "Lua" });
            listViewTypes.Items.Add(new ListViewItem() { Selected = SelectedType == CodeType.XML, ImageKey = "xml", Name = "Xml", Text = "Xml" });
            listViewTypes.Items.Add(new ListViewItem() { Selected = SelectedType == CodeType.PHP, ImageKey = "php", Name = "PHP", Text = "PHP" });
            listViewTypes.Items.Add(new ListViewItem() { Selected = SelectedType == CodeType.HTML, ImageKey = "html", Name = "HTML", Text = "HTML" });
            listViewTypes.Items.Add(new ListViewItem() { Selected = SelectedType == CodeType.MarkDown, ImageKey = "txt", Name = "Markdown", Text = "Markdown" });
            listViewTypes.Items.Add(new ListViewItem() { Selected = SelectedType == CodeType.RTF, ImageKey = "rtf", Name = "Rich Text", Text = "Rich Text" });
            listViewTypes.Items.Add(new ListViewItem() { Selected = SelectedType == CodeType.Template, ImageKey = "template", Name = "Template", Text = "Template" });

            tbName.Text = $"New Note ({ CodeLib.Instance.Counter })";
            dialogButton1.buttonOk.Enabled = !NameExists();
        }

        private bool NameExists()
        {
            if (ParentNode == null)
            {
                return false;
            }

            bool _nameAlreadyExists = false;
            if (cbRoot.Checked)
            {
                _nameAlreadyExists = TreeViewExtensions.NodesEnumerated(ParentNode.TreeView.Nodes).Where(p => p.Text.Equals(tbName.Text, StringComparison.OrdinalIgnoreCase)).Any();
            }
            else
            {
                _nameAlreadyExists = TreeViewExtensions.NodesEnumerated(ParentNode.Nodes).Where(p => p.Text.Equals(tbName.Text, StringComparison.OrdinalIgnoreCase)).Any();
            }

            return _nameAlreadyExists;
        }

        private void TbName_TextChanged(object sender, EventArgs e)
        {
            dialogButton1.buttonOk.Enabled = !NameExists();
        }
    }
}