using CodeLibrary.Controls;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace EditorPlugins.Engine
{
    public partial class PluginConfigurator : Form
    {
        public PluginConfigurator()
        {
            InitializeComponent();
            pluginListBox.CategoryProperty = "Category";
            pluginListBox.NameProperty = "Name";
            pluginListBox.ImageProperty = "Image";
            this.Load += PluginConfigurator_Load;
        }

        public List<PluginContainer> Plugins { get; set; }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PluginConfigurator_Load(object sender, EventArgs e)
        {
            pluginListBox.SetCollection<PluginContainer>(Plugins);
            pluginListBox.Refresh();
        }

        private void PluginListBox_ItemSelected(object sender, CollectionListBox.CollectionListBoxEventArgs e)
        {
            propertyGrid.SelectedObject = e.Item;
        }
    }
}