using System;
using System.Collections.Generic;

namespace GK.Template
{
    /// <summary>
    /// Datasources kan be used by the Template object in combination with the SQLSource command
    /// </summary>
    public class DataSourceCollection
    {
        public DataSourceCollection()
        {
            Items = new List<DataSource>();
        }

        public List<DataSource> Items { get; set; }

        public DataSource GetDataSource(string name)
        {
            foreach (DataSource ds in Items)
            {
                if (ds.Name.Equals(name, StringComparison.Ordinal))
                    return ds;
            }
            return null;
        }
    }
}