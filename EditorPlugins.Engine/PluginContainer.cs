using EditorPlugins.Core;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace EditorPlugins.Engine
{
    public class PluginContainer
    {
        private readonly string _id;
        private IEditorPlugin _plugin;
        private PluginHelper _pluginHelper;

        public PluginContainer(IEditorPlugin plugin, string id)
        {
            _plugin = plugin;
            _pluginHelper = new PluginHelper(_plugin);
            _id = id;
            Category = plugin.Category;
            CustomShortcutKeys = _plugin.ShortcutKeys;
        }

        public string Category { get; set; }

        public Keys CustomShortcutKeys { get; set; }

        public string Description
        {
            get
            {
                return _pluginHelper.GetDescription();
            }
        }

        public string Id
        {
            get
            {
                return _id;
            }
        }

        [Browsable(false)]
        public Image Image
        {
            get
            {
                return GetPlugin().Image;
            }
        }

        public string Name
        {
            get
            {
                return GetPlugin().DisplayName;
            }
        }

        public IEditorPlugin GetPlugin()
        {
            return _plugin;
        }

        public override string ToString()
        {
            return _plugin.DisplayName;
        }
    }
}