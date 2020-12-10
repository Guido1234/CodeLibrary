using System;
using System.Globalization;

namespace GK.Template
{
    /// <summary>
    /// Type converter class library
    /// </summary>
    public static class ConvertUtility
    {
        /// <summary>
        /// When value equals null it will return defaultValue otherwise the value
        /// </summary>
        /// <param name="value">value to ensure</param>
        /// <param name="defaultValue">default value to return</param>
        /// <returns></returns>
        public static object HandleNull(object value, object defaultValue)
        {
            if (value == null)
                return defaultValue;

            if (value == DBNull.Value)
                return defaultValue;

            return value;
        }

        /// <summary>
        /// Test whether nullable byte is null or 0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrZero(byte? value)
        {
            if (value.HasValue)
            {
                if (value.Value == 0)
                    return true; // value is 0

                return false; // value above 0
            }
            return true; // is null
        }

        /// <summary>
        /// Test whether nullable float is null or 0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrZero(float? value)
        {
            if (value.HasValue)
            {
                if (value.Value == 0)
                    return true; // value is 0

                return false; // value above 0
            }
            return true; // is null
        }

        /// <summary>
        /// Test whether nullable long is null or 0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrZero(long? value)
        {
            if (value.HasValue)
            {
                if (value.Value == 0)
                    return true; // value is 0

                return false; // value above 0
            }
            return true; // is null
        }

        /// <summary>
        /// Test whether nullable int is null or 0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrZero(int? value)
        {
            if (value.HasValue)
            {
                if (value.Value == 0)
                    return true; // value is 0

                return false; // value above 0
            }
            return true; // is null
        }

        /// <summary>
        /// Test whether nullable double is null or 0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrZero(double? value)
        {
            if (value.HasValue)
            {
                if (value.Value == 0)
                    return true; // value is 0

                return false; // value above 0
            }
            return true; // is null
        }

        /// <summary>
        /// Test whether nullable decimal is null or 0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrZero(decimal? value)
        {
            if (value.HasValue)
            {
                if (value.Value == 0)
                    return true; // value is 0

                return false; // value above 0
            }
            return true; // is null
        }

        /// <summary>
        /// Try to convert value to Boolean (bool)
        /// </summary>
        /// <param name="value">value to convert to Boolean (bool)</param>
        /// <param name="defaultValue">default value when not converted.</param>
        /// <returns></returns>
        public static Boolean ToBoolean(object value, Boolean defaultValue)
        {
            return BooleanUtility.ParseBool(HandleNull(value, defaultValue).ToString());
        }

        /// <summary>
        /// Try to convert value to Boolean (bool)
        /// </summary>
        /// <param name="value">value to convert to Boolean (bool)</param>
        /// <returns></returns>
        public static Boolean? ToBooleanNullable(object value)
        {
            if (value == null)
                return null;

            if (value == DBNull.Value)
                return null;

            return BooleanUtility.ParseBool(HandleNull(value, string.Empty).ToString());
        }

        /// <summary>
        /// Try to convert value to Byte (byte)
        /// </summary>
        /// <param name="value">value to convert to Byte (byte)</param>
        /// <param name="defaultValue">default value when not converted.</param>
        /// <returns></returns>
        public static Byte ToByte(object value, Byte defaultValue)
        {
            if (value == null)
                return defaultValue;

            if (value == DBNull.Value)
                return defaultValue;

            Byte _value = 0;
            try
            {
                return (Byte)value;
            }
            catch
            {
                bool succes = Byte.TryParse(value.ToString(), out _value);
                if (succes == false)
                    return defaultValue;
            }
            return _value;
        }

        /// <summary>
        /// Try to convert value to Byte (byte)
        /// </summary>
        /// <param name="value">value to convert to Byte (byte)</param>
        /// <returns></returns>
        public static Byte? ToByteNullable(object value)
        {
            if (value == null)
                return null;

            if (value == DBNull.Value)
                return null;

            try
            {
                // try normal pars
                return (Byte?)value;
            }
            catch
            {
                // try converting to string
                byte _value;
                bool succes = Byte.TryParse(HandleNull(value, string.Empty).ToString(), out _value);
                if (succes)
                    return _value;
            }
            return null;
        }

        /// <summary>
        /// Try to convert value to DateTime (DateTime)
        /// </summary>
        /// <param name="value">value to convert to DateTime (DateTime)</param>
        /// <param name="defaultValue">default value when not converted.</param>
        /// <returns></returns>
        public static DateTime ToDateTime(object value, DateTime defaultValue)
        {
            if (value == null)
                return defaultValue;

            if (value == DBNull.Value)
                return defaultValue;

            DateTime _value = DateTime.MinValue;
            try
            {
                return (DateTime)value;
            }
            catch
            {
                bool succes = DateTime.TryParse(value.ToString(), out _value);
                if (succes == false)
                    return defaultValue;
            }
            return _value;
        }

