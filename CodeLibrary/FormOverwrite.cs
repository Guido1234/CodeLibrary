using System;
using System.Windows.Forms;

namespace CodeLibrary
{
    public enum OverwriteMode
    {
        Overwrite = 0,
        Reload = 1,
        Cancel = 2
    }

    public partial class FormOverwrite : Form
    {
        public FormOverwrite()
        {
            InitializeComponent();
        }

        public OverwriteMode DlgResult { get; set; } = OverwriteMode.Cancel;

        private void Button1_Click(object sender, EventArgs e)
        {
            DlgResult = OverwriteMode.Overwrite;
            Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            DlgResult = OverwriteMode.Reload;
            Close();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            DlgResult = OverwriteMode.Cancel;
            Close();
        }

        private void FormOverwrite_Load(object sender, EventArgs e)
        {
        }
    }
}