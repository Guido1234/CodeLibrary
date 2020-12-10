using CodeLibrary.Core;
using DevToys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CodeLibrary
{
    public class CodeLib
    {
        private static CodeLib _Instance;
        private int _CurrentHashCode = 0;
        private int _InitialHashCode = 0;

        private CodeLib()
        {
            Library.RegisterLookup(Constants.LOOKUP_NAME, p => p.Name);
            Library.RegisterLookup(Constants.LOOKUP_TIMERACTIVE, p => p.AlarmActive);
            Library.RegisterLookup(Constants.LOOKUP_PATH, p => p.Path);
            Library.RegisterLookup(Constants.LOOKUP_SHORTCUT, p => p.ShortCutKeys);
            Library.RegisterLookup(Constants.LOOKUP_CYCLICSHORTCUT, p => p.CyclicShortCut);

            Library.CollectionChanged += Libary_CollectionChanged;
            Library.ItemsAdded += Libary_ItemsAdded;
            Library.ItemsRemoved += Libary_ItemsRemoved;
        }

        public static CodeLib Instance => _Instance ?? (_Instance = new CodeLib());

        public DictionaryList<CodeSnippet, string> Library { get; } = new DictionaryList<CodeSnippet, string>(p => p.Id);

        public DictionaryList<TreeNode, string> NodeIndexer { get; } = new DictionaryList<TreeNode, string>(p => p.Name);

        public static void Import(CodeSnippetCollection collection, DictionaryList<CodeSnippet, string> target, bool resetGuid)
        {
            foreach (CodeSnippet snippet in collection.Items)
            {
                if (resetGuid)
                    snippet.Id = Guid.NewGuid().ToString();

                if (string.IsNullOrWhiteSpace(snippet.Path))
                    snippet.Path = Constants.UNNAMED;
            }
            target.AddRange(collection.Items);
        }

        public void BuildNodeIndexer(TreeView treeview)
        {
            NodeIndexer.Clear();
            foreach (TreeNode node in treeview.Nodes)
            {
                NodeIndexer.Add(node);
                BuildNodeIndexer(node.Nodes);
            }
        }

        public void Defaults()
        {
            Instance.Library.Add(CodeSnippet.TrashcanSnippet());
            Instance.Library.Add(CodeSnippet.NewRoot("", CodeType.Folder, Constants.SNIPPETS));
        }

        public IEnumerable<CodeSnippet> GetByAlarmActive() => Library.Lookup(Constants.LOOKUP_TIMERACTIVE, true);

        public IEnumerable<CodeSnippet> GetByName(string name) => Library.Lookup(Constants.LOOKUP_NAME, name);

        public IEnumerable<CodeSnippet> GetByShortCut(Keys shortcut) => Library.Lookup(Constants.LOOKUP_SHORTCUT, shortcut);

        public IEnumerable<CodeSnippet> GetCyclicShortCuts() => Library.Lookup(Constants.LOOKUP_CYCLICSHORTCUT, true);

        public void Import(CodeSnippetCollection collection)
        {
            CodeLib.Import(collection, Library, false);

            _InitialHashCode = Library.GetHashCode();
            _CurrentHashCode = _InitialHashCode;
        }

        public void Load(CodeSnippetCollection collection)
        {
            foreach (CodeSnippet snippet in collection.Items)
            {
                if (string.IsNullOrWhiteSpace(snippet.Path))
                    snippet.Path = Constants.UNNAMED;
            }

            Library.Clear();
            Library.AddRange(collection.Items);
        }

        public void New()
        {
            Library.Clear();
            Defaults();
        }

        public void Refresh() => Library.Refresh();

        public void Save(CodeSnippetCollection collection)
        {
            collection.Items.Clear();
            collection.Items.AddRange(Library);
        }

        public IEnumerable<CodeSnippet> Search(string text)
        {
            foreach (CodeSnippet item in Library)
            {
                int _index = item.Code.IndexOf(text, StringComparison.OrdinalIgnoreCase);

                if (_index > -1)
                {
                    yield return item;
                }
            }
        }

        public IEnumerable<CodeSnippet> SearchMatchCase(string text)
        {
            foreach (var item in Library)
            {
                if (item.Code.Contains(text))
                {
                    yield return item;
                }
            }
        }

        public IEnumerable<CodeSnippet> SearchMatchPattern(string pattern)
        {
            foreach (var item in Library)
            {
                if (Utils.MatchPattern(item.Code, pattern))
                {
                    yield return item;
                }
            }
        }

        private void BuildNodeIndexer(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                NodeIndexer.Add(node);
                BuildNodeIndexer(node.Nodes);
            }
        }

        private void Libary_CollectionChanged(object sender, EventArgs e)
        {
            _CurrentHashCode = Library.GetHashCode();
        }

        private void Libary_ItemsAdded(object sender, ItemsAddedEventArgs<CodeSnippet> e)
        {
            _CurrentHashCode = Library.GetHashCode();
        }

        private void Libary_ItemsRemoved(object sender, ItemsRemovedEventArgs<CodeSnippet> e)
        {
            NodeIndexer.RemoveRange(e.Removed.Select(p => p.Id));
            _CurrentHashCode = Library.GetHashCode();
        }
    }
}