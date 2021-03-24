using System;
using System.Drawing;
using System.Windows.Forms;

namespace EditorPlugins.Core
{
    public interface IEditorPlugin
    {
        string Category { get; }
        string DisplayName { get; }
        Guid Id { get; }
        Image Image { get; }
        bool OmitResult { get; }

        Keys ShortcutKeys { get; }

        void Apply(ISelInfo selection);

        bool Configure();

        bool IsExtension { get; }
    }
}