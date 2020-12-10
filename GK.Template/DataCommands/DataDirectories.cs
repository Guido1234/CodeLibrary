//using GK.Library.Utilities;
using GK.Template.Attributes;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace GK.Template.DataCommands
{
    [DataCommand(Name = "Directory")]
    [Description(@"Reads out the file system
{0} Path
{1} Name
{2} CreationTime (yyyyMMdd hh:mm:ss)
{3} LastAccessTime (yyyyMMdd hh:mm:ss)
{4} LastWriteTime (yyyyMMdd hh:mm:ss)
    ")]
    public sealed class DataCommandDirectories : DataCommandBase
    {
        [DataCommandParameter(Order = 0)]
        public string Directory { get; set; }

        [DataCommandParameter(Order = 1)]
        public bool Recursive { get; set; }

        public override string Execute(StringTemplateItem template)
        {
            string directory = PathUtility.ParseSpecialFoldersNames(this.Directory, ParseSpecialFolderOption.WildCardToRealPath);

            StringBuilder sb = new StringBuilder();
            if (Utils.IsFileOrDirectory(directory) == Utils.FileOrDirectory.Directory)
            {
                List<string> directories = Utils.GetDirectories(directory, Recursive);
                for (int ii = 0; ii < directories.Count; ii++)
                {
                    string dir = directories[ii];
                    DirectoryInfo dirInfo = new DirectoryInfo(dir);
                    string[] inf = new string[] { dirInfo.FullName, dirInfo.Name, dirInfo.CreationTime.ToString("yyyyMMdd hh:mm:ss"), dirInfo.LastAccessTime.ToString("yyyyMMdd hh:mm:ss"), dirInfo.LastWriteTime.ToString("yyyyMMdd hh:mm:ss") };
                    string line = string.Join(";", inf);
                    template.IsLastCommand = ((ii == directories.Count - 1) && this.IsLastCommand);
                    sb.Append(template.Format(line, this.Data, dir));
                }
            }
            return sb.ToString();
        }
    }
}