namespace CodeLibrary.Core
{
    public enum UsbKeyDriveErrorEnum
    {
        Ok = 0,
        MultipleUsbKeyDrives = 1,
        NoUsbDrives = 2,
        NoUsbKeyDrives = 3,
        CannotFindKeyOnUsbKeyDrive = 4,
        MultipleUsbDrives = 5,
        ExceptionReadingKeyFile = 6,
        ExceptionWritingKeyFile = 7,
        ExceptionCreatingKeyDrive = 8
    }
} 