using EditorPlugins.Core;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack
{
    [Description("Evaluate selected text as a math expression.")]
    public class EvaluateExpression : IEditorPlugin
    {
        public bool IsExtension => false;
        public string Category => "";
        public string DisplayName => "Evaluate Expression";
        public Guid Id => Guid.Parse("955e0d77-cb1c-45df-9180-c9e441205ef5");
        public Image Image => ImageResource.math;
        public bool OmitResult => false;

        public Keys ShortcutKeys => (Keys.Control | Keys.E);

        public void Apply(ISelInfo sel)
        {
            sel.SelectedText = evaluateExpression(sel.SelectedText).ToString();
        }

        public bool Configure()
        {
            return false;
        }

        public override string ToString()
        {
            return DisplayName;
        }

        private static double evaluateExpression(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return 0;
            }
            double val = 0;
            try
            {
                val = (double)new System.Xml.XPath.XPathDocument
                (new StringReader("<r/>")).CreateNavigator().Evaluate
                (string.Format("number({0})", new
                System.Text.RegularExpressions.Regex(@"([\+\-\*])")
                .Replace(s, " ${1} ")
                .Replace("/", " div ")
                .Replace("%", " mod ")));
            }
            catch { }
            {
            }
            return val;
        }
    }
}