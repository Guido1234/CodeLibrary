using GK.Template.Attributes;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace GK.Template.DataCommands
{
    [DataCommand(Name = "TextFile")]
    [Description("Reads out a text file per line.")]
    public sealed class DataCommandTextFile : DataCommandBase
    {
        [DataCommandParameter(Order = 0)]
        public string File { get; set; }

        public override string Execute(StringTemplateItem template)
        {
            string file = PathUtility.ParseSpecialFoldersNames(this.File, ParseSpecialFolderOption.WildCardToRealPath);

            string line = string.Empty;
            StringBuilder sb = new StringBuilder();
            if (Utils.IsFileOrDirectory(this.File) == Utils.FileOrDirectory.File)
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    while (!reader.EndOfStream) // Do not use EndOfStream
                    {
                        line = reader.ReadLine();
                        template.IsLastCommand = (reader.EndOfStream && this.IsLastCommand);
                        sb.Append(template.Format(line, this.Data, line));
                    }
                    reader.Close();
                }
            }
            return sb.ToString();
        }
    }
}