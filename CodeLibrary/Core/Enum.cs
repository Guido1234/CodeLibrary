using System.ComponentModel;

namespace CodeLibrary
{
    public enum CodeType
    {
        Folder = 0,
        CSharp = 1,
        SQL = 2,
        VB = 3,
        None = 4,
        HTML = 5,
        Template = 6,

        [Browsable(false)]
        System = 10
    }
}