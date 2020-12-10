using DevToys;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace GK.Template
{
    public static class Utils
    {
        /// <summary>
        /// \
        /// </summary>
        public const char _BACKSLASH = '\\';

        /// <summary>
        /// \r = CR (Carriage Return)
        /// </summary>
        public const char _CR = '\r';

        /// <summary>
        /// "
        /// </summary>
        public const char _DOUBLEQUOTE = '"';

        /// <summary>
        ///  \n = LF (Line Feed)
        /// </summary>
        public const char _LF = '\n';

        /// <summary>
        /// '
        /// </summary>
        public const char _SINGLEQUOTE = '\'';

        /// <summary>
        /// /
        /// </summary>
        public const char _SLASH = '/';

        /// <summary>
        /// represents space character
        /// </summary>
        public const char _SPACE = ' ';

        /// <summary>
        /// \t = Tab
        /// </summary>
        public const char _TAB = '\t';

        private static int seed = DateTime.Now.Millisecond;

        public enum CaseComparison
        {
            /// <summary>
            /// Exact matching
            /// </summary>
            Ordinal = 0,

            /// <summary>
            /// Ignore case
            /// </summary>
            IgnoreCase = 1
        }

        /// <summary>
        /// One of containsmode
        /// </summary>
        public enum ContainsMode
        {
            /// <summary>
            /// all items must be contained
            /// </summary>
            All = 0,

            /// <summary>
            /// at least one of the items must be contained.
            /// </summary>
            AtLeastOneOf = 1,

            /// <summary>
            /// None of the items must be contained
            /// </summary>
            None = 2
        }

        public enum FileOrDirectory
        {
            /// <summary>
            /// Is path a file
            /// </summary>
            File = 0,

            /// <summary>
            /// is path a directory
            /// </summary>
            Directory = 1,

            /// <summary>
            /// does path exists
            /// </summary>
            DoesNotExist = 2
        }

        /// <summary>
        /// Checks whether a char array ends with another one.
        /// </summary>
        /// <param name="ch">array to test</param>
        /// <param name="endswith">comapare end with</param>
        /// <returns></returns>
        public static bool ArrayEndsWith(char[] ch, char[] endswith)
        {
            return ArrayEndsWith(ch, endswith, ch.Length - 1, CaseComparison.Ordinal, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// checks whether a char array ends with a char array
        /// </summary>
        /// <param name="ch">array to test</param>
        /// <param name="endswith">comapare end with</param>
        /// <param name="index">the end position to use in ch</param>
        /// <returns></returns>
        /// <example>
        /// <code lang="cs">
        /// string s = "aaaxxxaaa";
        /// string sc = "xxx";
        /// char[] ch = s.ToCharArray();
        /// char[] chcompare = sc.ToCharArray();
        /// bool b = CharArrayEndsWith(ch, chcompare, 4);
        /// </code>
        /// </example>
        public static bool ArrayEndsWith(char[] ch, char[] endswith, int index)
        {
            return ArrayEndsWith(ch, endswith, index, CaseComparison.Ordinal, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// checks whether a char array ends with a char array
        /// </summary>
        /// <param name="ch">array to test</param>
        /// <param name="endswith">comapare end with</param>
        /// <param name="index">the end position to use in ch</param>
        /// <param name="comparison">determine whether to ignore case</param>
        /// <returns></returns>
        /// <example>
        /// <code lang="cs">
        /// string s = "aaaxxxaaa";
        /// string sc = "xxx";
        /// char[] ch = s.ToCharArray();
        /// char[] chcompare = sc.ToCharArray();
        /// bool b = CompareCharArrayBackward(ch, chcompare, 6, false);
        /// </code>
        /// </example>
        public static bool ArrayEndsWith(char[] ch, char[] endswith, int index, CaseComparison comparison)
        {
            return ArrayEndsWith(ch, endswith, index, comparison, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// checks whether a char array ends with a char array
        /// </summary>
        /// <param name="ch">array to test</param>
        /// <param name="endswith">comapare end with</param>
        /// <param name="index">the end position to use in ch</param>
        /// <param name="comparison">determine whether to ignore case</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <example>
        /// <code lang="cs">
        /// string s = "aaaxxxaaa";
        /// string sc = "xxx";
        /// char[] ch = s.ToCharArray();
        /// char[] chcompare = sc.ToCharArray();
        /// bool b = CompareCharArrayBackward(ch, chcompare, 6, false);
        /// </code>
        /// </example>
        public static bool ArrayEndsWith(char[] ch, char[] endswith, int index, CaseComparison comparison, CultureInfo culture)
        {
            if (ch == null)
                return false;

            if (endswith == null)
                return false;

            if (endswith.Length == 0)
                return false;

            if (ch.Length == 0)
                return false;

            int xx = endswith.Length - 1;
            int comparelen = endswith.Length;

            if (index < 0)
                return false;

            if (index > ch.Length - 1)
                return false;

            if (comparison == CaseComparison.Ordinal)
            {
                for (int ii = index; ii > index - comparelen; ii--)
                {
                    if (xx < 0)
                        return false;

                    if (ii < 0)
                        return false;

                    if (ch[ii] != endswith[xx])
                        return false;

                    xx--;
                }
                return true;
            }
            char ch1 = _SPACE;
            char ch2 = _SPACE;
            for (int ii = index; ii > index - comparelen; ii--)
            {
                if (xx < 0)
                    return false;

                if (ii < 0)
                    return false;

                ch1 = char.ToUpper(ch[ii], culture);
                ch2 = char.ToUpper(endswith[xx], culture);
                if (ch1 != ch2)
                    return false;

                xx--;
            }
            return true;
        }

        public static string ArrayValue(string[] array, int index) => (array == null || index < 0 || index >= array.Length) ? string.Empty : array[index];

        public static Int32 Bound(Int32 value, Int32 lowbound, Int32 highbound) => (value >= highbound) ? highbound : (value <= lowbound) ? lowbound : value;

        public static string CamelCaseLower(string name)
        {
            StringBuilder result = new StringBuilder();
            char[] chars = name.ToCharArray();
            bool nextCap = false;
            bool isFirst = true;
            foreach (char c in chars)
            {
                if (char.IsLetter(c))
                {
                    if (isFirst)
                    {
                        result.Append(char.ToLower(c));
                        isFirst = false;
                        continue;
                    }
                    if (nextCap)
                    {
                        result.Append(char.ToUpper(c));
                        nextCap = false;
                    }
                    else
                        result.Append(c);
                }
                else
                    nextCap = true;
                if (nextCap)
                    continue;
            }
            return result.ToString();
        }

        public static string CamelCaseUpper(string name)
        {
            StringBuilder result = new StringBuilder();
            char[] chars = name.ToCharArray();
            bool nextCap = true;
            foreach (char c in chars)
            {
                if (char.IsLetter(c))
                {
                    if (nextCap)
                    {
                        result.Append(char.ToUpper(c));
                        nextCap = false;
                    }
                    else
                        result.Append(c);
                }
                else
                    nextCap = true;
                if (nextCap)
                    continue;
            }
            return result.ToString();
        }

        /// <summary>
        /// test whether a string contains multiple items. with various modes.
        /// </summary>
        /// <param name="s">string to test whether it contains something</param>
        /// <param name="containsmode">Determine the comparison mode</param>
        /// <param name="comparison">Comparison mode</param>
        /// <param name="values">param array of values to test</param>
        /// <returns></returns>
        /// <example>
        ///
        /// </example>
        public static bool Contains(string s, ContainsMode containsmode, StringComparison comparison, params string[] values)
        {
            bool b = false;
            if (containsmode == ContainsMode.All)
            {
                b = true;
            }
            if (containsmode == ContainsMode.None)
            {
                b = true;
            }
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }
            if (values == null)
            {
                return false;
            }

            foreach (string val in values)
            {
                if (!string.IsNullOrEmpty(val))
                {
                    switch (containsmode)
                    {
                        case ContainsMode.AtLeastOneOf:

                            //return s.IndexOf(s, startIndex, comparison);

                            if (s.IndexOf(val, 0, comparison) > -1)
                            {
                                return true;
                            }
                            break;

                        case ContainsMode.All:
                            if (!(s.IndexOf(val, 0, comparison) > -1))
                            {
                                return false;
                            }
                            break;

                        case ContainsMode.None:
                            if ((s.IndexOf(val, 0, comparison) > -1))
                            {
                                return false;
                            }
                            break;
                    }
                }
            }
            return b;
        }

        public static bool DerivesFromBaseType(Type type, Type basetype)
        {
            if (type == basetype)
                return true;

            if (type == null)
                return false;

            if (type.BaseType != null)
            {
                if (type.BaseType == basetype)
                    return true;

                return DerivesFromBaseType(type.BaseType, basetype);
            }
            return false;
        }

        public static string EnsureEndWith(string s, char endWith)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            s = s.TrimEnd(new char[] { endWith });
            return string.Format("{0}{1}", s, endWith);
        }

        public static bool Equals(string s1, string s2, StringComparison comparison)
        {
            if (s1 != null && s2 != null)
            {
                if (s1.Equals(s2, comparison))
                {
                    return true;
                }
            }
            if (s1 == null && s2 == null)
            {
                return true;
            }
            return false;
        }

        public static double EvaluateExpression(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return 0;
            }
            double val = 0;
            try
            {
                val = (double)new System.Xml.XPath.XPathDocument
                (new StringReader("<r/>")).CreateNavigator().Evaluate
                (string.Format("number({0})", new
                System.Text.RegularExpressions.Regex(@"([\+\-\*])")
                .Replace(s, " ${1} ")
                .Replace("/", " div ")
                .Replace("%", " mod ")));
            }
            catch { }
            {
            }
            return val;
        }

        /// <summary>
        /// Apply format to the values specific type.
        /// <para>see http://msdn.microsoft.com/en-us/library/26etazsy.aspx</para>
        /// <para>for boolean use: "truepart|falsepart"</para>
        /// <para>for boolean? use: "truepart|falsepart|nullpart"</para>
        /// <para>for TimeSpan use: d,h,m,s,t for days, hours, minutes, seconds, miliseconds</para>
        /// </summary>
        /// <param name="obj">Object to format</param>
        /// <param name="mergeobject">used to format strings, See MergeAny function</param>
        /// <param name="format">format string to use</param>
        /// <param name="cultureInfo">culture info to use when apply formats</param>
        /// <returns></returns>
        /// <example>
        /// <code lang="cs">
        /// DateTime d = DateTime.Now;
        /// string s = StringUtility.Format(d, "dd/MM/yyyy hh:mm");
        /// Console.WriteLine(s);
        /// </code>
        /// result: "15/03/2011 10:53"
        /// </example>
        public static string Format(object obj, string format, CultureInfo cultureInfo)
        {
            object value = obj;
            if (value == null)
            {
                return string.Empty;
            }
            if (string.IsNullOrEmpty(format))
            {
                return value.ToString();
            }
            if (value.GetType() == typeof(bool))
            {
                bool valueBool = (bool)value;
                return BooleanUtility.Format(valueBool, format);
            }

            // apply mergeAny for strings when mergeobject is specified.
            if (value.GetType() == typeof(string))
            {
                return (string)value;
            }

            if (value.GetType() == typeof(Enum))
            {
                return EnumUtility.GetDescription((Enum)value);
            }

            if (value.GetType() == typeof(bool?))
            {
                bool? valueBoolN = (bool?)value;
                return BooleanUtility.Format(valueBoolN, format);
            }

            if (value.GetType() == typeof(TimeSpan))
            {
                TimeSpan valueTimeSpan = (TimeSpan)value;
                return TimeSpanUtility.Format(valueTimeSpan, format);
            }

            if (value.GetType() == typeof(TimeSpan?))
            {
                TimeSpan? valueTimeSpanN = (TimeSpan?)value;
                if (valueTimeSpanN.HasValue)
                {
                    return TimeSpanUtility.Format(valueTimeSpanN.Value, format);
                }
                return TimeSpanUtility.Format(TimeSpan.Zero, format);
            }

            if (value.GetType() == typeof(byte))
            {
                byte valbyte = (byte)value;
                return valbyte.ToString(format, cultureInfo);
            }

            if (value.GetType() == typeof(byte?))
            {
                byte? valbyteN = (byte?)value;
                if (valbyteN.HasValue)
                {
                    return valbyteN.Value.ToString(format, cultureInfo);
                }
                else
                {
                    return string.Empty;
                }
            }

            if (value.GetType() == typeof(Int16))
            {
                Int16 valInt16 = (Int16)value;
                return valInt16.ToString(format, cultureInfo);
            }

            if (value.GetType() == typeof(Int16?))
            {
                Int16? valInt16n = (Int16?)value;
                if (valInt16n.HasValue)
                {
                    return valInt16n.Value.ToString(format, cultureInfo);
                }
                else
                {
                    return string.Empty;
                }
            }

            if (value.GetType() == typeof(Int32))
            {
                Int32 valInt32 = (Int32)value;
                return valInt32.ToString(format, cultureInfo);
            }

            if (value.GetType() == typeof(Int32?))
            {
                Int32? valInt32n = (Int32?)value;
                if (valInt32n.HasValue)
                {
                    return valInt32n.Value.ToString(format, cultureInfo);
                }
                else
                {
                    return string.Empty;
                }
            }

            if (value.GetType() == typeof(Int64))
            {
                Int64 valInt64 = (Int64)value;
                return valInt64.ToString(format, cultureInfo);
            }

            if (value.GetType() == typeof(Int64?))
            {
                Int64? valInt64n = (Int64?)value;
                if (valInt64n.HasValue)
                {
                    return valInt64n.Value.ToString(format, cultureInfo);
                }
                else
                {
                    return string.Empty;
                }
            }

            if (value.GetType() == typeof(double))
            {
                double valdouble = (double)value;
                return valdouble.ToString(format, cultureInfo);
            }

            if (value.GetType() == typeof(double?))
            {
                double? valdoublen = (double?)value;
                if (valdoublen.HasValue)
                {
                    return valdoublen.Value.ToString(format, cultureInfo);
                }
                else
                {
                    return string.Empty;
                }
            }

            if (value.GetType() == typeof(Single))
            {
                Single valsingle = (Single)value;
                return valsingle.ToString(format, cultureInfo);
            }

            if (value.GetType() == typeof(Single?))
            {
                Single? valsinglen = (Single?)value;
                if (valsinglen.HasValue)
                {
                    return valsinglen.Value.ToString(format, cultureInfo);
                }
                else
                {
                    return string.Empty;
                }
            }

            if (value.GetType() == typeof(decimal))
            {
                decimal valdecimal = (decimal)value;
                return valdecimal.ToString(format, cultureInfo);
            }

            if (value.GetType() == typeof(decimal?))
            {
                decimal? valdecimaln = (decimal?)value;
                if (valdecimaln.HasValue)
                {
                    return valdecimaln.Value.ToString(format, cultureInfo);
                }
                else
                {
                    return string.Empty;
                }
            }

            if (value.GetType() == typeof(DateTime))
            {
                DateTime valDateTime = (DateTime)value;
                return valDateTime.ToString(format, cultureInfo);
            }
            if (value.GetType() == typeof(DateTime?))
            {
                DateTime? valDateTimen = (DateTime?)value;
                if (valDateTimen.HasValue)
                {
                    return valDateTimen.Value.ToString(format, cultureInfo);
                }
                else
                {
                    return string.Empty;
                }
            }
            return value.ToString();
        }

        public static List<string> GetDirectories(string path, bool recursive)
        {
            List<string> found = new List<string>();
            if (string.IsNullOrEmpty(path))
                return found;

            DirectoryInfo dir = new DirectoryInfo(path);
            if (!dir.Exists)
                return found;

            GetDirectories(dir, recursive, ref found);
            return found;
        }

        /// <summary>
        /// Search for files within a directory
        /// </summary>
        /// <returns></returns>
        public static List<string> GetFiles(string path, bool includesubdirs)
        {
            List<string> found = new List<string>();
            if (string.IsNullOrEmpty(path))
                return found;

            DirectoryInfo dir = new DirectoryInfo(path);
            if (!dir.Exists)
                return found;

            GetFiles(dir, "*.*", includesubdirs, ref found);
            return found;
        }

        /// <summary>
        /// Search for files within a directory
        /// </summary>
        public static List<string> GetFiles(string path, string filter, bool includesubdirs)
        {
            List<string> found = new List<string>();
            if (string.IsNullOrEmpty(path))
                return found;

            DirectoryInfo dir = new DirectoryInfo(path);
            if (!dir.Exists)
                return found;

            GetFiles(dir, filter, includesubdirs, ref found);
            return found;
        }

        public static List<Type> GetObjectsWithBaseType(Type basetype, bool skipBaseType)
        {
            List<Type> types = new List<Type>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
                foreach (Type t in assembly.GetTypes())
                    if (DerivesFromBaseType(t, basetype))
                        if (!(t == basetype && skipBaseType))
                            types.Add(t);

            return types;
        }

        public static bool HasDirectoryAccess(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            if (!IsDriveReady(di))
                return false;

            try
            {
                FileInfo[] files = di.GetFiles("*.x");
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Test whether a string is alpha
        /// </summary>
        /// <param name="s">string to test</param>
        /// <returns></returns>
        public static bool IsAlpha(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }
            Char[] chars = s.ToCharArray();
            for (int ii = 0; ii < chars.Length; ii++)
            {
                if (!Char.IsLetter(chars[ii]))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsDriveReady(DirectoryInfo directory)
        {
            if (directory.Root.FullName.StartsWith("\\\\"))
                return true; // it's a network share

            DriveInfo di = new DriveInfo(directory.Root.Name);
            return di.IsReady;
        }

        public static FileOrDirectory IsFileOrDirectory(string path)
        {
            if (string.IsNullOrEmpty(path))
                return FileOrDirectory.DoesNotExist;

            if (File.Exists(path))
                return FileOrDirectory.File;
            else if (Directory.Exists(path))
                return FileOrDirectory.Directory;

            return FileOrDirectory.DoesNotExist;
        }

        public static bool IsNumeric(string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;

            Char[] chars = s.ToCharArray();
            for (int ii = 0; ii < chars.Length; ii++)
            {
                if (!Char.IsDigit(chars[ii]))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Test whether a char is one of
        /// </summary>
        /// <param name="c"></param>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static bool IsOneOfChars(char c, params char[] chars)
        {
            if (chars == null)
                return false;

            for (int ii = 0; ii < chars.Length; ii++)
            {
                if (c == chars[ii])
                    return true;
            }
            return false;
        }

        public static bool IsOneOfStrings(string s, StringComparison comparison, params string[] compareTo)
        {
            if (compareTo == null)
            {
                return false;
            }
            if (compareTo.Length == 0)
            {
                return false;
            }
            foreach (string compare in compareTo)
            {
                if (Equals(s, compare, comparison))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsOneOfTypes(Type type, params Type[] compareTo)
        {
            if (compareTo == null)
                return false;

            foreach (Type t in compareTo)
                if (type == t)
                    return true;

            return false;
        }

        public static string JoinCsvLine(char separator, params string[] values)
        {
            if (values == null)
                return string.Empty;

            if (values.Length == 0)
                return string.Empty;

            StringBuilder sb = new StringBuilder();

            for (int ii = 0; ii < values.Length; ii++)
            {
                bool setquotes = false;
                string s = values[ii];
                if (string.IsNullOrEmpty(s))
                    s = string.Empty;

                if (s.Contains("\r") || s.Contains("\n") || s.Contains(separator.ToString()))
                    setquotes = true;

                if (s.Contains("\""))
                {
                    s = s.Replace("\"", "\"\"");
                    setquotes = true;
                }

                if (setquotes)
                    s = string.Format("\"{0}\"", s);

                sb.Append(s);
                sb.Append(separator);
            }
            sb.Length--;
            return sb.ToString();
        }

        public static string KeepQuoted(string text, char quoteChar = '"')
        {
            if (string.IsNullOrEmpty(text))
                return null;

            StringBuilder _sb = new StringBuilder();

            char[] _chars = text.ToCharArray();
            char _prevChar = (char)0;

            bool _withinQuotes = false;

            foreach (char c in _chars)
            {
                if (_withinQuotes == false && c == quoteChar && _prevChar != '\\')
                {
                    _withinQuotes = true;
                    continue;
                }
                if (_withinQuotes == true && c == quoteChar && _prevChar != '\\')
                {
                    _withinQuotes = false;
                    continue;
                }

                if (_withinQuotes)
                {
                    _sb.Append(c);
                }
                _prevChar = c;
            }

            return _sb.ToString();
        }

        /// <summary>
        /// Match a pattern like: *.txt;*.jpg;
        /// </summary>
        public static bool MatchPattern(string s, string pattern)
        {
            if (pattern == null || s == null) return false;

            string[] patterns = pattern.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string subpattern in patterns)
            {
                string pat = string.Format("^{0}$", Regex.Escape(subpattern).Replace("\\*", ".*").Replace("\\?", "."));
                Regex regex = new Regex(pat, RegexOptions.IgnoreCase);
                if (regex.IsMatch(s))
                    return true;
            }
            return false;
        }

        public static decimal Negative(decimal value) => (value > 0) ? 0 - value : value;

        public static double Negative(double value) => (value > 0) ? 0 - value : value;

        public static long Negative(long value) => (value > 0) ? 0 - value : value;

        /// <summary>
        /// Normalizes single and double quotes
        /// </summary>
        /// <param name="s">string to normalize</param>
        /// <returns>returns empty string when input is null or empty</returns>
        public static string NormalizeAllQuotes(string s)
        {
            s = NormalizeSingleQuotes(s);
            return NormalizeDoubleQuotes(s);
        }

        /// <summary>
        /// Sometimes a string contains quotes copied from a document which are different.
        /// Following qoutes “ ” „ ¨ will be replace with "
        /// </summary>
        /// <param name="s">string to normalize</param>
        /// <returns>returns empty string when input is null or empty</returns>
        public static string NormalizeDoubleQuotes(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] array = s.ToCharArray();
            for (int ii = 0; ii < array.Length; ii++)
            {
                if (IsOneOfChars(array[ii], '“', '”', '„', '¨'))
                {
                    array[ii] = '"';
                }
            }
            return new string(array);
        }

        /// <summary>
        /// Sometimes a string contains quotes copied from a document which are different.
        /// Following qoutes ‘ ’ ‚ will be replace with '
        /// </summary>
        /// <param name="s">string to normalize</param>
        /// <returns>returns empty string when input is null or empty</returns>
        public static string NormalizeSingleQuotes(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            char[] array = s.ToCharArray();
            for (int ii = 0; ii < array.Length; ii++)
                if (IsOneOfChars(array[ii], '‘', '’', '‚'))
                    array[ii] = '\'';

            return new string(array);
        }

        public static decimal Positive(decimal value) => (value < 0) ? value - (value * 2) : value;

        public static double Positive(double value) => (value < 0) ? value - (value * 2) : value;

        public static long Positive(long value) => (value < 0) ? value - (value * 2) : value;

        public static int RandomInt(int min, int max)
        {
            if (min >= max)
            {
                return 0;
            }
            System.Random rnd = new System.Random(seed);
            seed = rnd.Next();
            return rnd.Next(min, max);
        }

        /// <summary>
        /// Remove empty entries from array
        /// </summary>
        /// <param name="array">array to filter</param>
        /// <returns>returns empty array when array is null</returns>
        public static string[] RemoveEmpties(string[] array)
        {
            if (array == null)
            {
                return new string[0];
            }
            int cnt = 0;
            foreach (string s in array)
            {
                if (string.IsNullOrEmpty(s))
                {
                    cnt++; // Count empties
                }
            }
            string[] result = new string[array.Length - cnt]; // new array size minus empties
            if (result.Length == 0)
            {
                return new string[0];
            }
            cnt = 0;
            for (int ii = 0; ii < array.Length; ii++)
            {
                if (!string.IsNullOrEmpty(array[ii]))
                {
                    result[cnt] = array[ii];
                    cnt++;
                }
            }
            return result;
        }

        public static string[] Split(string s, string splitter)
        {
            if (string.IsNullOrEmpty(s))
            {
                return new string[0];
            }
            return s.Split(new string[] { splitter }, StringSplitOptions.None);
        }

        /// <summary>
        /// Split string by index to 2 parts.
        /// </summary>
        /// <param name="s">string to split</param>
        /// <param name="positions">index to split</param>
        /// <returns>always returns a string array containing 2 items, when index is invalid the first item contains the full string.</returns>
        /// <example>
        /// <code lang="cs">
        /// string s = "1|0";
        /// string s = StringUtility.Split(s, 1, 10, 10);
        ///
        /// // result { "0", "1" }
        /// </code>
        /// </example>
        public static string[] SplitByPosition(string s, params int[] positions)
        {
            string[] item = new string[positions.Length];
            for (int ii = 0; ii < item.Length; ii++)
            {
                item[ii] = string.Empty;
            }
            if (string.IsNullOrEmpty(s))
            {
                return item;
            }
            int previndex = 0;
            for (int ii = 0; ii < item.Length; ii++)
            {
                int length = positions[ii] - previndex;
                if (previndex + length > s.Length)
                {
                    length = s.Length - previndex;
                }
                item[ii] = s.Substring(previndex, length);
                if (length == s.Length - previndex)
                {
                    return item;
                }
                previndex = positions[ii];
            }
            return item;
        }

        /// <summary>
        /// Splits a string by repetative chars
        /// </summary>
        /// <example>
        /// <code lang="cs">
        /// string[] s = SplitByRepetative("hh:mm:ss,ttt");
        ///
        /// // result:
        /// // ----------------------------------
        /// // hh
        /// // :
        /// // mm
        /// // :
        /// // ss
        /// // ,
        /// // ttt
        /// // ----------------------------------
        /// </code>
        /// </example>
        public static string[] SplitByRepetative(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return new string[0];
            }
            char[] chars = s.ToCharArray();

            // first calculate number of repatative items
            char prevchar = chars[0];
            int count = 1;
            for (int ii = 0; ii < chars.Length; ii++)
            {
                if (chars[ii] != prevchar)
                {
                    count++;
                }
                prevchar = chars[ii];
            }

            // create the result array
            string[] parts = new string[count];

            // substract the parts.
            count = 0;
            prevchar = chars[0];
            for (int ii = 0; ii < chars.Length; ii++)
            {
                if (chars[ii] != prevchar)
                {
                    count++;
                }
                parts[count] = parts[count] + chars[ii].ToString();

                prevchar = chars[ii];
            }
            return parts;
        }

        /// <summary>
        /// For large documents use the CsvStreamReader
        /// </summary>
        /// <param name="s"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string[] SplitCsvLines(string s, char separator)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(s);
            List<string> lines = new List<string>();
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                // using (CsvStreamReader reader = new CsvStreamReader(filename))
                using (CsvStreamReader reader = new CsvStreamReader(stream))
                {
                    reader.Separator = separator;

                    while (!reader.EndOfCsvStream) // Do not use EndOfStream
                        lines.Add(string.Join(separator.ToString(), reader.ReadCsvLine()));

                    reader.Close();
                    stream.Close();
                }
            }
            return lines.ToArray();
        }

        /// <summary>
        /// <para>Split a string based on start and end element.</para>
        /// <para>Start and element will be checked by Hierarchy if a starter is directly followed by a starter the same ammount of enders is required in order to set the end for a split.</para>
        /// <para>Escaping can be applied ensuring splitting will be ignored on items within escaped parts.</para>
        /// </summary>
        /// <param name="s">string to split</param>
        /// <param name="splitterStart">match start item</param>
        /// <param name="splitterEnd">match end item</param>
        /// <param name="splitterEnd2">match secondary end item</param>
        /// <param name="comparison">Case comparison to use.</param>
        /// <param name="escape">Escape string, when escape is opened splitterStart/splitterEnd will be skipped, when escape is closed splitterStart/splitterEnd are applied.</param>
        /// <example>
        /// <code lang="cs">
        /// string text = "aaa[bbb[ccc]ddd]eee";
        /// string[] s = StringUtility.SplitDualEscaped(text, "[", "]", string.Empty, StringSplitOptions.None);
        /// foreach (string s2 in s)
        /// {
        ///     Console.WriteLine(s2);
        /// }
        ///
        /// // result:
        /// // ------------------
        /// // aaa
        /// // [bbb[ccc]ddd]
        /// // eee
        /// ------------------
        /// </code>
        /// </example>
        public static string[] SplitDualEscaped(string s, CaseComparison comparison, string splitterStart, string splitterEnd, string splitterEnd2, string escape)
        {
            return SplitDualEscaped(s, comparison, splitterStart, splitterEnd, splitterEnd2, escape, string.Empty, string.Empty, StringSplitOptions.None);
        }

        /// <summary>
        /// <para>Split a string based on start and end element.</para>
        /// <para>Start and element will be checked by Hierarchy if a starter is directly followed by a starter the same ammount of enders is required in order to set the end for a split.</para>
        /// <para>Escaping can be applied ensuring splitting will be ignored on items within escaped parts.</para>
        /// </summary>
        /// <param name="s">string to split</param>
        /// <param name="splitterStart">match start item</param>
        /// <param name="splitterEnd">match end item</param>
        /// <param name="splitterEnd2">match secondary end item</param>
        /// <param name="escape">Escape string, when escape is opened splitterStart/splitterEnd will be skipped, when escape is closed splitterStart/splitterEnd are applied.</param>
        /// <param name="escapeEnd">Is treated same as escape when null or empty</param>
        /// <param name="escapeEnd2">Secondary escape string</param>
        /// <example>
        /// <code lang="cs">
        /// string text = "aaa[bbb[ccc]ddd]eee";
        /// string[] s = StringUtility.SplitDualEscaped(text, "[", "]", string.Empty, StringSplitOptions.None);
        /// foreach (string s2 in s)
        /// {
        ///     Console.WriteLine(s2);
        /// }
        ///
        /// // result:
        /// // ------------------
        /// // aaa
        /// // [bbb[ccc]ddd]
        /// // eee
        /// // ------------------
        /// </code>
        /// </example>
        public static string[] SplitDualEscaped(string s, string splitterStart, string splitterEnd, string splitterEnd2, string escape, string escapeEnd, string escapeEnd2)
        {
            return SplitDualEscaped(s, CaseComparison.Ordinal, splitterStart, splitterEnd, splitterEnd2, escape, escapeEnd, escapeEnd2, StringSplitOptions.None);
        }

        /// <summary>
        /// <para>Split a string based on start and end element.</para>
        /// <para>Start and element will be checked by Hierarchy if a starter is directly followed by a starter the same ammount of enders is required in order to set the end for a split.</para>
        /// <para>Escaping can be applied ensuring splitting will be ignored on items within escaped parts.</para>
        /// </summary>
        /// <param name="s">string to split</param>
        /// <param name="comparison">determine whether to ignore case</param>
        /// <param name="splitterStart">match start item</param>
        /// <param name="splitterEnd">match end item</param>
        /// <param name="splitterEnd2">match secondary end item</param>
        /// <param name="escape">Escape string, when escape is opened splitterStart/splitterEnd will be skipped, when escape is closed splitterStart/splitterEnd are applied.</param>
        /// <param name="escapeEnd">Is treated same as escape when null or empty</param>
        /// <param name="escapeEnd2">Secondary escape string</param>
        /// <param name="splitoptions">indicate to remove empty entries</param>
        /// <example>
        /// <code lang="cs">
        /// string text = "aaa[bbb[ccc=\"[\"]ddd]eee";
        /// string[] s = StringUtility.SplitDualEscaped(text, "[", "]", "\"", string.Empty, false, StringSplitOptions.None);
        /// foreach (string s2 in s)
        /// {
        ///     Console.WriteLine(s2);
        /// }
        ///
        /// // result:
        /// // ------------------
        /// // aaa
        /// // [bbb[ccc="["]ddd]
        /// // eee
        /// // ------------------
        /// </code>
        /// </example>
        public static string[] SplitDualEscaped(string s, CaseComparison comparison, string splitterStart, string splitterEnd, string splitterEnd2, string escape, string escapeEnd, string escapeEnd2, StringSplitOptions splitoptions)
        {
            if (string.IsNullOrEmpty(s))
            {
                return new string[0];
            }
            if (string.IsNullOrEmpty(splitterStart))
            {
                return new string[1] { s };
            }

            if (string.IsNullOrEmpty(splitterEnd))
            {
                throw new ArgumentException("splitterEnd is not allowed to be empty or null");
            }

            if (splitterStart.Equals(splitterEnd, StringComparison.Ordinal))
            {
                throw new ArgumentException("splitterStart and splitterEnd are not allowed to be equal");
            }
            if (splitterStart.Equals(splitterEnd2, StringComparison.Ordinal))
            {
                throw new ArgumentException("splitterStart and splitterEnd are not allowed to be equal");
            }

            if (string.IsNullOrEmpty(splitterEnd2))
            {
                splitterEnd2 = string.Empty;
            }
            if (string.IsNullOrEmpty(escape))
            {
                escape = string.Empty;
            }
            if (string.IsNullOrEmpty(escapeEnd))
            {
                escapeEnd = escape;
            }
            if (string.IsNullOrEmpty(escapeEnd2))
            {
                escapeEnd2 = escapeEnd;
            }

            char[] chtextOriginal = s.ToCharArray();
            char[] chtext;
            char[] chstart;
            char[] chend;
            char[] chend2;
            char[] chescape;
            char[] chescapeEnd;
            char[] chescapeEnd2;

            chtext = s.ToCharArray();
            chstart = splitterStart.ToCharArray();
            chend = splitterEnd.ToCharArray();
            chend2 = splitterEnd2.ToCharArray();
            chescape = escape.ToCharArray();
            chescapeEnd = escapeEnd.ToCharArray();
            chescapeEnd2 = escapeEnd2.ToCharArray();

            HashSet<int> positions = new HashSet<int>();
            int start = 0;
            int level = 0;
            bool escapeOpened = false;

            for (int ii = 0; ii < chtext.Length; ii++)
            {
                if (ArrayEndsWith(chtext, chescape, ii, comparison) && escapeOpened == false)
                {
                    escapeOpened = true;
                }
                else
                {
                    if ((ArrayEndsWith(chtext, chescapeEnd, ii, comparison) || ArrayEndsWith(chtext, chescapeEnd2, ii, comparison)) && escapeOpened == true)
                    {
                        escapeOpened = false;
                    }
                }
                if (escapeOpened == false)
                {
                    // look for starters
                    if (ArrayEndsWith(chtext, chstart, ii, comparison))
                    {
                        if (level == 0)
                        {
                            start = ii - chstart.Length + 1;
                            if (start > 0)
                            {
                                positions.Add(start);
                            }
                        }
                        level++; // starter found, ignore starters until new ender is found.
                    }
                    if (level > 0)
                    {
                        // look for enders
                        if (ArrayEndsWith(chtext, chend, ii, comparison))
                        {
                            level--; // ender found, ignore enders until new starter is found
                            if (level == 0)
                            {
                                start = ii + chend.Length;
                                if (chend.Length > 1)
                                {
                                    start--;
                                }
                                positions.Add(start);
                            }
                        }
                        else if (ArrayEndsWith(chtext, chend2, ii, comparison))
                        {
                            level--; // ender found, ignore enders until new starter is found
                            if (level == 0)
                            {
                                start = ii + chend2.Length;
                                if (chend2.Length > 1)
                                {
                                    start--;
                                }
                                positions.Add(start);
                            }
                        }
                    }
                }
            }
            positions.Add(chtext.Length);

            string[] result = new string[positions.Count];
            int cnt = 0;
            int prevpos = 0;
            foreach (int pos in positions)
            {
                result[cnt] = s.Substring(prevpos, pos - prevpos);
                prevpos = pos;
                cnt++;
            }
            if (splitoptions == StringSplitOptions.RemoveEmptyEntries)
            {
                return RemoveEmpties(result);
            }
            return result;
        }

        /// <summary>
        /// Splits a string based on a separator but only when escapechar is closed
        /// </summary>
        /// <example>
        /// <code lang="cs">
        /// string text = "AAAA,'this is a split test, comma between escapes should be ignored',BBBB";
        /// string[] items = StringUtility.SplitEscaped(text, ',', '\'');
        /// foreach (string item in items)
        /// {
        ///     Console.WriteLine(item);
        /// }
        ///
        /// // result:
        /// // --------------------
        /// // AAAA
        /// // 'this is a split test, comma between escapes should be ignored'
        /// // BBBB
        /// // --------------------
        /// </code>
        /// </example>
        /// <param name="s">string to split</param>
        /// <param name="separator">separator to use</param>
        /// <param name="escape">escape char for ignore splitting between</param>
        /// <returns></returns>
        public static string[] SplitEscaped(string s, char separator, char escape)
        {
            if (string.IsNullOrEmpty(s))
            {
                return new string[0];
            }
            if (!s.Contains(separator.ToString()))
            {
                // does not contain separator.
                return new string[] { s };
            }
            if (!s.Contains(escape.ToString()))
            {
                // normal split, no need for escaping.
                return s.Split(new char[] { separator });
            }
            List<string> builder = new List<string>();
            StringBuilder sb = new StringBuilder();
            char[] templ = s.ToCharArray();
            char charCurrent = ' ';
            char charPrev = ' ';
            bool escapeMode = false;
            bool skip = false;

            for (int ii = 0; ii < templ.Length; ii++)
            {
                skip = false;
                charCurrent = templ[ii];

                if (escapeMode == false)
                {
                    if (charCurrent == escape)
                    {
                        if (charPrev != '\\')
                        {
                            escapeMode = true;
                        }
                    }
                }
                else
                {
                    if (charCurrent == escape)
                    {
                        if (charPrev != '\\')
                        {
                            escapeMode = false;
                        }
                    }
                }
                if (escapeMode == false)
                {
                    if (charCurrent == separator)
                    {
                        builder.Add(sb.ToString());
                        sb = new StringBuilder();
                        skip = true;
                    }
                }
                if (charCurrent == '\\' && templ[Bound(ii + 1, 0, templ.Length - 1)] == escape)
                {
                    charPrev = charCurrent;
                    skip = true;
                }

                if (!skip)
                {
                    charPrev = charCurrent;
                    sb.Append(charCurrent);
                }
            }
            builder.Add(sb.ToString());
            return builder.ToArray();
        }

        /// <summary>
        /// Split string with escaping
        /// </summary>
        /// <param name="s">string to split</param>
        /// <param name="splitter">splitter to use</param>
        /// <param name="escape">escape starter</param>
        /// <example>
        /// <code lang="cs">
        /// string s = "aaaa||bbbb'cc||dd'ee||ee";
        /// string[] items = StringUtility.SplitEscaped(s, "||", "'", false);
        /// foreach (string item in items)
        /// {
        ///     Console.WriteLine(item);
        /// }
        /// </code>
        /// </example>
        /// <returns></returns>
        public static string[] SplitEscaped(string s, string splitter, string escape)
        {
            return SplitEscaped(s, splitter, escape, string.Empty, string.Empty, StringSplitOptions.None);
        }

        /// <summary>
        /// Split string with escaping
        /// </summary>
        /// <param name="s">string to split</param>
        /// <param name="separator">splitter to use</param>
        /// <param name="escape">escape starter</param>
        /// <param name="escapeEnd">escape ender</param>
        /// <example>
        /// <code lang="cs">
        /// string s = "aaaa||bbbb'cc||dd'ee||ee";
        /// string[] items = StringUtility.SplitEscaped(s, "||", "'", false, string.Empty);
        /// foreach (string item in items)
        /// {
        ///     Console.WriteLine(item);
        /// }
        /// </code>
        /// </example>
        /// <returns></returns>
        public static string[] SplitEscaped(string s, string separator, string escape, string escapeEnd)
        {
            return SplitEscaped(s, separator, escape, escapeEnd, string.Empty, StringSplitOptions.None);
        }

        /// <summary>
        /// Split string with escaping
        /// </summary>
        /// <param name="s">string to split</param>
        /// <param name="splitter">splitter to use</param>
        /// <param name="escape">escape starter</param>
        /// <param name="escapeEnd">escape ender</param>
        /// <param name="escapeEnd2">secondary escape ender</param>
        /// <example>
        /// <code lang="cs">
        /// string s = "aaaa||bbbb'cc||dd'ee||ee";
        /// string[] items = StringUtility.SplitEscaped(s, "||", "'", false, string.Empty, string.Empty);
        /// foreach (string item in items)
        /// {
        ///     Console.WriteLine(item);
        /// }
        /// </code>
        /// </example>
        /// <returns></returns>
        public static string[] SplitEscaped(string s, string splitter, string escape, string escapeEnd, string escapeEnd2)
        {
            return SplitEscaped(s, splitter, escape, escapeEnd, escapeEnd2, StringSplitOptions.None);
        }

        /// <summary>
        /// Split string with escaping
        /// </summary>
        /// <param name="s">string to split</param>
        /// <param name="splitter">splitter to use</param>
        /// <param name="escape">escape starter</param>
        /// <param name="escapeEnd">escape ender</param>
        /// <param name="escapeEnd2">secondary escape ender</param>
        /// <param name="splitoptions">keep or remove empty entries</param>
        /// <example>
        /// <code lang="cs">
        /// string s = "aaaa||bbbb'cc||dd'ee||ee";
        /// string[] items = StringUtility.SplitEscaped(s, "||", "'", false, string.Empty, string.Empty, StringSplitOptions.None);
        /// foreach (string item in items)
        /// {
        ///     Console.WriteLine(item);
        /// }
        /// </code>
        /// </example>
        /// <returns></returns>
        public static string[] SplitEscaped(string s, string splitter, string escape, string escapeEnd, string escapeEnd2, StringSplitOptions splitoptions)
        {
            if (string.IsNullOrEmpty(s))
            {
                return new string[0]; // there's nothing to split
            }
            if (string.IsNullOrEmpty(splitter))
            {
                return new string[1] { s }; // there's nothing to split
            }
            if (string.IsNullOrEmpty(escape))
            {
                escape = string.Empty;
            }
            if (string.IsNullOrEmpty(escapeEnd))
            {
                escapeEnd = escape; // use escape for end
            }
            if (string.IsNullOrEmpty(escapeEnd2))
            {
                escapeEnd2 = escapeEnd; // use escapeEnd for secondary escape.
            }

            char[] chtextOriginal = s.ToCharArray();
            char[] chtext = s.ToCharArray();
            char[] chsplitter = splitter.ToCharArray();
            char[] chescape = escape.ToCharArray();
            char[] chescapeEnd = escapeEnd.ToCharArray();
            char[] chescapeEnd2 = escapeEnd2.ToCharArray();

            HashSet<int> positions = new HashSet<int>();
            int start = 0;

            bool escapeOpened = false;

            for (int ii = 0; ii < chtext.Length; ii++)
            {
                if (ArrayEndsWith(chtext, chescape, ii) && escapeOpened == false)
                {
                    escapeOpened = true;
                }
                else
                {
                    if ((ArrayEndsWith(chtext, chescapeEnd, ii) || ArrayEndsWith(chtext, chescapeEnd2, ii)) && escapeOpened == true)
                    {
                        escapeOpened = false;
                    }
                }
                if (escapeOpened == false)
                {
                    // look for starters
                    if (ArrayEndsWith(chtext, chsplitter, ii))
                    {
                        start = ii - chsplitter.Length + 1;
                        positions.Add(start);
                    }
                }
            }
            positions.Add(chtext.Length);

            string[] result = new string[positions.Count];
            int cnt = 0;
            int prevpos = 0;
            foreach (int pos in positions)
            {
                if (cnt == 0)
                {
                    result[cnt] = s.Substring(prevpos, pos - prevpos);
                    prevpos = pos;
                }
                else
                {
                    result[cnt] = s.Substring(prevpos + chsplitter.Length, pos - prevpos - chsplitter.Length);
                    prevpos = pos;
                }
                cnt++;
            }
            if (splitoptions == StringSplitOptions.RemoveEmptyEntries)
            {
                return RemoveEmpties(result);
            }
            return result;
        }

        /// <summary>
        /// Split the template into parts separating formating from normal text.
        /// "aaa{0}bbb"
        /// will be:
        /// "aaa", "{0}", "bbb"
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public static string[] SplitTemplate(string template)
        {
            List<string> builders = new List<string>();
            StringBuilder sb = new StringBuilder();
            char[] templ = template.ToCharArray();
            char charCurrent = ' ';
            char charPrev = ' ';
            char charNext = ' ';
            bool readPlaceHolder = false;
            bool startNew = false;

            for (int ii = 0; ii < templ.Length; ii++)
            {
                charCurrent = templ[ii];
                if (ii < templ.Length - 1)
                {
                    charNext = templ[ii + 1];
                }

                if (readPlaceHolder)
                {
                    if (charCurrent == '}')
                    {
                        if (charNext != '}')
                        {
                            readPlaceHolder = false;
                            startNew = true;
                        }
                    }
                }

                if (charCurrent == '{')
                {
                    if (charPrev != '{')
                    {
                        readPlaceHolder = true;
                        builders.Add(sb.ToString());
                        sb = new StringBuilder();
                    }
                }
                charPrev = charCurrent;
                sb.Append(charCurrent);
                if (startNew)
                {
                    startNew = false;
                    string item = sb.ToString();
                    builders.Add(item);
                    sb = new StringBuilder();
                }
            }
            builders.Add(sb.ToString()); // add the last.

            return builders.ToArray();
        }

        /// <summary>
        /// Returns Last Split value item
        /// </summary>
        /// <param name="s">string to split</param>
        /// <param name="splitter">splitter to use</param>
        /// <returns></returns>
        public static string SplitValueLast(string s, string splitter)
        {
            string[] items = s.Split(new string[] { splitter }, StringSplitOptions.None);
            if (items.Length == 0)
            {
                return string.Empty;
            }
            return items[items.Length - 1];
        }

        public static bool StartsWithEndsWith(string value, string start, string end) => (value.StartsWith(start, StringComparison.Ordinal)) && (value.EndsWith(end, StringComparison.Ordinal));

        public static string TrimByLength(string value, int length) => TrimStartByLength(TrimEndByLength(value, length), length);

        public static string TrimEndByLength(string value, int length) => (length >= value.Length) ? "" : value.Substring(0, value.Length - length);

        public static string TrimStartByLength(string value, int length) => (length > value.Length) ? "" : value.Substring(length);

        public static string Truncate(string s, int length) => (string.IsNullOrEmpty(s)) ? string.Empty : (s.Length < length) ? s : s.Substring(0, length);

        private static void GetDirectories(DirectoryInfo directory, bool recursive, ref List<string> found)
        {
            if (HasDirectoryAccess(directory.FullName))
            {
                foreach (DirectoryInfo fi in directory.GetDirectories())
                    found.Add(fi.FullName);

                if (recursive)
                {
                    foreach (DirectoryInfo dir in directory.GetDirectories())
                        GetDirectories(dir, recursive, ref found);
                }
            }
        }

        private static void GetFiles(DirectoryInfo directory, string filter, bool includesubdirs, ref List<string> found)
        {
            if (HasDirectoryAccess(directory.FullName))
            {
                foreach (FileInfo fi in directory.GetFiles())
                {
                    if (MatchPattern(fi.Name, filter))
                        found.Add(fi.FullName);
                }

                if (includesubdirs)
                {
                    foreach (DirectoryInfo dir in directory.GetDirectories())
                        GetFiles(dir, filter, includesubdirs, ref found);
                }
            }
        }
    }
}