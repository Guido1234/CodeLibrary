using CodeLibrary.Core;
using System;
using System.Windows.Forms;

namespace CodeLibrary
{
    public partial class FormAddNote : Form
    {
        public FormAddNote()
        {
            InitializeComponent();
            Load += FormAddNote_Load;
        }

        public bool DefaultParent { get; set; }
        public string NoteName { get; set; }
        public int Repeat { get; set; } = 1;
        public bool Root { get; set; }
        public CodeType SelectedType { get; set; }

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

        private void FormAddNote_Load(object sender, EventArgs e)
        {
            cbRoot.Checked = Root;
            listViewTypes.Items.Add(new ListViewItem() { ImageKey = "folder", Name = "Folder", Text = "Folder", Selected = (Config.DefaultNoteType == "Folder") });
            listViewTypes.Items.Add(new ListViewItem() { ImageKey = "c#", Name = "C#", Text = "C#", Selected = (Config.DefaultNoteType == "C#") });
            listViewTypes.Items.Add(new ListViewItem() { ImageKey = "txt", Name = "Text", Text = "Text", Selected = (Config.DefaultNoteType == "Text") || (string.IsNullOrEmpty(Config.DefaultNoteType)) });
            listViewTypes.Items.Add(new ListViewItem() { ImageKey = "sql", Name = "Sql", Text = "Sql" });
            listViewTypes.Items.Add(new ListViewItem() { ImageKey = "vb", Name = "Visual Basic", Text = "Visual Basic", Selected = (Config.DefaultNoteType == "Visual Basic") });
            listViewTypes.Items.Add(new ListViewItem() { ImageKey = "lua", Name = "Lua", Text = "Lua", Selected = (Config.DefaultNoteType == "Lua") });
            listViewTypes.Items.Add(new ListViewItem() { ImageKey = "xml", Name = "Xml", Text = "Xml", Selected = (Config.DefaultNoteType == "Xml") });
            listViewTypes.Items.Add(new ListViewItem() { ImageKey = "php", Name = "PHP", Text = "PHP", Selected = (Config.DefaultNoteType == "PHP") });
            listViewTypes.Items.Add(new ListViewItem() { ImageKey = "html", Name = "HTML", Text = "HTML", Selected = (Config.DefaultNoteType == "HTML") });
            listViewTypes.Items.Add(new ListViewItem() { ImageKey = "txt", Name = "Markdown", Text = "Markdown", Selected = (Config.DefaultNoteType == "Markdown") });
            listViewTypes.Items.Add(new ListViewItem() { ImageKey = "rtf", Name = "Rich Text", Text = "Rich Text", Selected = (Config.DefaultNoteType == "Rich Text") });
            listViewTypes.Items.Add(new ListViewItem() { ImageKey = "template", Name = "Template", Text = "Template", Selected = (Config.DefaultNoteType == "Template") });
        }
    }
}