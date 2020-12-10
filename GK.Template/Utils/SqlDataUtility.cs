using System;
using System.Data;

namespace GK.Template
{
    /// <summary>
    /// Set of Sql Data Utility methods
    /// </summary>
    public static class SqlDataUtility
    {
        /// <summary>
        /// Returns the first dataRow of The First Table in a dataset.
        /// </summary>
        /// <param name="dataset">Dataset</param>
        /// <returns></returns>
        public static DataRow FirstDataRow(DataSet dataset)
        {
            if (dataset == null)
                return null;

            if (dataset.Tables.Count == 0)
                return null;

            if (dataset.Tables[0].Rows.Count == 0)
                return null;

            return dataset.Tables[0].Rows[0];
        }

        /// <summary>
        /// Format sql datetime
        /// </summary>
        /// <param name="value">datetime value</param>
        /// <returns></returns>
        public static string Format(DateTime value)
        {
            return value.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        /// <summary>
        /// Convert bool to 1 or 0
        /// </summary>
        /// <param name="value">value to format</param>
        /// <returns></returns>
        public static string Format(bool value)
        {
            return BooleanUtility.Format(value, "1|0");
        }

        /// <summary>
        /// Ensures proper sql string to use in sql statement, normalizes all single and double quotes, then duplicates single quotes.
        /// </summary>
        /// <param name="value">value to format to proper sql string</param>
        /// <returns></returns>
        public static string Format(string value)
        {
            string s = Utils.NormalizeAllQuotes(value); // do not accept strange quotes.
            return s.Replace("'", "''");
        }

        /// <summary>
        /// SqlType converteren naar Net TypeName.
        /// </summary>
        /// <param name="sqlTypename">Sql Typename</param>
        /// <returns></returns>
        public static string GetNetTypeName(string sqlTypename)
        {
            if (string.IsNullOrEmpty(sqlTypename))
                return string.Empty;

            switch (sqlTypename.ToLower())
            {
                case "bigint":
                    return "Int64";

                case "binary":
                    return "byte[]";

                case "bit":
                    return "bool";

                case "char":
                    return "char";

                case "datetime":
                    return "DateTime";

                case "decimal":
                    return "decimal";

                case "float":
                    return "float";

                case "image":
                    return "byte[]";

                case "int":
                    return "int";

                case "money":
                    return "decimal";

                case "nchar":
                    return "char";

                case "ntext":
                    return "string";

                case "numeric":
                    return "int";

                case "nvarchar":
                    return "string";

                case "real":
                    return "float";

                case "smalldatetime":
                    return "DateTime";

                case "smallint":
                    return "int";

                case "smallmoney":
                    return "decimal";

                case "sql_variant":
                    return "byte[]";

                case "sysname":
                    return "string";

                case "text":
                    return "string";

                case "timestamp":
                    return "DateTime";

                case "tinyint":
                    return "Int16";

                case "uniqueidentifier":
                    return "Guid";

                case "varbinary":
                    return "byte[]";

                case "varchar":
                    return "string";
            }
            return string.Empty;
        }

        /// <summary>
        /// Determines whether first table in dataset has rows.
        /// </summary>
        /// <param name="dataset"></param>
        /// <returns></returns>
        public static bool TableHasRows(DataSet dataset)
        {
            if (dataset == null)
                return false;

            return dataset.Tables[0].Rows.Count > 0;
        }

        /// <summary>
        /// Determines whether first table in dataset has rows.
        /// </summary>
        /// <param name="datatable">DataTable to check</param>
        /// <returns></returns>
        public static bool TableHasRows(DataTable datatable)
        {
            if (datatable == null)
                return false;

            return datatable.Rows.Count > 0;
        }

        /// <summary>
        /// Convert datarow to Byte array
        /// </summary>
        /// <param name="datarow">datarow to use</param>
        /// <param name="column">datarow column name</param>
        /// <param name="defaultvalue">default value on failure or null</param>
        /// <returns></returns>
        public static byte[] ToBinary(DataRow datarow, string column, byte[] defaultvalue)
        {
            if (datarow == null)
                return defaultvalue;

            byte[] value = new byte[0];
            try
            {
                value = (byte[])datarow[column];
                return value;
            }
            catch
            {
                return defaultvalue;
            }
        }

        /// <summary>
        /// Convert datarow column to Boolean (bool)
        /// </summary>
        /// <param name="datarow">datarow to use</param>
        /// <param name="column">datarow column name</param>
        /// <param name="defaultvalue">default value on failure or null</param>
        /// <returns></returns>
        public static Boolean ToBoolean(DataRow datarow, string column, Boolean defaultvalue)
        {
            if (datarow == null)
                return defaultvalue;

            return ConvertUtility.ToBoolean(datarow[column], defaultvalue);
        }

        /// <summary>
        /// Convert datarow column to Boolean (bool)
        /// </summary>
        /// <param name="datarow">datarow to use</param>
        /// <param name="column">datarow column name</param>
        /// <returns></returns>
        public static Boolean? ToBooleanNullable(DataRow datarow, string column)
        {
            if (datarow == null)
                return null;

            return ConvertUtility.ToBooleanNullable(datarow[column]);
        }

        /// <summary>
        /// Convert datarow column to Byte (byte)
        /// </summary>
        /// <param name="datarow">datarow to use</param>
        /// <param name="column">datarow column name</param>
        /// <param name="defaultvalue">default value on failure or null</param>
        /// <returns></returns>
        public static Byte ToByte(DataRow datarow, string column, Byte defaultvalue)
        {
            if (datarow == null)
                return defaultvalue;

            return ConvertUtility.ToByte(datarow[column], defaultvalue);
        }

        /// <summary>
        /// Convert datarow column to Byte (byte)
        /// </summary>
        /// <param name="datarow">datarow to use</param>
        /// <param name="column">datarow column name</param>
        /// <returns></returns>
        public static Byte? ToByteNullable(DataRow datarow, string column)
        {
            if (datarow == null)
                return null;

            return ConvertUtility.ToByteNullable(datarow[column]);
        }

        /// <summary>
        /// Convert datarow column to DateTime (DateTime)
        /// </summary>
        /// <param name="datarow">datarow to use</param>
        /// <param name="column">datarow column name</param>
        /// <param name="defaultvalue">default value on failure or null</param>
        /// <returns></returns>
        public static DateTime ToDateTime(DataRow datarow, string column, DateTime defaultvalue)
        {
            if (datarow == null)
                return defaultvalue;

            return ConvertUtility.ToDateTime(datarow[column], defaultvalue);
        }

        /// <summary>
        /// Convert datarow column to DateTime (DateTime)
        /// </summary>
        /// <param name="datarow">datarow to use</param>
        /// <param name="column">datarow column name</param>
        /// <returns></returns>
        public static DateTime? ToDateTimeNullable(DataRow datarow, string column)
        {
            if (datarow == null)
                return null;

            return ConvertUtility.ToDateTimeNullable(datarow[column]);
        }

        /// <summary>
        /// Convert datarow column to Decimal (decimal)
        /// </summary>
        /// <param name="datarow">datarow to use</param>
        /// <param name="column">datarow column name</param>
        /// <param name="defaultvalue">default value on failure or null</param>
        /// <returns></returns>
        public static Decimal ToDecimal(DataRow datarow, string column, Decimal defaultvalue)
        {
            if (datarow == null)
                return defaultvalue;

            return ConvertUtility.ToDecimal(datarow[column], defaultvalue);
        }

        /// <summary>
        /// Convert datarow column to Decimal (decimal)
        /// </summary>
        /// <param name="datarow">datarow to use</param>
        /// <param name="column">datarow column name</param>
        /// <returns></returns>
        public static Decimal? ToDecimalNullable(DataRow datarow, string column)
        {
            if (datarow == null)
                return null;

            return ConvertUtility.ToDecimalNullable(datarow[column]);
        }

        /// <summary>
        /// Convert datarow column to Double (double)
        /// </summary>
        /// <param name="datarow">datarow to use</param>
        /// <param name="column">datarow column name</param>
        /// <param name="defaultvalue">default value on failure or null</param>
        /// <returns></returns>
        public static Double ToDouble(DataRow datarow, string column, Double defaultvalue)
        {
            if (datarow == null)
                return defaultvalue;

            return ConvertUtility.ToDouble(datarow[column], defaultvalue);
        }

        /// <summary>
        /// Convert datarow column to Double (double)
        /// </summary>
        /// <param name="datarow">datarow to use</param>
        /// <param name="column">datarow column name</param>
        /// <returns></returns>
        public static Double? ToDoubleNullable(DataRow datarow, string column)
        {
            if (datarow == null)
                return null;

            return ConvertUtility.ToDoubleNullable(datarow[column]);
        }

        /// <summary>
        /// Convert datarow column to Guid (Guid)
        /// </summary>
        /// <param name="datarow">datarow to use</param>
        /// <param name="column">datarow column name</param>
        /// <param name="defaultvalue">default value on failure or null</param>
        /// <returns></returns>
        public static Guid ToGuid(DataRow datarow, string column, Guid defaultvalue)
        {
            if (datarow == null)
                return defaultvalue;

            return ConvertUtility.ToGuid(datarow[column], defaultvalue);
        }

        /// <summary>
        /// Convert datarow column to Guid (Guid)
        /// </summary>
        /// <param name="datarow">datarow to use</param>
        /// <param name="column">datarow column name</param>
        /// <returns></returns>
        public static Guid? ToGuidNullable(DataRow datarow, string column)
        {
            if (datarow == null)
                return null;

            return ConvertUtility.ToGuidNullable(datarow[column]);
        }

        /// <summary>
        /// Convert datarow column to Int16 (short)
        /// </summary>
        /// <param name="datarow">datarow to use</param>
        /// <param name="column">datarow column name</param>
        /// <param name="defaultvalue">default value on failure or null</param>
        /// <returns></returns>
        public static Int16 ToInt16(DataRow datarow, string column, Int16 defaultvalue)
        {
            if (datarow == null)
                return defaultvalue;

            return ConvertUtility.ToInt16(datarow[column], defaultvalue);
        }

        /// <summary>
        /// Convert datarow column to Int16 (short)
        /// </summary>
        /// <param name="datarow">datarow to use</param>
        /// <param name="column">datarow column name</param>
        /// <returns></returns>
        public static Int16? ToInt16Nullable(DataRow datarow, string column)
        {
            if (datarow == null)
                return null;

            return ConvertUtility.ToInt16Nullable(datarow[column]);
        }

        /// <summary>
        /// Convert datarow column to Int32 (int)
        /// </summary>
        /// <param name="datarow">datarow to use</param>
        /// <param name="column">datarow column name</param>
        /// <param name="defaultvalue">default value on failure or null</param>
        /// <returns></returns>
        public static Int32 ToInt32(DataRow datarow, string column, Int32 defaultvalue)
        {
            if (datarow == null)
                return defaultvalue;

            return ConvertUtility.ToInt32(datarow[column], defaultvalue);
        }

        /// <summary>
        /// Convert datarow column to Int32 (int)
        /// </summary>
        /// <param name="datarow">datarow to use</param>
        /// <param name="column">datarow column name</param>
        /// <returns></returns>
        public static Int32? ToInt32Nullable(DataRow datarow, string column)
        {
            if (datarow == null)
                return null;

            return ConvertUtility.ToInt32Nullable(datarow[column]);
        }

        /// <summary>
        /// Convert datarow column to Int64 (long)
        /// </summary>
        /// <param name="datarow">datarow to use</param>
        /// <param name="column">datarow column name</param>
        /// <param name="defaultvalue">default value on failure or null</param>
        /// <returns></returns>
        public static Int64 ToInt64(DataRow datarow, string column, Int64 defaultvalue)
        {
            if (datarow == null)
                return defaultvalue;

            return ConvertUtility.ToInt64(datarow[column], defaultvalue);
        }

        /// <summary>
        /// Convert datarow column to Int64 (long)
        /// </summary>
        /// <param name="datarow">datarow to use</param>
        /// <param name="column">datarow column name</param>
        /// <returns></returns>
        public static Int64? ToInt64Nullable(DataRow datarow, string column)
        {
            if (datarow == null)
                return null;

            return ConvertUtility.ToInt64Nullable(datarow[column]);
        }

        /// <summary>
        /// Convert datarow column to Single (float)
        /// </summary>
        /// <param name="datarow">datarow to use</param>
        /// <param name="column">datarow column name</param>
        /// <param name="defaultvalue">default value on failure or null</param>
        /// <returns></returns>
        public static Single ToSingle(DataRow datarow, string column, Single defaultvalue)
        {
            if (datarow == null)
                return defaultvalue;

            return ConvertUtility.ToSingle(datarow[column], defaultvalue);
        }

        /// <summary>
        /// Convert datarow column to Single (float)
        /// </summary>
        /// <param name="datarow">datarow to use</param>
        /// <param name="column">datarow column name</param>
        /// <returns></returns>
        public static Single? ToSingleNullable(DataRow datarow, string column)
        {
            if (datarow == null)
                return null;

            return ConvertUtility.ToSingleNullable(datarow[column]);
        }

        /// <summary>
        /// Convert datarow to String
        /// </summary>
        /// <param name="datarow">datarow to use</param>
        /// <param name="column">datarow column name</param>
        /// <param name="defaultvalue">default value on failure or null</param>
        /// <returns></returns>
        public static string ToString(DataRow datarow, string column, string defaultvalue)
        {
            if (datarow == null)
                return defaultvalue;

            return datarow[column].ToString();
        }
    }
}