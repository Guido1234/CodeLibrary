using System;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack.Forms
{
    public partial class FormSurroundLines : Form
    {
        public FormSurroundLines()
        {
            InitializeComponent();
            CancelButton = buttonCancel;
        }

        public string Postfix { get; set; }
        public string Prefix { get; set; }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Prefix = txtPrefix.Text;
            Postfix = txtPostFix.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void FormSurroundLines_Load(object sender, EventArgs e)
        {
            txtPrefix.Text = Prefix;
            txtPostFix.Text = Postfix;
        }
    }
}