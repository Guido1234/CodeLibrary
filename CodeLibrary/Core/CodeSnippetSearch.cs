using System.ComponentModel;
using System.Runtime.Serialization;

namespace CodeLibrary.Core
{
    internal class CodeSnippetSearch
    {
        [DataMember(Name = "Contents", Order = 3)]
        public string Code { get; set; } = string.Empty;

        [DataMember(Name = "Type", Order = 2)]
        public CodeType CodeType { get; set; }

        [Browsable(false)]
        [DataMember(Name = "Created", Order = 1)]
        public string CreationDate { get; set; }

        [Browsable(false)]
        [DataMember(Name = "Id")]
        public string Id { get; set; } = string.Empty;

        [Browsable(false)]
        [DataMember(Name = "Name")]
        public string Name { get; set; } = string.Empty;

        [DataMember(Name = "Document", Order = 0)]
        public string Path { get; set; } = string.Empty;
    }
}