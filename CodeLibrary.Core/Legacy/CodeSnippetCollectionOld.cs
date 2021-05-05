using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CodeLibrary.Core
{

    // #TODO naar Lecacy
    [DataContract()]
    public class CodeSnippetCollectionOld
    {
        public CodeSnippetCollectionOld()
        {
            Items = new List<CodeSnippetOld> { CodeSnippetOld.TrashcanSnippet() };
        }

        // #TODO Move to CodeSnippetDictionaryList
        [DataMember(Name = "AutoBackup")]
        public bool AutoBackup { get; set; }

        [DataMember(Name = "Snippets")]
        public List<CodeSnippetOld> Items { get; set; }

        // #TODO Move to CodeSnippetDictionaryList
        [DataMember(Name = "LastSaved")]
        public DateTime LastSaved { get; set; }

        // #TODO Move to CodeSnippetDictionaryList
        [DataMember(Name = "LastSelected")]
        public string LastSelected { get; set; }

        // #TODO Move to CodeSnippetDictionaryList
        [DataMember(Name = "Locked")]
        public bool Locked { get; set; }

        [DataMember(Name = "Converted")]
        public int Version { get; set; }

        // #TODO Move to CodeSnippetDictionaryList
        [DataMember(Name = "Counter")]
        public int Counter { get; set; }

        public void FromBase64()
        {
            if (Version == 0)
                return;

            foreach (CodeSnippetOld item in Items)
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

        public void ToBase64()
        {
            foreach (CodeSnippetOld item in Items)
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