using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CodeLibrary.Controls
{
    public partial class CollectionListBox : UserControl
    {
        private readonly List<object> _collection = new List<object>();

        private readonly List<Type> _collectionTypes;

        private readonly List<Color> _colors = new List<Color>();

        private readonly object _previous = null;

        private string _lastCategory = string.Empty;

        private object _selectedObject = null;

        private List<object> _selectedObjects = new List<object>();

        private bool _showAdd = true;

        private bool _showCopy = false;

        private bool _showDelete = true;

        private bool _showRefresh = true;

        private bool _showSearch = true;

        private bool showToolstrip = true;

        public CollectionListBox()
        {
            _colors.Add(Color.FromArgb(255, 255, 72, 72)); // FF4848
            _colors.Add(Color.FromArgb(255, 255, 98, 176)); // FF62B0
            _colors.Add(Color.FromArgb(255, 128, 0, 128)); // 800080
            _colors.Add(Color.FromArgb(255, 219, 0, 219)); // DB00DB
            _colors.Add(Color.FromArgb(255, 154, 3, 254)); // 9A03FE
            _colors.Add(Color.FromArgb(255, 189, 92, 254)); // BD5CFE
            _colors.Add(Color.FromArgb(255, 57, 35, 214)); // 3923D6
            _colors.Add(Color.FromArgb(255, 149, 136, 236)); // 9588EC
            _colors.Add(Color.FromArgb(255, 41, 102, 184)); // 2966B8
            _colors.Add(Color.FromArgb(255, 96, 148, 219)); // 6094DB
            _colors.Add(Color.FromArgb(255, 35, 129, 156)); // 23819C
            _colors.Add(Color.FromArgb(255, 68, 180, 213)); // 44B4D5
            _colors.Add(Color.FromArgb(255, 87, 87, 255)); // 5757FF
            _colors.Add(Color.FromArgb(255, 98, 208, 255)); // 62D0FF
            _colors.Add(Color.FromArgb(255, 117, 214, 255)); // 75D6FF
            _colors.Add(Color.FromArgb(255, 6, 220, 251)); // 06DCFB
            _colors.Add(Color.FromArgb(255, 117, 236, 253)); // 75ECFD
            _colors.Add(Color.FromArgb(255, 1, 252, 239)); // 01FCEF
            _colors.Add(Color.FromArgb(255, 146, 254, 249)); // 92FEF9
            _colors.Add(Color.FromArgb(255, 3, 235, 166)); // 03EBA6
            _colors.Add(Color.FromArgb(255, 125, 253, 215)); // 7DFDD7
            _colors.Add(Color.FromArgb(255, 1, 243, 62)); // 01F33E
            _colors.Add(Color.FromArgb(255, 139, 254, 168)); // 8BFEA8
            _colors.Add(Color.FromArgb(255, 31, 203, 74)); // 1FCB4A
            _colors.Add(Color.FromArgb(255, 164, 240, 183)); // A4F0B7
            _colors.Add(Color.FromArgb(255, 89, 149, 92)); // 59955C
            _colors.Add(Color.FromArgb(255, 180, 209, 182)); // B4D1B6
            _colors.Add(Color.FromArgb(255, 72, 251, 13)); // 48FB0D
            _colors.Add(Color.FromArgb(255, 186, 254, 163)); // BAFEA3
            _colors.Add(Color.FromArgb(255, 45, 200, 0)); // 2DC800
            _colors.Add(Color.FromArgb(255, 143, 255, 111)); // 8FFF6F
            _colors.Add(Color.FromArgb(255, 89, 223, 0)); // 59DF00
            _colors.Add(Color.FromArgb(255, 192, 255, 151)); // C0FF97
            _colors.Add(Color.FromArgb(255, 157, 157, 0)); // 9D9D00
            _colors.Add(Color.FromArgb(255, 255, 255, 153)); // FFFF99
            _colors.Add(Color.FromArgb(255, 182, 186, 24)); // B6BA18
            _colors.Add(Color.FromArgb(255, 242, 244, 179)); // F2F4B3

            _collectionTypes = new List<Type>();
            CollectionType = typeof(object);
            InitializeComponent();
            DefaultCategoryName = "No Category";
            splitContainer2.Dock = DockStyle.Fill;
            AskBeforeDelete = false;
            _selectedObjects = new List<object>();
            listViewSelected.LabelEdit = true;
            listViewSelected.BeforeLabelEdit += ListViewSelected_BeforeLabelEdit;
            listViewSelected.AfterLabelEdit += ListViewSelected_AfterLabelEdit;
            ShowButtons();
            resize();
        }

        public event EventHandler<AfterLabelEditEventArgs> AfterLabelEdit = delegate { };

        /// <summary>
        /// Raised prior to when an object is selected.
        /// </summary>
        [Description("Raised prior to when an object is selected.")]
        [Category("CollectionListBox")]
        public event EventHandler<CollectionListBoxEventArgs> BeforeItemSelected = delegate { };

        /// <summary>
        /// Raised prior to when an object is selected.
        /// </summary>
        [Description("Raised prior to when an object is selected.")]
        [Category("CollectionListBox")]
        public event EventHandler<EventArgs> BeforeItemsSelected = delegate { };

        /// <summary>
        /// Raised when an object is added to the collection
        /// </summary>
        [Description("Raised when an object is duplicated")]
        [Category("CollectionListBox")]
        public event EventHandler<CollectionListBoxEventArgs> Copy = delegate { };

        /// <summary>
        /// Raised when an item is selected
        /// </summary>
        [Description("Raised when an item is selected.")]
        [Category("CollectionListBox")]
        public event EventHandler<CollectionListBoxEventArgs> ItemSelected = delegate { };

        /// <summary>
        /// Raised when an item is selected
        /// </summary>
        [Description("Raised when an item is selected.")]
        [Category("CollectionListBox")]
        public event EventHandler<CollectionListBoxMultiEventArgs> ItemsSelected = delegate { };

        /// <summary>
        /// Raised when an object is added to the collection
        /// </summary>
        [Description("Raised when an object is added to the collection")]
        [Category("CollectionListBox")]
        public event EventHandler<CollectionListBoxEventArgs> OnAdd = delegate { };

        /// <summary>
        /// Raised when the collection has changed
        /// </summary>
        [Description("Raised when the collection has changed")]
        [Category("CollectionListBox")]
        public event EventHandler<EventArgs> OnCollectionChanged = delegate { };

        /// <summary>
        /// Raised when an object is added to the collection
        /// </summary>
        [Description("Raised when an object is added to the collection")]
        [Category("CollectionListBox")]
        public event EventHandler<CollectionListBoxDeleteEventArgs> OnDelete = delegate { };

        /// <summary>
        /// Raised when an object is added to the collection
        /// </summary>
        [Description("Raised when an object is duplicated")]
        [Category("CollectionListBox")]
        public event EventHandler<CollectionListBoxEventArgs> Paste = delegate { };

        /// <summary>
        /// Determines whether deleting should be asked first.
        /// </summary>
        [Description("Determines whether deleting should be asked first.")]
        [Category("CollectionListBox")]
        public bool AskBeforeDelete { get; set; }

        /// <summary>
        /// Defines which property should be used as category attribute.
        /// </summary>
        [Description("Defines which property should be used as category attribute.")]
        [Category("CollectionListBox")]
        public string CategoryProperty { get; set; }

        /// <summary>
        /// Type of collection to use.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Type CollectionType { get; set; }

        /// <summary>
        /// Type of collection to use.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<Type> CollectionTypes
        {
            get
            {
                return _collectionTypes;
            }
        }

        /// <summary>
        /// Defines which property should be used for the color.
        /// </summary>
        [Description("Defines which property should be used for the color.")]
        [Category("CollectionListBox")]
        public string ColorProperty { get; set; }

        /// <summary>
        /// Gets / Sets the default category name
        /// </summary>
        [Description("Gets / Sets the default category name")]
        [Category("CollectionListBox")]
        public string DefaultCategoryName { get; set; }

        /// <summary>
        /// Defines which property should be used as category attribute.
        /// </summary>
        [Description("")]
        [Category("CollectionListBox")]
        public string ImageProperty { get; set; }

        public bool MultiSelect
        {
            get
            {
                return listViewSelected.MultiSelect;
            }
            set
            {
                listViewSelected.MultiSelect = value;
            }
        }

        /// <summary>
        /// Defines which property should be used as category attribute.
        /// </summary>
        [Description("")]
        [Category("CollectionListBox")]
        public string NameProperty { get; set; }

        /// <summary>
        /// Returns the selected object.
        /// </summary>
        public object SelectedObject
        {
            get
            {
                return _selectedObject;
            }
        }

        /// <summary>
        /// Returns a list of selected objects.
        /// </summary>
        public IEnumerable<object> SelectedObjects
        {
            get
            {
                return _selectedObjects;
            }
        }

        /// <summary>
        /// Show Add
        /// </summary>
        [Description("Show Add.")]
        [Category("CollectionListBox")]
        public bool ShowAdd
        {
            get
            {
                return _showAdd;
            }
            set
            {
                _showAdd = value;
                ShowButtons();
            }
        }

        /// <summary>
        /// Show Cut Copy and Paste.
        /// </summary>
        [Description("Show Cut Copy and Paste.")]
        [Category("CollectionListBox")]
        public bool ShowCopy
        {
            get
            {
                return _showCopy;
            }
            set
            {
                _showCopy = value;
                ShowButtons();
            }
        }

        /// <summary>
        /// Show Delete
        /// </summary>
        [Description("Show Delete.")]
        [Category("CollectionListBox")]
        public bool ShowDelete
        {
            get
            {
                return _showDelete;
            }
            set
            {
                _showDelete = value;
                ShowButtons();
            }
        }

        /// <summary>
        /// Show Refresh
        /// </summary>
        [Description("Show Refresh.")]
        [Category("CollectionListBox")]
        public bool ShowRefresh
        {
            get
            {
                return _showRefresh;
            }
            set
            {
                _showRefresh = value;
                ShowButtons();
            }
        }

        /// <summary>
        /// Show Search
        /// </summary>
        [Description("Show Search.")]
        [Category("CollectionListBox")]
        public bool ShowSearch
        {
            get
            {
                return _showSearch;
            }
            set
            {
                _showSearch = value;
                ShowButtons();
            }
        }

        /// <summary>
        /// Show Add
        /// </summary>
        [Description("Show Toolstrip.")]
        [Category("CollectionListBox")]
        public bool ShowToolstrip
        {
            get
            {
                return showToolstrip;
            }
            set
            {
                showToolstrip = value;
                ShowHideToolstrip();
            }
        }

        public void AddNewItemType(object sender, EventArgs ea)
        {
            ToolStripItem item = (ToolStripItem)sender;
            Type t = (Type)item.Tag;
            Add(t);
        }

        /// <summary>
        /// Returns the collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetCollection<T>()
        {
            if (this.CollectionType != typeof(T))
            {
                throw new ArrayTypeMismatchException(string.Format("GetCollection T should match type {0} used by SetCollection<T>", this.CollectionType.ToString()));
            }
            List<T> collection = new List<T>();
            foreach (object item in this._collection)
            {
                collection.Add((T)item);
            }
            return collection;
        }

        public List<object> GetCollection()
        {
            List<object> collection = new List<object>();
            foreach (object item in this._collection)
            {
                collection.Add(item);
            }
            return collection;
        }

        public override void Refresh()
        {
            base.Refresh();
            ShowHideToolstrip();
            BuildCollectionList();
            SetSelected();
        }

        /// <summary>
        /// Sets the collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        public void SetCollection<T>(IEnumerable<T> collection)
        {
            this._collection.Clear();
            this.CollectionType = typeof(T);
            foreach (T item in collection)
                _collection.Add((object)item);
        }

        /// <summary>
        /// Sets the collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        public void SetCollection(List<object> collection)
        {
            this._collection.Clear();
            foreach (object item in collection)
            {
                this._collection.Add((object)item);
            }
        }

        private void Add(Type addtype)
        {
            RaiseEventBeforeItemSelected(_selectedObject);

            var _obj = Activator.CreateInstance(addtype);
            var _prop = _obj.GetType().GetProperty(CategoryProperty);

            if (_prop.CanWrite && _prop.PropertyType == typeof(string))
                _prop.SetValue(_obj, _lastCategory);

            CollectionListBoxEventArgs ea = new CollectionListBoxEventArgs(_obj);
            _collection.Add(ea.Item);

            OnAdd?.Invoke(this, ea);
            OnCollectionChanged?.Invoke(this, new EventArgs());

            BuildCollectionList();
            foreach (ListViewItem lvi in listViewSelected.Items)
            {
                if (lvi.Tag == ea.Item)
                {
                    lvi.Selected = true;
                    lvi.EnsureVisible();
                    ListViewClick();
                }
            }
            listViewSelected.Focus();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (_collectionTypes.Count > 0)
            {
                FormTypeSelector f = new FormTypeSelector();
                f.Types.AddRange(_collectionTypes);
                DialogResult r = f.ShowDialog();

                if (r == DialogResult.OK)
                    Add(f.SelectedType);

                return;
            }

            Add(CollectionType);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (AskBeforeDelete)
            {
                DialogResult dr = MessageBox.Show("Are you sure?", "Delete?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.Cancel)
                    return;
            }

            _selectedObject = null;
            foreach (ListViewItem lvi in listViewSelected.SelectedItems)
            {
                var o = (Object)lvi.Tag;
                CollectionListBoxDeleteEventArgs ea = new CollectionListBoxDeleteEventArgs(o);
                OnDelete(this, ea);
                if (ea.Cancel)
                    return;

                _collection.Remove(o);
                lvi.Remove();
                OnCollectionChanged?.Invoke(this, new EventArgs());
            }
            RaiseEventItemSelected(null);
            BuildCollectionList();
        }

        private void BtnRefresh_Click(object sender, EventArgs e) => Refresh();

        private void BuildCollectionList()
        {
            ShowButtons();

            GetImages(NameProperty, ImageProperty);

            var categorized = !string.IsNullOrEmpty(CategoryProperty);
            listViewSelected.Sorting = SortOrder.Descending;

            listViewSelected.Groups.Clear();
            listViewSelected.Items.Clear();

            var search = string.Format("*{0}*", toolStripTextBoxSearch.Text);

            foreach (Object f in _collection)
            {
                if (MatchPattern(f.ToString(), search) || String.IsNullOrWhiteSpace(search))
                {
                    var lvi = new ListViewItem();
                    if (categorized)
                    {
                        string cat = GetCategory(f, CategoryProperty);
                        ListViewGroup lvg = GetListViewGroup(cat, listViewSelected);
                        lvi.Group = lvg;
                    }
                    lvi.Name = f.GetType().Name;
                    if (string.IsNullOrEmpty(ImageProperty))
                    {
                        Color color = GetColor(f, ColorProperty);
                        lvi.ImageIndex = ImageListInsert(imageList, color, new Size(16, 16));
                    }
                    else
                    {
                        var name = GetProperty(f, NameProperty).GetValue(f) as string;

                        if (imageList.Images.ContainsKey(name))
                        {
                            lvi.ImageIndex = imageList.Images.IndexOfKey(name);
                        }
                        else
                        {
                            Color color = GetColor(f, ColorProperty);
                            lvi.ImageIndex = ImageListInsert(imageList, color, new Size(16, 16));
                        }
                    }
                    lvi.Text = f.ToString();
                    lvi.Tag = f;
                    listViewSelected.Items.Add(lvi);
                }
            }
            if (categorized)
            {
                listViewSelected.Sort();
            }
        }

        private void ButtonCopy_Click(object sender, EventArgs e)
        {
            if (listViewSelected.SelectedItems.Count == 0)
                return;

            if (Copy != null)
            {
                var args = new CollectionListBoxEventArgs(listViewSelected.SelectedItems[0].Tag);
                Copy(this, args);
            }
        }

        private void ButtonPaste_Click(object sender, EventArgs e)
        {
            if (Paste != null)
            {
                var args = new CollectionListBoxEventArgs(listViewSelected.SelectedItems[0].Tag);
                Paste(this, args);

                _collection.Add(args.Item);

                BuildCollectionList();
                foreach (ListViewItem lvi in this.listViewSelected.Items)
                {
                    if (lvi.Tag == args.Item)
                    {
                        lvi.Selected = true;
                        lvi.EnsureVisible();
                        ListViewClick();
                    }
                }
                listViewSelected.Focus();
            }
        }

        private void ButtonRefresh_Click(object sender, EventArgs e) => Refresh();

        private void CollectionEditor_Load(object sender, EventArgs e) => BuildCollectionList();

        /// <summary>
        /// Retrieves category property Value.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private string GetCategory(Object obj, string propertyName)
        {
            if (obj == null)
                return string.Empty;

            foreach (PropertyInfo property in obj.GetType().GetProperties())
            {
                if (property.Name.Equals(propertyName))
                {
                    if (property.PropertyType == typeof(string))
                    {
                        if (property.GetValue(obj, null) == null)
                            return DefaultCategoryName;
                        else
                        {
                            string value = (string)property.GetValue(obj, null).ToString();
                            if (string.IsNullOrEmpty(value))
                                return DefaultCategoryName;

                            return value;
                        }
                    }
                }
            }
            return DefaultCategoryName;
        }

        /// <summary>
        /// Retrieves category property Value.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyname"></param>
        /// <returns></returns>
        private Color GetColor(Object obj, string propertyname)
        {
            foreach (PropertyInfo property in obj.GetType().GetProperties())
            {
                if (property.Name.Equals(propertyname))
                {
                    if (property.PropertyType == typeof(Color))
                    {
                        if (property.GetValue(obj, null) == null)
                            return Color.White;
                        else
                            return (Color)property.GetValue(obj, null);
                    }
                }
            }
            return Color.White;
        }

        private void GetImages(string nameProperty, string imageProperty)
        {
            foreach (object obj in this._collection)
            {
                var _nameProperty = GetProperty(obj, nameProperty);
                var _imageProperty = GetProperty(obj, imageProperty);
                if (_nameProperty == null || _imageProperty == null)
                    return;

                if (_imageProperty.PropertyType != typeof(Image) || _nameProperty.PropertyType != typeof(string))
                    return;

                Image _image = _imageProperty.GetValue(obj) as Image;
                string _name = _nameProperty.GetValue(obj) as string;

                if (_image != null)
                    imageList.Images.Add(_name, _image);
            }
        }

        private PropertyInfo GetProperty(object obj, string name) => obj.GetType().GetProperties().Where(p => p.Name.Equals(name)).FirstOrDefault();

        private void ListViewClick()
        {
            RaiseEventBeforeItemSelected(_previous);
            RaiseEventBeforeItemsSelected();

            if (_selectedObjects == null)
                _selectedObjects = new List<object>();

            _selectedObjects.Clear();
            foreach (ListViewItem lvi in listViewSelected.SelectedItems)
                _selectedObjects.Add(lvi.Tag);

            if (listViewSelected.SelectedItems.Count == 1)
                _selectedObject = listViewSelected.SelectedItems[0].Tag;
            else
                _selectedObject = null;

            RaiseEventItemSelected(_selectedObject);
            RaiseEventItemsSelected(_selectedObjects);
        }

        private void ListViewSelected_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            AfterLabelEdit(this, new AfterLabelEditEventArgs() { NewLabel = e.Label, Tag = listViewSelected.Items[e.Item].Tag });
        }

        private void ListViewSelected_BeforeLabelEdit(object sender, LabelEditEventArgs e)
        {
        }

        private void ListViewSelected_Click(object sender, EventArgs e) => ListViewClick();

        private void ListViewSelected_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
            }

            ListViewClick();
        }

        private void RaiseEventBeforeItemSelected(object item)
        {
            EventHandler<CollectionListBoxEventArgs> handler = BeforeItemSelected;
            if (handler != null)
            {
                CollectionListBoxEventArgs ea = new CollectionListBoxEventArgs(item);
                handler(this, ea);
            }
        }

        private void RaiseEventBeforeItemsSelected() => BeforeItemsSelected?.Invoke(this, new EventArgs());

        private void RaiseEventItemSelected(object item)
        {
            EventHandler<CollectionListBoxEventArgs> handler = ItemSelected;
            if (handler != null)
            {
                CollectionListBoxEventArgs ea = new CollectionListBoxEventArgs(item);

                _lastCategory = GetCategory(item, CategoryProperty);
                handler(this, ea);
            }
        }

        private void RaiseEventItemsSelected(List<object> items)
        {
            buttonCut.Enabled = items.Count == 1;
            buttonCopy.Enabled = items.Count == 1;

            EventHandler<CollectionListBoxMultiEventArgs> handler = ItemsSelected;
            if (handler != null)
            {
                CollectionListBoxMultiEventArgs ea = new CollectionListBoxMultiEventArgs(items);
                handler(this, ea);
            }
        }

        private void resize()
        {
            splitContainer2.Left = 0;
            listViewSelected.Columns[0].Width = Width - 25;
        }

        private void SetSelected()
        {
            foreach (ListViewItem item in listViewSelected.Items)
            {
                if (item.Tag == _selectedObject)
                    item.Selected = true;
            }
        }

        private void ShowButtons()
        {
            separatorCopy.Visible = _showCopy;
            buttonCopy.Visible = _showCopy;
            buttonCut.Visible = _showCopy;
            buttonPaste.Visible = _showCopy;
            btnDelete.Visible = _showDelete;
            separatorDelete.Visible = _showDelete;
            btnAdd.Visible = _showAdd;
            separatorAdd.Visible = _showAdd;
            buttonRefresh.Visible = _showRefresh;
            separatorRefresh.Visible = _showRefresh;
            toolStripTextBoxSearch.Visible = _showSearch;
            toolStripLabelSearch.Visible = _showSearch;
        }

        private void ShowHideToolstrip()
        {
            if (showToolstrip)
            {
                splitContainer2.Panel1Collapsed = false;
                return;
            }
            splitContainer2.Panel1Collapsed = true;
        }

        private void ToolStripButton1_Click(object sender, EventArgs e)
        {
            if (listViewSelected.SelectedItems.Count == 0)
                return;

            if (Copy != null)
            {
                CollectionListBoxEventArgs args = new CollectionListBoxEventArgs(listViewSelected.SelectedItems[0].Tag);
                Copy(this, args);

                _selectedObject = null;
                foreach (ListViewItem lvi in listViewSelected.SelectedItems)
                {
                    Object o = (Object)lvi.Tag;
                    _collection.Remove(o);
                    lvi.Remove();
                    OnCollectionChanged?.Invoke(this, new EventArgs());
                }
                RaiseEventItemSelected(null);
                BuildCollectionList();
            }
        }

        private void ToolStripTextBox1_TextChanged(object sender, EventArgs e) => Refresh();

        private void UpdateListViewItems()
        {
            foreach (ListViewItem lvi in this.listViewSelected.Items)
            {
                lvi.Text = lvi.Tag.ToString();
                Color color = GetColor(lvi, this.ColorProperty);
                lvi.ImageIndex = ImageListInsert(imageList, color, new Size(16, 16));
            }
        }

        public class CollectionListBoxDeleteEventArgs : EventArgs
        {
            public CollectionListBoxDeleteEventArgs(object item)
            {
                Item = item;
            }

            public bool Cancel { get; set; } = false;
            public object Item { get; set; }
        }

        public class CollectionListBoxEventArgs : EventArgs
        {
            public CollectionListBoxEventArgs(object item)
            {
                Item = item;
            }

            public object Item { get; set; }
        }

        public class CollectionListBoxMultiEventArgs : EventArgs
        {
            private readonly List<object> _items;

            public CollectionListBoxMultiEventArgs(List<object> item)
            {
                this._items = item;
            }

            public IEnumerable<object> Items
            {
                get
                {
                    return this._items;
                }
            }
        }

        #region ListViewGroups

        private ListViewGroup GetListViewGroup(string pCategory, ListView pListView)
        {
            foreach (ListViewGroup lvg in pListView.Groups)
            {
                if (lvg.Name.Equals(pCategory))
                {
                    return lvg;
                }
            }

            ListViewGroup lvgNew = new ListViewGroup(pCategory, pCategory);
            pListView.Groups.Add(lvgNew);
            return lvgNew;
        }

        #endregion ListViewGroups

        #region ImageList Helpers

        public static Color ColorFromHtml(string htmlColor)
        {
            string color = htmlColor.Replace("#", "").PadRight(6, '0');

            string[] rgb = Enumerable.Range(0, color.Length / 2).Select(i => color.Substring(i * 2, 2)).ToArray();

            int r = int.Parse(rgb[0], NumberStyles.HexNumber);
            int g = int.Parse(rgb[1], NumberStyles.HexNumber);
            int b = int.Parse(rgb[2], NumberStyles.HexNumber);

            return Color.FromArgb(r, g, b);
        }

        public static Image CreateColorImage(Size size, Color color, Color bordercolor)
        {
            Rectangle rectangle = new Rectangle(0, 0, size.Width, size.Height);
            Image bitmap = new Bitmap(size.Width, size.Height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                using (Brush fillBrush = new SolidBrush(color))
                {
                    graphics.FillRectangle(fillBrush, rectangle);
                    using (Brush borderBrush = new SolidBrush(bordercolor))
                    {
                        using (Pen pen = new Pen(borderBrush, 1))
                        {
                            rectangle = new Rectangle(0, 0, rectangle.Width - 1, rectangle.Height - 1);
                            graphics.DrawRectangle(pen, rectangle);
                        }
                    }
                }
            }
            return bitmap;
        }

        public static int ImageListInsert(ImageList imageList, string htmlColor, Size iconSize)
        {
            Color color = ColorFromHtml(htmlColor);
            return ImageListInsert(imageList, color, iconSize);
        }

        public static int ImageListInsert(ImageList imageList, Color color, Size iconSize)
        {
            int index = imageList.Images.IndexOfKey(color.Name);
            if (index > 0)
            {
                return index; // image already exists.
            }
            Image image = CreateColorImage(iconSize, color, Color.LightGray);
            imageList.Images.Add(color.Name, image);
            index = imageList.Images.IndexOfKey(color.Name);
            return index;
        }

        public static class IconSize
        {
            public static readonly Size Size16x16 = new Size(16, 16);
            public static readonly Size Size32x32 = new Size(32, 32);
            public static readonly Size Size4x4 = new Size(4, 4);
            public static readonly Size Size64x64 = new Size(64, 64);
            public static readonly Size Size8x8 = new Size(8, 8);
        }

        #endregion ImageList Helpers

        #region Helpers

        public static bool MatchPattern(string s, string pattern)
        {
            if (pattern == null || s == null) return false;

            string[] patterns = pattern.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string subpattern in patterns)
            {
                string pat = string.Format("^{0}$", Regex.Escape(subpattern).Replace("\\*", ".*").Replace("\\?", "."));
                Regex regex = new Regex(pat, RegexOptions.IgnoreCase);
                if (regex.IsMatch(s))
                    return true;
            }
            return false;
        }

        #endregion Helpers

        #region Themes

        public void DarkTheme()
        {
            toolStripTextBoxSearch.BackColor = Color.FromArgb(255, 40, 40, 40);
            toolStripTextBoxSearch.ForeColor = Color.LightYellow;
            listViewSelected.BackColor = Color.FromArgb(255, 40, 40, 40);
            listViewSelected.ForeColor = Color.LightYellow;
            btnDelete.BackColor = Color.FromArgb(255, 100, 100, 100);
            btnDelete.ForeColor = Color.White;
            btnAdd.BackColor = Color.FromArgb(255, 100, 100, 100);
            btnAdd.ForeColor = Color.White;
            buttonRefresh.BackColor = Color.FromArgb(255, 100, 100, 100);
            buttonRefresh.ForeColor = Color.White;
            buttonCopy.BackColor = Color.FromArgb(255, 100, 100, 100);
            buttonCopy.ForeColor = Color.White;
            buttonCut.BackColor = Color.FromArgb(255, 100, 100, 100);
            buttonCut.ForeColor = Color.White;
            buttonPaste.BackColor = Color.FromArgb(255, 100, 100, 100);
            buttonPaste.ForeColor = Color.White;
            toolStripButton1.BackColor = Color.FromArgb(255, 100, 100, 100);
            toolStripButton1.ForeColor = Color.White;
            ForeColor = Color.White;
            BackColor = Color.FromArgb(255, 100, 100, 100);
            toolStrip1.BackColor = Color.FromArgb(255, 100, 100, 100);
        }

        public void HighContrastTheme()
        {
            toolStripTextBoxSearch.BackColor = Color.FromArgb(255, 10, 10, 10);
            toolStripTextBoxSearch.ForeColor = Color.LightYellow;
            listViewSelected.BackColor = Color.FromArgb(255, 10, 10, 10);
            listViewSelected.ForeColor = Color.LightYellow;
            btnDelete.BackColor = Color.FromArgb(255, 100, 100, 100);
            btnDelete.ForeColor = Color.White;
            btnAdd.BackColor = Color.FromArgb(255, 100, 100, 100);
            btnAdd.ForeColor = Color.White;
            buttonRefresh.BackColor = Color.FromArgb(255, 100, 100, 100);
            buttonRefresh.ForeColor = Color.White;
            buttonCopy.BackColor = Color.FromArgb(255, 100, 100, 100);
            buttonCopy.ForeColor = Color.White;
            buttonCut.BackColor = Color.FromArgb(255, 100, 100, 100);
            buttonCut.ForeColor = Color.White;
            buttonPaste.BackColor = Color.FromArgb(255, 100, 100, 100);
            buttonPaste.ForeColor = Color.White;
            toolStripButton1.BackColor = Color.FromArgb(255, 100, 100, 100);
            toolStripButton1.ForeColor = Color.White;
            ForeColor = Color.White;
            BackColor = Color.FromArgb(255, 100, 100, 100);
            toolStrip1.BackColor = Color.FromArgb(255, 100, 100, 100);
        }

        public void LightTheme()
        {
            toolStripTextBoxSearch.BackColor = Color.White;
            toolStripTextBoxSearch.ForeColor = Color.Black;
            listViewSelected.BackColor = Color.FromArgb(255, 40, 40, 40);
            listViewSelected.ForeColor = Color.LightYellow;
            btnDelete.BackColor = Color.FromArgb(255, 240, 240, 240);
            btnDelete.ForeColor = Color.Black;
            btnAdd.BackColor = Color.FromArgb(255, 240, 240, 240);
            btnAdd.ForeColor = Color.Black;
            buttonRefresh.BackColor = Color.FromArgb(255, 240, 240, 240);
            buttonRefresh.ForeColor = Color.Black;
            buttonCopy.BackColor = Color.FromArgb(255, 240, 240, 240);
            buttonCopy.ForeColor = Color.Black;
            buttonCut.BackColor = Color.FromArgb(255, 240, 240, 240);
            buttonCut.ForeColor = Color.Black;
            buttonPaste.BackColor = Color.FromArgb(255, 240, 240, 240);
            buttonPaste.ForeColor = Color.Black;
            toolStripButton1.BackColor = Color.FromArgb(255, 240, 240, 240);
            toolStripButton1.ForeColor = Color.Black;
            ForeColor = Color.Black;
            BackColor = Color.FromArgb(255, 240, 240, 240);
            toolStrip1.BackColor = Color.FromArgb(255, 240, 240, 240);
        }

        #endregion Themes
    }
}