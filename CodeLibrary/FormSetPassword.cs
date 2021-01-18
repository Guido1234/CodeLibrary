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
            AcceptButton = dialogButton.buttonOk;
            Load += FormSetPassword_Load;
        }

        private void FormSetPassword_Load(object sender, System.EventArgs e)
        {
            textBoxPassword.SelectAll();
            textBoxPassword.Focus();
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