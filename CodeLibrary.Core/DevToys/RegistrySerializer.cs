using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DevToys
{
    public sealed class RegistryAttribute : Attribute
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public bool ReadOnly { get; set; } = false;
    }

    public class RegistrySerializer<T> where T : class, new()
    {
        private Dictionary<string, RegistryAttribute> _Keys = new Dictionary<string, RegistryAttribute>();
        private List<string> _Names;
        private Dictionary<string, PropertyInfo> _Properties = new Dictionary<string, PropertyInfo>();

        public RegistrySerializer()
        { }

        public static object Convert(object value, Type target, out bool succes)
        {
            succes = true;
            target = Nullable.GetUnderlyingType(target) ?? target;
            try
            {
                return (target.IsEnum) ? Enum.Parse(target, value.ToString()) : (value == null) ? null : System.Convert.ChangeType(value, target);
            }
            catch { }
            succes = false;
            return null;
        }

        public T Read()
        {
            Init();
            T _retval = new T();
            foreach (string name in _Names)
            {
                RegistryAttribute _key = _Keys[name];
                object _value = Registry.GetValue(_key.Key, _key.Name, _Properties[name].GetValue(_retval));
                bool _succes;
                object _converted = Convert(_value, _Properties[name].PropertyType, out _succes);
                if (_succes && _value != null)
                    _Properties[name].SetValue(_retval, _converted);
            }
            return _retval;
        }

        public void Write(T item)
        {
            Init();
            foreach (string name in _Names)
            {
                RegistryAttribute _key = _Keys[name];
                if (!_key.ReadOnly)
                    Registry.SetValue(_key.Key, _key.Name, _Properties[name].GetValue(item));
            }
        }

        private void Init()
        {
            if (_Properties.Count > 0)
                return;

            var _type = typeof(T);
            _Properties = _type.GetProperties()
                .Where(p => p.GetCustomAttribute(typeof(RegistryAttribute)) != null)
                .Select(p => new { Value = p, Key = p.Name })
                .ToDictionary(p => p.Key, p => p.Value);

            _Names = _type.GetProperties()
                .Where(p => p.GetCustomAttribute(typeof(RegistryAttribute)) != null)
                .Select(p => p.Name)
                .ToList();

            _Keys = _type.GetProperties()
                .Where(p => p.GetCustomAttribute(typeof(RegistryAttribute)) != null)
                .Select(p => new { Value = p.GetCustomAttribute(typeof(RegistryAttribute)) as RegistryAttribute, Key = p.Name })
                .ToDictionary(p => p.Key, p => p.Value);
        }
    }
}