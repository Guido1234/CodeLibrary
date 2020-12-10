using GK.Template.Attributes;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace GK.Template.DataCommands
{
    [DataCommand(Name = "PositionBasedFile")]
    [Description("Split each line in a document by fixed positions.")]
    public sealed class DataCommandPositionBasedFile : DataCommandBase
    {
        [DataCommandParameter(Order = 0)]
        public string File { get; set; }

        [DataCommandParameter(Order = 1)]
        public int[] Positions { get; set; }

        public override string Execute(StringTemplateItem template)
        {
            string file = PathUtility.ParseSpecialFoldersNames(this.File, ParseSpecialFolderOption.WildCardToRealPath);

            string line = string.Empty;
            string[] values = new string[0];
            StringBuilder sb = new StringBuilder();
            if (Utils.IsFileOrDirectory(file) == Utils.FileOrDirectory.File)
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    while (!reader.EndOfStream) // Do not use EndOfStream
                    {
                        line = reader.ReadLine();
                        template.IsLastCommand = (reader.EndOfStream && this.IsLastCommand);
                        values = Utils.SplitByPosition(line, Positions);
                        sb.Append(template.Format(line, this.Data, values));
                    }
                    reader.Close();
                }
            }
            return sb.ToString();
        }
    }
}