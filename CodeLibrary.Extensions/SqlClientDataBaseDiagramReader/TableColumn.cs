using CodeLibrary.Core;
using CodeLibrary.Extensions.Utils;
using System;
using System.Data;

namespace CodeLibrary.Extensions.SqlClientDataBaseDiagramReader
{
    public class TableColumn
    {

        public TableColumn(DataRow row)
        {
            TableCatalog = row["TABLE_CATALOG"].ToString();
            TableSchema = row["TABLE_SCHEMA"].ToString();
            TableName = row["TABLE_NAME"].ToString();
            ColumnName = row["COLUMN_NAME"].ToString();
            ColumnDefault = row["COLUMN_DEFAULT"].ToString();
            IsNullable = row["IS_NULLABLE"].ToString();
            DataType = row["DATA_TYPE"].ToString();
            CharacterSetCatalog = row["CHARACTER_SET_CATALOG"].ToString();
            CharacterSetSchema = row["CHARACTER_SET_SCHEMA"].ToString();
            CharacterSetName = row["CHARACTER_SET_NAME"].ToString();
            CollationCatalog = row["COLLATION_CATALOG"].ToString();
            OrdinalPosition = ConvertUtility.ToInt32Nullable(row["ORDINAL_POSITION"]);
            CharacterMaximumLength = ConvertUtility.ToInt32Nullable(row["CHARACTER_MAXIMUM_LENGTH"]);
            CharacterOctetLength = ConvertUtility.ToInt32Nullable(row["CHARACTER_OCTET_LENGTH"]);
            NumericScale = ConvertUtility.ToInt32Nullable(row["NUMERIC_SCALE"]);
            NumericPrecisionRadix = ConvertUtility.ToInt16Nullable(row["NUMERIC_PRECISION_RADIX"]);
            DatetimePrecision = ConvertUtility.ToInt16Nullable(row["DATETIME_PRECISION"]);
            NumericPrecision = ConvertUtility.ToByteNullable(row["NUMERIC_PRECISION"]);
            IsSparse = ConvertUtility.ToBooleanNullable(row["IS_SPARSE"]);
            IsColumnSet = ConvertUtility.ToBooleanNullable(row["IS_COLUMN_SET"]);
            IsFilestream = ConvertUtility.ToBooleanNullable(row["IS_FILESTREAM"]);

            NetTypeName = DataTypeUtils.GetNetTypeName(DataType).Name;
        }

        public string NetTypeName { get; set; }
        public string TableCatalog { get; set; }
        public string TableSchema { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string ColumnDefault { get; set; }
        public string IsNullable { get; set; }
        public string DataType { get; set; }
        public string CharacterSetCatalog { get; set; }
        public string CharacterSetSchema { get; set; }
        public string CharacterSetName { get; set; }
        public string CollationCatalog { get; set; }
        public Int32? OrdinalPosition { get; set; }
        public Int32? CharacterMaximumLength { get; set; }
        public Int32? CharacterOctetLength { get; set; }
        public Int32? NumericScale { get; set; }
        public Int16? NumericPrecisionRadix { get; set; }
        public Int16? DatetimePrecision { get; set; }
        public Byte? NumericPrecision { get; set; }
        public bool? IsSparse { get; set; }
        public bool? IsColumnSet { get; set; }
        public bool? IsFilestream { get; set; }

        public override string ToString()
        {
            return $"{TableCatalog};{TableSchema};{TableName};{ColumnName};{ColumnDefault};{IsNullable};{DataType};{CharacterSetCatalog};{CharacterSetSchema};{CharacterSetName};{CollationCatalog};{OrdinalPosition};{CharacterMaximumLength};{CharacterOctetLength};{NumericScale};{NumericPrecisionRadix};{DatetimePrecision};{NumericPrecision};{IsSparse};{IsColumnSet};{IsFilestream};{NetTypeName}";
        }
        public static string Header()
        {
            return $"TableCatalog;TableSchema;TableName;ColumnName;ColumnDefault;IsNullable;DataType;CharacterSetCatalog;CharacterSetSchema;CharacterSetName;CollationCatalog;OrdinalPosition;CharacterMaximumLength;CharacterOctetLength;NumericScale;NumericPrecisionRadix;DatetimePrecision;NumericPrecision;IsSparse;IsColumnSet;IsFilestreaNetTypeName";
        }
    }
}