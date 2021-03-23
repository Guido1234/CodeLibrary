using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CodeLibrary.Core
{
    public class CodeSnippetSearch
    {
        [DataMember(Name = "Contents", Order = 3)]
        public string Code { get; set; } = string.Empty;

        [Browsable(true)]
        [DataMember(Name = "CodeLastModificationDate", Order = 1)]
        [DisplayFormatAttribute(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime? CodeLastModificationDate { get; set; }

        [DataMember(Name = "Type", Order = 2)]
        public CodeType CodeType { get; set; }

        //[Browsable(false)]
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