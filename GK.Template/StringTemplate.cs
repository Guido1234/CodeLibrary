//using GK.Library.Utilities;
using GK.Template.Attributes;
using GK.Template.DataCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace GK.Template
{
    public sealed class StringTemplate
    {
        internal List<StringTemplateItem> stringTemplateItems = new List<StringTemplateItem>();
        private static string availableCommandNames = string.Empty;
        private List<Type> commandTypes = new List<Type>();
        private List<DataCommandBase> datacommands = new List<DataCommandBase>();
        private int lineNumberStep = 1;

        private string prev_data = string.Empty;
        private string prev_template = string.Empty;
        private char separator = ';';

        /// <summary>
        /// Constructor.
        /// </summary>
        public StringTemplate()
        {
            this.DataSources = new DataSourceCollection();
            if (commandTypes.Count == 0)
            {
                commandTypes = Utils.GetObjectsWithBaseType(typeof(DataCommandBase), true);
            }
            if (string.IsNullOrEmpty(availableCommandNames))
            {
                availableCommandNames = getAvailableCommandNames(commandTypes);
            }
        }

        /// <summary>
        /// Data consists of lines based on CSV rules (http://en.wikipedia.org/wiki/Comma-separated_values</para>)
        /// Datalines can end with #0 (default) #1 #2 etc to point to a specific template in a Multipart template.
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Datasource to be used with the datasource command
        /// </summary>
        public DataSourceCollection DataSources { get; set; }

        public int LineNumberOffset { get; set; }

        public int LineNumberStep
        {
            get
            {
                return lineNumberStep;
            }
            set
            {
                lineNumberStep = value;
            }
        }

        public char Separator
        {
            get
            {
                return separator;
            }
            set
            {
                separator = value;
            }
        }

        /// <summary>
        /// String template or Multipart string template separated by #endtemplate
        /// </summary>
        public string Template { get; set; }

        /// <summary>
        /// Formats the Template and Data
        /// </summary>
        /// <returns></returns>
        public string Format()
        {
            initTemplate(Template);
            initCommands(Data);
            return Format(Template, Data);
        }

        /// <summary>
        /// Formats a template string with data. (if template is unchaigned it will be the same as Format(string data) ).
        /// </summary>
        /// <param name="template">formatting template</param>
        /// <param name="data">Multi line CSV data</param>
        /// <returns></returns>
        public string Format(string template, string data)
        {
            Template = template;
            Data = data;
            initTemplate(template);
            initCommands(data);
            StringBuilder sb = new StringBuilder();
            int cnt = LineNumberOffset;
            string result = string.Empty;
            foreach (DataCommandBase command in datacommands)
            {
                if (command.TemplateNumber < stringTemplateItems.Count)
                {
                    StringTemplateItem templateItem = stringTemplateItems[command.TemplateNumber];
                    templateItem.LineNumber = cnt;
                    templateItem.IsLastCommand = command.IsLastCommand;
                    result = command.Execute(templateItem);
                    cnt = templateItem.LineNumber;
                    sb.Append(result);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Existing template will be used for formatting the data.
        /// </summary>
        /// <param name="data">Multi line CSV data</param>
        /// <returns></returns>
        public string Format(string data)
        {
            Data = data;
            initTemplate(Template);
            initCommands(data);
            StringBuilder sb = new StringBuilder();
            int cnt = LineNumberOffset;
            string result = string.Empty;
            foreach (DataCommandBase command in datacommands)
            {
                if (command.TemplateNumber < stringTemplateItems.Count)
                {
                    StringTemplateItem templateItem = stringTemplateItems[command.TemplateNumber];
                    templateItem.LineNumber = cnt;
                    templateItem.IsLastCommand = command.IsLastCommand;
                    result = command.Execute(templateItem);
                    cnt = templateItem.LineNumber;
                    sb.Append(result);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Existing template will be used for formatting the data.
        /// </summary>
        /// <param name="data">Single Line of data.</param>
        /// <returns></returns>
        public string Format(params string[] dataitems)
        {
            Data = Utils.JoinCsvLine(separator, dataitems);
            initTemplate(Template);
            initCommands(Data);
            StringBuilder sb = new StringBuilder();
            int cnt = LineNumberOffset;
            string result = string.Empty;
            foreach (DataCommandBase command in datacommands)
            {
                if (command.TemplateNumber < stringTemplateItems.Count)
                {
                    StringTemplateItem templateItem = stringTemplateItems[command.TemplateNumber];
                    templateItem.LineNumber = cnt;
                    templateItem.IsLastCommand = command.IsLastCommand;
                    result = command.Execute(templateItem);
                    cnt = templateItem.LineNumber;
                    sb.Append(result);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Formats a template string with data. (if template is unchaigned it will be the same as Format(string data) ).
        /// </summary>
        /// <param name="template">Formatting template</param>
        /// <param name="data">Single Line of data.</param>
        /// <returns></returns>
        public string Format(string template, params string[] dataitems)
        {
            Template = template;
            Data = Utils.JoinCsvLine(separator, dataitems);
            initTemplate(template);
            initCommands(Data);
            StringBuilder sb = new StringBuilder();
            int cnt = LineNumberOffset;
            string result = string.Empty;
            foreach (DataCommandBase command in datacommands)
            {
                if (command.TemplateNumber < stringTemplateItems.Count)
                {
                    StringTemplateItem templateItem = stringTemplateItems[command.TemplateNumber];
                    templateItem.LineNumber = cnt;
                    templateItem.IsLastCommand = command.IsLastCommand;
                    result = command.Execute(templateItem);
                    cnt = templateItem.LineNumber;
                    sb.Append(result);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Formats a template string in which after last #endtemplate the data begins.
        /// </summary>
        /// <param name="templateAndData">formatting template followed by Multi line CSV data</param>
        /// <returns></returns>
        public string FormatExtended(string templateAndData)
        {
            int index1 = templateAndData.LastIndexOf("#endtemplate");
            if (index1 < 0)
            {
                Template = templateAndData;
                Data = string.Empty;
                return Format(Template, Data);
            }
            string templ = templateAndData.Substring(0, index1);
            int index2 = index1 + "#endtemplate".Length;
            string dt = templateAndData.Substring(index2, templateAndData.Length - index2);
            if (dt.StartsWith("\r\n"))
            {
                dt = Utils.TrimStartByLength(dt, 2);
            }
            return Format(templ, dt);
        }

        public string FormatSeparate(string template, params string[] items)
        {
            StringBuilder sb = new StringBuilder();
            Template = template;
            foreach (string item in items)
            {
                initTemplate(template);
                initCommands(item, true);
                int cnt = LineNumberOffset;
                string result = string.Empty;
                foreach (DataCommandBase command in datacommands)
                {
                    if (command.TemplateNumber < stringTemplateItems.Count)
                    {
                        StringTemplateItem templateItem = stringTemplateItems[command.TemplateNumber];
                        templateItem.LineNumber = cnt;
                        templateItem.IsLastCommand = command.IsLastCommand;
                        result = command.Execute(templateItem);
                        cnt = templateItem.LineNumber;
                        sb.Append(result);
                    }
                }
            }
            return sb.ToString();
        }

        private static string getAvailableCommandNames(List<Type> commandTypes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Type t in commandTypes)
            {
                DataCommandAttribute att = DataCommandAttribute.GetAttribute(t);
                if (att != null)
                {
                    sb.Append(att.Name);
                    sb.Append(",");
                    sb.Append(att.Aliasses);
                    sb.Append(",");
                }
            }
            return sb.ToString();
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

        private DataCommandBase GetCommandInstance(string name, List<Type> commandTypes)
        {
            string[] aliasses = new string[0];
            foreach (Type t in commandTypes)
            {
                DataCommandAttribute att = DataCommandAttribute.GetAttribute(t);
                if (att != null)
                {
                    if (att.Name.Equals(name, StringComparison.Ordinal))
                        return (DataCommandBase)Activator.CreateInstance(t);
                    else
                    {
                        aliasses = att.Aliasses.Split(new char[] { ',' });
                        foreach (string alias in aliasses)
                        {
                            if (alias.Trim().Equals(name, StringComparison.Ordinal))
                                return (DataCommandBase)Activator.CreateInstance(t);
                        }
                    }
                }
            }
            return new DataCommandBase();
        }

        private void initCommands(string data)
        {
            initCommands(data, false);
        }

        /// <summary>
        /// Data consists of lines based on CSV rules (http://en.wikipedia.org/wiki/Comma-separated_values</para>)
        /// <para>Separator will be auto detected.</para>
        /// </summary>
        /// <param name="data"></param>
        private void initCommands(string data, bool handleDataAsOne)
        {
            if (data != prev_data || (data == string.Empty && prev_data == string.Empty))
            {
                datacommands.Clear();
                string callname = string.Empty;
                string callparams = string.Empty;
                string[] dataLines = new string[1] { string.Empty };
                if (!string.IsNullOrEmpty(data))
                    dataLines = Utils.SplitCsvLines(data, separator);
                if (handleDataAsOne)
                {
                    dataLines = new string[1];
                    dataLines[0] = data;
                }

                int templateNumber = 0;
                DataCommandBase dc = new DataCommandBase();
                string line = string.Empty;
                for (int xx = 0; xx < dataLines.Length; xx++)
                {
                    line = dataLines[xx];
                    templateNumber = templateNumberFromLine(line);
                    dc = new DataCommandBase();

                    string[] lineValues = Utils.SplitEscaped(lineFromTemplateLine(line), separator, '"');

                    if (lineValues.Length == 1)
                    {
                        callname = getCallName(line);
                        if (availableCommandNames.Contains(callname))
                        {
                            dc = GetCommandInstance(callname, commandTypes);
                            if (dc.GetType() != typeof(DataCommandBase))
                            {
                                dc.StringTemplate = this;
                                // set dc parameters
                                callparams = getCallParameters(line);
                                List<DataCommandParameterAttribute> attribs = DataCommandParameterAttribute.GetAttributes(dc.GetType());

                                string parametersString = callparams.Trim(new char[] { '(', ')' });
                                string[] parameters = Utils.SplitEscaped(parametersString, ',', '"');
                                for (int ii = 0; ii < parameters.Length; ii++)
                                {
                                    Type t = typeof(object);
                                    if (attribs.Count > 0)
                                    {
                                        t = attribs[Utils.Bound(ii, 0, attribs.Count)].PropertyReference.PropertyType;
                                    }
                                    if (ii < attribs.Count)
                                    {
                                        if (StringTemplateItem.PropertyTypeSupported(t))
                                        {
                                            StringTemplateItem.SetPropertyValue(attribs[ii].PropertyReference, dc, parameters[ii]);
                                        }
                                        if (StringTemplateItem.PropertyArrayTypeSupported(t))
                                        {
                                            // if next property to be set is an array, all remaining parameters will be asigned to this property.
                                            StringTemplateItem.SetPropertyValueArray(attribs[ii].PropertyReference, dc, parameters, ii);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    dc.IsLastCommand = (xx == dataLines.Length - 1);
                    dc.LineValues = lineValues;
                    dc.Line = line;
                    dc.Data = data;
                    dc.TemplateNumber = templateNumber;
                    datacommands.Add(dc);
                }
                dc.IsLastCommand = true;
            }
        }

        /// <summary>
        /// Initializes a template (if template unchainged it will not reinitialize).
        /// </summary>
        /// <param name="template"></param>
        private void initTemplate(string template)
        {
            if (template != prev_template)
            {
                stringTemplateItems.Clear();
                string[] templates = Utils.Split(template, "#endtemplate\r\n");
                foreach (string templ in templates)
                {
                    stringTemplateItems.Add(new StringTemplateItem(templates, templ));
                }
            }
        }

        private string lineFromTemplateLine(string line)
        {
            if (string.IsNullOrEmpty(line))
                return string.Empty;

            int index = line.LastIndexOf('#');
            if (index == -1)
                return line;

            return line.Substring(0, index);
        }

        private int templateNumberFromLine(string line)
        {
            if (line.Contains("#"))
                return ConvertUtility.ToInt32(Utils.SplitValueLast(line, "#"), 0);

            return 0;
        }
    }
}