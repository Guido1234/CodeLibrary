using CodeLibrary.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeLibrary.Helpers
{
    internal class FileHelperLegacy
    {
        public static void OpenFileLegacy(FileHelper fileHelper, string filename, PasswordHelper passwordHelper, FormCodeLibrary mainform, TreeviewHelper TreeHelper, out bool _succes)
        {
             // Legacy
            CodeSnippetCollectionOld _collectionOld = ReadCollectionLegacy2(filename, passwordHelper.Password, passwordHelper, mainform, out  _succes);
            if (_succes == false)
            {
                // Double Legacy
                _collectionOld = ReadCollectionLegacy1(filename, passwordHelper.Password, out _succes);
            }

            if (_succes == false)
            {
                MessageBox.Show($"Could not open the file '{filename}'", "Error opening file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                fileHelper.NewDoc();
                fileHelper.EndUpdate();
                return;
            }



            CodeLib.Instance.LoadLegacy(_collectionOld);

            if (!CodeLib.Instance.CodeSnippets.ContainsKey(Constants.TRASHCAN))
            {
                CodeLib.Instance.CodeSnippets.Add(CodeSnippet.TrashcanSnippet());
            }

            if (!CodeLib.Instance.CodeSnippets.ContainsKey(Constants.CLIPBOARDMONITOR))
            {
                CodeLib.Instance.CodeSnippets.Add(CodeSnippet.ClipboardMonitorSnippet());
            }

            fileHelper.CodeCollectionToForm(string.Empty);

            fileHelper.EndUpdate();

            TreeHelper.FindNodeByPath(_collectionOld.LastSelected);

            Config.LastOpenedFile = filename;
            FileInfo fi = new FileInfo(filename);
            Config.LastOpenedDir = fi.Directory.FullName;

            fileHelper.CurrentFile = filename;
            CodeLib.Instance.Changed = false;
            fileHelper._lastOpenedDate = DateTime.Now;
            fileHelper.SetTitle();
            _succes = true;
        }


        public static CodeSnippetCollectionOld ReadCollectionLegacy2(string filename, SecureString password, PasswordHelper passwordHelper, FormCodeLibrary mainform, out bool succes)
        {
            string usbKeyId = null;
            succes = true;
            string _fileData = string.Empty;
            SecureString _usbKeyPassword = null;
            FileContainer _container = new FileContainer();

            try
            {
                _fileData = File.ReadAllText(filename, Encoding.Default);
                _container = Utils.FromJson<FileContainer>(_fileData);
                usbKeyId = _container.UsbKeyId;
            }
            catch
            {
                succes = false;
                return null;
            }

            if (_container.Encrypted)
            {
                if (!string.IsNullOrEmpty(_container.UsbKeyId))
                {
                    bool _canceled;
                    usbKeyId = _container.UsbKeyId;

                    byte[] _key = passwordHelper.GetUsbKey(_container.UsbKeyId, false, out _canceled);
                    if (_canceled)
                    {
                        succes = false;
                        usbKeyId = null;
                        return null;
                    }
                    _usbKeyPassword = StringCipher.ToSecureString(Utils.ByteArrayToString(_key));

                    CodeSnippetCollectionOld _result1 = TryDecrypt(_container.Data, _usbKeyPassword, out succes);
                    if (succes)
                    {
                        passwordHelper.Password = null;
                        passwordHelper.UsbKeyId = usbKeyId;
                        passwordHelper.ShowKey();
                        return _result1;
                    }
                    else
                    {
                        MessageBox.Show(mainform, $"Could not open file {filename}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        passwordHelper.Password = null;
                        passwordHelper.UsbKeyId = null;
                        passwordHelper.ShowKey();
                    }
                }

                // Decrypt with given password.
                if (password == null)
                {
                    goto setPassword;
                }

            retryPassword:
                CodeSnippetCollectionOld _result = TryDecrypt(_container.Data, password, out succes);
                if (succes)
                {
                    passwordHelper.Password = password;
                    passwordHelper.UsbKeyId = null;
                    passwordHelper.ShowKey();
                    return _result;
                }

            setPassword:
                FormSetPassword _formSet = new FormSetPassword();
                DialogResult _dg = _formSet.ShowDialog();
                if (_dg == DialogResult.OK)
                {
                    password = _formSet.Password;
                    goto retryPassword;
                }
                else
                {
                    succes = false;
                    return null;
                }
            }
            else
            {
                try
                {
                    CodeSnippetCollectionOld _collection = Utils.FromJson<CodeSnippetCollectionOld>(Utils.FromBase64(_container.Data));
                    _collection.FromBase64();
                    passwordHelper.Password = null;
                    passwordHelper.UsbKeyId = null;
                    passwordHelper.ShowKey();
                    succes = true;
                    return _collection;
                }
                catch
                {
                    succes = false;
                    return null;
                }
            }
        }


        internal static CodeSnippetCollectionOld TryDecrypt(string data, SecureString password, out bool succes)
        {
            try
            {
                data = Utils.FromBase64(data);
                data = StringCipher.Decrypt(data, password);
                CodeSnippetCollectionOld _collection = Utils.FromJson<CodeSnippetCollectionOld>(data);
                _collection.FromBase64();
                succes = true;
                return _collection;
            }
            catch (Exception)
            {
            }
            succes = false;
            return null;
        }

        public static CodeSnippetCollectionOld ReadCollectionLegacy1(string filename, SecureString password, out bool succes)
        {
            succes = true;
            string _data = File.ReadAllText(filename, Encoding.Default);
            try
            {
                if (password != null)
                {
                    _data = StringCipher.Decrypt(_data, password);
                }
            }
            catch
            {
                MessageBox.Show($"Could not decrypt: '{filename}' with the current password! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                succes = false;
                return null;
            }
            try
            {
                CodeSnippetCollectionOld _collection = Utils.FromJson<CodeSnippetCollectionOld>(_data);
                _collection.FromBase64();
                if (_collection.Items == null)
                {
                    succes = false;
                    return null;
                }
                succes = true;
                return _collection;
            }
            catch
            {
                succes = false;
                return null;
            }
        }

    }
}
