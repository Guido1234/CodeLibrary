using CodeLibrary.Controls;
using System.Windows.Forms;

namespace CodeLibrary
{
    public partial class FormInput : Form
    {
        public FormInput()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        public string InputText
        {
            get
            {
                return textBox1.Text;
            }
            set
            {
                textBox1.Text = value;
            }
        }

        public string Message
        {
            get
            {
                return labelMessage.Text;
            }
            set
            {
                labelMessage.Text = value;
            }
        }

        private void DialogButton_DialogButtonClick(object sender, DialogButton.DialogButtonClickEventArgs e)
        {
            DialogResult = e.Result;
            Close();
        }
    }
}