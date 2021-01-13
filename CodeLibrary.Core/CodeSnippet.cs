﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Forms;
 
namespace CodeLibrary.Core
{
    [DataContract()]
    public class CodeSnippet
    {
        public CodeSnippet()
        {
            CreationDate = DateTime.Now.ToString("yyyyMMdd hh:nn");
            Id = Guid.NewGuid().ToString();
        }

        [DataMember(Name = "AlarmActive")]
        public bool AlarmActive { get; set; }

        [DataMember(Name = "AlarmDate")]
        public DateTime? AlarmDate { get; set; }

        [DataMember(Name = "Blob")]
        public byte[] Blob { get; set; }

        [DataMember(Name = "CodeBookmarks")]
        public List<CodeBookmark> Bookmarks { get; set; }

        [DataMember(Name = "Code")]
        public string Code { get; set; } = string.Empty;

        [DataMember(Name = "CodeType")]
        public CodeType CodeType { get; set; }

        [DataMember(Name = "CreationDate")]
        public string CreationDate { get; set; }

        [DataMember(Name = "CurrentLine")]
        public int CurrentLine { get; set; }

        [DataMember(Name = "CyclicShortCut")]
        public bool CyclicShortCut { get; set; }

        [DataMember(Name = "DefaultChildCode")]
        public string DefaultChildCode { get; set; } = string.Empty;

        [DataMember(Name = "DefaultChildCodeType")]
        public CodeType DefaultChildCodeType { get; set; }

        [DataMember(Name = "DefaultChildCodeTypeEnabled")]
        public bool DefaultChildCodeTypeEnabled { get; set; }

        [DataMember(Name = "DefaultChildName")]
        public string DefaultChildName { get; set; } = string.Empty;

        [DataMember(Name = "DefaultChildRtf")]
        public string DefaultChildRtf { get; set; } = string.Empty;

        [DataMember(Name = "HtmlPreview")]
        public bool HtmlPreview { get; set; } = false;

        [DataMember(Name = "Id")]
        public string Id { get; set; } = string.Empty;

        //[DataMember(Name = "PId")]
        //public string ParentId { get; set; } = string.Empty;

        //[DataMember(Name = "PCSet")]
        //public bool ParentChildRelationSet { get; set; } = false;

        [DataMember(Name = "Important")]
        public bool Important { get; set; }

        [DataMember(Name = "Expanded")]
        public bool Expanded { get; set; }

        public bool Locked { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; } = string.Empty;

        [DataMember(Name = "Order")]
        public int Order { get; set; }

        [DataMember(Name = "Path")]
        public string Path { get; set; } = string.Empty;

        [DataMember(Name = "RTF")]
        public string RTF { get; set; } = string.Empty;

        [DataMember(Name = "RTFAlwaysWhite")]
        public bool RTFAlwaysWhite { get; set; } = false;

        [DataMember(Name = "ShortCutKeys")]
        public Keys ShortCutKeys { get; set; }

        [DataMember(Name = "Wordwrap")]
        public bool Wordwrap { get; set; }


        [DataMember(Name = "RTFLM")]
        public bool RTFOwnTheme { get; set; } = false;

        [DataMember(Name = "RtfTheme")]
        public RtfTheme RTFTheme { get; set; } =  RtfTheme.Light;


        public static CodeSnippet ClipboardMonitorSnippet() => new CodeSnippet() { Code = string.Empty, CodeType = CodeType.System, Name = Constants.CLIPBOARDMONITOR_TITLE, Path = Constants.CLIPBOARDMONITOR_TITLE, Id = Constants.CLIPBOARDMONITOR };

        public static CodeSnippet NewRoot(string text, CodeType codetype, string name) => new CodeSnippet() { Code = text, CodeType = codetype, Locked = false, Name = name, Path = name };

        public static CodeSnippet TrashcanSnippet() => new CodeSnippet() { Code = string.Empty, CodeType = CodeType.System, Name = Constants.TRASHCAN_TITLE, Path = Constants.TRASHCAN_TITLE, Id = Constants.TRASHCAN };

        public override int GetHashCode() => Code.GetHashCode() + Path.GetHashCode() + Name.GetHashCode() + Order;

        public bool HasBookMarks() => (Bookmarks == null) ? false : Bookmarks.Count > 0;

        public string LabelName() => Utils.SplitPath(this.Path).Last();

        public override string ToString() => string.Format("{0}", Path);
    }
}