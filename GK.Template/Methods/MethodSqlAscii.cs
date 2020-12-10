using GK.Template.Attributes;
using System.ComponentModel;

namespace GK.Template.Methods
{
    [FormatMethod(Name = "SqlAscii",
        Aliasses = "Sql",
        Example = "{0:SqlAscii(true, \"null\")} {0:Sql(true, \"null\")}")]
    [Description("Converts a value to Sql Ascii")]
    public sealed class MethodSqlAscii : MethodBase
    {
        [FormatMethodParameter(Optional = false, Order = 1)]
        [Description("Default value to use for empty items")]
        public string Default { get; set; }

        [FormatMethodParameter(Optional = false, Order = 0)]
        [Description("Indicates whether the result should be surrounded with quotes")]
        public bool Quoted { get; set; }

        public override string Apply(string value)
        {
            if (string.IsNullOrEmpty(value))
                return Default;

            if (Quoted)
                return string.Format("'{0}'", SqlDataUtility.Format(value));

            return SqlDataUtility.Format(value);
        }
    }
}