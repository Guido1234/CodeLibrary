using CodeLibrary.Core;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CodeLibrary
{
    public partial class FormBackupRestore : Form
    {
        private BackupHelper _backupHelper;
        private string _currentFile;
        private BackupInfo _Selected;

        public FormBackupRestore(string currentFile)
        {
            _currentFile = currentFile;
            _backupHelper = new BackupHelper(currentFile);
            InitializeComponent();
            Load += FormBackupRestore_Load;
        }

        public string CurrentFile
        {
            get
            {
                return _currentFile;
            }
        }

        public BackupInfo Selected
        {
            get
            {
                return _Selected;
            }
        }

        private void btBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "json Files (*.json)|*.json|All Files (*.*)|*.*",
                InitialDirectory = Config.LastOpenedDir
            };
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                _currentFile = openFileDialog.FileName;
                _backupHelper = new BackupHelper(_currentFile);
                Show();
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btRestore_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void FormBackupRestore_Load(object sender, EventArgs e)
        {
            Show();
        }

        private void LbBackups_ItemSelected(object sender, Controls.CollectionListBox.CollectionListBoxEventArgs e)
        {
            _Selected = e.Item as BackupInfo;
            btRestore.Enabled = true;
        }

        private void Show()
        {
            FileInfo file = new FileInfo(_currentFile);
            lbName.Text = file.Name;

            lbBackups.Sorting = SortOrder.None;
            lbBackups.NameProperty = "FileName";
            lbBackups.CategoryProperty = "Day";
            lbBackups.ColumnWidth = 300;
            lbBackups.SetCollection<BackupInfo>(_backupHelper.GetBackups().OrderByDescending(b => b.DateTime).ToList());
            lbBackups.ItemSelected += LbBackups_ItemSelected;
            lbBackups.Refresh();
        }
    }
}