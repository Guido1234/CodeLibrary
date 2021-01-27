using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLibrary.Core
{
    public class FileContainer
    {
        public string Version { get; set; }

        public bool Encrypted { get; set; }

        public string Data { get; set; }

        public string UsbKeyId { get; set; }

    }
}
