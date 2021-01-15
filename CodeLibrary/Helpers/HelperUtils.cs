using CodeLibrary.Core;
using FastColoredTextBoxNS;

namespace CodeLibrary.Helpers
{
    public static class HelperUtils
    {
        public static Language CodeTypeToLanguage(CodeType codeType)
        {
            switch (codeType)
            {
                case CodeType.CSharp:
                    return Language.CSharp;

                case CodeType.HTML:
                    return Language.HTML;

                case CodeType.Image:
                    return Language.Custom;

                case CodeType.JS:
                    return Language.JS;

                case CodeType.Lua:
                    return Language.Lua;

                case CodeType.PHP:
                    return Language.PHP;

                case CodeType.SQL:
                    return Language.SQL;

                case CodeType.Folder:
                case CodeType.MarkDown:
                case CodeType.None:
                case CodeType.RTF:
                case CodeType.Template:
                case CodeType.UnSuported:
                case CodeType.System:
                    return Language.Custom;

                case CodeType.VB:
                    return Language.VB;

                case CodeType.XML:
                    return Language.XML;

                default:
                    return Language.Custom;
            }
        }
    }
}