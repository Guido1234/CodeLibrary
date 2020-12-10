using GK.Template.Attributes;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace GK.Template.DataCommands
{
    [DataCommand(Name = "Files")]
    [Description(@"Reads out the file system
{0} Path
{1} Directory Path
{2} Directory Name
{3} FileName
{4} Extension
{5} Length
{6} CreationTime (yyyyMMdd hh:mm:ss)
{7} LastAccessTime (yyyyMMdd hh:mm:ss)
{8} LastWriteTime (yyyyMMdd hh:mm:ss)
    ")]
    public sealed class DataCommandFiles : DataCommandBase
    {
        [DataCommandParameter(Order = 0)]
        public string Directory { get; set; }

        [DataCommandParameter(Order = 2)]
        public string Filter { get; set; }

        [DataCommandParameter(Order = 1)]
        public bool Recursive { get; set; }

        public override string Execute(StringTemplateItem template)
        {
            string directory = PathUtility.ParseSpecialFoldersNames(this.Directory, ParseSpecialFolderOption.WildCardToRealPath);

            StringBuilder sb = new StringBuilder();
            if (Utils.IsFileOrDirectory(directory) == Utils.FileOrDirectory.Directory)
            {
                List<string> files = Utils.GetFiles(directory, Filter, Recursive);
                for (int ii = 0; ii < files.Count; ii++)
                {
                    string file = files[ii];
                    FileInfo fi = new FileInfo(file);
                    string[] inf = new string[] { fi.FullName, fi.Directory.FullName, fi.DirectoryName, fi.Name, fi.Extension, fi.Length.ToString(), fi.CreationTime.ToString("yyyyMMdd hh:mm:ss"), fi.LastAccessTime.ToString("yyyyMMdd hh:mm:ss"), fi.LastWriteTime.ToString("yyyyMMdd hh:mm:ss") };
                    string line = string.Join(";", inf);
                    template.IsLastCommand = ((ii == files.Count - 1) && this.IsLastCommand);
                    sb.Append(template.Format(line, this.Data, file));
                }
            }
            return sb.ToString();
        }
    }
}