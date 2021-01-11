using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
 
namespace CodeLibrary.Core
{
    public class EnumComboBoxModeHelper<T>
        where T : Enum, new()
    {
        private ComboBox _ComboBox;
        private T _DefaultValue;
        private Type _EnumType;

        public EnumComboBoxModeHelper(ComboBox comboBox, T defaultValue)
        {
            _ComboBox = comboBox;
            _EnumType = typeof(T);
            _DefaultValue = defaultValue;
            _ComboBox.Font = new Font("Microsoft Sans Serif", (float)9.857143);
        }

        public void Fill()
        {
            List<EnumListBoxObject> _enums = GetEnumValues();
            foreach (EnumListBoxObject _enum in _enums)
            {
                if (_enum.Visible)
                    _ComboBox.Items.Add(_enum);
            }
            SetSelectedIndex(_DefaultValue);
        }

        public List<EnumListBoxObject> GetEnumValues()
        {
            List<EnumListBoxObject> _result = new List<EnumListBoxObject>();

            string[] _names = Enum.GetNames(_EnumType);
            Array _values = Enum.GetValues(_EnumType);

            for (int ii = 0; ii < _names.Length; ii++)
            {
                string _displayName = GetDisplayName(_values.GetValue(ii));
                _displayName = string.IsNullOrEmpty(_displayName) ? _names[ii] : _displayName;
                int _key = Convert.ToInt32(_values.GetValue(ii));
                bool _visible = IsVisible(_values.GetValue(ii));
                _result.Add(new EnumListBoxObject() { Value = _key, Name = _names[ii], Visible = _visible });
            }
            return _result;
        }

        public int GetValue()
        {
            int _index = _ComboBox.SelectedIndex;
            EnumListBoxObject _item = _ComboBox.Items[_index] as EnumListBoxObject;
            return _item.Value;
        }

        public string GetName()
        {
            int _index = _ComboBox.SelectedIndex;
            EnumListBoxObject _item = _ComboBox.Items[_index] as EnumListBoxObject;
            return _item.Name;
        }

        public void SetDefaultValue() => SetSelectedIndex(_DefaultValue);

        public void SetSelectedIndex(T _enum)
        {
            int _value = Convert.ToInt32(_enum);
            for (int ii = 0; ii < _ComboBox.Items.Count; ii++)
            {
                EnumListBoxObject _item = _ComboBox.Items[ii] as EnumListBoxObject;
                if (_item.Value == _value)
                {
                    _ComboBox.SelectedIndex = ii;
                    return;
                }
            }
            _ComboBox.SelectedIndex = 0;
        }

        private static string GetDisplayName(object enumValue)
        {
            Type _type = enumValue.GetType();
            MemberInfo[] memInfo = _type.GetMember(enumValue.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                DisplayNameAttribute _att = memInfo[0].GetCustomAttribute(typeof(DisplayNameAttribute), false) as DisplayNameAttribute;
                if (_att != null)
                    return _att.DisplayName;
            }
            return null;
        }

        private static bool IsVisible(object enumValue)
        {
            Type type = enumValue.GetType();
            MemberInfo[] memInfo = type.GetMember(enumValue.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                BrowsableAttribute _att = memInfo[0].GetCustomAttribute(typeof(BrowsableAttribute), false) as BrowsableAttribute;
                if (_att != null)
                    return _att.Browsable;
            }
            return true;
        }

        public class EnumListBoxObject
        {
            public string Name { get; set; }
            public int Value { get; set; }
            public bool Visible { get; set; }

            public override string ToString() => Name;
        }
    }
}