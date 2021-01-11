using System;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack.Forms
{
    public partial class FormRemoveLinesContaining : Form
    {
        public FormRemoveLinesContaining()
        {
            InitializeComponent();
            CancelButton = buttonCancel;
        }

        public string Containing { get; set; }

        public string Title
        {
            get { return Text; }
            set
            {
                Text = value;
                labelTitle.Text = value;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Containing = tbContaining.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void FormTrimLines_Load(object sender, EventArgs e)
        {
            tbContaining.Text = Containing;
        }
    }
}