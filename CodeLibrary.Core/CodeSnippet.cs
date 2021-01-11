using CodeLibrary.Core;
using System;
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

        [Browsable(false)]
        [DataMember(Name = "CodeBookmarks")]
        public List<CodeBookmark> Bookmarks { get; set; }

        [Browsable(false)]
        [DataMember(Name = "Code")]
        public string Code { get; set; } = string.Empty;

        [Browsable(false)]
        [DataMember(Name = "RTF")]
        public string RTF { get; set; } = string.Empty;

        [Browsable(false)]
        [DataMember(Name = "RTFAlwaysWhite")]
        public bool RTFAlwaysWhite { get; set; } = false;


        [DataMember(Name = "CodeType")]
        public CodeType CodeType { get; set; }

        [Browsable(false)]
        [DataMember(Name = "CreationDate")]
        public string CreationDate { get; set; }

        [Browsable(false)]
        [DataMember(Name = "CurrentLine")]
        public int CurrentLine { get; set; }

        [Browsable(false)]
        [DataMember(Name = "CyclicShortCut")]
        public bool CyclicShortCut { get; set; }

        [Browsable(false)]
        [DataMember(Name = "DefaultChildCode")]
        public string DefaultChildCode { get; set; } = string.Empty;

        [Browsable(false)]
        [DataMember(Name = "DefaultChildRtf")]
        public string DefaultChildRtf { get; set; } = string.Empty;

        [Browsable(false)]
        [DataMember(Name = "DefaultChildCodeType")]
        public CodeType DefaultChildCodeType { get; set; }

        [Browsable(false)]
        [DataMember(Name = "DefaultChildCodeTypeEnabled")]
        public bool DefaultChildCodeTypeEnabled { get; set; }

        [Browsable(false)]
        [DataMember(Name = "DefaultChildName")]
        public string DefaultChildName { get; set; } = string.Empty;

        [Browsable(false)]
        [DataMember(Name = "Id")]
        public string Id { get; set; } = string.Empty;

        [DataMember(Name = "Important")]
        public bool Important { get; set; }

        [Browsable(false)]
        public bool Locked { get; set; }

        [Browsable(false)]
        [DataMember(Name = "Name")]
        public string Name { get; set; } = string.Empty;

        [Browsable(false)]
        [DataMember(Name = "Order")]
        public int Order { get; set; }

        [Browsable(false)]
        [DataMember(Name = "Path")]
        public string Path { get; set; } = string.Empty;

        [DataMember(Name = "ShortCutKeys")]
        public Keys ShortCutKeys { get; set; }

        [DataMember(Name = "Wordwrap")]
        public bool Wordwrap { get; set; }


        [DataMember(Name = "HtmlPreview")]
        public bool HtmlPreview { get; set; } = false;


        [Browsable(false)]
        [DataMember(Name = "Blob")]
        public byte[] Blob { get; set; }

        public static CodeSnippet NewRoot(string text, CodeType codetype, string name) => new CodeSnippet() { Code = text, CodeType = codetype, Locked = false, Name = name, Path = name };

        public static CodeSnippet TrashcanSnippet() => new CodeSnippet() { Code = string.Empty, CodeType = CodeType.System, Name = Constants.TRASHCAN_TITLE, Path = Constants.TRASHCAN_TITLE, Id = Constants.TRASHCAN };

        public static CodeSnippet ClipboardMonitorSnippet() => new CodeSnippet() { Code = string.Empty, CodeType = CodeType.System, Name = Constants.CLIPBOARDMONITOR_TITLE, Path = Constants.CLIPBOARDMONITOR_TITLE, Id = Constants.CLIPBOARDMONITOR };


        public override int GetHashCode() => Code.GetHashCode() + Path.GetHashCode() + Name.GetHashCode() + Order;

        public bool HasBookMarks() => (Bookmarks == null) ? false : Bookmarks.Count > 0;

        public string LabelName() => Utils.SplitPath(this.Path).Last();

        public override string ToString() => string.Format("{0}", Path);
    }
}