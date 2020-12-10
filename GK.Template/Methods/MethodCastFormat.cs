using GK.Template.Attributes;
using System;
using System.ComponentModel;
using System.Globalization;

namespace GK.Template.Methods
{
    [Category("Casting")]
    [FormatMethod(Name = "CastFormat", Aliasses = "Cast,Format")]
    [Description("Converts a value to a DotNet type then applies DotNet formatting")]
    public sealed class MethodCastFormat : MethodBase
    {
        [FormatMethodParameter(Optional = false, Order = 1)]
        public string Format { get; set; }

        [FormatMethodParameter(Optional = false, Order = 0)]
        public string ToType { get; set; }

        public override string Apply(string value)
        {
            bool succes = false;
            object val = TryConvert(value, ToType, ref succes);
            if (!succes)
            {
                return string.Empty;
            }
            return Utils.Format(val, Format, CultureInfo.CurrentCulture);
        }

        private object TryConvert(object value, string toType, ref bool succes)
        {
            object retval = null;

            if (toType == typeof(Object).Name)
            {
                succes = true;
                return (object)value;
            }

            if (toType == typeof(Guid?).Name)
            {
                succes = true;
                return ConvertUtility.ToGuidNullable(value);
            }

            if (toType == typeof(Guid).Name)
            {
                succes = true;
                return ConvertUtility.ToGuid(value, Guid.Empty);
            }

            if (toType == typeof(Byte?).Name)
            {
                succes = true;
                return ConvertUtility.ToByteNullable(value);
            }
            if (toType == typeof(Byte).Name)
            {
                succes = true;
                return ConvertUtility.ToByte(value, 0);
            }
            if (toType == typeof(int?).Name)
            {
                succes = true;
                return ConvertUtility.ToInt32Nullable(value);
            }
            if (toType == typeof(int).Name)
            {
                succes = true;
                return ConvertUtility.ToInt32(value, 0);
            }
            if (toType == typeof(decimal?).Name)
            {
                succes = true;
                return ConvertUtility.ToDecimalNullable(value);
            }
            if (toType == typeof(decimal).Name)
            {
                succes = true;
                return ConvertUtility.ToDecimal(value, 0);
            }

            if (toType == typeof(Int16?).Name)
            {
                succes = true;
                return ConvertUtility.ToInt16Nullable(value);
            }
            if (toType == typeof(Int16).Name)
            {
                succes = true;
                return ConvertUtility.ToInt16(value, 0);
            }
            if (toType == typeof(Int32?).Name)
            {
                succes = true;
                return ConvertUtility.ToInt32Nullable(value);
            }
            if (toType == typeof(Int32).Name)
            {
                succes = true;
                return ConvertUtility.ToInt32(value, 0);
            }
            if (toType == typeof(Int64?).Name)
            {
                succes = true;
                return ConvertUtility.ToInt64Nullable(value);
            }
            if (toType == typeof(Int64).Name)
            {
                succes = true;
                return ConvertUtility.ToInt64(value, 0);
            }
            if (toType == typeof(float?).Name)
            {
                succes = true;
                return ConvertUtility.ToSingleNullable(value);
            }
            if (toType == typeof(float).Name)
            {
                succes = true;
                return ConvertUtility.ToSingle(value, 0);
            }
            if (toType == typeof(Single?).Name)
            {
                succes = true;
                return ConvertUtility.ToSingleNullable(value);
            }
            if (toType == typeof(Single).Name)
            {
                succes = true;
                return ConvertUtility.ToSingle(value, 0);
            }
            if (toType == typeof(double?).Name)
            {
                succes = true;
                return ConvertUtility.ToDoubleNullable(value);
            }
            if (toType == typeof(double).Name)
            {
                succes = true;
                return ConvertUtility.ToDouble(value, 0);
            }
            if (toType == typeof(Double?).Name)
            {
                succes = true;
                return ConvertUtility.ToDoubleNullable(value);
            }
            if (toType == typeof(Double).Name)
            {
                succes = true;
                return ConvertUtility.ToDouble(value, 0);
            }

            if (toType == typeof(bool?).Name)
            {
                succes = true;
                return ConvertUtility.ToBooleanNullable(value);
            }
            if (toType == typeof(bool).Name)
            {
                succes = true;
                return ConvertUtility.ToBoolean(value, false);
            }

            if (toType == typeof(DateTime).Name)
            {
                DateTime DateTimeValue = DateTime.Now;
                succes = DateTime.TryParse(ConvertUtility.HandleNull(value, string.Empty).ToString(), out DateTimeValue);
                retval = DateTimeValue;
            }
            if (toType == typeof(DateTime?).Name)
            {
                DateTime DateTimeValue = DateTime.Now;
                succes = DateTime.TryParse(ConvertUtility.HandleNull(value, string.Empty).ToString(), out DateTimeValue);
                if (!succes)
                {
                    retval = null;
                }
                retval = DateTimeValue;
            }

            return retval;
        }
    }
}