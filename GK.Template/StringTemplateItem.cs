//using GK.Library.Utilities;
using GK.Template.Methods;
using System;
using System.Collections.Generic;
using System.Text;

namespace GK.Template
{
    public sealed class StringTemplateItem
    {
        public List<PlaceHolder> PlaceHolders = new List<PlaceHolder>();
         
        public StringTemplateItem()
        {
        }

        public StringTemplateItem(string[] templates, string template)
        {
            string[] templateItems = Utils.SplitTemplate(template);
            bool isTemplate = false;
            for (int ii = 0; ii < templateItems.Length; ii++)
            {
                isTemplate = (Utils.StartsWithEndsWith(templateItems[ii], "{", "}") && !Utils.StartsWithEndsWith(templateItems[ii], "{{", "}}"));
                if (!isTemplate)
                {
                    string text = templateItems[ii];
                    if (Utils.StartsWithEndsWith(text, "{{", "}}"))
                        text = Utils.TrimByLength(text, 1);

                    this.PlaceHolders.Add(new PlaceHolder(templates, text));
                }
                else
                {
                    CompilePlaceHolder(templates, templateItems[ii]);
                }
            }
        }

        internal bool IsLastCommand { get; set; }
        internal int LineNumber { get; set; }

        public string Format(string line, string data, params string[] values)
        {
            // #TODO Logic for skip line should be implemented somewhere here.

            StringBuilder sb = new StringBuilder();
            PlaceHolder placeholder = new PlaceHolder();
            PlaceHolderMethods placeholderMethods = new PlaceHolderMethods();
            bool skippedData = false;
            foreach (PlaceHolder p in PlaceHolders)
            {
                if (p.GetType() == typeof(PlaceHolder))
                {
                    placeholder = (PlaceHolder)p;
                    sb.Append(placeholder.Text);
                }
                if (p.GetType() == typeof(PlaceHolderMethods))
                {
                    placeholderMethods = (PlaceHolderMethods)p;
                    sb.Append(placeholderMethods.Execute(values, line, data, LineNumber, IsLastCommand, out skippedData));
                    if (skippedData)
                    {
                        LineNumber++;
                        return string.Empty;
                    }
                }
            }
            LineNumber++;
            return sb.ToString();
        }

        internal static bool PropertyArrayTypeSupported(Type t)
        {
            return (Utils.IsOneOfTypes(t, typeof(string[])));
        }

        internal static bool PropertyTypeSupported(Type t)
        {
            return (Utils.IsOneOfTypes(t,
                                 typeof(string),
                                 typeof(bool),
                                 typeof(char),
                                 typeof(Int16),
                                 typeof(Int32),
                                 typeof(Int64),
                                 typeof(byte),
                                 typeof(double),
                                 typeof(decimal),
                                 typeof(Int16?),
                                 typeof(Int32?),
                                 typeof(Int64?),
                                 typeof(byte?),
                                 typeof(double?),
                                 typeof(decimal?)
                                 )) || t.BaseType == typeof(Enum);
        }

