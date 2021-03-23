using System.ComponentModel;

namespace CodeLibrary.Core
{
    public enum CodeType
    {
        Folder = 0,
        CSharp = 1,
        SQL = 2,
        VB = 3,
        None = 4, // Plain Text
        HTML = 5,
        Template = 6,
        XML = 7,
        PHP = 8,
        Lua = 9,
        JS = 11,
        RTF = 12,
        MarkDown = 13,
        ReferenceLink = 20,

        [Browsable(false)]
        Image = 100,

        [Browsable(false)]
        System = 10,

        [Browsable(false)]
        UnSuported = 9999
    }

    public enum ESortMode
    {
        Alphabetic = 0,
        AlphabeticGrouped = 1
    }

    public enum ETheme
    {
        Dark,
        Light,
        HighContrast
    }

    public enum SnippetFlags
    {
        None = 0,
        Important = 1,
        Ok = 2,
        NotOk = 3
    }
}