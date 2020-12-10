using GK.Template.Attributes;
using System;
using System.ComponentModel;

namespace GK.Template.Methods
{
    [Category("Numeric")]
    [FormatMethod(Name = "Hex", Aliasses = "")]
    [Description("Convert numericvalue to Hex.")]
    public sealed class MethodHex : MethodBase
    {
        public override string Apply(string value)
        {
            int dec = ToInt32(value, 0);
            return ToHex(dec).ToString();
        }

        private string ToHex(int dec)
        {
            return dec.ToString("X");
        }

        private Int32 ToInt32(object value, Int32 defaultValue)
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
    }
}