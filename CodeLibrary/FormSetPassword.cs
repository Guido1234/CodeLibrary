using CodeLibrary.Controls;
using CodeLibrary.Core;
using System.Security;
using System.Windows.Forms;

namespace CodeLibrary
{ 
    public partial class FormSetPassword : Form
    {
        public FormSetPassword()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
            textBoxPassword.PasswordChar = '*';
        } 

        public SecureString Password
        {
            get
            {
                if (string.IsNullOrEmpty(textBoxPassword.Text))
                    return null;

                return StringCipher.ToSecureString(textBoxPassword.Text);
            }
        }

        private void dialogButton_DialogButtonClick(object sender, DialogButton.DialogButtonClickEventArgs e)
        {
            DialogResult = e.Result;
            Close();
        }
    }
}