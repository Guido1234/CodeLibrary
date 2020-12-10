using EditorPlugins.Core;

namespace EditorPlugins.Engine
{
    public class SelInfo : ISelInfo
    {
        public string CurrentLine { get; set; }
        public string SelectedText { get; set; }
        public string Text { get; set; }
    }
}