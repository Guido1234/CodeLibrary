using CodeLibrary.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeLibrary
{
    public partial class FormBackupRestoreSettings : Form
    {
        private EnumComboBoxModeHelper<EBackupMode> _ComboBoxHelper;

        public FormBackupRestoreSettings()
        {
            InitializeComponent();         
            _ComboBoxHelper = new EnumComboBoxModeHelper<EBackupMode>(comboBoxMode, EBackupMode.FileLocation);
            _ComboBoxHelper.Fill();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            _ComboBoxHelper.SetSelectedIndex(Config.BackupMode);
            textBoxLocation.Text = Config.BackupLocation;

        }

        private void dialogButton_DialogButtonClick(object sender, Controls.DialogButton.DialogButtonClickEventArgs e)
        {
            DialogResult = e.Result;
            if (e.Result == DialogResult.OK)
            {
                Config.BackupMode = (EBackupMode)_ComboBoxHelper.GetValue();
                Config.BackupLocation = textBoxLocation.Text;

            }
            Close();
        }

        private void btBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            var _result = folderBrowserDialog.ShowDialog();
            if (_result == DialogResult.OK)
            {
                textBoxLocation.Text = folderBrowserDialog.SelectedPath;
            }
        }
    }
}
