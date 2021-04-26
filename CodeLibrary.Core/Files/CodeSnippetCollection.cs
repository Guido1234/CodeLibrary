using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CodeLibrary.Core
{
    [DataContract()]
    public class CodeSnippetCollection
    {
        public CodeSnippetCollection()
        {
            Items = new List<CodeSnippet> { CodeSnippet.TrashcanSnippet() };
        }

        [DataMember(Name = "AutoBackup")]
        public bool AutoBackup { get; set; }

        [DataMember(Name = "Snippets")]
        public List<CodeSnippet> Items { get; set; }

        [DataMember(Name = "LastSaved")]
        public DateTime LastSaved { get; set; }

        [DataMember(Name = "LastSelected")]
        public string LastSelected { get; set; }

        [DataMember(Name = "Locked")]
        public bool Locked { get; set; }

        [DataMember(Name = "Converted")]
        public int Version { get; set; }

        [DataMember(Name = "Counter")]
        public int Counter { get; set; }

        // #TODO ignore for fileversion version 2.4 and higher
        public void FromBase64()
        {
            if (Version == 0)
                return;

            foreach (CodeSnippet item in Items)
            {
                item.Name = Utils.FromBase64(item.Name);
                item.Code = Utils.FromBase64(item.Code);
                item.Path = Utils.FromBase64(item.Path);
                if (!string.IsNullOrEmpty(item.RTF))
                {
                    try
                    {
                        item.RTF = Utils.FromBase64(item.RTF);
                    }
                    catch
                    { }
                }
            }
            Version = 1;
        }

        // #TODO ignore for fileversion version 2.4 and higher
        public void ToBase64()
        {
            foreach (CodeSnippet item in Items)
            {
                item.Name = Utils.ToBase64(item.Name);
                item.Code = Utils.ToBase64(item.Code);
                item.Path = Utils.ToBase64(item.Path);
                if (!string.IsNullOrEmpty(item.RTF))
                {
                    try
                    {
                        item.RTF = Utils.ToBase64(item.RTF);
                    }
                    catch { }
                }
            }
            Version = 1;
        }
    }
}