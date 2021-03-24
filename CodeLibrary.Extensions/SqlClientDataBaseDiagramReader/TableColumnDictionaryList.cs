using DevToys;
using System.Collections.Generic;

namespace CodeLibrary.Extensions.SqlClientDataBaseDiagramReader
{
    public class TableColumnDictionaryList : DictionaryList<TableColumn, string>
    {
        private const string LOOKUP_DATATYPE = "DataType";
        private const string LOOKUP_TABLE = "TableName";
        private const string PRIMARY_KEY = "{0}_{1}";

        public TableColumnDictionaryList() : base(p => PrimaryKey(p.TableName, p.ColumnName))
        {
            RegisterLookup(LOOKUP_TABLE, p => p.TableName);
            RegisterLookup(LOOKUP_DATATYPE, p => DataTypeLookup(p.TableName, p.DataType));
        }

        public static string DataTypeLookup(string tableName, string datatype)
        {
            return string.Format(PRIMARY_KEY, tableName, datatype);
        }

        public static string PrimaryKey(string tableName, string columnname)
        {
            return string.Format(PRIMARY_KEY, tableName, columnname);
        }

        public IEnumerable<TableColumn> GetByTable(string tablename) => Lookup(LOOKUP_TABLE, tablename);

        public IEnumerable<TableColumn> GetByType(string tablename, string dataType) => Lookup(LOOKUP_DATATYPE, DataTypeLookup(tablename, dataType));
    }
}