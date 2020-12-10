using System;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack.Forms
{
    public partial class FormMultiReplace : Form
    {
        public FormMultiReplace()
        {
            InitializeComponent();
            CancelButton = buttonCancel;
        }

        public string Find { get; set; }
        public string Replace { get; set; }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Find = txtFind.Text;
            Replace = txtReplace.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void FormMultiReplace_Load(object sender, EventArgs e)
        {
            this.txtFind.Text = Find;
            this.txtReplace.Text = Replace;
        }
    }
}