using CodeLibrary.Core;
using System.Security;
using System.Windows.Forms;

namespace CodeLibrary.Helpers
{
    public class PasswordHelper
    {
        private readonly FormCodeLibrary _mainform;
        private readonly StateIconHelper _StateIconHelper;

        public PasswordHelper(FormCodeLibrary mainform, StateIconHelper stateIconHelper)
        {
            _mainform = mainform;
            _StateIconHelper = stateIconHelper;
        }

        public SecureString Password { get; set; }

        public string UsbKeyId { get; set; }

        public void ClearPassWord()
        {
            Password = null;
            ShowKey();
        }

        /// <param name="silent">Cancels on error without message</param>
        /// <returns></returns>
        public byte[] GetUsbKey(string id, bool silent, out bool canceled)
        {
        retry:
            UsbKey _usbKeyDrive = new UsbKey();
            UsbKeyDriveMessages _messages = new UsbKeyDriveMessages();

            UsbKeyDriveErrorEnum _result;
            byte[] _key = _usbKeyDrive.GetKey(id, out _result);
            switch (_result)
            {
                case UsbKeyDriveErrorEnum.Ok:
                    canceled = false;
                    return _key;

                case UsbKeyDriveErrorEnum.NoUsbKeyDrives:
                case UsbKeyDriveErrorEnum.MultipleUsbKeyDrives:
                case UsbKeyDriveErrorEnum.ExceptionReadingKeyFile:
                case UsbKeyDriveErrorEnum.CannotFindKeyOnUsbKeyDrive:
                    if (silent)
                    {
                        canceled = true;
                        return null;
                    }

                    DialogResult _mb = MessageBox.Show(_mainform, _messages.GetMessage(_result), "USBKey", MessageBoxButtons.OKCancel);
                    if (_mb == DialogResult.OK)
                    {
                        goto retry;
                    }
                    canceled = true;
                    return null;
            }
            canceled = false;
            return _key;
        }

        public void SetPassWord()
        {
            FormSetPassword f = new FormSetPassword();
            DialogResult result = f.ShowDialog();

            if (result == DialogResult.OK)
            {
                Password = f.Password;
            }
            ShowKey();
        }

        public void SetPassWordMenuState()
        {
            bool pw = Password != null;
            bool uk = UsbKeyId != null;

            _mainform.mnuSetPassword.Enabled = !(pw == true || uk == true);
            _mainform.mnuSetUsbKey.Enabled = !(pw == true || uk == true);
            _mainform.mnuClearPassword.Enabled = (pw == true || uk == true);
            if (pw == true)
            {
                _mainform.mnuClearPassword.Text = "Clear Password";
            }
            if (uk == true)
            {
                _mainform.mnuClearPassword.Text = "Clear USBKey";
            }
            if (uk == false && pw == false)
            {
                _mainform.mnuClearPassword.Text = "Clear Password / Clear USBKey";
            }
        }

        public void SetUsbKey()
        {
        retry:
            UsbKey _usbKeyDrive = new UsbKey();
            UsbKeyDriveMessages _messages = new UsbKeyDriveMessages();
            UsbKeyDriveErrorEnum _result;
            string _key = _usbKeyDrive.CreateKey(out _result);
            switch (_result)
            {
                case UsbKeyDriveErrorEnum.NoUsbKeyDrives:

                retry2:
                    FormUsbKeyDrive _f = new FormUsbKeyDrive();
                    DialogResult _dr = _f.ShowDialog();
                    if (_dr == DialogResult.OK)
                    {
                        if (_f.CreateUsbKeyDrive)
                        {
                            _usbKeyDrive.CreateUSBKeyContainer(out _result);
                            switch (_result)
                            {
                                case UsbKeyDriveErrorEnum.NoUsbDrives:
                                case UsbKeyDriveErrorEnum.MultipleUsbKeyDrives:
                                case UsbKeyDriveErrorEnum.ExceptionCreatingKeyDrive:
                                    DialogResult _mb = MessageBox.Show(_mainform, _messages.GetMessage(_result), "USBKey", MessageBoxButtons.OKCancel);
                                    if (_mb == DialogResult.OK)
                                    {
                                        goto retry2;
                                    }
                                    return;
                            }
                        }
                        goto retry;
                    }
                    else
                    {
                        UsbKeyId = null;
                        return;
                    }
                    break;

                case UsbKeyDriveErrorEnum.MultipleUsbKeyDrives:
                case UsbKeyDriveErrorEnum.ExceptionWritingKeyFile:
                    MessageBox.Show(_messages.GetMessage(_result));
                    UsbKeyId = null;

                    break;

                case UsbKeyDriveErrorEnum.Ok:
                    UsbKeyId = _key;

                    break;
            }
            ShowKey();
        }

        internal void ShowKey()
        {
            _StateIconHelper.Encrypted = Password != null;
            SetPassWordMenuState();
        }
    }
}