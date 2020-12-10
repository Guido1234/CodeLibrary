using Mini.Net.Library.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mini.Net.Template
{
    public static class CSharpUtility
    {
        private static string escapeTabsAndEnters(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            s = s.Replace("\\r", "\r");
            s = s.Replace("\\n", "\n");
            s = s.Replace("\\t", "\t");
            return s;
        }

        private static string CreatePrivateMember(string name, string type, string defaultvalue, string publicaccessmodifiers)
        {
            if (string.IsNullOrEmpty(defaultvalue))
            {
                return CreatePrivateMember(name, type);
            }
            if (StringUtility.OneOfNullOrEmpty(name, type))
            {
                return string.Empty;
            }
            string template = Template.Resources.CodeSnippetResources.PrivateMember;
            if (StringUtility.Contains(publicaccessmodifiers, ContainsMode.AtLeastOneOf, StringComparison.Ordinal, "static", "stat", "sta"))
            {
                template = Template.Resources.CodeSnippetResources.PrivateStaticMember;
            }
            name = name.Trim();
            type = type.Trim();
            string nameU = StringUtility.SetCase(name, CaseMode.FirstLetterToUpper);
            string nameL = StringUtility.SetCase(name, CaseMode.FirstLetterToLower);
            return string.Format(template, type, nameL, nameU, defaultvalue);
        }

        private static string CreatePrivateMember(string name, string type)
        {
            if (StringUtility.OneOfNullOrEmpty(name, type))
            {
                return string.Empty;
            }
            string template = Template.Resources.CodeSnippetResources.PrivateMemberNoDefault;
            name = name.Trim();
            type = type.Trim();
            string nameU = StringUtility.SetCase(name, CaseMode.FirstLetterToUpper);
            string nameL = StringUtility.SetCase(name, CaseMode.FirstLetterToLower);
            return string.Format(template, type, nameL, nameU);
        }

        private static string CreateProperty(string name, string type, string accessmodifiers, string comment, bool region, bool serializable, bool getteronly, bool method, bool publiconly)
        {
            string commentTemplate = Template.Resources.CodeSnippetResources.Comment;
            string template = Template.Resources.CodeSnippetResources.Property;
            if (getteronly)
            {
                template = @"";
            }
            if (publiconly)
            {
                template = Template.Resources.CodeSnippetResources.PropertyShort;
            }

            if (method)
            {
                template = Template.Resources.CodeSnippetResources.PropertyGetOnly;
            }

            if (string.IsNullOrWhiteSpace(accessmodifiers))
            {
                accessmodifiers = "public";
            }
            accessmodifiers = accessmodifiers.ToLower();

            string commentBlock = string.Empty;
            if (!string.IsNullOrEmpty(comment))
            {
                commentBlock = string.Format(commentTemplate, comment);
            }

            string nameU = StringUtility.CamelCaseUpper(name);
            string nameL = StringUtility.CamelCaseLower(name);
            string regionstart = string.Empty;
            string regionend = string.Empty;
            if (region)
            {
                regionstart = "#region " + name;
                regionend = "#endregion";
            }
            string serialization = string.Empty;
            if (serializable)
            {
                serialization = string.Format(Template.Resources.CodeSnippetResources.XmlElement, nameU, type);
            }
            string prop = string.Format(template, type, nameL, nameU, accessmodifiers, StringUtility._TAB, regionstart, regionend, serialization, commentBlock);
            return prop;
        }

        private static string CreateEventDelegate(string name, string[] items)
        {
            if (!HasEventArgumentParams(items))
            {
                return string.Format(Template.Resources.CodeSnippetResources.EventDelegateDefault, StringUtility.CamelCaseUpper(name));
            }
            else
            {
                return string.Format(Template.Resources.CodeSnippetResources.EventDelegate, StringUtility.CamelCaseUpper(name));
            }
        }

        private static string CreateEventDeclaration(string name, string[] items)
        {
            if (!HasEventArgumentParams(items))
            {
                return string.Format(Template.Resources.CodeSnippetResources.PublicEventDefault, StringUtility.CamelCaseUpper(name));
            }
            else
            {
                return string.Format(Template.Resources.CodeSnippetResources.PublicEvent, StringUtility.CamelCaseUpper(name));
            }
        }

        private static bool HasEventArgumentParams(string[] items)
        {
            return (items.Length > 2);
        }

        private static string CreateEventArgumentClass(string name, string[] items)
        {
            StringBuilder sbProperties = new StringBuilder();
            for (int ii = 2; ii < items.Length; ii = ii + 2)
            {
                string typename = StringArrayUtility.ArrayValue(items, ii);
                string declarename = StringArrayUtility.ArrayValue(items, ii + 1);
                sbProperties.AppendFormat(escapeTabsAndEnters(Template.Resources.CodeSnippetResources.EventArgProperty), typename, StringUtility.CamelCaseUpper(declarename));
            }
            return string.Format(escapeTabsAndEnters(Template.Resources.CodeSnippetResources.EventArgClass), StringUtility.CamelCaseUpper(name), sbProperties.ToString());
        }

        private static string CreateRaiseEvent(string name, string[] items)
        {
            StringBuilder sbParams = new StringBuilder();
            for (int ii = 2; ii < items.Length; ii = ii + 2)
            {
                string typename = StringArrayUtility.ArrayValue(items, ii);
                string declarename = StringArrayUtility.ArrayValue(items, ii + 1);
                sbParams.AppendFormat(Template.Resources.CodeSnippetResources.RaiseEventParameter, typename, StringUtility.CamelCaseLower(declarename));
            }
            StringBuilder assigns = new StringBuilder();
            for (int ii = 2; ii < items.Length; ii = ii + 2)
            {
                string declarename = StringArrayUtility.ArrayValue(items, ii + 1);
                assigns.AppendFormat(escapeTabsAndEnters(Template.Resources.CodeSnippetResources.RaiseEventParameterAssignment), StringUtility.CamelCaseUpper(declarename), StringUtility.CamelCaseLower(declarename));
            }
            if (HasEventArgumentParams(items))
            {
                return string.Format(Template.Resources.CodeSnippetResources.RaiseEventMethod, StringUtility.CamelCaseUpper(name), StringUtility.TrimEndByLength(sbParams.ToString(), 1), assigns.ToString());
            }
            else
            {
                return string.Format(Template.Resources.CodeSnippetResources.RaiseEventMethodDefault, StringUtility.CamelCaseUpper(name), StringUtility.TrimEndByLength(sbParams.ToString(), 1), assigns.ToString());
            }
        }

        /// <summary>
        /// ClassName:InheretedClassName,Interfaces
        /// PropertyType;PropertyName;DefaultValue;AccessModifiers|Options
        /// PropertyType;PropertyName;
        /// PropertyType;PropertyName;DefaultValue;
        /// PropertyType;PropertyName;DefaultValue;AccessModifiers;
        /// PropertyType;PropertyName|Options
        ///
        /// Options:
        /// constructor con, tostring, serializable ser, region reg, publiconly pub, method met, getteronly get
        ///
        /// person
        /// string;name|serializable ser constructor con region reg tostring getteronly
        /// </summary>
        /// <param name="propertydefinitions"></param>
        /// <returns></returns>
        public static string CreateClass(string propertydefinitions)
        {
            if (string.IsNullOrEmpty(propertydefinitions))
            {
                return string.Empty;
            }
            StringBuilder sbClassDeclaration = new StringBuilder();
            StringBuilder sbPrivate = new StringBuilder();
            StringBuilder sbPublic = new StringBuilder();
            StringBuilder sbParams = new StringBuilder();
            StringBuilder sbParamAssigns = new StringBuilder();
            StringBuilder sbUsing = new StringBuilder();
            StringBuilder sbEventDelegates = new StringBuilder();
            StringBuilder sbEventDeclarations = new StringBuilder();
            StringBuilder sbEventArgumentClasses = new StringBuilder();
            StringBuilder sbEventRaises = new StringBuilder();

            sbUsing.AppendFormat(Template.Resources.CodeSnippetResources.UsingLine, "System");
            List<string> toStringList = new List<string>();

            string[] items = new string[0];

            string[] lines = StringUtility.Lines(propertydefinitions);
            lines = StringArrayUtility.RemoveEmpties(lines);
            if (lines.Length <= 1)
            {
                return string.Empty;
            }
            bool usingadded = false;
            for (int ii = 1; ii < lines.Length; ii++)
            {
                string line = lines[ii];
                items = StringUtility.SplitEscaped(line, '|', '"');
                string options = StringArrayUtility.ArrayValue(items, 1);
                items = StringUtility.SplitEscaped(items[0], ';', '"');
                string type = StringArrayUtility.ArrayValue(items, 0).Trim();
                string name = StringUtility.CamelCaseUpper(StringArrayUtility.ArrayValue(items, 1).Trim());
                string defaultvalue = StringArrayUtility.ArrayValue(items, 2);
                string accessmodifiers = StringArrayUtility.ArrayValue(items, 3);
                string comment = StringArrayUtility.ArrayValue(items, 4);
                bool publiconly = StringUtility.Contains(options, ContainsMode.AtLeastOneOf, StringComparison.OrdinalIgnoreCase, "publiconly", "pub");

                if (type.Equals("event"))
                {
                    // public delegate void BrowserFileClickEventHandler(object sender, BrowserClickEventArgs ce);
                    sbEventDelegates.Append(CreateEventDelegate(name, items));
                    sbEventDeclarations.Append(CreateEventDeclaration(name, items));
                    if (HasEventArgumentParams(items))
                    {
                        sbEventArgumentClasses.Append(CreateEventArgumentClass(name, items));
                    }
                    sbEventRaises.Append(CreateRaiseEvent(name, items));
                }

                if (!type.Equals("event"))
                {
                    // Constructor
                    if (StringUtility.Contains(options, ContainsMode.AtLeastOneOf, StringComparison.OrdinalIgnoreCase, "constructor", "con"))
                    {
                        sbParams.Append(type);
                        sbParams.Append(StringUtility._SPACE);
                        sbParams.Append("p");
                        sbParams.Append(StringUtility.CamelCaseUpper(name));
                        sbParams.Append(",");
                        if (publiconly == false)
                        {
                            sbParamAssigns.Append(StringUtility.CamelCaseLower(name));
                        }
                        else
                        {
                            sbParamAssigns.Append(StringUtility.CamelCaseUpper(name));
                        }
                        sbParamAssigns.Append(" = ");
                        sbParamAssigns.Append("p");
                        sbParamAssigns.Append(StringUtility.CamelCaseUpper(name));
                        sbParamAssigns.Append(";");
                        sbParamAssigns.Append(StringUtility._CRLF);
                    }

                    if (StringUtility.Contains(options, ContainsMode.All, StringComparison.OrdinalIgnoreCase, "tostring"))
                    {
                        if (publiconly == false)
                        {
                            toStringList.Add(StringUtility.CamelCaseLower(name));
                        }
                        else
                        {
                            toStringList.Add(StringUtility.CamelCaseUpper(name));
                        }
                    }

                    if (StringUtility.Contains(options, ContainsMode.AtLeastOneOf, StringComparison.OrdinalIgnoreCase, "serializable", "ser"))
                    {
                        if (usingadded == false)
                        {
                            sbUsing.AppendFormat(Template.Resources.CodeSnippetResources.UsingLine, "System.Xml.Serialization");
                            usingadded = true;
                        }
                    }
                    if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(type))
                    {
                        if (publiconly == false)
                        {
                            sbPrivate.Append(CreatePrivateMember(name, type, defaultvalue, accessmodifiers));
                            sbPrivate.Append(StringUtility._CRLF);
                        }
                        sbPublic.Append(CreateProperty(name,
                            type,
                            accessmodifiers,
                            comment,
                            StringUtility.Contains(options, ContainsMode.AtLeastOneOf, StringComparison.OrdinalIgnoreCase, "region", "reg"),
                            StringUtility.Contains(options, ContainsMode.AtLeastOneOf, StringComparison.OrdinalIgnoreCase, "serializable", "ser"),
                            StringUtility.Contains(options, ContainsMode.AtLeastOneOf, StringComparison.OrdinalIgnoreCase, "getteronly", "get"),
                            StringUtility.Contains(options, ContainsMode.AtLeastOneOf, StringComparison.OrdinalIgnoreCase, "method", "met"),
                            publiconly
                            ));
                        sbPublic.Append(StringUtility._CRLF);
                        sbPublic.Append(StringUtility._CRLF);
                    }
                }
            }
            items = lines[0].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            string classname = items[0].Trim();
            string inheretedClassName = string.Empty;
            string parameters = sbParams.ToString();
            parameters = StringUtility.TrimEndByLength(parameters, 1);
            string constructor = string.Empty;
            if (!string.IsNullOrEmpty(parameters))
            {
                constructor = string.Format(Template.Resources.CodeSnippetResources.Constructor, classname, parameters, StringUtility.TabIndent(StringUtility.TrimEndByLength(sbParamAssigns.ToString(), 2), 3), StringUtility._TAB);
            }

            sbClassDeclaration.Append(classname);

            if (items.Length > 1)
            {
                items = items[1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                inheretedClassName = items[0];
                sbClassDeclaration.Append(" : ");
                sbClassDeclaration.Append(inheretedClassName);

                for (int ii = 1; ii < items.Length; ii++)
                {
                    sbClassDeclaration.Append(" , ");
                    sbClassDeclaration.Append(items[ii]);
                }
            }

            string toString = Template.Resources.CodeSnippetResources.DefaultToString;
            if (toStringList.Count > 0)
            {
                StringBuilder sbToString = new StringBuilder();
                sbToString.Append("return string.Format(\"");
                for (int ii = 0; ii < toStringList.Count; ii++)
                {
                    sbToString.Append("{");
                    sbToString.Append(ii.ToString());
                    sbToString.Append("}");
                }
                sbToString.Append("\"");
                sbToString.Append(",");
                for (int ii = 0; ii < toStringList.Count; ii++)
                {
                    sbToString.Append(toStringList[ii]);
                    if (ii < toStringList.Count - 1)
                    {
                        sbToString.Append(",");
                    }
                }
                sbToString.Append(");");
                toString = sbToString.ToString();
            }

            string pri = sbPrivate.ToString();
            string pub = sbPublic.ToString();

            string result = string.Format(Template.Resources.CodeSnippetResources.Class,
                sbClassDeclaration.ToString(), // 0
                StringUtility.TabIndent(pri, 2), // 1
                StringUtility.TabIndent(pub, 2), // 2
                classname, // 3
                StringUtility._TAB, // 4
                constructor, // 5
                toString, // 6
                sbUsing.ToString(), //7
                StringUtility.TabIndent(sbEventDelegates.ToString(), 1), // 8
                StringUtility.TabIndent(sbEventDeclarations.ToString(), 2), // 9
                StringUtility.TabIndent(sbEventArgumentClasses.ToString(), 1), // 10
                StringUtility.TabIndent(sbEventRaises.ToString(), 2) // 11
                );

            return StringUtility.RemoveDuplicateEmptyLines(result, true);
        }
    }
}