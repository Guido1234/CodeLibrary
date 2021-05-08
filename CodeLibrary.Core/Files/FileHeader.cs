using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLibrary.Core.Files
{

    public enum FileEncyptionMode
    {
        DefaultEncryption = 0,
        PasswordEncryption = 1,
        UsbKEYEncryption = 2
    }

    public enum FileReadResult
    {
        Succes = 0,
        ErrorUnknownIdentifier = 1,
        ErrorVersionToOld = 2,
        FileNotFound = 3,
        OpenCanceled = 4,
        ErrorReadingFile = 5
    }



    [Serializable]
    public class FileHeader
    {
        public string Identifier { get; internal set; } = Constants.FILEHEADERIDENTIFIER;
        public string Version { get; set; }
        public FileEncyptionMode FileEncyptionMode { get; set; }
        public string UsbKeyId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateChanged { get; set; }
    }
}