        /// <summary>
        /// Try to convert value to DateTime (DateTime)
        /// </summary>
        /// <param name="value">value to convert to DateTime (DateTime)</param>
        /// <returns></returns>
        public static DateTime? ToDateTimeNullable(object value)
        {
            if (value == null)
                return null;

            if (value == DBNull.Value)
                return null;

            DateTime _value = DateTime.MinValue;
            try
            {
                return (DateTime?)value;
            }
            catch
            {
                bool succes = DateTime.TryParse(value.ToString(), out _value);
                if (succes == false)
                    return null;
            }
            return _value;
        }

        /// <summary>
        /// Try to convert value to Decimal (decimal)
        /// </summary>
        /// <param name="value">value to convert to Decimal (decimal)</param>
        /// <param name="defaultValue">default value when not converted.</param>
        /// <returns></returns>
        public static Decimal ToDecimal(object value, Decimal defaultValue)
        {
            if (value == null)
                return defaultValue;

            if (value == DBNull.Value)
                return defaultValue;

            Decimal _value = 0;
            try
            {
                return (Decimal)value;
            }
            catch
            {
                bool succes = Decimal.TryParse(value.ToString(), out _value);
                if (succes == false)
                    return defaultValue;
            }
            return _value;
        }

        /// <summary>
        /// Try to convert value to Decimal (decimal)
        /// </summary>
        /// <param name="value">value to convert to Byte (byte)</param>
        /// <returns></returns>
        public static Decimal? ToDecimalNullable(object value)
        {
            if (value == null)
                return null;

            if (value == DBNull.Value)
                return null;

            try
            {
                // try normal pars
                return (Decimal?)value;
            }
            catch
            {
                // try converting to string
                Decimal _value;
                bool succes = Decimal.TryParse(HandleNull(value, string.Empty).ToString(), out _value);
                if (succes)
                    return _value;
            }
            return null;
        }

        /// <summary>
        /// Try to convert value to Double (double)
        /// </summary>
        /// <param name="value">value to convert to Double (double)</param>
        /// <param name="defaultValue">default value when not converted.</param>
        /// <returns></returns>
        public static Double ToDouble(object value, Double defaultValue)
        {
            if (value == null)
                return defaultValue;

            if (value == DBNull.Value)
                return defaultValue;

            Double _value = 0;
            try
            {
                return (Double)value;
            }
            catch
            {
                bool succes = Double.TryParse(value.ToString(), out _value);
                if (succes == false)
                    return defaultValue;
            }
            return _value;
        }

        /// <summary>
        /// Try to convert value to Double (double)
        /// </summary>
        /// <param name="value">value to convert to Byte (byte)</param>
        /// <returns></returns>
        public static Double? ToDoubleNullable(object value)
        {
            if (value == null)
                return null;

            if (value == DBNull.Value)
                return null;

            try
            {
                // try normal pars
                return (Double?)value;
            }
            catch
            {
                // try converting to string
                Double _value;
                bool succes = Double.TryParse(HandleNull(value, string.Empty).ToString(), out _value);
                if (succes)
                    return _value;
            }
            return null;
        }

        /// <summary>
        /// Try to convert value to Guid (Guid)
        /// </summary>
        /// <param name="value">value to convert to Guid (Guid)</param>
        /// <param name="defaultValue">default value when not converted.</param>
        /// <returns></returns>
        public static Guid ToGuid(object value, Guid defaultValue)
        {
            if (value == null)
                return defaultValue;

            if (value == DBNull.Value)
                return defaultValue;

            return new Guid(ToString(value, string.Empty));
        }

        /// <summary>
        /// Try to convert value to Guid (Guid)
        /// </summary>
        /// <param name="value">value to convert to Guid (Guid)</param>
        /// <returns></returns>
        public static Guid? ToGuidNullable(object value)
        {
            if (value == null)
                return null;

            if (value == DBNull.Value)
                return null;

            return new Guid(ToString(value, string.Empty));
        }

        /// <summary>
        /// Try to convert value to Int16 (short)
        /// </summary>
        /// <param name="value">value to convert to Int16 (short)</param>
        /// <param name="defaultValue">default value when not converted.</param>
        /// <returns></returns>
        public static Int16 ToInt16(object value, Int16 defaultValue)
        {
            if (value == null)
                return defaultValue;

            if (value == DBNull.Value)
                return defaultValue;

            Int16 _value = 0;
            try
            {
                return (Int16)value;
            }
            catch
            {
                bool succes = Int16.TryParse(value.ToString(), out _value);
                if (succes == false)
                    return defaultValue;
            }
            return _value;
        }

