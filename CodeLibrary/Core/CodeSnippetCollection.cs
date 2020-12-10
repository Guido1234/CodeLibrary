using CodeLibrary.Core;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CodeLibrary
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

        [DataMember(Name = "Converted")]
        public int Version { get; set; }

        public void FromBase64()
        {
            if (Version == 0)
                return;

            foreach (CodeSnippet item in Items)
            {
                item.Name = Utils.FromBase64(item.Name);
                item.Code = Utils.FromBase64(item.Code);
                item.Path = Utils.FromBase64(item.Path);
            }
            Version = 1;
        }

        public void ToBase64()
        {
            foreach (CodeSnippet item in Items)
            {
                item.Name = Utils.ToBase64(item.Name);
                item.Code = Utils.ToBase64(item.Code);
                item.Path = Utils.ToBase64(item.Path);
            }
            Version = 1;
        }
    }
}