using System;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack.Forms
{
    public partial class FormTrimLines : Form
    {
        public FormTrimLines()
        {
            InitializeComponent();
            CancelButton = buttonCancel;
        }

        public string TrimChars { get; set; }
        public bool TrimEnd { get; set; }
        public bool TrimStart { get; set; }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            TrimStart = chkTrimStart.Checked;
            TrimEnd = chkTrimEnd.Checked;
            TrimChars = tbTrimChars.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void FormTrimLines_Load(object sender, EventArgs e)
        {
            chkTrimStart.Checked = TrimStart;
            chkTrimEnd.Checked = TrimEnd;
            tbTrimChars.Text = TrimChars;
        }
    }
}