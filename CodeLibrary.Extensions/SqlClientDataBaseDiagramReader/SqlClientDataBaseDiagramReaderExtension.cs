using EditorPlugins.Core;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CodeLibrary.Extensions.SqlClientDataBaseDiagramReader
{
    [Description("SqlClient Diagram Reader")]
    public class SqlClientDataBaseDiagramReaderExtension : IEditorPlugin
    {
        public string Category => null;
        public string ConnectionString { get; set; }
        public string DisplayName => "SqlClient Diagram Reader";
        public string FilterColumnNames { get; set; } = "*";
        public string FilterTableNames { get; set; } = "*";
        public string FilterTypeNames { get; set; } = "*";

        public bool IncludeHeader { get; set; } = true;

        public bool IncludeForeignKeys { get; set; } = true;

        public Guid Id => Guid.Parse("f9414972-cedd-4da6-9d7b-fed5a6847790");
        public Image Image => null;
        public bool IsExtension => true;
        public bool OmitResult => false;
        public Keys ShortcutKeys => Keys.None;

        public void Apply(ISelInfo sel)
        {
            try
            {
                sel.SelectedText = Read();
            }
            catch
            {
                MessageBox.Show("Error", "Apply", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public bool Configure()
        {
            FormConfigureDiagramReader _f = new FormConfigureDiagramReader(this);
            DialogResult _r = _f.ShowDialog();
            if (_r == DialogResult.OK)
            {
                return true;
            }
            return false;
        }

        public string Read()
        {
            TableColumnDictionaryList _columns = new TableColumnDictionaryList();

            using (SqlConnection _connection = new SqlConnection(ConnectionString))
            {
                _connection.Open();

                DataTable schemas = _connection.GetSchema();


                DataTable schema_columns = _connection.GetSchema("Columns");

                foreach (DataRow row in schema_columns.Rows)
                {
                    _columns.Add(new TableColumn(row) );
                }

                StringBuilder _sb = new StringBuilder();

                if (IncludeHeader)
                {
                    _sb.Append(TableColumn.Header());
                    _sb.Append("\r\n");
                }

                foreach (TableColumn column in _columns)
                {
                    if (MatchPattern(column.ColumnName, FilterColumnNames) &&
                        MatchPattern(column.TableName, FilterTableNames) &&
                        MatchPattern(column.DataType, FilterTypeNames)
                        )
                    {
                        _sb.Append(column.ToString());
                        _sb.Append("\r\n");
                    }
                }

                return _sb.ToString();
            }
        }

        private bool MatchPattern(string s, string pattern)
        {
            if (pattern == null || s == null) return false;

            string[] patterns = pattern.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string subpattern in patterns)
            {
                string pat = string.Format("^{0}$", Regex.Escape(subpattern).Replace("\\*", ".*").Replace("\\?", "."));
                Regex regex = new Regex(pat, RegexOptions.IgnoreCase);
                if (regex.IsMatch(s))
                    return true;
            }
            return false;
        }
    }
}