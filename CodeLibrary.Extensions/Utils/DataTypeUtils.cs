using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLibrary.Extensions.Utils
{
    public static class DataTypeUtils
    {
        /// <summary>
        /// Converts a named sql typename to a compatible .Net Type.
        /// </summary>
        public static Type GetNetTypeName(string sqlTypename)
        {
            if (string.IsNullOrEmpty(sqlTypename))
                return null;

            switch (sqlTypename.ToLower())
            {
                case "ntext":
                case "nvarchar":
                case "varchar":
                case "sysname":
                case "text":
                    return typeof(String);

                case "char":
                case "nchar":
                    return typeof(Char);

                case "sql_variant":
                case "binary":
                case "image":
                case "varbinary":
                    return typeof(Byte[]);

                case "datetime":
                case "smalldatetime":
                case "timestamp":
                    return typeof(DateTime);

                case "tinyint":
                    return typeof(Int16);

                case "int":
                case "numeric":
                case "smallint":
                    return typeof(Int32);

                case "bigint":
                    return typeof(Int64);

                case "float":
                case "real":
                    return typeof(Single);

                case "decimal":
                case "money":
                case "smallmoney":
                    return typeof(Decimal);

                case "bit":
                    return typeof(Boolean);

                case "uniqueidentifier":
                    return typeof(Guid);
            }
            return null;
        }

    }
}
