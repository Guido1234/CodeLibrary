using DevToys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CodeLibrary.Core
{
    public class CodeLib
    {

        public event EventHandler<EventArgs> ChangeStateChanged = delegate { };

        private static CodeLib _Instance;

        private int _Updating = 0;

        private bool _Changed = false;

        private CodeLib()
        {
            Library.RegisterLookup(Constants.LOOKUP_NAME, p => p.Name);
            Library.RegisterLookup(Constants.LOOKUP_TIMERACTIVE, p => p.AlarmActive);
            Library.RegisterLookup(Constants.LOOKUP_PATH, p => p.Path);
            Library.RegisterLookup(Constants.LOOKUP_SHORTCUT, p => p.ShortCutKeys);
            Library.ItemsRemoved += Libary_ItemsRemoved;
            Library.ItemsAdded += Library_ItemsAdded;
        }

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

        public CodeSnippet ClipboardMonitor
        {
            get
            {
                return Library.Get(Constants.CLIPBOARDMONITOR);
            }
        }

        public DictionaryList<CodeSnippet, string> Library { get; } = new DictionaryList<CodeSnippet, string>(p => p.Id);

        public DictionaryList<TreeNode, string> NodeIndexer { get; } = new DictionaryList<TreeNode, string>(p => p.Name);

        public CodeSnippet Trashcan
        {
            get
            {
                return Library.Get(Constants.TRASHCAN);
            }
        }

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

        public void AddNodeIndexer(TreeNode node)
        {
            if (NodeIndexer.ContainsKey(node.Name))
            {
                return;
            }
            NodeIndexer.Add(node);
        }

        public void BeginUpdate()
        {
            _Updating++;
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
            Instance.Library.Add(CodeSnippet.ClipboardMonitorSnippet());
            var _root = CodeSnippet.NewRoot("", CodeType.Folder, Constants.SNIPPETS);
            Instance.Library.Add(_root);
        }

        public void EndUpdate()
        {
            _Updating--;
        }

        public IEnumerable<CodeSnippet> GetByAlarmActive() => Library.Lookup(Constants.LOOKUP_TIMERACTIVE, true);

        public CodeSnippet GetById(string id) => Library.Get(id);

        public IEnumerable<CodeSnippet> GetByName(string name) => Library.Lookup(Constants.LOOKUP_NAME, name);

        public CodeSnippet GetByPath(string name) => Library.Lookup(Constants.LOOKUP_PATH, name).FirstOrDefault();

        public IEnumerable<CodeSnippet> GetByShortCut(Keys shortcut) => Library.Lookup(Constants.LOOKUP_SHORTCUT, shortcut);


        public void Load(CodeSnippetCollection collection)
        {
            BeginUpdate();

            foreach (CodeSnippet snippet in collection.Items)
            {
                if (string.IsNullOrWhiteSpace(snippet.Path))
                    snippet.Path = Constants.UNNAMED;
            }

            Library.Clear();
            Library.AddRange(collection.Items);

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

        private void Libary_ItemsRemoved(object sender, ItemsRemovedEventArgs<CodeSnippet> e)
        {
            if (_Updating != 0)
            {
                return;
            }
            Changed = true;
            NodeIndexer.RemoveRange(e.Removed.Select(p => p.Id));
        }

        private void Library_ItemsAdded(object sender, ItemsAddedEventArgs<CodeSnippet> e)
        {
            if (_Updating != 0)
            {
                return;
            }
            Changed = true;
        }
    }
}