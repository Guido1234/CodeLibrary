using System;
using System.ComponentModel;
using System.Drawing;
using System.Xml;
using System.Xml.Serialization;

namespace GK.Template
{
    public enum DataSourceType
    {
        SQLConnection = 0,
        OleDBConnection = 1,
        CsvFile = 2,
        TextFile = 3
    }

    /// <summary>
    /// Datasources kan be used by the Template object in combination with the SQLSource command
    /// </summary>
    [Serializable(), XmlRoot("DataSource")]
    public class DataSource
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DataSource()
        {
            Name = "New Datasource";
            Category = "New Datasources";
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        public DataSource(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Category name, used by the collection editor.
        /// </summary>
        [Description("Category name, used by the collection editor.")]
        [XmlElement("Category", typeof(string))]
        [Browsable(true)]
        public string Category { get; set; }

        /// <summary>
        /// SQLConnection string see: www.connectionstrings.com
        /// </summary>
        [Description("SQLConnection string see: www.connectionstrings.com, for CSVFile/TextFile just the path to the file.")]
        [XmlElement("ConnectionString", typeof(string))]
        [Browsable(false)]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Each line represents a formatter for a column.
        /// </summary>
        [XmlElement("Formatters", typeof(string))]
        [Description("Each line represents a formatter for a column. (does not apply to CSVFile/TextFile)")]
        [Browsable(false)]
        public string Formatters { get; set; }

        /// <summary>
        /// Name for the Datasource to be used in template.
        /// </summary>
        [Description("Name for the Datasource to be used in template.")]
        [XmlElement("Name", typeof(string))]
        [Browsable(true)]
        public string Name { get; set; }

        /// <summary>
        /// Query to execute
        /// </summary>
        [Description("Query to execute, does not apply to CSVFile/TextFile")]
        [XmlElement("Query", typeof(string))]
        [Browsable(false)]
        public string Query { get; set; }

        /// <summary>
        /// Type of source to use.
        /// </summary>
        [Description("Type of source to use")]
        [XmlElement("SourceType", typeof(DataSourceType))]
        [Browsable(true)]
        public DataSourceType SourceType { get; set; }

        [Browsable(false)]
        public Color TypeColor
        {
            get
            {
                switch (SourceType)
                {
                    case DataSourceType.CsvFile:
                        return Color.LightGray;

                    case DataSourceType.OleDBConnection:
                        return Color.LightBlue;

                    case DataSourceType.SQLConnection:
                        return Color.DarkBlue;

                    case DataSourceType.TextFile:
                        return Color.Gray;
                }
                return Color.White;
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}