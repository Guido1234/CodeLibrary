using DevToys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CodeLibrary.Core
{
    public class CodeSnippetDictionaryList : DictionaryList<CodeSnippet, string>
    {
        private const string LOOKUP_NAME = "LookupName";
        private const string LOOKUP_PATH = "LookupPath";
        private const string LOOKUP_PARENT_PATH = "LookupParentPath";
        private const string LOOKUP_REFERENCELINKID = "LookupReferenceLinkId";
        private const string LOOKUP_SHORTCUT = "LookupShorcut";
        private const string LOOKUP_TIMERACTIVE = "LookupAlarmActive";

        public CodeSnippetDictionaryList() : base(p => p.Id)
        {
            RegisterLookup(LOOKUP_NAME, p => p.Name);
            RegisterLookup(LOOKUP_TIMERACTIVE, p => p.AlarmActive);
            RegisterLookup(LOOKUP_PATH, p => p.Path.ToLower());
            RegisterLookup(LOOKUP_PARENT_PATH, p => Utils.ParentPath( p.Path, '\\').ToLower());
            RegisterLookup(LOOKUP_SHORTCUT, p => p.ShortCutKeys);
            RegisterLookup(LOOKUP_REFERENCELINKID, p => p.ReferenceLinkId); 
        }

        public IEnumerable<CodeSnippet> GetByAlarmActive() => Lookup(LOOKUP_TIMERACTIVE, true);

        public IEnumerable<CodeSnippet> GetByName(string name) => Lookup(LOOKUP_NAME, name);

        public CodeSnippet GetByPath(string path) => Lookup(LOOKUP_PATH, path.ToLower()).FirstOrDefault();

        public IEnumerable<CodeSnippet> GetChildsByPath(string path) => Lookup(LOOKUP_PARENT_PATH, path.ToLower());

        /// <summary>
        /// Expects the last part to be a pattern like *_*_
        /// </summary>
        public IEnumerable<CodeSnippet> GetChildsByPathAndPattern(string path)
        {
            string _pattern = Utils.PathName(path);
            string _path = Utils.ParentPath(path);

            IEnumerable<CodeSnippet> _snippets = Lookup(LOOKUP_PARENT_PATH, _path.ToLower());
            foreach (CodeSnippet snippet in _snippets)
            {
                if (Utils.MatchPattern(snippet.LabelName(), _pattern))
                {
                    yield return snippet;
                }
            }
        }

        public IEnumerable<CodeSnippet> GetByShortCut(Keys shortcut) => Lookup(LOOKUP_SHORTCUT, shortcut);

        public IEnumerable<CodeSnippet> GetReferencesById(string id) => Lookup(LOOKUP_REFERENCELINKID, id);

        public IEnumerable<CodeSnippet> Search(string text)
        {
            foreach (CodeSnippet item in this)
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
            foreach (var item in this)
            {
                if (item.Code.Contains(text))
                {
                    yield return item;
                }
            }
        }

        public IEnumerable<CodeSnippet> SearchMatchPattern(string pattern)
        {
            foreach (var item in this)
            {
                if (Utils.MatchPattern(item.Code, pattern))
                {
                    yield return item;
                }
            }
        }
    }
}