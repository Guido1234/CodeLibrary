using System.IO;
using System.Linq;

namespace CodeLibrary.Core
{
    public class UsbKey
    {
        /// <summary>
        /// requires a single usb drive marked as USBKey to be inserted. Other usb drives are allowed as long the do not have the EncrKeys folder
        /// </summary>
        public string CreateKey(out UsbKeyDriveErrorEnum error)
        {
            UsbDriveInfo _driveInfo = new UsbDriveInfo();
            if (_driveInfo.GetUsbKeyDrives().Count == 0)
            {
                error = UsbKeyDriveErrorEnum.NoUsbKeyDrives;
                return null;
            }
            if (_driveInfo.GetUsbKeyDrives().Count > 1)
            {
                error = UsbKeyDriveErrorEnum.MultipleUsbKeyDrives;
                return null;
            }

            return _driveInfo.CreateKey(out error);
        }

        /// <summary>
        /// Requires a single usb drive to be inserted.
        /// </summary>
        /// <param name="error"></param>
        public void CreateUSBKeyContainer(out UsbKeyDriveErrorEnum error)
        {
            UsbDriveInfo _driveInfo = new UsbDriveInfo();
            if (_driveInfo.GetUsbDrives().Count == 0)
            {
                error = UsbKeyDriveErrorEnum.NoUsbDrives;
                return;
            }
            if (_driveInfo.GetUsbDrives().Count > 1)
            {
                error = UsbKeyDriveErrorEnum.MultipleUsbDrives;
                return;
            }
            DriveInfo _drive = _driveInfo.GetUsbDrives().First();

            _driveInfo.CreateUsbKeyDrive(_drive, out error);
        }

        /// <summary>
        /// requires a single usb drive marked as key drive to be inserted. Other usb drives are allowed as long the do not have the EncrKeys folder
        /// </summary>
        public byte[] GetKey(string id, out UsbKeyDriveErrorEnum error)
        {
            UsbDriveInfo _driveInfo = new UsbDriveInfo();
            if (_driveInfo.GetUsbKeyDrives().Count == 0)
            {
                error = UsbKeyDriveErrorEnum.UsbKeyRequiredToOpen;
                return null;
            }
            if (_driveInfo.GetUsbKeyDrives().Count > 1)
            {
                error = UsbKeyDriveErrorEnum.MultipleUsbKeyDrives;
                return null;
            }

            return _driveInfo.GetKey(id, out error);
        }

        public bool UsbKeyDrivePresent()
        {
            UsbDriveInfo _driveInfo = new UsbDriveInfo();
            return _driveInfo.GetUsbKeyDrives().Count() == 1;
        }
    }
}