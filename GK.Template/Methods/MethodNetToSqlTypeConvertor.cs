using GK.Template.Attributes;
using System.ComponentModel;

namespace GK.Template.Methods
{
    [Category("Casting")]
    [FormatMethod(Name = "DotNetToSqlType",
        Example = "{0:DotNetToSqlType()} {0:DotNetToSqlType} ")]
    [Description("Converts a Dot Net Value type to SQL equivalent")]
    public sealed class MethodNetToSqlTypeConvertor : MethodBase
    {
        public override string Apply(string value)
        {
            return sqlType(value);
        }

        private bool IsOneOff<T>(T value, params T[] values)
        {
            foreach (T val in values)
            {
                if (val.Equals(value))
                    return true;
            }
            return false;
        }

        private string sqlType(string type)
        {
            if (IsOneOff(type, "String", "string"))
                return "nvarchar(max)";

            if (IsOneOff(type, "char", "char?", "Char", "Char?"))
                return "nchar(1)";

            if (IsOneOff(type, "byte[]", "Byte[]"))
                return "binary";

            if (IsOneOff(type, "bool", "bool?", "Boolean", "Boolean?"))
                return "bit";

            if (IsOneOff(type, "byte", "byte?", "Byte", "Byte?"))
                return "tinyint";

            if (IsOneOff(type, "short", "short?", "Int16", "Int16?"))
                return "smallint";

            if (IsOneOff(type, "int", "int?", "Int32", "Int32?"))
                return "int";

            if (IsOneOff(type, "float", "float?", "Single", "Single?"))
                return "real";

            if (IsOneOff(type, "long", "long?", "Int64", "Int64?"))
                return "bigint";

            if (IsOneOff(type, "double", "double?", "Double", "Double?"))
                return "float";

            if (IsOneOff(type, "decimal", "decimal?", "Decimal", "Decimal?"))
                return "decimal";

            if (IsOneOff(type, "DateTime", "DateTime?"))
                return "datetime";

            if (IsOneOff(type, "TimeSpan", "TimeSpan?"))
                return "time";

            if (IsOneOff(type, "Guid", "Guid?"))
                return "uniqueidentifier";

            return string.Empty; // type not supported.
        }
    }
}