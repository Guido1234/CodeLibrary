namespace GK.Template
{
    /// <summary>
    /// Extenders to bool type
    ///
    /// String.Format(“{0:yes;;no}”, value)
    ///
    ///
    /// </summary>
    public static class BooleanUtility
    {
        /// <summary>
        /// test whether all bools are false
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool AllFalse(params bool[] b)
        {
            foreach (bool item in b)
            {
                if (item == true)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// test whether all bools are true
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool AllTrue(params bool[] b)
        {
            foreach (bool item in b)
            {
                if (item == false)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Formats a boolean with a format string
        /// use "truepart|falsepart"
        /// this method is also an extension for bool.
        /// </summary>
        /// <param name="b">boolean to format</param>
        /// <param name="format">format to use: truepart|falsepart</param>
        /// <returns></returns>
        /// <example>
        /// <code lang="cs">
        /// bool b = true;
        /// string s = bool.Format(b, "yes|no");
        /// </code>
        /// </example>
        public static string Format(bool b, string format)
        {
            if (string.IsNullOrEmpty(format))
                return IIF(b, "1", "0");

            int index = 0;
            if (b == false)
                index = 1;

            string[] items = format.Split(new char[] { '|' });
            return Utils.ArrayValue(items, index);
        }

        /// <summary>
        /// Formats a nullable boolean with a format string
        /// use "truepart|falsepart|nullpart"
        /// </summary>
        /// <param name="b">boolean to format</param>
        /// <param name="format">format to use: truepart|falsepart|nullpart</param>
        /// <returns></returns>
        /// <example>
        /// <code lang="cs">
        /// bool b = true;
        /// string s = bool.Format(b, "yes|no|unknown");
        /// </code>
        /// </example>
        public static string Format(bool? b, string format)
        {
            if (string.IsNullOrEmpty(format))
                return IIF(b, "1", "0", "0");

            int index = 0;
            if (b == null)
                index = 2;

            if (b == false)
                index = 1;

            string[] items = format.Split(new char[] { '|' });
            return Utils.ArrayValue(items, index);
        }

        /// <summary>
        /// Returns truepart string on true otherwise falsepart.
        /// </summary>
        /// <param name="b">boolean</param>
        /// <param name="truepart">string to return on truepart</param>
        /// <param name="falsepart">string to return on falsepart</param>
        /// <returns></returns>
        public static string IIF(bool b, string truepart, string falsepart)
        {
            if (b)
                return truepart;

            return falsepart;
        }

        /// <summary>
        /// Returns truepart string on true otherwise falsepart.
        /// </summary>
        /// <param name="b">boolean</param>
        /// <param name="truepart">string to return on truepart</param>
        /// <param name="falsepart">string to return on falsepart</param>
        /// <param name="nullpart">string to return on nullpart</param>
        /// <returns></returns>
        public static string IIF(bool? b, string truepart, string falsepart, string nullpart)
        {
            if (b == null)
                return nullpart;

            if ((bool)b)
                return truepart;

            return falsepart;
        }

        /// <summary>
        /// Returns truepart string on true otherwise falsepart.
        /// </summary>
        /// <param name="b">boolean</param>
        /// <param name="truepart">object to return on truepart</param>
        /// <param name="falsepart">object to return on falsepart</param>
        /// <returns></returns>
        public static object IIF(bool b, object truepart, object falsepart)
        {
            if (b)
                return truepart;

            return falsepart;
        }

        /// <summary>
        /// Returns truepart string on true otherwise falsepart.
        /// </summary>
        /// <param name="b">boolean</param>
        /// <param name="truepart">object to return on truepart</param>
        /// <param name="falsepart">object to return on falsepart</param>
        /// <param name="nullpart">object to return on nullpart</param>
        /// <returns></returns>
        public static object IIF(bool? b, object truepart, object falsepart, object nullpart)
        {
            if (b == null)
                return nullpart;

            if ((bool)b)
                return truepart;

            return falsepart;
        }

        /// <summary>
        /// returns true when one of compareTo matches o
        /// </summary>
        /// <param name="o"></param>
        /// <param name="compareTo"></param>
        /// <returns></returns>
        public static bool IsOneOf(object o, params object[] compareTo)
        {
            foreach (object to in compareTo)
            {
                if (o == to)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Converts true/false yes/no on/off 1/0 to a boolean
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool ParseBool(string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;

            s = s.Trim();
            if (s.ToUpper().Equals("TRUE", System.StringComparison.Ordinal))
                return true;

            if (s.ToUpper().Equals("FALSE", System.StringComparison.Ordinal))
                return false;

            if (s.ToUpper().Equals("YES", System.StringComparison.Ordinal))
                return true;

            if (s.ToUpper().Equals("NO", System.StringComparison.Ordinal))
                return false;

            if (s.ToUpper().Equals("ON", System.StringComparison.Ordinal))
                return true;

            if (s.ToUpper().Equals("OFF", System.StringComparison.Ordinal))
                return false;

            if (s.Equals("1", System.StringComparison.Ordinal))
                return true;

            if (s.Equals("0", System.StringComparison.Ordinal))
                return false;

            return false;
        }
    }
}