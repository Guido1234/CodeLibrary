using CodeLibrary.PluginPack.Common;
using EditorPlugins.Core;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack
{
    [Description("Swaps left hand to right hand (left=right; => right=left;)")]
    public class SwapAssignments : IEditorPlugin
    {
        public bool IsExtension => false;
        public string Category => "C#";
        public string DisplayName => "Swap Assignments";
        public Guid Id => Guid.Parse("c04c5f72-ebfa-4570-a0f8-5f4973ad40d5");
        public Image Image => null;
        public bool OmitResult => false;
        public Keys ShortcutKeys => Keys.None;

        public void Apply(ISelInfo sel)
        {
            sel.SelectedText = SwapAssignment(sel.SelectedText);
        }

        public bool Configure()
        {
            return false;
        }

        private string SwapAssignment(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            string[] lines = text.Split(new string[] { Utils._CRLF }, StringSplitOptions.RemoveEmptyEntries);

            StringBuilder sb = new StringBuilder();

            foreach (string line in lines)
            {
                string[] items = line.Split(new char[] { '=' });
                if (items.Length == 2)
                {
                    string left = items[0].Trim();
                    string right = items[1].Trim(new char[] { ' ', ';' });
                    sb.AppendFormat("{0} = {1};\r\n", right, left);
                }
                else
                {
                    sb.AppendFormat("{0}\r\n", line);
                }
            }
            return sb.ToString();
        }
    }
}