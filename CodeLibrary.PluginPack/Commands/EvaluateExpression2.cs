using EditorPlugins.Core;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack
{
    [Description("Evaluate selected text as a math expression. Places result after expression.")]
    public class EvaluateExpression2 : IEditorPlugin
    {
        public string Category => "";
        public string DisplayName => "Evaluate Expression 2";
        public Guid Id => Guid.Parse("c99f0b26-71cf-4069-b2bc-3007c38a44f9");
        public Image Image => ImageResource.math;
        public bool OmitResult => false;

        public Keys ShortcutKeys => (Keys.Control | Keys.E | Keys.LShiftKey);

        public void Apply(ISelInfo sel)
        {
            sel.SelectedText = $"{sel.SelectedText} = {evaluateExpression(sel.SelectedText).ToString()}";
        }

        public bool Configure()
        {
            return false;
        }

        public override string ToString()
        {
            return this.DisplayName;
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