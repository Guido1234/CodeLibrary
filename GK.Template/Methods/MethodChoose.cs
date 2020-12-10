using GK.Template.Attributes;
using System;
using System.ComponentModel;

namespace GK.Template.Methods
{
    [Category("Logic")]
    [FormatMethod(Name = "Choose",
        Example = "{0:Choose(\"A\",\"B\",\"C\")}")]
    [Description("Chooses an item identified by the value of the pointer.")]
    public sealed class MethodChoose : MethodBase
    {
        [Category("Misc")]
        [FormatMethodParameter(Optional = false, Order = 0)]
        public string[] Switches { get; set; }

        public override string Apply(string value)
        {
            int index = ToInt32(value, 0);
            return Utils.ArrayValue(Switches, index);
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