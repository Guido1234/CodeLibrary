using System.IO;

namespace CodeLibrary.Core.Library
{
    public class CodeSnippetCollectionExporterService
    {
        private CodeSnippetCollection _Collection = null;
        private DirectoryInfo _TargetDirectory = null;

        public CodeSnippetCollectionExporterService(string targetDirectory, CodeSnippetCollection collection)
        {
            _TargetDirectory = new DirectoryInfo(targetDirectory);
            _Collection = collection;
        }

        private string CleanPath(string path)
        {
            char[] _invalid = Path.GetInvalidPathChars();
            foreach (char c in _invalid)
            {
                path = path.Replace(c.ToString(), "");    
            }
            return path;
        }

        public void Export()
        {
            foreach (CodeSnippet snippet in _Collection.Items)
            {
                string _extension = GetExtension(snippet.CodeType);
                string _code = null;
                string _path = CleanPath(snippet.GetPath());
               

                FileInfo _file = new FileInfo(Path.Combine(_TargetDirectory.FullName, $"{_path}.{_extension}"));
                int counter = 0;
                
                while (_file.Exists)                
                {
                    counter++;
                    _file = new FileInfo(Path.Combine(_TargetDirectory.FullName, $"{_path} ({counter}).{_extension}"));
                }

                Directory.CreateDirectory(_file.Directory.FullName);

                switch (snippet.CodeType)
                {
                    case CodeType.System:
                    case CodeType.UnSuported:
                    case CodeType.ReferenceLink:
                        continue;

                    case CodeType.Image:
                        byte[] _blob = snippet.Blob;
                        File.WriteAllBytes(_file.FullName, _blob);
                        break;

                    case CodeType.RTF:
                        _code = snippet.GetRTF();
                        File.WriteAllText(_file.FullName, _code);
                        break;

                    default:
                        _code = snippet.GetCode();
                        File.WriteAllText(_file.FullName, _code);
                        break;
                }
            }
        }

        private string GetExtension(CodeType codeType)
        {
            switch (codeType)
            {
                case CodeType.Image:
                    return "jpeg";

                case CodeType.CSharp:
                    return "cs";

                case CodeType.HTML:
                    return "html";

                case CodeType.JS:
                    return "js";

                case CodeType.Lua:
                    return "lua";

                case CodeType.None:
                    return "txt";

                case CodeType.MarkDown:
                    return "md";

                case CodeType.PHP:
                    return "php";

                case CodeType.RTF:
                    return "rtf";

                case CodeType.SQL:
                    return "sql";
                    break;

                case CodeType.XML:
                    return "xml";

                case CodeType.Template:
                    return "txt";
            }

            return "txt";
        }
    }
}