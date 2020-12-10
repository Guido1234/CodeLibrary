using DevToys;
using GK.Template.Attributes;
using System.ComponentModel;
using System.Text;

namespace GK.Template.DataCommands
{
    [DataCommand(Name = "CsvFile", Example = "CsvFile(\"Path\")")]
    [Description("Reads out a comma separated file. The separator will be auto detected.")]
    public sealed class DataCommandCsvFile : DataCommandBase
    {
        [DataCommandParameter(Order = 0)]
        public string File { get; set; }

        public override string Execute(StringTemplateItem template)
        {
            string file = PathUtility.ParseSpecialFoldersNames(this.File, ParseSpecialFolderOption.WildCardToRealPath);

            string[] values = new string[0];
            StringBuilder sb = new StringBuilder();
            if (Utils.IsFileOrDirectory(file) == Utils.FileOrDirectory.File)
            {
                using (CsvStreamReader reader = new CsvStreamReader(file))
                {
                    reader.Separator = ',';

                    while (!reader.EndOfCsvStream) // Do not use EndOfStream
                    {
                        string line = reader.ReadLine();
                        values = Utils.SplitEscaped(line, reader.Separator, '"');

                        template.IsLastCommand = (reader.EndOfCsvStream && this.IsLastCommand);
                        sb.Append(template.Format(line, this.Data, values));
                    }
                    reader.Close();
                }
            }
            return sb.ToString();
        }
    }
}