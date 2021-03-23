using CodeLibrary.Core;
using FastColoredTextBoxNS;
using System.IO;
using System.Windows.Forms;

namespace CodeLibrary
{
    public static class LocalUtils
    {
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

        public static int GetImageIndex(CodeSnippet snippet)
        {
            if (snippet.CodeType == CodeType.ReferenceLink)
                return 14;

            if (snippet.Important)
                return 2;

            if (snippet.CodeType == CodeType.System && snippet.Id == Constants.TRASHCAN)
                return 3;

            if (snippet.CodeType == CodeType.System && snippet.Id == Constants.CLIPBOARDMONITOR)
                return 11;

            if (snippet.AlarmActive)
                return 5;

            return GetImageIndex(snippet.CodeType);
        }

        public static int GetImageIndex(CodeType type)
        {
            switch (type)
            {
                case CodeType.Template:
                    return 1;

                case CodeType.CSharp:
                case CodeType.HTML:
                case CodeType.VB:
                case CodeType.JS:
                case CodeType.PHP:
                case CodeType.XML:
                case CodeType.Lua:
                case CodeType.None:
                case CodeType.RTF:
                case CodeType.SQL:
                case CodeType.MarkDown:
                    return 1;

                case CodeType.Folder:
                    return 0;

                case CodeType.Image:
                    return 10;

                case CodeType.ReferenceLink:
                    return 14;
            }
            return 0;
        }

        public static TreeNode GetNodeByParentPath(TreeNodeCollection collection, string path)
        {
            foreach (TreeNode node in collection)
            {
                if (node.FullPath.Equals(path))
                    return node;
            }
            foreach (TreeNode node in collection)
            {
                TreeNode subnode = GetNodeByParentPath(node.Nodes, path);
                if (subnode != null)
                    return subnode;
            }
            return null;
        }

        public static string LastPart(string path)
        {
            int ii = path.IndexOf('\\');
            if (ii < 0)
                return path;

            return path.Substring(ii, path.Length - ii);
        }
    }
}