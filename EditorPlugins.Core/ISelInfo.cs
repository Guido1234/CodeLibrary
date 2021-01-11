namespace EditorPlugins.Core
{ 
    public interface ISelInfo
    {
        string CurrentLine { get; }
        string SelectedText { get; set; }
        string Text { get; }
    }
}