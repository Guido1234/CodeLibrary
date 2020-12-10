using GK.Template.Attributes;
using System;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace GK.Template.Methods
{
    [Category("FileSystem")]
    [FormatMethod(Name = "FileToBinary", Aliasses = "")]
    [Description("Read a file and convert it to binary, input value (data) is expected to be a valid file path.")]
    public sealed class MethodFileToBinary : MethodBase
    {
        public static string ConvertToHex(byte[] data)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("0x");
            foreach (byte c in data)
            {
                int tmp = c;
                sb.AppendFormat("{0:X2}", tmp);
            }
            return sb.ToString();
        }

        public override string Apply(string value)
        {
            string file = ParseSpecialFoldersNames(value, ParseSpecialFolderOption.WildCardToRealPath);

            if (Utils.IsFileOrDirectory(file) != Utils.FileOrDirectory.File)
                return string.Format("'File: '{0}' not found'", file);

            byte[] data = File.ReadAllBytes(file);
            return ConvertToHex(data);
        }

        /// <summary>
        /// replaces #[Special folder name]
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string ParseSpecialFoldersNames(string path, ParseSpecialFolderOption option)
        {
            if (path == null)
            {
                return string.Empty;
            }
            string[] names = Enum.GetNames(typeof(Environment.SpecialFolder));
            if (option == ParseSpecialFolderOption.WildCardToRealPath)
            {
                foreach (string name in names)
                {
                    Environment.SpecialFolder specialfolder = (Environment.SpecialFolder)EnumUtility.GetValueByName(typeof(Environment.SpecialFolder), name);
                    path = path.Replace(string.Format("#{0}", name), Environment.GetFolderPath(specialfolder));
                }
                return path;
            }

            foreach (string name in names)
            {
                Environment.SpecialFolder specialfolder = (Environment.SpecialFolder)EnumUtility.GetValueByName(typeof(Environment.SpecialFolder), name);
                string toreplace = Environment.GetFolderPath(specialfolder);
                if (!string.IsNullOrEmpty(toreplace))
                {
                    path = path.Replace(Environment.GetFolderPath(specialfolder), string.Format("#{0}", name));
                }
            }
            return path;
        }
    }
}