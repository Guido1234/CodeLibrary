using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CodeLibrary.Core
{
    public class ComboBoxHelper<T>
    {
        private ComboBox _comboBox;
        private Dictionary<T, int> _indexes = new Dictionary<T, int>();
        private bool _SupressSelectionChange = false;

        public ComboBoxHelper(ComboBox comboBox)
        {
            _comboBox = comboBox;
            _comboBox.SelectedIndexChanged += _comboBox_SelectedIndexChanged;
        }

        public event EventHandler<EventArgs> ManualSelectedIndexChanged = delegate { };

        public void Add(T item)
        {
            _SupressSelectionChange = true;
            int _index = _comboBox.Items.Add(item);
            _indexes.Add(item, _index);
            _comboBox.SelectedIndex = _index;
            _SupressSelectionChange = false;
        }

        public void Deselect()
        {
            _SupressSelectionChange = true;
            _comboBox.SelectedIndex = -1;
            _SupressSelectionChange = false;
        }

        public void Fill(IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                int _index = _comboBox.Items.Add(item);
                _indexes.Add(item, _index);
            }
        }

        public void Fill(IEnumerable<T> items, int setSelectedIndex)
        {
            foreach (T item in items)
            {
                _comboBox.Items.Add(item);
            }
            _comboBox.SelectedIndex = setSelectedIndex;
        }

        public T GetSelected()
        {
            if (_comboBox.SelectedIndex == -1)
                return default;

            return (T)_comboBox.SelectedItem;
        }

        public bool HasSelection()
        {
            if (_comboBox.SelectedIndex == -1)
                return false;

            return true;
        }

        public void Refresh()
        {
            _comboBox.Refresh();
        }

        public void Remove(T value)
        {
            _SupressSelectionChange = true;

            if (!_indexes.ContainsKey(value))
            {
                _SupressSelectionChange = false;
                return;
            }
            _comboBox.Items.Remove(value);
            _SupressSelectionChange = false;
        }

        public void Select(T value)
        {
            _SupressSelectionChange = true;
            if (!_indexes.ContainsKey(value))
            {
                Deselect();
                _SupressSelectionChange = false;
                return;
            }
            _comboBox.SelectedIndex = _indexes[value];
            _SupressSelectionChange = false;
        }

        private void _comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_SupressSelectionChange)
            {
                return;
            }
            ManualSelectedIndexChanged(this._comboBox, new EventArgs());
        }
    }
}