        internal static void SetPropertyValue(System.Reflection.PropertyInfo property, object obj, string value)
        {
            value = value.Trim(new char[] { '"' });
            Type t = property.PropertyType;
            object val = null;
            if (t.BaseType == typeof(Enum))
            {
                val = EnumUtility.GetValueByName(t, value);
                property.SetValue(obj, val, null);
            }

            if (t == typeof(Int16))
            {
                val = ConvertUtility.ToInt16(value, 0);
                property.SetValue(obj, val, null);
            }
            if (t == typeof(Int32))
            {
                val = ConvertUtility.ToInt32(value, 0);
                property.SetValue(obj, val, null);
            }
            if (t == typeof(Int64))
            {
                val = ConvertUtility.ToInt64(value, 0);
                property.SetValue(obj, val, null);
            }
            if (t == typeof(byte))
            {
                val = ConvertUtility.ToByte(value, 0);
                property.SetValue(obj, val, null);
            }
            if (t == typeof(double))
            {
                val = ConvertUtility.ToDouble(value, 0);
                property.SetValue(obj, val, null);
            }
            if (t == typeof(decimal))
            {
                val = ConvertUtility.ToDecimal(value, 0);
                property.SetValue(obj, val, null);
            }
            if (t == typeof(bool))
            {
                val = ConvertUtility.ToBoolean(value, false);
                property.SetValue(obj, val, null);
            }
            if (t == typeof(Int16?))
            {
                val = ConvertUtility.ToInt16Nullable(value);
                property.SetValue(obj, val, null);
            }
            if (t == typeof(Int32?))
            {
                val = ConvertUtility.ToInt32Nullable(value);
                property.SetValue(obj, val, null);
            }
            if (t == typeof(Int64?))
            {
                val = ConvertUtility.ToInt64Nullable(value);
                property.SetValue(obj, val, null);
            }
            if (t == typeof(byte?))
            {
                val = ConvertUtility.ToByteNullable(value);
                property.SetValue(obj, val, null);
            }
            if (t == typeof(double?))
            {
                val = ConvertUtility.ToDoubleNullable(value);
                property.SetValue(obj, val, null);
            }
            if (t == typeof(decimal?))
            {
                val = ConvertUtility.ToDecimalNullable(value);
                property.SetValue(obj, val, null);
            }
            if (t == typeof(string))
                property.SetValue(obj, value, null);

            if (t == typeof(char))
            {
                char[] cval = value.ToCharArray();
                if (cval.Length > 0)
                    property.SetValue(obj, cval[0], null);
            }
        }

        internal static void SetPropertyValueArray(System.Reflection.PropertyInfo property, object obj, string[] values, int valuesStartIndex)
        {
            Type t = property.PropertyType;
            object val = null;
            if (t == typeof(char[]))
            {
                char[] arrayParameters = new char[values.Length - valuesStartIndex];
                int cnt = 0;
                for (int xx = valuesStartIndex; xx < values.Length; xx++)
                {
                    char[] cval = values[xx].Trim(new char[] { '"' }).ToCharArray();
                    if (cval.Length > 0)
                        property.SetValue(obj, cval[0], null);

                    cnt++;
                }
                property.SetValue(obj, arrayParameters, null);
                return;
            }
            if (t == typeof(string[]))
            {
                string[] arrayParameters = new string[values.Length - valuesStartIndex];
                int cnt = 0;
                for (int xx = valuesStartIndex; xx < values.Length; xx++)
                {
                    arrayParameters[cnt] = values[xx].Trim(new char[] { '"' });
                    cnt++;
                }
                property.SetValue(obj, arrayParameters, null);
                return;
            }
            if (t == typeof(byte[]))
            {
                byte[] arrayParametersByte = new byte[values.Length - valuesStartIndex];
                int cnt = 0;
                for (int xx = valuesStartIndex; xx < values.Length; xx++)
                {
                    val = values[xx].Trim(new char[] { '"' });
                    arrayParametersByte[cnt] = ConvertUtility.ToByte(val, 0);
                    cnt++;
                }
                property.SetValue(obj, arrayParametersByte, null);
                return;
            }
            if (t == typeof(Int16[]))
            {
                Int16[] arrayParametersInt16 = new Int16[values.Length - valuesStartIndex];
                int cnt = 0;
                for (int xx = valuesStartIndex; xx < values.Length; xx++)
                {
                    val = values[xx].Trim(new char[] { '"' });
                    arrayParametersInt16[cnt] = ConvertUtility.ToInt16(val, 0);
                    cnt++;
                }
                property.SetValue(obj, arrayParametersInt16, null);
                return;
            }
            if (t == typeof(Int32[]))
            {
                Int32[] arrayParametersInt32 = new Int32[values.Length - valuesStartIndex];
                int cnt = 0;
                for (int xx = valuesStartIndex; xx < values.Length; xx++)
                {
                    val = values[xx].Trim(new char[] { '"' });
                    arrayParametersInt32[cnt] = ConvertUtility.ToInt32(val, 0);
                    cnt++;
                }
                property.SetValue(obj, arrayParametersInt32, null);
                return;
            }
            if (t == typeof(Int64[]))
            {
                Int64[] arrayParametersInt64 = new Int64[values.Length - valuesStartIndex];
                int cnt = 0;
                for (int xx = valuesStartIndex; xx < values.Length; xx++)
                {
                    val = values[xx].Trim(new char[] { '"' });
                    arrayParametersInt64[cnt] = ConvertUtility.ToInt64(val, 0);
                    cnt++;
                }
                property.SetValue(obj, arrayParametersInt64, null);
                return;
            }
        }

