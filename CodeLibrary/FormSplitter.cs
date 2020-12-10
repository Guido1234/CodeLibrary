using CodeLibrary.Controls;
using System.Windows.Forms;

namespace CodeLibrary
{
    public partial class FormSplitter : Form
    {
        public FormSplitter()
        {
            InitializeComponent();
        }

        public string Splitter
        {
            get
            {
                return textBoxSplitter.Text;
            }
            set
            {
                textBoxSplitter.Text = value;
            }
        }

        private void dialogButton_DialogButtonClick(object sender, DialogButton.DialogButtonClickEventArgs e)
        {
            this.DialogResult = e.Result;
            this.Close();
        }
    }
}