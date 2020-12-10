using System;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack.Forms
{
    public partial class FormDateFormat : Form
    {
        public FormDateFormat()
        {
            InitializeComponent();
            CancelButton = buttonCancel;
        }

        public string DateFormat { get; set; }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            DateFormat = txtFormat.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void FormDateFormat_Load(object sender, EventArgs e)
        {
            txtFormat.Text = DateFormat;
        }
    }
}