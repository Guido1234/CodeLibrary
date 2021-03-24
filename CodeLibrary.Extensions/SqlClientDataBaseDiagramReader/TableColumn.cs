namespace CodeLibrary.Extensions.SqlClientDataBaseDiagramReader
{
    public class TableColumn
    {
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public string TableName { get; set; }

        public override string ToString()
        {
            return $"{TableName};{ColumnName};{DataType}";
        }
    }
}