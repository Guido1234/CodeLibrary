using EditorPlugins.Core;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace EditorPlugins.Engine
{
    public class PluginHelper
    {
        private IEditorPlugin _plugin;

        public PluginHelper(IEditorPlugin plugin)
        {
            _plugin = plugin; 
        }

        public string GetDescription()
        {
            var _description = _plugin.GetType().GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (_description == null)
                return string.Empty;

            return _description.Description;
        }

        public void LoadSettings()
        {
            if (!File.Exists(Filename()))
                return;

            var json = File.ReadAllText(Filename());
            var _keyValuePairs = Utils.FromJson<Dictionary<string, object>>(json);

            foreach (string propertyName in _keyValuePairs.Keys)
            {
                object _value = _keyValuePairs[propertyName];

                PropertyInfo property = _plugin.GetType().GetProperty(propertyName);
                if (property == null)
                    continue;

                if (!property.CanRead && !property.CanWrite)
                    continue;

                if (!Utils.IsSimpleType(property.PropertyType))
                    continue;

                property.SetValue(_plugin, _value);
            }
        }

        public void SaveSettings()
        {
            var _keyValuePairs = new Dictionary<string, object>();
            var _interfaceProperties = typeof(IEditorPlugin).GetProperties().ToDictionary(p => p.Name);
            var _pluginProperties = _plugin.GetType().GetProperties().ToList();

            foreach (PropertyInfo property in _pluginProperties)
            {
                if (_interfaceProperties.ContainsKey(property.Name))
                    continue;

                if (!property.CanRead && !property.CanWrite)
                    continue;

                if (!Utils.IsSimpleType(property.PropertyType))
                    continue;

                object _value = property.GetValue(_plugin);
                _keyValuePairs.Add(property.Name, _value);
            }

            if (_keyValuePairs.Count == 0)
                return;

            var _json = Utils.ToJson(_keyValuePairs);
            File.WriteAllText(Filename(), _json);
        }

        private string Filename() => Path.Combine(Utils.AppDataPath, $"{_plugin.Id}.json");

    }
}