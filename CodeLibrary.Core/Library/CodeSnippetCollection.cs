using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CodeLibrary.Core
{

    // #TODO naar Lecacy
    [DataContract()]
    [Serializable()]
    public class CodeSnippetCollection
    {
        public CodeSnippetCollection()
        {
            Items = new List<CodeSnippet> { CodeSnippet.TrashcanSnippet() };
        }

        [DataMember(Name = "DocumentId")]
        public Guid DocumentId { get; set; }


        [DataMember(Name = "AutoBackup")]
        public bool AutoBackup { get; set; }

        [DataMember(Name = "Snippets")]
        public List<CodeSnippet> Items { get; set; }

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
    }
}