using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace GK.Template.Attributes
{
    public class DataCommandAttribute : Attribute
    {
        public DataCommandAttribute()
        {
            Aliasses = string.Empty;
            Category = string.Empty;
            Description = string.Empty;
            Example = string.Empty;
            Name = string.Empty;
        }

        /// <summary>
        /// comma separated list of alias names.
        /// </summary>
        public string Aliasses { get; set; }

        public string Category { get; set; }
        public string Description { get; set; }
        public string Example { get; set; }
        public string Name { get; set; }
        public List<DataCommandParameterAttribute> Parameters { get; set; }

        public static DataCommandAttribute GetAttribute(Type type)
        {
            foreach (Object oAttrib in type.GetCustomAttributes(true))
            {
                if (oAttrib.GetType() == typeof(DataCommandAttribute))
                {
                    DataCommandAttribute fma = (DataCommandAttribute)oAttrib;
                    fma.Parameters = new List<DataCommandParameterAttribute>();
                    foreach (DataCommandParameterAttribute at in DataCommandParameterAttribute.GetAttributes(type))
                        fma.Parameters.Add(at);

                    if (string.IsNullOrEmpty(fma.Description))
                    {
                        DescriptionAttribute _attribute = type.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault() as DescriptionAttribute;
                        if (_attribute != null)
                            fma.Description = _attribute.Description;
                    }

                    if (string.IsNullOrEmpty(fma.Category))
                    {
                        CategoryAttribute _attribute = type.GetCustomAttributes(typeof(CategoryAttribute), true).FirstOrDefault() as CategoryAttribute;
                        if (_attribute != null)
                            fma.Category = _attribute.Category;
                    }

                    return fma;
                }
            }
            return null;
        }

        public string HelpBuilderString()
        {
            return Utils.JoinCsvLine(';', this.Name, this.Aliasses, this.ToString(), this.Description);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataCommandParameterAttribute prop in Parameters)
            {
                sb.Append("[");
                sb.Append(prop.PropertyReference.Name);
                sb.Append("]");
                sb.Append(",");
            }
            string prs = sb.ToString();
            if (prs.Length > 0)
            {
                prs = Utils.TrimEndByLength(prs, 1);
            }
            return string.Format("{0}({1})", Name, prs);
        }
    }
}