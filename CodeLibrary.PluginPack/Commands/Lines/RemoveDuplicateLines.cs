using CodeLibrary.PluginPack.Common;
using EditorPlugins.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack
{
    [Description("Removes all duplicate lines from selection.")]
    public class RemoveDuplicateLines : IEditorPlugin
    {
        public bool IsExtension => false;
        public string Category => "Lines";
        public string DisplayName => "Remove Duplicate Lines";
        public Guid Id => Guid.Parse("96d9b1b9-2cc7-4726-8160-d3fb1fa33385");
        public Image Image => null;
        public bool OmitResult => false;
        public Keys ShortcutKeys => Keys.None;

        public void Apply(ISelInfo sel)
        {
            string text = sel.SelectedText;

            HashSet<string> _hashsetLines = new HashSet<string>();
            StringBuilder _sb = new StringBuilder();

            string[] _lines = Utils.Lines(text);

            foreach (string line in _lines)
            {
                if (!_hashsetLines.Contains(line))
                    _hashsetLines.Add(line);
            }

            foreach (string line in _hashsetLines)
                _sb.AppendLine(line);

            sel.SelectedText = _sb.ToString();
        }

        public bool Configure()
        {
            return false;
        }
    }
}