        private static string getCallName(string call)
        {
            string[] items = Utils.SplitDualEscaped(call.Trim(), Utils.CaseComparison.Ordinal, "(", ")", ")", "\"");
            if (items.Length > 0)
                return items[0];

            return string.Empty;
        }

        private static string getCallParameters(string call)
        {
            string[] items = Utils.SplitDualEscaped(call.Trim(), Utils.CaseComparison.Ordinal, "(", ")", ")", "\"");
            if (items.Length > 1)
                return items[1];

            return string.Empty;
        }

        private void CompilePlaceHolder(string[] templates, string template)
        {
            // First element in placeholder has two tastes name or index.
            string[] fields = Utils.TrimByLength(template, 1).Split(new char[] { ':' });
            string inlinecalls = string.Empty;
            int pointer = -1;

            // {0}
            // {0:Guid}
            // {Guid}
            if (fields.Length == 1)
            {
                if (Utils.IsNumeric(fields[0]) || fields[0] == "X" || fields[0] == "Y")
                {
                    // index based
                    pointer = ConvertUtility.ToInt32(fields[0], 0);

                    if (fields[0] == "X")
                        pointer = -2;

                    if (fields[0] == "Y")
                        pointer = -3;

                    PlaceHolderMethods phmIndexOnly = new PlaceHolderMethods();
                    phmIndexOnly.Methods = new List<MethodBase>();
                    phmIndexOnly.Templates = templates;
                    phmIndexOnly.Text = template;
                    phmIndexOnly.ValuesIndex = pointer;
                    this.PlaceHolders.Add(phmIndexOnly);
                    return;
                }
                else
                {
                    // Method based
                    inlinecalls = fields[0];
                }
            }
            if (fields.Length == 2)
            {
                // index based
                pointer = ConvertUtility.ToInt32(fields[0], 0);
                if (fields[0] == "X")
                    pointer = -2;

                if (fields[0] == "Y")
                    pointer = -3;

                inlinecalls = fields[1];
            }

            // find out which methods to use.
            string[] methodcalls = Utils.SplitEscaped(inlinecalls, ",", "(", ")");

            PlaceHolderMethods phm = new PlaceHolderMethods();
            phm.Text = template;
            phm.Templates = templates;
            phm.ValuesIndex = pointer;

            foreach (string call in methodcalls)
            {
                MethodInstance clone = MethodInstance.GetMethodInstance(getCallName(call));
                if (clone != null)
                {
                    // can be Guid, Guid() PadLeft(3,0) etc
                    // parameters
                    string parametersString = getCallParameters(call).Trim(new char[] { '(', ')' });
                    string[] parameters = Utils.SplitEscaped(parametersString, ',', '"');
                    
                    for (int ii = 0; ii < parameters.Length; ii++)
                    {
                        parameters[ii] = parameters[ii].Trim();
                        if (parameters[ii].Length > 1)
                        {
                            parameters[ii] = parameters[ii].Trim(new char[] { '"' });
                        }

                        Type t = typeof(object);
                        if (clone.Properties.Count > 0)
                        {
                            t = clone.Properties[Utils.Bound(ii, 0, clone.Properties.Count)].PropertyReference.PropertyType;
                        }
                        if (ii < clone.Properties.Count)
                        {
                            if (StringTemplateItem.PropertyTypeSupported(t))
                                StringTemplateItem.SetPropertyValue(clone.Properties[ii].PropertyReference, clone.Formatter, parameters[ii]);

                            if (StringTemplateItem.PropertyArrayTypeSupported(t))
                            {
                                // if next property to be set is an array, all remaining parameters will be asigned to this property.
                                StringTemplateItem.SetPropertyValueArray(clone.Properties[ii].PropertyReference, clone.Formatter, parameters, ii);
                                break;
                            }
                        }
                    }
                    phm.Methods.Add(clone.Formatter);
                }
            }
            this.PlaceHolders.Add(phm);
        }
    }
}