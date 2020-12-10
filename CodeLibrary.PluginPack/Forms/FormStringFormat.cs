using System;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack.Forms
{
    public partial class FormStringFormat : Form
    {
        public FormStringFormat()
        {
            InitializeComponent();
            CancelButton = buttonCancel;
        }

        public string SplitterChar { get; set; }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            SplitterChar = tbSplitterChar.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void FormTrimLines_Load(object sender, EventArgs e)
        {
            tbSplitterChar.Text = SplitterChar;
        }
    }
}