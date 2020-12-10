using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GK.Template
{
    public static class EnumUtility
    {
        /// <summary>
        /// Add a value to a bitwise enum
        /// </summary>
        /// <typeparam name="T">Typed value</typeparam>
        /// <param name="type">Enum type to alter</param>
        /// <param name="value">value to add</param>
        /// <returns></returns>
        public static T Add<T>(System.Enum type, T value)
        {
            try
            {
                return (T)(object)(((int)(object)type | (int)(object)value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(
                    string.Format(
                        "Could not append value from enumerated type '{0}'.",
                        typeof(T).Name
                        ), ex);
            }
        }

        /// <summary>
        /// Retrieve Category Attribute from enum value
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns>"Default" when enum value does not have a category.</returns>
        public static string GetCategory(Enum enumValue)
        {
            Type type = enumValue.GetType();
            MemberInfo[] memInfo = type.GetMember(enumValue.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                CategoryAttribute _CategoryAttribute = memInfo[0].GetCustomAttributes(typeof(CategoryAttribute), false).FirstOrDefault() as CategoryAttribute;
                if (_CategoryAttribute != null)
                    return _CategoryAttribute.Category;
            }
            return string.Empty;
        }

        public static string GetDescription(Enum enumValue)
        {
            if (enumValue == null)
                return string.Empty;

            Type type = enumValue.GetType();
            MemberInfo[] memInfo = type.GetMember(enumValue.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                DescriptionAttribute _DescriptionAttribute = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
                if (_DescriptionAttribute != null)
                    return _DescriptionAttribute.Description;
            }
            return string.Empty;
        }

        public static string[] GetDescriptions(Type enumType)
        {
            if (enumType == null)
                return new string[0];

            Array values = Enum.GetValues(enumType);
            string[] ret = new string[values.Length];
            for (int ii = 0; ii < values.Length; ii++)
                ret[ii] = GetDescription((Enum)Convert.ChangeType(values.GetValue(ii), enumType));

            return ret;
        }

        public static string[] GetDescriptionsByCategory(Type enumType, string filterCategory)
        {
            if (enumType == null)
                return new string[0];

            Array values = Enum.GetValues(enumType);

            string cat = string.Empty;
            int count = 0;
            for (int ii = 0; ii < values.Length; ii++)
            {
                cat = GetCategory((Enum)System.Convert.ChangeType(values.GetValue(ii), enumType));
                if (cat.Equals(filterCategory, StringComparison.OrdinalIgnoreCase))
                    count++;
            }

            string[] ret = new string[count];
            count = 0;
            for (int ii = 0; ii < values.Length; ii++)
            {
                cat = GetCategory((Enum)System.Convert.ChangeType(values.GetValue(ii), enumType));
                if (cat.Equals(filterCategory, StringComparison.OrdinalIgnoreCase))
                {
                    ret[count] = GetDescription((Enum)System.Convert.ChangeType(values.GetValue(ii), enumType));
                    count++;
                }
            }
            return ret;
        }

        public static string GetDisplayName(Enum enumValue)
        {
            if (enumValue == null)
                return string.Empty;

            Type type = enumValue.GetType();
            MemberInfo[] memInfo = type.GetMember(enumValue.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                DisplayNameAttribute _DisplayNameAttribute = memInfo[0].GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault() as DisplayNameAttribute;
                if (_DisplayNameAttribute != null)
                    return _DisplayNameAttribute.DisplayName;
            }
            return enumValue.ToString();
        }

        public static string[] GetDisplayNames(Type enumType)
        {
            if (enumType == null)
                return new string[0];

            Array values = Enum.GetValues(enumType);
            string[] ret = new string[values.Length];
            for (int ii = 0; ii < values.Length; ii++)
                ret[ii] = GetDisplayName((Enum)System.Convert.ChangeType(values.GetValue(ii), enumType));

            return ret;
        }

        public static string[] GetDisplayNamesByCategory(Type enumType, string filterCategory, CultureInfo cultureInfo)
        {
            if (enumType == null)
            {
                return new string[0];
            }

            Array values = Enum.GetValues(enumType);

            string cat = string.Empty;
            int count = 0;
            for (int ii = 0; ii < values.Length; ii++)
            {
                cat = GetCategory((Enum)System.Convert.ChangeType(values.GetValue(ii), enumType));
                if (cat.Equals(filterCategory, StringComparison.OrdinalIgnoreCase))
                {
                    count++;
                }
            }

            string[] ret = new string[count];
            count = 0;
            for (int ii = 0; ii < values.Length; ii++)
            {
                cat = GetCategory((Enum)System.Convert.ChangeType(values.GetValue(ii), enumType));
                if (cat.Equals(filterCategory, StringComparison.OrdinalIgnoreCase))
                {
                    ret[count] = GetDisplayName((Enum)System.Convert.ChangeType(values.GetValue(ii), enumType));
                    count++;
                }
            }
            return ret;
        }

        public static string[] GetDisplayNamesByCategory(Type enumType, string filterCategory)
        {
            return GetDescriptionsByCategory(enumType, filterCategory);
        }

        public static int GetIntValueByName(Type enumType, string name)
        {
            string[] names = Enum.GetNames(enumType);
            string[] descriptions = GetDisplayNames(enumType);
            Array values = Enum.GetValues(enumType);

            if (string.IsNullOrEmpty(name))
                return (int)values.GetValue(0);

            // try by basic name
            for (int ii = 0; ii < values.Length; ii++)
            {
                if (names[ii].Equals(name, StringComparison.OrdinalIgnoreCase))
                    return (int)values.GetValue(ii);
            }
            // try by description attribute.
            for (int ii = 0; ii < values.Length; ii++)
            {
                if (descriptions[ii].Equals(name, StringComparison.OrdinalIgnoreCase))
                    return (int)values.GetValue(ii);
            }
            return (int)values.GetValue(0);
        }

        public static Enum GetValueByName(Type enumType, string name)
        {
            string[] names = Enum.GetNames(enumType);
            string[] descriptions = GetDisplayNames(enumType);
            Array values = Enum.GetValues(enumType);

            if (string.IsNullOrEmpty(name))
                return (Enum)values.GetValue(0);

            // try by basic name
            for (int ii = 0; ii < values.Length; ii++)
            {
                if (names[ii].Equals(name, StringComparison.OrdinalIgnoreCase))
                    return (Enum)values.GetValue(ii);
            }
            // try by description attribute.
            for (int ii = 0; ii < values.Length; ii++)
            {
                if (descriptions[ii].Equals(name, StringComparison.OrdinalIgnoreCase))
                    return (Enum)values.GetValue(ii);
            }
            return (Enum)values.GetValue(0);
        }

        public static int[] GetValues(Type enumType)
        {
            if (enumType == null)
                return new int[0];

            Array values = Enum.GetValues(enumType);
            int[] ret = new int[values.Length];
            for (int ii = 0; ii < values.Length; ii++)
                ret[ii] = (int)values.GetValue(ii);

            return ret;
        }

        public static int[] GetValuesByCategory(Type enumType, string filterCategory)
        {
            if (enumType == null)
                return new int[0];

            Array values = Enum.GetValues(enumType);

            string cat = string.Empty;
            int count = 0;
            for (int ii = 0; ii < values.Length; ii++)
            {
                cat = GetCategory((Enum)System.Convert.ChangeType(values.GetValue(ii), enumType));
                if (cat.Equals(filterCategory, StringComparison.OrdinalIgnoreCase))
                    count++;
            }

            int[] ret = new int[count];
            count = 0;
            for (int ii = 0; ii < values.Length; ii++)
            {
                cat = GetCategory((Enum)System.Convert.ChangeType(values.GetValue(ii), enumType));
                if (cat.Equals(filterCategory, StringComparison.OrdinalIgnoreCase))
                {
                    ret[count] = (int)System.Convert.ChangeType(values.GetValue(ii), enumType);
                    count++;
                }
            }
            return ret;
        }

        public static bool Has<T>(System.Enum type, T value)
        {
            try
            {
                return (((int)(object)type & (int)(object)value) == (int)(object)value);
            }
            catch
            {
                return false;
            }
        }

        public static bool Is<T>(System.Enum type, T value)
        {
            try
            {
                return (int)(object)type == (int)(object)value;
            }
            catch
            {
                return false;
            }
        }

        public static string JoinValues(string separator, params Enum[] values)
        {
            if (string.IsNullOrEmpty(separator))
                separator = string.Empty;

            StringBuilder sb = new StringBuilder();
            foreach (Enum o in values)
                sb.AppendFormat("{0}{1}", System.Convert.ToInt32(o), separator);

            return Utils.TrimEndByLength(sb.ToString(), separator.Length);
        }

        public static T Remove<T>(System.Enum type, T value)
        {
            try
            {
                return (T)(object)(((int)(object)type & ~(int)(object)value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(
                    string.Format(
                        "Could not remove value from enumerated type '{0}'.",
                        typeof(T).Name
                        ), ex);
            }
        }
    }
}