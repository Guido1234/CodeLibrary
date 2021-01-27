using System.Collections.Generic;

namespace CodeLibrary.Core
{ 
    public class UsbKeyDriveMessages
    {
        private Dictionary<UsbKeyDriveErrorEnum, string> _Messages = new Dictionary<UsbKeyDriveErrorEnum, string>();

        public UsbKeyDriveMessages()
        {
            _Messages.Add(UsbKeyDriveErrorEnum.Ok, "");
            _Messages.Add(UsbKeyDriveErrorEnum.CannotFindKeyOnUsbKeyDrive, "Key '{0}' cannot be found on the USBKey.");
            _Messages.Add(UsbKeyDriveErrorEnum.ExceptionReadingKeyFile, "An Exception occurred while trying to read key file {0}.");
            _Messages.Add(UsbKeyDriveErrorEnum.ExceptionWritingKeyFile, "An Exception occurred while trying to write key file {0}.");
            _Messages.Add(UsbKeyDriveErrorEnum.ExceptionCreatingKeyDrive, "An Exception occurred while trying to create a USBKey.");
            _Messages.Add(UsbKeyDriveErrorEnum.MultipleUsbDrives, "If you want to create a USBKey, only one USB Flash Drive needs to be inserted, please remove the USB Flash Drive you do not want to use as a USBKey. any formatted USB Flash Drive can be used as a USBKey, existing data will not get lost.");
            _Messages.Add(UsbKeyDriveErrorEnum.MultipleUsbKeyDrives, "There needs to be only one USB Flash Drive used as a USBKey, please remove the USBKeys you are not using.");
            _Messages.Add(UsbKeyDriveErrorEnum.NoUsbDrives, "A USB Flash Drive is required to be inserted in a USB port.");
            _Messages.Add(UsbKeyDriveErrorEnum.NoUsbKeyDrives, "There are no USBKeys available. please insert a USBKey or create one.");
        }

        public string GetMessage(UsbKeyDriveErrorEnum error) => _Messages[error];
    }
}