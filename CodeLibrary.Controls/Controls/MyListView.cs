using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Windows.Forms;


namespace CodeLibrary.Controls
{
    public class MyListView : ListView
    {
        private Dictionary<string, DisplayFormatAttribute> _Formatters;
        private HashSet<string> _Hidden;
        private dynamic _ItemDict;
        private PropertyInfo[] _Properties;
        private string[] _PropertyNames;
        private readonly List<object> _RemindSelected = new List<object>();
        private List<ColumnInfo> _RemindSettings = new List<ColumnInfo>();
        private Type _Type;
        private Dictionary<string, int> _Widths;
        private IContainer components;
        private ContextMenuStrip contextMenu;
        private ListViewColumnSorter lvwColumnSorter;

        public MyListView()
        {
            View = View.Details;
            FullRowSelect = true;
            ListViewItemSorter = lvwColumnSorter;
            ColumnClick += MyListView_ColumnClick;
            ColumnReordered += MyListView_ColumnReordered;
            DrawItem += MyListView_DrawItem;
            DrawColumnHeader += MyListView_DrawColumnHeader;
            BackColor = Color.FromArgb(45, 45, 45);
            ForeColor = Color.FromArgb(255, 224, 192);
            DoubleBuffered = true;
            Font = new Font("Microsoft Sans Serif", (float)9.857143);
            MouseUp += MyListView_MouseUp;
        }

        public event EventHandler<EventArgs> MenuItemClick = delegate { };

        public bool OrderAble { get; set; } = true;

        public void AddItem<T, KEY>(CultureInfo culture, T item, Func<T, KEY> keySelector)
        {
            if (_Type == null)
                throw new Exception("Only available when Fill is used, does not work for FillDynamic!");

            ListViewItem _lv = new ListViewItem();
            KEY _key = keySelector(item);
            _lv.Tag = _key;
            _ItemDict.Add(_key, _lv);

            bool _first = true;
            for (int ii = 0; ii < _Properties.Count(); ii++)
            {
                string _propname = _Properties[ii].Name;

                if (_Hidden.Contains(_propname))
                    continue;

                var _value = _Properties[ii].GetValue(item);
                string _textvalue = Convert.ToString(_value, culture);
                if (_Formatters.ContainsKey(_propname))
                {
                    string _format = _Formatters[_propname].DataFormatString;
                    _textvalue = string.Format(culture, _format, _value);
                }

                if (_first)
                {
                    _lv.Text = _textvalue;
                    _first = false;
                }
                else
                {
                    ListViewItem.ListViewSubItem _sub = new ListViewItem.ListViewSubItem(_lv, _textvalue);
                    _lv.SubItems.Add(_sub);
                }
            }
            Items.Add(_lv);
        }

        public ToolStripMenuItem AddMenuItem(string key, string text)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(text);
            item.Click += MenuItem_Click;
            item.Name = key;
            contextMenu.Items.Add(item);
            return item;
        }

