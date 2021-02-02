using CodeLibrary.Core;
using FastColoredTextBoxNS;
using System.IO;

namespace CodeLibrary
{
    public static class LocalUtils
    {

        public static string LastPart(string path)
        {
            int ii = path.IndexOf('\\');
            if (ii < 0)
                return path;

            return path.Substring(ii, path.Length - ii);
        }

        public static CodeType CodeTypeByExtension(FileInfo file)
        {
            string _extension = file.Extension.Trim(new char[] { '.' }).ToLower();
            switch (_extension)
            {
                case "vb":
                    return CodeType.VB;

                case "cs":
                    return CodeType.CSharp;

                case "js":
                case "ts":
                case "json":
                    return CodeType.JS;

                case "txt":
                case "inf":
                case "info":
                case "nfo":
                    return CodeType.None;

                case "md":
                    return CodeType.MarkDown;

                case "html":
                case "htm":
                    return CodeType.HTML;

                case "resx":
                case "xml":
                case "xmlt":
                case "xlt":
                case "xslt":
                    return CodeType.XML;

                case "sql":
                    return CodeType.SQL;

                case "rtf":
                    return CodeType.RTF;

                case "jpg":
                case "jpeg":
                case "png":
                case "bmp":
                    return CodeType.Image;
            }
            return CodeType.UnSuported;
        }

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