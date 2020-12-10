using GK.Template.Attributes;
using GK.Template.Methods;
using System;
using System.Collections.Generic;

namespace GK.Template
{
    /// <summary>
    /// Used to create an instance of a Method
    /// </summary>
    public sealed class MethodInstance
    {
        public MethodInstance()
        {
            Properties = new List<FormatMethodParameterAttribute>();
        }

        public MethodBase Formatter { get; set; }

        public string Name { get; set; }

        public List<FormatMethodParameterAttribute> Properties { get; set; }

        public static MethodInstance GetMethodInstance(string name)
        {
            List<Type> methodtTypes = Utils.GetObjectsWithBaseType(typeof(MethodBase), true);
            string[] aliasses = new string[0];
            foreach (Type t in methodtTypes)
            {
                FormatMethodAttribute att = FormatMethodAttribute.GetAttribute(t);
                if (att != null)
                {
                    if (att.Name.Equals(name, StringComparison.Ordinal))
                    {
                        MethodInstance instance = new MethodInstance() { Formatter = (MethodBase)Activator.CreateInstance(t), Name = att.Name };
                        foreach (FormatMethodParameterAttribute at in att.Parameters)
                            instance.Properties.Add(at);

                        return instance;
                    }
                    else
                    {
                        aliasses = Utils.Split(att.Aliasses, ",");
                        foreach (string alias in aliasses)
                        {
                            if (alias.Trim().Equals(name, StringComparison.Ordinal))
                            {
                                MethodInstance instance = new MethodInstance() { Formatter = (MethodBase)Activator.CreateInstance(t), Name = att.Name };
                                foreach (FormatMethodParameterAttribute at in att.Parameters)
                                    instance.Properties.Add(at);

                                return instance;
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}