        public ToolStripMenuItem AddSubMenuItem(ToolStripMenuItem menuItem, string key, string text)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(text);
            item.Click += MenuItem_Click;
            item.Name = key;
            menuItem.DropDownItems.Add(item);
            return item;
        }

        public void ClearMenu()
        {
            contextMenu.Items.Clear();
        }

        public void ClearSelection()
        {
            SelectedItems.Clear();
            _RemindSelected.Clear();
        }

        public void Fill<T, KEY>(CultureInfo culture, IEnumerable<T> items, Func<T, KEY> keySelector)
        {
            Reset();

            _Type = typeof(T);

            _Properties = _Type.GetProperties()
                .OrderBy(p => (p.GetCustomAttribute(typeof(DataMemberAttribute)) as DataMemberAttribute)?.Order == null ? 0 : (p.GetCustomAttribute(typeof(DataMemberAttribute)) as DataMemberAttribute).Order)
                .ToArray();

            _PropertyNames = _Type.GetProperties()
                .OrderBy(p => (p.GetCustomAttribute(typeof(DataMemberAttribute)) as DataMemberAttribute)?.Order == null ? 0 : (p.GetCustomAttribute(typeof(DataMemberAttribute)) as DataMemberAttribute).Order)
                .Select(p => string.IsNullOrEmpty((p.GetCustomAttribute(typeof(DataMemberAttribute)) as DataMemberAttribute)?.Name) ? p.Name : (p.GetCustomAttribute(typeof(DataMemberAttribute)) as DataMemberAttribute).Name)
                .ToArray();

            _Formatters = _Type.GetProperties()
                .Where(p => p.GetCustomAttribute(typeof(DisplayFormatAttribute)) != null)
                .Select(p => new { Key = p.Name, Value = (p.GetCustomAttribute(typeof(DisplayFormatAttribute)) as DisplayFormatAttribute) })
                .ToDictionary(p => p.Key, p => p.Value);

            _Widths = _Type.GetProperties()
                .Where(p => p.GetCustomAttribute(typeof(ListViewColumnAttribute)) != null)
                .Select(p => new { Key = p.Name, Value = (p.GetCustomAttribute(typeof(ListViewColumnAttribute)) as ListViewColumnAttribute).Width })
                .ToDictionary(p => p.Key, p => p.Value);

            _Hidden = _Type.GetProperties()
                .Where(p => (p.GetCustomAttribute(typeof(BrowsableAttribute)) as BrowsableAttribute)?.Browsable == false)
                .Select(p => p.Name)
                .ToHashSet();

            _ItemDict = new Dictionary<KEY, ListViewItem>();

            ListViewItemSorter = null;
            lvwColumnSorter = null;

            BeginUpdate();

            Items.Clear();
            Columns.Clear();
            for (int ii = 0; ii < _Properties.Length; ii++)
            {
                var property = _Properties[ii];
                if (_Hidden.Contains(property.Name))
                    continue;

                ColumnHeader _column = Columns.Add(property.Name, _PropertyNames[ii]);
                if (IsNumericType(property.PropertyType))
                {
                    _column.TextAlign = HorizontalAlignment.Right;
                }

                if (_Widths.ContainsKey(property.Name))
                    _column.Width = _Widths[property.Name];
                else
                    _column.Width = 150;
            }

            foreach (T item in items)
                AddItem(culture, item, keySelector);

            EndUpdate();

            if (OrderAble)
            {
                lvwColumnSorter = new ListViewColumnSorter();
                ListViewItemSorter = lvwColumnSorter;
            }
        }

        public IEnumerable<ColumnInfo> GetCollumnInfo()
        {
            foreach (ColumnHeader column in Columns)
                yield return new ColumnInfo() { Name = column.Name, Width = column.Width, Index = column.Index };
        }

        public IEnumerable<KEY> GetSelectedByKeys<KEY>()
        {
            if (_Type == null)
                throw new Exception("Only available when Fill is used, does not work for FillDynamic!");

            foreach (ListViewItem item in SelectedItems)
                yield return (KEY)item.Tag;
        }

        // OrderAble needs to be set to false;
        public void MoveSelectionDown()
        {
            if (_Type == null)
                throw new Exception("Only available when Fill is used, does not work for FillDynamic!");

            int _swapIndex = 0;
            List<int> _items = new List<int>();
            foreach (int index in SelectedIndices)
                _items.Add(index);

            _items.Reverse();

            foreach (int index in _items)
            {
                if (index < (Items.Count - 1))
                    _swapIndex = index + 1;
                else
                    _swapIndex = 0;

                ListViewItem _selected = (ListViewItem)Items[index].Clone();
                ListViewItem _swapWith = (ListViewItem)Items[_swapIndex].Clone();
                Items[index] = _swapWith;
                Items[_swapIndex] = _selected;
                Items[_swapIndex].Selected = true;
            }
            Refresh();
        }

        // OrderAble needs to be set to false;
        public void MoveSelectionUp()
        {
            if (_Type == null)
                throw new Exception("Only available when Fill is used, does not work for FillDynamic!");

            int _swapIndex = 0;
            foreach (int index in SelectedIndices)
            {
                if (index > 0)
                    _swapIndex = index - 1;
                else
                    _swapIndex = Items.Count - 1;

                ListViewItem _selected = (ListViewItem)Items[index].Clone();
                ListViewItem _swapWith = (ListViewItem)Items[_swapIndex].Clone();
                Items[index] = _swapWith;
                Items[_swapIndex] = _selected;
                Items[_swapIndex].Selected = true;
            }
            Refresh();
        }

        public void RemindSelection()
        {
            var _items = SelectedIndices;
            foreach (int _item in _items)
                _RemindSelected.Add(Items[_item].Tag);

            _RemindSettings = GetCollumnInfo().ToList();
        }

        public void RemoveByKeys<KEY>(IEnumerable<KEY> keys)
        {
            if (_Type == null)
                throw new Exception("Only available when Fill is used, does not work for FillDynamic!");

            foreach (KEY key in keys)
                RemoveKey(key);
        }

        public void RemoveKey<KEY>(KEY key)
        {
            if (_Type == null)
                throw new Exception("Only available when Fill is used, does not work for FillDynamic!");

            if (!_ItemDict.ContainsKey(key))
                return;

            ListViewItem _lv = _ItemDict[key];
            Items.Remove(_lv);
        }

        public void RemoveSelectedByKeys<KEY>()
        {
            if (_Type == null)
                throw new Exception("Only available when Fill is used, does not work for FillDynamic!");

            BeginUpdate();

            IEnumerable<KEY> _keys = GetSelectedByKeys<KEY>();
            RemoveByKeys(_keys);

            EndUpdate();
        }

        public void RestoreSelection()
        {
            BeginUpdate();
            SelectedItems.Clear();

            foreach (dynamic _item in _RemindSelected)
            {
                if (!_ItemDict.ContainsKey(_item))
                    continue;

                ListViewItem _lv = _ItemDict[_item] as ListViewItem;
                _lv.Selected = true;
            }

            SetColumnInfo(_RemindSettings);

            EndUpdate();
        }

        public void SetColumnInfo(IEnumerable<ColumnInfo> infocolumns)
        {
            foreach (ColumnInfo info in infocolumns)
            {
                foreach (ColumnHeader header in Columns)
                {
                    if (header.Name.Equals(info.Name))
                    {
                        header.Width = info.Width;
                    }
                }
            }
        }

        public void SetSelectedKeys<KEY>(IEnumerable<KEY> keys)
        {
            if (_Type == null)
                throw new Exception("Only available when Fill is used, does not work for FillDynamic!");

            foreach (KEY key in keys)
            {
                if (!_ItemDict.ContainsKey(key))
                    continue;

                ListViewItem _lv = _ItemDict[key] as ListViewItem;
                _lv.Selected = true;
            }
        }

        public void UpdateItem<T, KEY>(CultureInfo culture, T item, Func<T, KEY> keySelector)
        {
            if (_Type == null)
                throw new Exception("Only available when Fill is used, does not work for FillDynamic!");

            KEY _key = keySelector(item);

            if (!_ItemDict.ContainsKey(_key))
                return;

            ListViewItem _lv = _ItemDict[_key] as ListViewItem;

            bool _first = true;
            for (int ii = 0; ii < _Properties.Count(); ii++)
            {
                string _propname = _Properties[ii].Name;

                if (_Hidden.Contains(_propname))
                    continue;

                var _value = _Properties[ii].GetValue(item);
                string _textvalue = Convert.ToString(_value, culture);
                if (_Formatters.ContainsKey(_propname))
                {
                    string _format = _Formatters[_propname].DataFormatString;
                    _textvalue = string.Format(culture, _format, _value);
                }

                if (_first)
                {
                    _lv.Text = _textvalue;
                    _first = false;
                }
                else
                {
                    if (ii < _lv.SubItems.Count)
                    {
                        ListViewItem.ListViewSubItem _sub = _lv.SubItems[ii];
                        _sub.Text = _textvalue;
                    }
                }
            }
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SuspendLayout();
            //
            // contextMenu
            //
            this.contextMenu.Name = "contextMenu";
            this.ResumeLayout(false);
        }

        private bool IsNumericType(Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;

            if (type == typeof(Byte)
                || type == typeof(Int16) || type == typeof(Int32) || type == typeof(Int64)
                || type == typeof(UInt16) || type == typeof(UInt32) || type == typeof(UInt64)
                || type == typeof(IntPtr) || type == typeof(UIntPtr)
                || type == typeof(Single) || type == typeof(Double) || type == typeof(Decimal))
                return true;

            return false;
        }

        private void MenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            if (menuItem == null)
                return;

            MenuItemClick(menuItem, e);
        }

        private void MyListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            Sort();
        }

        private void MyListView_ColumnReordered(object sender, ColumnReorderedEventArgs e)
        {
        }

        private void MyListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(255, 50, 50, 50)))
            {
                Rectangle rectangle = new Rectangle(e.Bounds.X, e.Bounds.Y, this.Width, e.Bounds.Height);
                e.Graphics.FillRectangle(brush, rectangle);
                using (SolidBrush brushFont = new SolidBrush(Color.FromArgb(255, 255, 224, 192)))
                {
                    using (Font f = new Font("Arial", 10, FontStyle.Bold))
                    {
                        e.Graphics.DrawString(e.Header.Text, f, brushFont, new Point(e.Bounds.X + 4, 4));
                    }
                }
            }
        }

        private void MyListView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
            e.DrawBackground();
        }

        private void MyListView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            if (contextMenu == null)
                return;

            contextMenu.Show();
        }

        private void Reset()
        {
            _Type = null;
            _Properties = null;
            _PropertyNames = null;
            _Formatters = null;
            _Widths = null;
            _Hidden = null;
        }

        public class ColumnInfo
        {
            public int Index { get; set; }
            public string Name { get; set; }

            public int Width { get; set; }
        }

        #region Dynamic

        /// <typeparam name="DYNAMICDESCRIPTOR">Class with properties and their attributes describing how properties in dynamic class should be formatted.</typeparam>
        public void FillDynamic<DYNAMICDESCRIPTOR>(CultureInfo culture, IEnumerable<dynamic> items)
        {
            Reset();

            Type _type = typeof(DYNAMICDESCRIPTOR);

            dynamic _v = items.FirstOrDefault();

            if (_v == null)
            {
                ListViewItemSorter = null;
                lvwColumnSorter = null;

                BeginUpdate();

                Items.Clear();
                Columns.Clear();

                EndUpdate();
                return;
            }

            var _properties = ((object)_v).GetType().GetProperties().ToArray();

            var _propertyNames = _type.GetProperties()
                .Select(p => new { Key = p.Name, Value = string.IsNullOrEmpty((p.GetCustomAttribute(typeof(DataMemberAttribute)) as DataMemberAttribute)?.Name) ? p.Name : (p.GetCustomAttribute(typeof(DataMemberAttribute)) as DataMemberAttribute).Name })
                .ToDictionary(p => p.Key, p => p.Value);

            var _formatters = _type.GetProperties()
                .Where(p => p.GetCustomAttribute(typeof(DisplayFormatAttribute)) != null)
                .Select(p => new { Key = p.Name, Value = (p.GetCustomAttribute(typeof(DisplayFormatAttribute)) as DisplayFormatAttribute) })
                .ToDictionary(p => p.Key, p => p.Value);

            var _widths = _type.GetProperties()
                .Where(p => p.GetCustomAttribute(typeof(ListViewColumnAttribute)) != null)
                .Select(p => new { Key = p.Name, Value = (p.GetCustomAttribute(typeof(ListViewColumnAttribute)) as ListViewColumnAttribute).Width })
                .ToDictionary(p => p.Key, p => p.Value);

            var _hidden = _type.GetProperties()
                .Where(p => (p.GetCustomAttribute(typeof(BrowsableAttribute)) as BrowsableAttribute)?.Browsable == false)
                .Select(p => p.Name)
                .ToHashSet();

            ListViewItemSorter = null;
            lvwColumnSorter = null;

            BeginUpdate();

            Items.Clear();
            Columns.Clear();
            for (int ii = 0; ii < _properties.Length; ii++)
            {
                var property = _properties[ii];
                if (_hidden.Contains(property.Name))
                    continue;

                ColumnHeader _column;
                if (_propertyNames.ContainsKey(property.Name))
                    _column = Columns.Add(property.Name, _propertyNames[property.Name]);
                else
                    _column = Columns.Add(property.Name, property.Name);

                if (IsNumericType(property.PropertyType))
                    _column.TextAlign = HorizontalAlignment.Right;

                if (_widths.ContainsKey(property.Name))
                    _column.Width = _widths[property.Name];
                else
                    _column.Width = 150;
            }

            foreach (dynamic item in items)
            {
                ListViewItem _lv = new ListViewItem();
                //_lv.Tag = keySelector(item);

                bool _first = true;
                for (int ii = 0; ii < _properties.Count(); ii++)
                {
                    string _propname = _properties[ii].Name;

                    if (_hidden.Contains(_propname))
                        continue;

                    var _value = _properties[ii].GetValue(item);
                    string _textvalue = Convert.ToString(_value, culture);
                    if (_formatters.ContainsKey(_propname))
                    {
                        string _format = _formatters[_propname].DataFormatString;
                        _textvalue = string.Format(culture, _format, _value);
                    }

                    if (_first)
                    {
                        _lv.Text = _textvalue;
                        _first = false;
                    }
                    else
                    {
                        ListViewItem.ListViewSubItem _sub = new ListViewItem.ListViewSubItem(_lv, _textvalue);
                        _lv.SubItems.Add(_sub);
                    }
                }
                Items.Add(_lv);
            }

            EndUpdate();

            if (OrderAble)
            {
                lvwColumnSorter = new ListViewColumnSorter();
                ListViewItemSorter = lvwColumnSorter;
            }
        }

        #endregion Dynamic

        public class ListViewColumnAttribute : Attribute
        {
            public int Width { get; set; }
        }

        private class ListViewColumnSorter : IComparer
        {
            private readonly CaseInsensitiveComparer ObjectCompare;

            public ListViewColumnSorter()
            {
                ObjectCompare = new CaseInsensitiveComparer();
            }

            public SortOrder Order { get; set; } = SortOrder.None;
            public int SortColumn { get; set; } = 0;

            public int Compare(object x, object y)
            {
                int compareResult;
                ListViewItem listviewX, listviewY;

                // Cast the objects to be compared to ListViewItem objects
                listviewX = (ListViewItem)x;
                listviewY = (ListViewItem)y;

                // Compare the two items
                compareResult = ObjectCompare.Compare(listviewX.SubItems[SortColumn].Text, listviewY.SubItems[SortColumn].Text);

                // Calculate correct return value based on object comparison
                if (Order == SortOrder.Ascending)
                {
                    // Ascending sort is selected, return normal result of compare operation
                    return compareResult;
                }
                else if (Order == SortOrder.Descending)
                {
                    // Descending sort is selected, return negative result of compare operation
                    return (-compareResult);
                }
                else
                {
                    // Return '0' to indicate they are equal
                    return 0;
                }
            }
        }
    }
}