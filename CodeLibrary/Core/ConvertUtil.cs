using System;
using System.Globalization;

namespace CodeLibrary.Core
{
    public static class ConvertUtility
    {
        public static object HandleNull(object value, object defaultValue) => value == null ? defaultValue : (value == DBNull.Value ? defaultValue : value);

        public static bool IsNullOrZero(byte? value) => value.HasValue ? (value.Value == 0 ? true : false) : true;

        public static bool IsNullOrZero(float? value) => value.HasValue ? (value.Value == 0 ? true : false) : true;

        public static bool IsNullOrZero(long? value) => value.HasValue ? (value.Value == 0 ? true : false) : true;

        public static bool IsNullOrZero(int? value) => value.HasValue ? (value.Value == 0 ? true : false) : true;

        public static bool IsNullOrZero(double? value) => value.HasValue ? (value.Value == 0 ? true : false) : true;

        public static bool IsNullOrZero(decimal? value) => value.HasValue ? (value.Value == 0 ? true : false) : true;

        public static bool ParseBool(string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;

            switch (s.Trim().ToLower())
            {
                case "true":
                case "yes":
                case "on":
                case "1":
                    return true;

                case "false":
                case "no":
                case "off":
                case "0":
                    return false;

                default:
                    return false;
            }
        }

        public static Boolean ToBoolean(object value, Boolean defaultValue) => ParseBool(HandleNull(value, defaultValue).ToString());

        public static Boolean? ToBooleanNullable(object value)
        {
            if (value == null)
                return null;

            if (value == DBNull.Value)
                return null;

            return ParseBool(HandleNull(value, string.Empty).ToString());
        }

        public static Byte ToByte(object value, Byte defaultValue)
        {
            if (value == null)
                return defaultValue;

            if (value == DBNull.Value)
                return defaultValue;

            byte _value;
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
                bool succes = Byte.TryParse(HandleNull(value, string.Empty).ToString(), out byte _value);
                if (succes)
                    return _value;
            }
            return null;
        }

        public static DateTime ToDateTime(object value, DateTime defaultValue)
        {
            if (value == null)
                return defaultValue;

            if (value == DBNull.Value)
                return defaultValue;

            DateTime _value;
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

        public static DateTime? ToDateTimeNullable(object value)
        {
            if (value == null)
                return null;

            if (value == DBNull.Value)
                return null;

            DateTime _value;
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

        public static Decimal ToDecimal(object value, Decimal defaultValue)
        {
            if (value == null)
                return defaultValue;

            if (value == DBNull.Value)
                return defaultValue;

            Decimal _value;
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
                bool succes = Decimal.TryParse(HandleNull(value, string.Empty).ToString(), out decimal _value);
                if (succes)
                    return _value;
            }
            return null;
        }

        public static Double ToDouble(object value, Double defaultValue)
        {
            if (value == null)
                return defaultValue;

            if (value == DBNull.Value)
                return defaultValue;

            Double _value;
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
                bool succes = Double.TryParse(HandleNull(value, string.Empty).ToString(), out double _value);
                if (succes)
                    return _value;
            }
            return null;
        }

        public static Guid ToGuid(object value, Guid defaultValue)
        {
            if (value == null)
                return defaultValue;

            if (value == DBNull.Value)
                return defaultValue;

            bool _succes = Guid.TryParse(value.ToString(), out Guid _result);
            if (_succes)
                return _result;

            return defaultValue;
        }

        public static Guid? ToGuidNullable(object value)
        {
            if (value == null)
                return null;

            if (value == DBNull.Value)
                return null;

            bool _succes = Guid.TryParse(value.ToString(), out Guid _result);
            if (_succes)
                return _result;

            return null;
        }

        public static Int16 ToInt16(object value, Int16 defaultValue)
        {
            if (value == null)
                return defaultValue;

            if (value == DBNull.Value)
                return defaultValue;

            Int16 _value;
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
                bool succes = Int16.TryParse(HandleNull(value, string.Empty).ToString(), out short _value);
                if (succes)
                    return _value;
            }
            return null;
        }

        public static Int32 ToInt32(object value, Int32 defaultValue)
        {
            if (value == null)
                return defaultValue;

            if (value == DBNull.Value)
                return defaultValue;

            Int32 _value;
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
                bool succes = Int32.TryParse(HandleNull(value, string.Empty).ToString(), out int _value);
                if (succes)
                    return _value;
            }
            return null;
        }

        public static Int64 ToInt64(object value, Int64 defaultValue)
        {
            if (value == null)
                return defaultValue;

            if (value == DBNull.Value)
                return defaultValue;

            Int64 _value;
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
                bool succes = Int64.TryParse(HandleNull(value, string.Empty).ToString(), out long _value);
                if (succes)
                    return _value;
            }
            return null;
        }

        public static Single ToSingle(object value, Single defaultValue)
        {
            if (value == null)
                return defaultValue;

            if (value == DBNull.Value)
                return defaultValue;

            Single _value;
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
                bool succes = Single.TryParse(HandleNull(value, string.Empty).ToString(), out float _value);
                if (succes)
                    return _value;
            }
            return null;
        }

        public static string ToString(object value, string defaultValue)
        {
            return ToString(CultureInfo.CurrentCulture, value, defaultValue);
        }

        public static string ToString(CultureInfo cultureInfo, object value, string defaultValue)
        {
            if (value == null)
                return defaultValue;

            if (value == DBNull.Value)
                return defaultValue;

            return string.Format(cultureInfo, "{0}", value);
        }

        public static object TryConvert(object value, Type toType, ref bool succes)
        {
            object retval = null;

            if (toType == typeof(Object))
            {
                succes = true;
                return value;
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
                succes = DateTime.TryParse(HandleNull(value, string.Empty).ToString(), out DateTime DateTimeValue);
                retval = DateTimeValue;
            }
            if (toType == typeof(DateTime?))
            {
                succes = DateTime.TryParse(HandleNull(value, string.Empty).ToString(), out DateTime DateTimeValue);
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