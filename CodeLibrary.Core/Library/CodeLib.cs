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

        public Guid DocumentId { get; private set; } = Guid.NewGuid();

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

        // #LEGACY
        public void Cleanup(CodeSnippetCollectionOld collection)
        {
            if (collection == null)
            {
                return;
            }

            foreach (var _snip in collection.Items)
            {
                if (_snip == null)
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(_snip.Code))
                {
                    if (_snip.Code.Contains("Ãƒ"))
                    {
                        _snip.Code = "";
                    }
                }
                if (_snip.RTF != null)
                {
                    if (_snip.RTF.Contains("Ãƒ"))
                    {
                        _snip.RTF = "";
                    }
                }
                if (!string.IsNullOrEmpty(_snip.Name))
                { 
                    if (_snip.Name.Contains("Ãƒ"))
                    {
                        _snip.Name = "";
                    }
                }
                if (!string.IsNullOrEmpty(_snip.Path))
                {
                    if (_snip.Path.Contains("Ãƒ"))
                    {
                        _snip.Path = "";
                    }
                }
            }

            // Ãƒ
        }

        public void LoadLegacy(CodeSnippetCollectionOld collection)
        {
            BeginUpdate();

            Cleanup(collection);

            Counter = collection.Counter;

            CodeSnippetCollection _newCollection = new CodeSnippetCollection();
            _newCollection.Items.Clear();

            foreach (CodeSnippetOld _old in collection.Items)
            {
                if (string.IsNullOrWhiteSpace(_old.Path))
                    _old.Path = Constants.UNNAMED;

                CodeSnippet _new = new CodeSnippet()
                {       
                    DefaultChildCodeType = _old.DefaultChildCodeType, 
                    DefaultChildCodeTypeEnabled = _old.DefaultChildCodeTypeEnabled, 
                    DefaultChildName = _old.DefaultChildName, 
                    Name = _old.Name,             
                    Blob = _old.Blob,  
                    AlarmActive = _old.AlarmActive,
                    AlarmDate = _old.AlarmDate,
                    CodeLastModificationDate = _old.CodeLastModificationDate,
                    CodeType = _old.CodeType,
                    CreationDate = _old.CreationDate,
                    CurrentLine = _old.CurrentLine,
                    Expanded = _old.Expanded,
                    Flag = _old.Flag,
                    HtmlPreview = _old.HtmlPreview,
                    Id = _old.Id,
                    Important = _old.Important,
                    Locked = _old.Locked,
                    Order = _old.Order,
                    ReferenceLinkId = _old.ReferenceLinkId,
                    RTFAlwaysWhite = _old.RTFAlwaysWhite,
                    RTFOwnTheme = _old.RTFOwnTheme,
                    RTFTheme = _old.RTFTheme,
                    ShortCutKeys = _old.ShortCutKeys,
                    Wordwrap = _old.Wordwrap
                };

                if (_old.Path.Equals(@"Trashcan"))
                {

                }

                if (_old.Path.Equals(@"C#\Classes\VersionNumber"))
                {

                }

                bool _changed = false;
                _new.SetPath(_old.Path, out _changed);
                _new.SetCode(_old.Code, out _changed);
                _new.SetRtf(_old.RTF, out _changed);
                _new.SetDefaultChildCode(_old.DefaultChildCode, out _changed);
                _new.SetDefaultChildRtf(_old.DefaultChildRtf, out _changed);


                _newCollection.Items.Add(_new);


            }

            TreeNodes.Clear();
            CodeSnippets.Clear();
            CodeSnippets.AddRange(_newCollection.Items);

            if (Counter < collection.Items.Count)
            {
                Counter = collection.Items.Count;
            }

            if (ClipboardMonitor != null)
            {
                ClipboardMonitor.Order = -1;
                ClipboardMonitor.SetPath("Clipboard Monitor", out bool _changed);
            }
            if (Trashcan != null)
            {
                Trashcan.Order = -2;
                Trashcan.SetPath("Trashcan", out bool _changed);
            }

            DocumentId = Guid.NewGuid(); // Legacy always new DocumentId

            EndUpdate();
        }


        public void Load(CodeSnippetCollection collection)
        {
            BeginUpdate();

            Counter = collection.Counter;

            try
            {
                foreach (CodeSnippet snippet in collection.Items)
                {
                        if (string.IsNullOrWhiteSpace(snippet.GetPath()))
                            snippet.SetPath(Constants.UNNAMED, out bool _changed);

                        snippet.Refresh();

                }
            }
            catch { }


            TreeNodes.Clear();
            CodeSnippets.Clear();
            CodeSnippets.AddRange(collection.Items);
            if (collection.DocumentId.Equals(Guid.Empty))
            {
                DocumentId = Guid.NewGuid();
            }
            else
            {
                DocumentId = collection.DocumentId;
            }

            if (Counter < collection.Items.Count)
            {
                Counter = collection.Items.Count;
            }

            if (ClipboardMonitor != null)
            {
                ClipboardMonitor.Order = -1;
                ClipboardMonitor.SetPath("Clipboard Monitor", out bool _changed);
            }
            if (Trashcan != null)
            {
                Trashcan.Order = -2;
                Trashcan.SetPath("Trashcan", out bool _changed);
            }

            EndUpdate();
        }


        public void New()
        {
            DocumentId = Guid.NewGuid();
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
            collection.DocumentId = DocumentId;
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