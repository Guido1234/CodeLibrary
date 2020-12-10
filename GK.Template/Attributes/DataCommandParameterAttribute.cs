using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace GK.Template.Attributes
{
    public sealed class DataCommandParameterAttribute : Attribute
    {
        private string _description = string.Empty;
        private bool _isParams = false;
        private string _name = string.Empty;
        public string Category { get; set; }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        public bool IsParams
        {
            get
            {
                return _isParams;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public int Order { get; set; }
        internal PropertyInfo PropertyReference { get; set; }

        public static List<DataCommandParameterAttribute> GetAttributes(Type type)
        {
            List<DataCommandParameterAttribute> result = new List<DataCommandParameterAttribute>();

            foreach (PropertyInfo p in type.GetProperties())
            {
                foreach (Object oAttrib in p.GetCustomAttributes(true))
                {
                    if (oAttrib.GetType() == typeof(DataCommandParameterAttribute))
                    {
                        DataCommandParameterAttribute att = (DataCommandParameterAttribute)oAttrib;
                        att.PropertyReference = p;

                        DescriptionAttribute _descriptionAttribute = type.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault() as DescriptionAttribute;
                        if (_descriptionAttribute != null)
                            att.Description = _descriptionAttribute.Description;

                        CategoryAttribute _categoryAttribute = type.GetCustomAttributes(typeof(CategoryAttribute), true).FirstOrDefault() as CategoryAttribute;
                        if (_categoryAttribute != null)
                            att.Category = _categoryAttribute.Category;

                        att._name = p.Name;
                        att._isParams = PropertyArrayTypeSupported(p.GetType());

                        result.Add(att);
                    }
                }
            }

            List<DataCommandParameterAttribute> sorted =
                     (from n in result
                      orderby n.Order ascending
                      select n).ToList();
            return sorted;
        }

        internal static bool PropertyArrayTypeSupported(Type t)
        {
            return (Utils.IsOneOfTypes(t, typeof(string[])));
        }
    }
}