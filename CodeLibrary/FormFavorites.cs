using CodeLibrary.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CodeLibrary
{
    public partial class FormFavorites : Form
    {
        public FormFavorites()
        {
            InitializeComponent();
            lsbItems.CategoryProperty = "Category";
            lsbItems.OnAdd += LsbItems_OnAdd;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void dlgButton_DialogButtonClick(object sender, Controls.DialogButton.DialogButtonClickEventArgs e)
        {
            DialogResult = e.Result;

            if (e.Result == DialogResult.OK)
            {
                Config.FavoriteLibs.Clear();
                foreach (string file in lsbItems.GetCollection<Fav>().Select(p => p.FileName))
                {
                    Config.FavoriteLibs.Add(file);
                }
            }
            Close();
        }

        private void FormFavorites_Load(object sender, EventArgs e)
        {
            List<Fav> _items = Config.FavoriteLibs.Select(p => new Fav() { FileName = p }).ToList();
            lsbItems.SetCollection(_items);
            lsbItems.Refresh();
        }

        private void LsbItems_OnAdd(object sender, Controls.CollectionListBox.CollectionListBoxEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "json Files (*.json)|*.json|All Files (*.*)|*.*",
                InitialDirectory = Config.LastOpenedDir
            };
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string filename = openFileDialog.FileName;
                e.Item = new Fav() { FileName = filename };
            }
            else
            {
                e.Canceled = true;
            }
        }

        private class Fav
        {
            public string Category
            {
                get
                {
                    if (File.Exists(FileName))
                    {
                        return "Favorites";
                    }
                    return "Deleted";
                }
            }

            public string FileName { get; set; }



            public override string ToString()
            {
                FileInfo file = new FileInfo(FileName);
                return file.Name;
            }
        }
    }
}