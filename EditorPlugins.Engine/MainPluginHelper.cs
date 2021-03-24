using DevToys;
using EditorPlugins.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace EditorPlugins.Engine
{
    public class MainPluginHelper
    {
        private readonly Dictionary<string, ToolStripMenuItem> _CategoryMenus = new Dictionary<string, ToolStripMenuItem>();
        private readonly ToolStripMenuItem _contextMenuItem;
        private readonly ToolStripMenuItem editorMenuItem;
        private readonly ToolStripMenuItem _ExtensionMenu;
        private readonly Dictionary<string, ToolStripItem> _MenuItems = new Dictionary<string, ToolStripItem>();
        private readonly DictionaryList<PluginContainer, string> _Plugins = new DictionaryList<PluginContainer, string>(p => p.Id);
        private TextEditorContainer editor;

        public MainPluginHelper(TextEditorContainer textEditor, ToolStripMenuItem editMenuPluginMenuItem, ToolStripMenuItem contextMenuPluginMenuItem, ToolStripMenuItem extensionMenu)
        {
            editor = textEditor;
            editorMenuItem = editMenuPluginMenuItem;
            _contextMenuItem = contextMenuPluginMenuItem;
            _ExtensionMenu = extensionMenu;
            LoadAssemblies();
            CreateMenus();
        }

        public List<PluginContainer> Plugins
        {
            get
            {
                return _Plugins.Select(p => p).ToList();
            }
        }

        public void CreateMenus()
        {
            LoadCustomSettings();

            _CategoryMenus.Clear();

            CreateMenu(editorMenuItem, true, false);
            CreateMenu(_contextMenuItem, false, false);
            CreateMenu(_ExtensionMenu, true, true);
        }

        public void LoadCustomSettings()
        {
            string _file = Utils.ConfigFile();

            if (!File.Exists(_file))
                return;

            string json = File.ReadAllText(_file);
            List<CustomPluginConfig> _configs = Utils.FromJsonToList<CustomPluginConfig>(json);
            foreach (CustomPluginConfig config in _configs)
            {
                if (_Plugins.ContainsKey(config.PluginId.ToString()))
                {
                    PluginContainer pluginContainer = _Plugins[config.PluginId.ToString()];
                    pluginContainer.CustomShortcutKeys = config.CustomShortCutKeys;
                    pluginContainer.Category = config.Category;
                }
            }
        }

        public void SaveCustomSettings()
        {
            var customSettings = _Plugins.Select(p => new CustomPluginConfig() { CustomShortCutKeys = p.CustomShortcutKeys, PluginId = p.GetPlugin().Id, Category = p.Category });
            var _json = Utils.ToJson(customSettings);

            string _file = Utils.ConfigFile();

            FileInfo fileInfo = new FileInfo(_file);
            if (!fileInfo.Directory.Exists)
                fileInfo.Directory.Create();

            File.WriteAllText(_file, _json);
        }

        public void SetMenuState(ToolStripMenuItem menu)
        {
            foreach (PluginContainer pluginContainer in _Plugins)
            {
                string _name = $"{menu.Name}_{pluginContainer.Id}";
                if (_MenuItems.ContainsKey(_name))
                {
                    ToolStripItem _menu = _MenuItems[_name];
                }
            }
        }

        private void _menuItem_Click(object sender, EventArgs e)
        {
            _menuItem_LeftClick(sender, e);
        }

        private void _menuItem_LeftClick(object sender, EventArgs e)
        {
            ToolStripItem _menuItem = sender as ToolStripItem;
            string key = _menuItem.Tag as string;
            IEditorPlugin _plugin = _Plugins.Get(key).GetPlugin();

            PluginHelper _pluginSettingsHelper = new PluginHelper(_plugin);

            _pluginSettingsHelper.LoadSettings();

            if (_plugin.IsExtension)
            {

                bool _result = _plugin.Configure();
                if (!_result)
                    return;

                _pluginSettingsHelper.SaveSettings();

            }

            _pluginSettingsHelper.LoadSettings();

            SelInfo _selInfo = new SelInfo()
            {
                Text = editor.Editor.Text,
                SelectedText = editor.Editor.SelectedText,
                CurrentLine = editor.Editor.CurrentLine()
            };

            try
            {
                if (!_plugin.OmitResult)
                {
                    _plugin.Apply(_selInfo);
                    editor.Editor.SelectedText = _selInfo.SelectedText;
                }
                else
                {
                    _plugin.Apply(_selInfo);
                }
            }
            catch (Exception)
            {
            }

            _pluginSettingsHelper.SaveSettings();
            
        }

        private void _menuItem_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _menuItem_RightClick(sender, e);
            }
        }

        private void _menuItem_RightClick(object sender, EventArgs e)
        {
            ToolStripItem _menuItem = sender as ToolStripItem;
            string key = _menuItem.Tag as string;
            IEditorPlugin _plugin = _Plugins.Get(key).GetPlugin();
            
            if (_plugin.IsExtension)
            {
                return;
            }

            PluginHelper _pluginSettingsHelper = new PluginHelper(_plugin);

            _pluginSettingsHelper.LoadSettings();

            bool _result = _plugin.Configure();
            if (!_result)
                return;

            _pluginSettingsHelper.SaveSettings();
        }

        private void CreateCategoryMenuItems(ToolStripMenuItem menu, bool extensions)
        {
            var _categories = _Plugins.Where(p => p.IsExtension == extensions).Select(p => p.GetPlugin().Category).Distinct();
            foreach (string cat in _categories)
            {
                if (string.IsNullOrEmpty(cat))
                    continue;

                ToolStripMenuItem _menuItem = menu.DropDownItems.Add(cat) as ToolStripMenuItem;
                string _name = $"{menu.Name}_{cat}";
                _CategoryMenus.Add(_name, _menuItem);
            }
        }

        private void CreateMenu(ToolStripMenuItem menu, bool setShortCutKeys, bool extensions)
        {
            if (menu == null)
                return;

            menu.DropDownItems.Clear();

            CreateCategoryMenuItems(menu, extensions);

            foreach (PluginContainer pluginContainer in _Plugins.Where(p => p.IsExtension == extensions))
            {
                ToolStripMenuItem _menuItem;

                string _catname = $"{menu.Name}_{pluginContainer.GetPlugin().Category}";

                if (_CategoryMenus.ContainsKey(_catname))
                    _menuItem = _CategoryMenus[_catname].DropDownItems.Add(pluginContainer.GetPlugin().DisplayName) as ToolStripMenuItem;
                else
                    _menuItem = menu.DropDownItems.Add(pluginContainer.GetPlugin().DisplayName) as ToolStripMenuItem;

                if (setShortCutKeys)
                    _menuItem.ShortcutKeys = pluginContainer.CustomShortcutKeys;

                if (pluginContainer.GetPlugin().Image != null)
                    _menuItem.Image = pluginContainer.GetPlugin().Image;

                _menuItem.ToolTipText = pluginContainer.Description;

                _menuItem.Visible = true;
                _menuItem.Tag = pluginContainer.Id;
                _menuItem.Click += _menuItem_Click;
                _menuItem.MouseDown += _menuItem_MouseDown;
                string _name = $"{menu.Name}_{pluginContainer.Id}";
                _MenuItems.Add(_name, _menuItem);
            }
        }

        private void LoadAssemblies()
        {
            String _pluginDirectory = Path.Combine(Application.StartupPath, "Plugins");
            DirectoryInfo _directory = new DirectoryInfo(_pluginDirectory);
            if (!_directory.Exists)
                return;

            FileInfo[] _files = _directory.GetFiles("*.dll");

            foreach (FileInfo file in _files)
            {
                Assembly _assembly = Assembly.LoadFrom(file.FullName);
                Type[] _types = _assembly.GetTypes();
                foreach (Type type in _types)
                {
                    if (!Utils.HasInterface(type, typeof(IEditorPlugin)))
                        continue;

                    IEditorPlugin _plugin = Activator.CreateInstance(type) as IEditorPlugin;
                    PluginContainer _pluginref = new PluginContainer(_plugin, _plugin.Id.ToString());
                    _Plugins.Add(_pluginref);
                }
            }
        }
    }
}