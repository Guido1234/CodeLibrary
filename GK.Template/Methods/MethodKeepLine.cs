using GK.Template.Attributes;
using System;
using System.ComponentModel;

namespace GK.Template.Methods
{ 
    public enum KeepMode
    {
        [Description("The part of the value equals one of specified values.")]
        ContainsOneOf = 1,

        [Description("The value starts with one of the specified values")]
        StartsWith = 2,

        [Description("The value ends with one of the specified values")]
        EndsWith = 3,

        [Description("The value starts and ends with the specified values")]
        StartEndsWith = 4,

        [Description("The value equals one of specified values.")]
        EqualsOneOf = 5,

        [Description("The value does not equal one of specified values.")]
        NotEqualsOneOf = 6,

        [Description("The value is numeric")]
        IsNumeric = 7,

        [Description("The value is Alpha")]
        IsAlpha = 8
    }

    [Category("Logic")]
    [FormatMethod(Name = "KeepLine",
        Aliasses = "Keep",
        Example = "{0:Keep(\"Equals\", \"aa\", \"cc\")}")]
    [Description("Keep line when criteria is met.")]
    public sealed class MethodKeepLine : MethodBase
    {
        [FormatMethodParameter(Optional = false, Order = 2)]
        [Description("Depending on type of mode: StartEndsWith requires 2 parameters (1 for start, 1 for end), isNumeric and isAlpha both requires 1 parameter, other modes have unlimited parameters.")]
        public string[] Items { get; set; }

        [FormatMethodParameter(Optional = false, Order = 1)]
        [Description("Type of matching to use.")]
        public SkipMode Mode { get; set; }

        public override string Apply(string value)
        {
            SkipData = true;
            switch (Mode)
            {
                case SkipMode.ContainsOneOf:
                    foreach (string s in Items)
                    {
                        if (value.Contains(s))
                        {
                            SkipData = false;
                            return value;
                        }
                    }
                    break;

                case SkipMode.EndsWith:
                    foreach (string s in Items)
                    {
                        if (value.EndsWith(s))
                        {
                            SkipData = false;
                            return value;
                        }
                    }
                    break;

                case SkipMode.EqualsOneOf:
                    foreach (string s in Items)
                    {
                        if (value.Equals(s))
                        {
                            SkipData = false;
                            return value;
                        }
                    }
                    break;

                case SkipMode.IsAlpha:
                    if (IsAlpha(value))
                    {
                        SkipData = false;
                        return value;
                    }
                    break;

                case SkipMode.IsNumeric:
                    if (IsNumeric(value))
                    {
                        SkipData = false;
                        return value;
                    }
                    break;

                case SkipMode.NotEqualsOneOf:
                    foreach (string s in Items)
                    {
                        if (!value.Equals(s))
                        {
                            SkipData = false;
                            return value;
                        }
                    }
                    break;

                case SkipMode.StartEndsWith:
                    if (Items.Length == 2)
                    {
                        if (value.StartsWith(Items[0]) && value.EndsWith(Items[1]))
                        {
                            SkipData = false;
                            return value;
                        }
                    }
                    break;

                case SkipMode.StartsWith:
                    foreach (string s in Items)
                    {
                        if (value.StartsWith(s))
                        {
                            SkipData = false;
                            return value;
                        }
                    }
                    break;
            }
            return string.Empty;
        }

        private bool IsAlpha(string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;

            Char[] chars = s.ToCharArray();

            for (int ii = 0; ii < chars.Length; ii++)
                if (!Char.IsLetter(chars[ii]))
                    return false;

            return true;
        }

        private bool IsNumeric(string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;

            Char[] chars = s.ToCharArray();

            for (int ii = 0; ii < chars.Length; ii++)
                if (!Char.IsDigit(chars[ii]))
                    return false;

            return true;
        }
    }
}