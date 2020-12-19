using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace DevToys
{
    public static class DictionaryListExtensions
    {
        public static DictionaryList<TPOCO, TPRIMARYKEY> ToDictionaryList<TPOCO, TPRIMARYKEY>(this List<TPOCO> list, Func<TPOCO, TPRIMARYKEY> predicate)
        {
            DictionaryList<TPOCO, TPRIMARYKEY> fastlist = new DictionaryList<TPOCO, TPRIMARYKEY>(predicate);
            fastlist.AddRange(list);
            return fastlist;
        }

        public static DictionaryList<TPOCO, TPRIMARYKEY> ToDictionaryList<TPOCO, TPRIMARYKEY>(this IEnumerable<TPOCO> ienumerable, Func<TPOCO, TPRIMARYKEY> predicate)
        {
            DictionaryList<TPOCO, TPRIMARYKEY> fastlist = new DictionaryList<TPOCO, TPRIMARYKEY>(predicate);
            fastlist.AddRange(ienumerable.ToList());
            return fastlist;
        }

        public static DictionaryList<TPOCO, TPRIMARYKEY> ToDictionaryList<TPOCO, TPRIMARYKEY>(this Dictionary<TPRIMARYKEY, TPOCO> dictionary, Func<TPOCO, TPRIMARYKEY> predicate)
        {
            DictionaryList<TPOCO, TPRIMARYKEY> fastlist = new DictionaryList<TPOCO, TPRIMARYKEY>(predicate);
            fastlist.AddRange(dictionary.Select(p => p.Value).ToList());
            return fastlist;
        }

        public static DictionaryList<TPOCO, TPRIMARYKEY> ToDictionaryList<TPOCO, TPRIMARYKEY>(this TPOCO[] array, Func<TPOCO, TPRIMARYKEY> predicate)
        {
            DictionaryList<TPOCO, TPRIMARYKEY> fastlist = new DictionaryList<TPOCO, TPRIMARYKEY>(predicate);
            fastlist.AddRange(array);
            return fastlist;
        }
    }

    /// <summary>
    /// Combines A Dictionary and Multiple Lookup objects as one List Object.
    /// </summary>
    public sealed class DictionaryList<TPOCO, TPRIMARYKEY> : IEnumerable<TPOCO>, ICollection<TPOCO>
    {
        private LookupItem<TPRIMARYKEY> _AlternativeIndexer = null;
        private int _Autonumber = 0;
        private Expression<Func<TPOCO, int>> _AutoNumberField;
        private int _AutonumberStepSize = 1;
        private TimeSpan _ExpirationTime = TimeSpan.Zero;
        private readonly Func<TPOCO, TPRIMARYKEY> _IndexFunctionPrimaryKey;
        private Dictionary<TPRIMARYKEY, TPOCO> _Items = new Dictionary<TPRIMARYKEY, TPOCO>();
        private Dictionary<string, LookupItem<dynamic>> _Lookupfunctions;
        private bool _MustRebuild = false; // indicates whether the lookups should be rebuild-ed.
        private readonly Dictionary<TPRIMARYKEY, int> _PhysicalIndexes1 = new Dictionary<TPRIMARYKEY, int>();
        private readonly Dictionary<int, TPRIMARYKEY> _PhysicalIndexes2 = new Dictionary<int, TPRIMARYKEY>();
        private bool _SupressPropertyChanged = false;

        public DictionaryList(Func<TPOCO, TPRIMARYKEY> predecate) => _IndexFunctionPrimaryKey = predecate;

        public event EventHandler<EventArgs> CollectionChanged = delegate { };

        public event EventHandler<ItemsAddedEventArgs<TPOCO>> ItemsAdded = delegate { };

        public event EventHandler<ItemsRemovedEventArgs<TPOCO>> ItemsRemoved = delegate { };

        public int Count => _Items.Count;

        public bool IsReadOnly => false;

        public bool RaiseEvents { get; set; } = true;
        public bool UseIndexer { get; set; } = true;

        private bool MustRebuild
        {
            get
            {
                return _MustRebuild;
            }
            set
            {
                if (value == true && _MustRebuild == false)
                    CollectionChanged?.Invoke(this, new EventArgs());

                _MustRebuild = value;
            }
        }

        public TPOCO this[TPRIMARYKEY primaryKey] => Get(primaryKey);

        public static DictionaryList<TPOCO, TPRIMARYKEY> Create(Func<TPOCO, TPRIMARYKEY> indexfunction) => new DictionaryList<TPOCO, TPRIMARYKEY>(indexfunction);

        public void Add(TPOCO item) => AddItem(item);

        public void AddRange(List<TPOCO> newitems) => AddItemRange(newitems);

        public void AddRange(params TPOCO[] newitems) => AddItemRange(newitems);

        public void AddRange(IEnumerable<TPOCO> newitems) => AddItemRange(newitems);

        public void Clear()
        {
            if (_Items == null)
                _Items = new Dictionary<TPRIMARYKEY, TPOCO>();

            _Items.Clear();
            ClearLookups();
        }

        public bool Contains(TPOCO item) => (item == null) ? false : ContainsKey(GetRealPrimaryKey(_IndexFunctionPrimaryKey(item)));

        public bool ContainsKey(TPRIMARYKEY primaryKey) => (primaryKey == null) ? false : _Items.ContainsKey(GetRealPrimaryKey(primaryKey));

        public bool ContainsKey(TPOCO item) => (item == null) ? false : Contains(item);

        public bool ContainsOneOffKeys(params TPRIMARYKEY[] primaryKeys)
        {
            foreach (TPRIMARYKEY primaryKey in primaryKeys)
            {
                if (ContainsKey(primaryKey))
                    return true;
            }
            return false;
        }

        public void CopyTo(TPOCO[] array, int arrayIndex) => _Items.Select(p => p.Value).ToList().CopyTo(array, arrayIndex);

        public TPOCO Get(TPRIMARYKEY primaryKey)
        {
            primaryKey = GetRealPrimaryKey(primaryKey);

            if (primaryKey == null || !_Items.ContainsKey(primaryKey))
                return default;

            return _Items[primaryKey];
        }

        public TPOCO Get(TPOCO item) => Get(_IndexFunctionPrimaryKey(item));

        public IEnumerator<TPOCO> GetEnumerator() => _Items.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _Items.GetEnumerator();

        public override int GetHashCode()
        {
            StringBuilder _sb = new StringBuilder();

            foreach (TPOCO item in _Items.Values)
                _sb.Append(item.GetHashCode());

            return _sb.ToString().GetHashCode();
        }

        public int GetIndex(TPRIMARYKEY key)
        {
            if (MustRebuild)
            {
                ClearLookups();
                Indexer();
            }

            if (!_PhysicalIndexes1.ContainsKey(key))
                return -1;

            return _PhysicalIndexes1[key];
        }

        public TPRIMARYKEY GetKey(int index)
        {
            if (MustRebuild)
            {
                ClearLookups();
                Indexer();
            }

            if (!_PhysicalIndexes2.ContainsKey(index))
                return default;

            return _PhysicalIndexes2[index];
        }

        public IEnumerable<TPOCO> GetRange(List<TPRIMARYKEY> primaryKeys)
        {
            foreach (TPRIMARYKEY key in primaryKeys)
            {
                TPOCO item = Get(key);
                if (item != null)
                    yield return item;
            }
        }

        public IEnumerable<TPOCO> GetRange(params TPRIMARYKEY[] primaryKeys)
        {
            foreach (TPRIMARYKEY key in primaryKeys)
            {
                TPOCO item = Get(key);
                if (item != null)
                    yield return item;
            }
        }

        public IEnumerable<TPOCO> GetRange(IEnumerable<TPRIMARYKEY> primaryKeys)
        {
            foreach (TPRIMARYKEY key in primaryKeys)
            {
                TPOCO item = Get(key);
                if (item != null)
                    yield return item;
            }
        }

        public bool IsInitialized() => (_Items != null);

        public TPRIMARYKEY NextKey(int index)
        {
            if (MustRebuild)
            {
                ClearLookups();
                Indexer();
            }

            if (!_PhysicalIndexes2.ContainsKey(index + 1))
                return default;

            return _PhysicalIndexes2[index + 1];
        }

        public TPRIMARYKEY PrevKey(int index)
        {
            if (MustRebuild)
            {
                ClearLookups();
                Indexer();
            }

            if (!_PhysicalIndexes2.ContainsKey(index - 1))
                return default;

            return _PhysicalIndexes2[index - 1];
        }

        public void Refresh() => _MustRebuild = true;

        public bool Remove(TPOCO item) => Remove(_IndexFunctionPrimaryKey(item));

        public bool Remove(TPRIMARYKEY primaryKey)
        {
            TPRIMARYKEY realprimaryKey = GetRealPrimaryKey(primaryKey);

            if (!_Items.ContainsKey(realprimaryKey))
                return false;

            TPOCO item = Get(primaryKey);
            MustRebuild = true;
            bool retval = _Items.Remove(realprimaryKey);

            if (retval)
                if (RaiseEvents)
                    ItemsRemoved(this, new ItemsRemovedEventArgs<TPOCO>(new List<TPOCO>() { item }));

            return retval;
        }

        public List<TPOCO> RemoveRange(Func<TPOCO, bool> predicate)
        {
            List<TPRIMARYKEY> primaryKeys = new List<TPRIMARYKEY>();
            foreach (TPOCO item in _Items.Values)
            {
                if (predicate(item))
                {
                    TPRIMARYKEY id = GetRealPrimaryKey(_IndexFunctionPrimaryKey(item));
                    primaryKeys.Add(id);
                }
            }
            return RemoveRange((IEnumerable<TPRIMARYKEY>)primaryKeys);
        }

        public List<TPOCO> RemoveRange(IEnumerable<TPRIMARYKEY> primaryKeys)
        {
            MustRebuild = true;
            List<TPOCO> removed = new List<TPOCO>();
            foreach (TPRIMARYKEY primaryKey in primaryKeys)
            {
                if (_Items.ContainsKey(primaryKey))
                {
                    TPOCO item = _Items[primaryKey];
                    removed.Add(item);
                    UnRegisterPropertyChanged(item);
                    _Items.Remove(primaryKey);
                }
            }

            if (RaiseEvents)
                ItemsRemoved(this, new ItemsRemovedEventArgs<TPOCO>(removed));

            return removed;
        }

        public List<TPOCO> RemoveRange(params TPRIMARYKEY[] primaryKeys) => RemoveRange((IEnumerable<TPRIMARYKEY>)primaryKeys.ToList());

        public List<TPOCO> RemoveRange(List<TPRIMARYKEY> primaryKeys) => RemoveRange((IEnumerable<TPRIMARYKEY>)primaryKeys);

        public void SetAlternativeIndexer(Func<TPOCO, TPRIMARYKEY> predecate)
        {
            _AlternativeIndexer = new LookupItem<TPRIMARYKEY> { LookupFunction = predecate, Lookup = null };
        }

        public void SetAutoNumber(Expression<Func<TPOCO, int>> predecate) => _AutoNumberField = predecate;

        public void SetAutoNumber(Expression<Func<TPOCO, int>> predecate, int stepSize, int startNumber)
        {
            _AutoNumberField = predecate;
            _Autonumber = startNumber;
            _AutonumberStepSize = stepSize;
        }

        public TPOCO[] ToArray() => _Items.Values.ToArray();

        public List<TPOCO> ToList() => _Items.Values.ToList();

        private void AddItem(TPOCO item)
        {
            if (_Items == null)
                _Items = new Dictionary<TPRIMARYKEY, TPOCO>();

            TPRIMARYKEY primaryKey = _IndexFunctionPrimaryKey.Invoke(item);

            RegisterPropertyChanged(item);

            _Items.Add(primaryKey, item);
            IncreaseAutoNumber(item);

            if (RaiseEvents)
                ItemsAdded(this, new ItemsAddedEventArgs<TPOCO>(new List<TPOCO>() { item }));

            MustRebuild = true;
        }

        private void AddItemRange(IEnumerable<TPOCO> newitems)
        {
            if (_Items == null)
                _Items = new Dictionary<TPRIMARYKEY, TPOCO>();

            _SupressPropertyChanged = true;

            IncreaseAutoNumber(newitems);

            foreach (TPOCO item in newitems)
            {
                TPRIMARYKEY primaryKey = _IndexFunctionPrimaryKey.Invoke(item);

                _Items.Add(primaryKey, item);
                RegisterPropertyChanged(item);
            }

            if (RaiseEvents)
                ItemsAdded(this, new ItemsAddedEventArgs<TPOCO>(newitems.ToList()));

            _SupressPropertyChanged = false;

            MustRebuild = true;
        }

        private TPRIMARYKEY GetRealPrimaryKey(TPRIMARYKEY primaryKey)
        {
            if (_Items.ContainsKey(primaryKey))
                return primaryKey; // the items collection contains the primary key, return it.

            if (_AlternativeIndexer == null)
                return primaryKey; // no alternative indexer return normal primary key.

            // Translate the specified key to the normal key.
            if (_AlternativeIndexer.Lookup == null)
                _AlternativeIndexer.Lookup = this.ToLookup(_AlternativeIndexer.LookupFunction);

            TPOCO item = _AlternativeIndexer.Lookup[primaryKey].FirstOrDefault();

            if (item != null)
            {
                TPRIMARYKEY realKey = _IndexFunctionPrimaryKey(item);
                return realKey;
            }
            return primaryKey;
        }

        private void IncreaseAutoNumber(TPOCO item)
        {
            if (_AutoNumberField == null)
                return;

            var memberSelectorExpression = _AutoNumberField.Body as MemberExpression;
            if (memberSelectorExpression != null)
            {
                PropertyInfo property = memberSelectorExpression.Member as PropertyInfo;
                if (property != null)
                {
                    _Autonumber += _AutonumberStepSize;
                    property.SetValue(item, _Autonumber, null);
                }
            }
        }

        private void IncreaseAutoNumber(IEnumerable<TPOCO> items)
        {
            if (_AutoNumberField == null)
                return;

            foreach (TPOCO item in items)
                IncreaseAutoNumber(item);
        }

        private void Indexer()
        {
            if (!UseIndexer)
                return;

            _PhysicalIndexes1.Clear();
            _PhysicalIndexes2.Clear();
            int _index = 0;
            foreach (TPOCO item in _Items.Values)
            {
                TPRIMARYKEY primaryKey = _IndexFunctionPrimaryKey.Invoke(item);
                _PhysicalIndexes1.Add(primaryKey, _index);
                _PhysicalIndexes2.Add(_index, primaryKey);
                _index++;
            }
        }

        private void Propertychanged_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_SupressPropertyChanged || MustRebuild)
                return;

            foreach (LookupItem<dynamic> lookup in _Lookupfunctions.Select(p => p.Value))
            {
                if (lookup.RelatedProperties == null)
                {
                    MustRebuild = true;
                    return;
                }

                if (lookup.RelatedProperties.Contains(e.PropertyName))
                {
                    MustRebuild = true;
                    return;
                }
            }
        }

        private void RegisterPropertyChanged(TPOCO item)
        {
            if (item is INotifyPropertyChanged)
                ((INotifyPropertyChanged)item).PropertyChanged += Propertychanged_PropertyChanged;
        }



        private void UnRegisterPropertyChanged(TPOCO item)
        {
            if (item is INotifyPropertyChanged)
            {
                INotifyPropertyChanged propertychanged = item as INotifyPropertyChanged;
                propertychanged.PropertyChanged -= Propertychanged_PropertyChanged;
            }
        }

        private class LookupItem<TLOOKUPKEY>
        {
            public ILookup<TLOOKUPKEY, TPOCO> Lookup { get; set; }

            public Func<TPOCO, TLOOKUPKEY> LookupFunction { get; set; }

            public string Name { get; set; }

            public string[] RelatedProperties { get; set; }
        }

        #region Lookups

        public void ClearLookups()
        {
            if (_Lookupfunctions != null)
                foreach (LookupItem<dynamic> lookupitem in _Lookupfunctions.Select(p => p.Value))
                    lookupitem.Lookup = null;

            if (_AlternativeIndexer != null)
                _AlternativeIndexer.Lookup = null;

            MustRebuild = false;
        }

        public IEnumerable<TPOCO> Lookup(string name, dynamic value)
        {
            if (MustRebuild)
            {
                ClearLookups();
                Indexer();
            }

            LookupItem<dynamic> item = _Lookupfunctions[name];

            if (item == null)
                throw new ArgumentException("FastList Lookup not found !");

            if (item.Lookup == null)
                item.Lookup = this.ToLookup(item.LookupFunction);

            IEnumerable<TPOCO> result = item.Lookup[value];
            return result;
        }

        public IEnumerable<TPOCO> LookupRange(string name, params dynamic[] keys)
        {
            foreach (int key in keys)
            {
                foreach (TPOCO item in Lookup(name, key))
                    yield return item;
            }
        }

        public void RegisterLookup(string name, Func<TPOCO, dynamic> lookupIndexer)
        {
            if (_Lookupfunctions == null)
                _Lookupfunctions = new Dictionary<string, LookupItem<dynamic>>();

            LookupItem<dynamic> lkp = new LookupItem<dynamic> { Name = name, LookupFunction = lookupIndexer, Lookup = null };
            _Lookupfunctions.Add(name, lkp);
        }

        public void RegisterLookup(string name, Func<TPOCO, dynamic> lookupIndexer, params string[] relatedproperties)
        {
            if (_Lookupfunctions == null)
                _Lookupfunctions = new Dictionary<string, LookupItem<dynamic>>();

            LookupItem<dynamic> lkp = new LookupItem<dynamic> { Name = name, LookupFunction = lookupIndexer, Lookup = null, RelatedProperties = relatedproperties };
            _Lookupfunctions.Add(name, lkp);
        }

        #endregion Lookups
    }

    public class ItemsAddedEventArgs<TPOCO> : EventArgs
    {
        private readonly IList<TPOCO> _Items;

        public ItemsAddedEventArgs(IList<TPOCO> items) => _Items = items;

        public IEnumerable<TPOCO> Added => _Items;
    }

    public class ItemsRemovedEventArgs<TPOCO> : EventArgs
    {
        private readonly IList<TPOCO> _Items;

        public ItemsRemovedEventArgs(IList<TPOCO> items) => _Items = items;

        public IEnumerable<TPOCO> Removed => _Items;
    }
}