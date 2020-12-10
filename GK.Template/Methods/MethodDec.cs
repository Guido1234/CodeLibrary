using GK.Template.Attributes;
using System.ComponentModel;

namespace GK.Template.Methods
{
    [Category("Numeric")]
    [FormatMethod(Name = "Dec", Aliasses = "")]
    [Description("Convert numericvalue from Hex to Dec.")]
    public sealed class MethodDec : MethodBase
    {
        public override string Apply(string value)
        {
            return ToDec(value).ToString();
        }

        private int ToDec(string hex)
        {
            int value = 0;
            try
            {
                value = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            }
            catch
            {
            }
            return value;
        }
    }
}