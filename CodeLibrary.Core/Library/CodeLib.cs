using DevToys;
using System;
using System.Linq;

namespace CodeLibrary.Core
{
    public class CodeLib
    {
        private static CodeLib _Instance;

        private bool _Changed = false;

        private int _Updating = 0;

        public int Counter { get; private set; } = 0;

        private CodeLib()
        {
            CodeSnippets.ItemsRemoved += Libary_ItemsRemoved;
            CodeSnippets.ItemsAdded += Library_ItemsAdded;
        }

        public event EventHandler<EventArgs> ChangeStateChanged = delegate { };

        public static CodeLib Instance => _Instance ?? (_Instance = new CodeLib());

        public bool Changed
        {
            get
            {
                return _Changed;
            }

            set
            {
                _Changed = value;
                ChangeStateChanged(this, new EventArgs());
            }
        }

        public CodeSnippet ClipboardMonitor => CodeSnippets.Get(Constants.CLIPBOARDMONITOR);

        public CodeSnippetDictionaryList CodeSnippets { get; } = new CodeSnippetDictionaryList();

        public CodeSnippet Trashcan => CodeSnippets.Get(Constants.TRASHCAN);

        /// <summary>
        /// Indexed List of all TreeNodes in TreeViewLibrary
        /// </summary>
        public TreeNodeDictionaryList TreeNodes { get; } = new TreeNodeDictionaryList();

        public void BeginUpdate()
        {
            _Updating++;
        }

        public void EndUpdate()
        {
            _Updating--;
        }

        public void Load(CodeSnippetCollection collection)
        {
            BeginUpdate();

            Counter = collection.Counter;


            foreach (CodeSnippet snippet in collection.Items)
            {
                if (string.IsNullOrWhiteSpace(snippet.Path))
                    snippet.Path = Constants.UNNAMED;
            }

            TreeNodes.Clear();
            CodeSnippets.Clear();
            CodeSnippets.AddRange(collection.Items);

            if (Counter < collection.Items.Count)
            {
                Counter = collection.Items.Count;
            }

            if (ClipboardMonitor != null)
            {
                ClipboardMonitor.Order = -1;
                ClipboardMonitor.Path = "Clipboard Monitor";
            }
            if (Trashcan != null)
            {
                Trashcan.Order = -2;
                Trashcan.Path = "Trashcan";
            }

            EndUpdate();
        }

        public void New()
        {
            TreeNodes.Clear();
            CodeSnippets.Clear();
            Defaults();
        }

        public void Refresh()
        {
            CodeSnippets.Refresh();
            TreeNodes.Refresh();
        }

        public void Save(CodeSnippetCollection collection)
        {
            collection.Items.Clear();
            collection.Counter = Counter;
            collection.Items.AddRange(CodeSnippets);
        }

        private void Defaults()
        {
            CodeSnippets.Add(CodeSnippet.TrashcanSnippet());
            CodeSnippets.Add(CodeSnippet.ClipboardMonitorSnippet());
            var _root = CodeSnippet.NewRoot("", CodeType.Folder, Constants.SNIPPETS);
            CodeSnippets.Add(_root);
        }

        private void Libary_ItemsRemoved(object sender, ItemsRemovedEventArgs<CodeSnippet> e)
        {
            if (_Updating != 0)
            {
                return;
            }
            Changed = true;
            TreeNodes.RemoveRange(e.Removed.Select(p => p.Id));
        }

        private void Library_ItemsAdded(object sender, ItemsAddedEventArgs<CodeSnippet> e)
        {
            if (_Updating != 0)
            {
                return;
            }
            Counter += e.Added.Count();
            Changed = true;
        }
    }
}