        /// <summary>
        /// Try to convert value to Int16 (short)
        /// </summary>
        /// <param name="value">value to convert to Byte (byte)</param>
        /// <returns></returns>
        public static Int16? ToInt16Nullable(object value)
        {
            if (value == null)
                return null;

            if (value == DBNull.Value)
                return null;

            try
            {
                // try normal pars
                return (Int16?)value;
            }
            catch
            {
                // try converting to string
                Int16 _value;
                bool succes = Int16.TryParse(HandleNull(value, string.Empty).ToString(), out _value);
                if (succes)
                    return _value;
            }
            return null;
        }

        /// <summary>
        /// Try to convert value to Int32 (int)
        /// </summary>
        /// <param name="value">value to convert to Int32 (int)</param>
        /// <param name="defaultValue">default value when not converted.</param>
        /// <returns></returns>
        public static Int32 ToInt32(object value, Int32 defaultValue)
        {
            if (value == null)
                return defaultValue;

            if (value == DBNull.Value)
                return defaultValue;

            Int32 _value = 0;
            try
            {
                return (Int32)value;
            }
            catch
            {
                bool succes = Int32.TryParse(value.ToString(), out _value);
                if (succes == false)
                    return defaultValue;
            }
            return _value;
        }

        /// <summary>
        /// Try to convert value to Int32
        /// </summary>
        /// <param name="value">value to convert to Int32</param>
        /// <returns></returns>
        public static Int32? ToInt32Nullable(object value)
        {
            if (value == null)
                return null;

            if (value == DBNull.Value)
                return null;

            try
            {
                // try normal pars
                return (Int32?)value;
            }
            catch
            {
                // try converting to string
                Int32 _value;
                bool succes = Int32.TryParse(HandleNull(value, string.Empty).ToString(), out _value);
                if (succes)
                    return _value;
            }
            return null;
        }

        /// <summary>
        /// Try to convert value to Int64 (long)
        /// </summary>
        /// <param name="value">value to convert to Int64 (long)</param>
        /// <param name="defaultValue">default value when not converted.</param>
        /// <returns></returns>
        public static Int64 ToInt64(object value, Int64 defaultValue)
        {
            if (value == null)
                return defaultValue;

            if (value == DBNull.Value)
                return defaultValue;

            Int64 _value = 0;
            try
            {
                return (Int64)value;
            }
            catch
            {
                bool succes = Int64.TryParse(value.ToString(), out _value);
                if (succes == false)
                    return defaultValue;
            }
            return _value;
        }

        /// <summary>
        /// Try to convert value to Int64 (long)
        /// </summary>
        /// <param name="value">value to convert to Int32</param>
        /// <returns></returns>
        public static Int64? ToInt64Nullable(object value)
        {
            if (value == null)
                return null;

            if (value == DBNull.Value)
                return null;

            try
            {
                // try normal pars
                return (Int64?)value;
            }
            catch
            {
                // try converting to string
                Int64 _value;
                bool succes = Int64.TryParse(HandleNull(value, string.Empty).ToString(), out _value);
                if (succes)
                    return _value;
            }
            return null;
        }

        /// <summary>
        /// Try to convert value to Single (float)
        /// </summary>
        /// <param name="value">value to convert to Single (float)</param>
        /// <param name="defaultValue">default value when not converted.</param>
        /// <returns></returns>
        public static Single ToSingle(object value, Single defaultValue)
        {
            if (value == null)
                return defaultValue;

            if (value == DBNull.Value)
                return defaultValue;

            Single _value = 0;
            try
            {
                return (Single)value;
            }
            catch
            {
                bool succes = Single.TryParse(value.ToString(), out _value);
                if (succes == false)
                    return defaultValue;
            }
            return _value;
        }

        /// <summary>
        /// TTry to convert value to Single (float)
        /// </summary>
        /// <param name="value">value to convert to Int32</param>
        /// <returns></returns>
        public static Single? ToSingleNullable(object value)
        {
            if (value == null)
                return null;

            if (value == DBNull.Value)
                return null;

            try
            {
                // try normal pars
                return (Single?)value;
            }
            catch
            {
                // try converting to string
                Single _value;
                bool succes = Single.TryParse(HandleNull(value, string.Empty).ToString(), out _value);
                if (succes)
                    return _value;
            }
            return null;
        }

