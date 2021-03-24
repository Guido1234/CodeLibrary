using EditorPlugins.Core;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack.Commands.Encoding
{
    [Description("Import file as Base 64")]
    public class ImportAsBase64 : IEditorPlugin
    {
        public bool IsExtension => false;
        public string Category => "Encoding";
        public string DisplayName => "Import file as Base 64";
        public Guid Id => Guid.Parse("f0e9336d-ff22-411c-8d56-f4404100060a");
        public Image Image => null;
        public bool OmitResult => false;
        public Keys ShortcutKeys => Keys.None;

        public void Apply(ISelInfo sel)
        {
            sel.SelectedText = Import();
        }

        public bool Configure()
        {
            return false;
        }

        private static string Import()
        {
            OpenFileDialog _dialog = new OpenFileDialog();
            _dialog.Title = "Select File";
            var _dialogResult = _dialog.ShowDialog();
            if (_dialogResult == DialogResult.OK)
            {
                byte[] _bytes = File.ReadAllBytes(_dialog.FileName);
                return Convert.ToBase64String(_bytes);
            }
            return string.Empty;
        }
    }
}