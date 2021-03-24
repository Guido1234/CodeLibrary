using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CodeLibrary.Extensions.SqlClientDataBaseDiagramReader
{
    public partial class FormConfigureDiagramReader : Form
    {
        private SqlClientDataBaseDiagramReaderExtension _Extension;

        public FormConfigureDiagramReader(SqlClientDataBaseDiagramReaderExtension _extension)
        {
            InitializeComponent();
            _Extension = _extension;
            tbConnectionString.Text = _Extension.ConnectionString;
            tbFilterColumnNames.Text = _Extension.FilterColumnNames;
            tbFilterTableNames.Text = _Extension.FilterTableNames;
            tbFilterTypeNames.Text = _Extension.FilterTypeNames;
        }

        private void btTestConnection_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection _connection = new SqlConnection(tbConnectionString.Text))
                {
                    _connection.Open();
                }
                MessageBox.Show("Succes", "Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Error", "Test", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dialogButton1_DialogButtonClick(object sender, Controls.DialogButton.DialogButtonClickEventArgs e)
        {
            this.DialogResult = e.Result;
            if (e.Result == DialogResult.Cancel)
            {
                Close();
                return;
            }
            _Extension.ConnectionString = tbConnectionString.Text;

            _Extension.FilterColumnNames = tbFilterColumnNames.Text;
            _Extension.FilterTableNames = tbFilterTableNames.Text;
            _Extension.FilterTypeNames = tbFilterTypeNames.Text;
            Close();
        }
    }
}