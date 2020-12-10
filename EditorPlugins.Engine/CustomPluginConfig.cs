using System;
using System.Windows.Forms;

namespace EditorPlugins.Engine
{
    public class CustomPluginConfig
    {
        public string Category { get; set; }
        public Keys CustomShortCutKeys { get; set; }
        public Guid PluginId { get; set; }
    }
}