        /// <summary>
        /// Convert an object value to string with default value support
        /// </summary>
        /// <param name="value">value to convert to string</param>
        /// <param name="defaultValue">default value to return when value is null</param>
        /// <returns></returns>
        public static string ToString(object value, string defaultValue)
        {
            return ToString(CultureInfo.CurrentCulture, value, defaultValue);
        }

        /// <summary>
        /// Convert an object value to string with culture and default value support
        /// </summary>
        /// <param name="value">value to convert to string</param>
        /// <param name="defaultValue">default value to return when value is null</param>
        /// <param name="cultureInfo">Culture to use</param>
        /// <returns></returns>
        public static string ToString(CultureInfo cultureInfo, object value, string defaultValue)
        {
            if (value == null)
                return defaultValue;

            if (value == DBNull.Value)
                return defaultValue;

            return string.Format(cultureInfo, "{0}", value);
        }

        /// <summary>
        /// Try convert a value to a specific type
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <param name="toType">convert it to type</param>
        /// <param name="succes">Output parameter true on succesfull conversion</param>
        /// <returns></returns>
        public static object TryConvert(object value, Type toType, ref bool succes)
        {
            object retval = null;

            if (toType == typeof(Object))
            {
                succes = true;
                return (object)value;
            }

            if (toType.BaseType == typeof(System.Enum))
            {
                succes = true;
                return System.Enum.ToObject(toType, value);
            }

            if (toType == typeof(Guid?))
            {
                succes = true;
                return ToGuidNullable(value);
            }

            if (toType == typeof(Guid))
            {
                succes = true;
                return ToGuid(value, Guid.Empty);
            }

            if (toType == typeof(Byte?))
            {
                succes = true;
                return ToByteNullable(value);
            }
            if (toType == typeof(Byte))
            {
                succes = true;
                return ToByte(value, 0);
            }
            if (toType == typeof(int?))
            {
                succes = true;
                return ToInt32Nullable(value);
            }
            if (toType == typeof(int))
            {
                succes = true;
                return ToInt32(value, 0);
            }
            if (toType == typeof(decimal?))
            {
                succes = true;
                return ToDecimalNullable(value);
            }
            if (toType == typeof(decimal))
            {
                succes = true;
                return ToDecimal(value, 0);
            }

            if (toType == typeof(Int16?))
            {
                succes = true;
                return ToInt16Nullable(value);
            }
            if (toType == typeof(Int16))
            {
                succes = true;
                return ToInt16(value, 0);
            }
            if (toType == typeof(Int32?))
            {
                succes = true;
                return ToInt32Nullable(value);
            }
            if (toType == typeof(Int32))
            {
                succes = true;
                return ToInt32(value, 0);
            }
            if (toType == typeof(Int64?))
            {
                succes = true;
                return ToInt64Nullable(value);
            }
            if (toType == typeof(Int64))
            {
                succes = true;
                return ToInt64(value, 0);
            }
            if (toType == typeof(float?))
            {
                succes = true;
                return ToSingleNullable(value);
            }
            if (toType == typeof(float))
            {
                succes = true;
                return ToSingle(value, 0);
            }
            if (toType == typeof(Single?))
            {
                succes = true;
                return ToSingleNullable(value);
            }
            if (toType == typeof(Single))
            {
                succes = true;
                return ToSingle(value, 0);
            }
            if (toType == typeof(double?))
            {
                succes = true;
                return ToDoubleNullable(value);
            }
            if (toType == typeof(double))
            {
                succes = true;
                return ToDouble(value, 0);
            }
            if (toType == typeof(Double?))
            {
                succes = true;
                return ToDoubleNullable(value);
            }
            if (toType == typeof(Double))
            {
                succes = true;
                return ToDouble(value, 0);
            }

            if (toType == typeof(bool?))
            {
                succes = true;
                return ToBooleanNullable(value);
            }
            if (toType == typeof(bool))
            {
                succes = true;
                return ToBoolean(value, false);
            }

            if (toType == typeof(DateTime))
            {
                DateTime DateTimeValue = DateTime.Now;
                succes = DateTime.TryParse(HandleNull(value, string.Empty).ToString(), out DateTimeValue);
                retval = DateTimeValue;
            }
            if (toType == typeof(DateTime?))
            {
                DateTime DateTimeValue = DateTime.Now;
                succes = DateTime.TryParse(HandleNull(value, string.Empty).ToString(), out DateTimeValue);
                if (!succes)
                    retval = null;

                retval = DateTimeValue;
            }
            try
            {
                retval = System.Convert.ChangeType(value, toType);
                succes = true;
            }
            catch (Exception)
            {
            }

            return retval;
        }
    }
}