using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace CodeLibrary.Core
{
    public class EncryptedBinaryFile<DATA, HEADER> where DATA : class
    {
        // This constant determines the number of iterations for the password bytes generation function.
        private const int DerivationIterations = 1000;

        private const int ivSize = 16;

        // This constant is used to determine the key size of the encryption algorithm in bits.
        // We divide this by 8 within the code below to get the equivalent number of bytes.
        private const int Keysize = 256;

        private string _FileName = string.Empty;
        private SecureString _Password = null;

        public EncryptedBinaryFile(string filename, SecureString password)
        {
            _FileName = filename;
            if (password == null)
            {
                _Password = ToSecureString("GUISDHISDDEFAULT2312#$%^&*JKAXw");
            }
            else
            {
                _Password = password;
            }
        }

        // block size is 128-bit
        public CryptoStream CreateDecryptionStream(byte[] key, Stream inputStream)
        {
            byte[] iv = new byte[ivSize];

            if (inputStream.Read(iv, 0, iv.Length) != iv.Length)
            {
                throw new ApplicationException("Failed to read IV from stream.");
            }

            Rijndael rijndael = new RijndaelManaged();
            rijndael.KeySize = Keysize;

            CryptoStream decryptor = new CryptoStream(inputStream, rijndael.CreateDecryptor(key, iv), CryptoStreamMode.Read);
            return decryptor;
        }

        public CryptoStream CreateEncryptionStream(byte[] key, Stream outputStream)
        {
            byte[] iv = new byte[ivSize];

            using (var rng = new RNGCryptoServiceProvider())
            {
                // Using a cryptographic random number generator
                rng.GetNonZeroBytes(iv);
            }

            // Write IV to the start of the stream
            outputStream.Write(iv, 0, iv.Length);

            Rijndael rijndael = new RijndaelManaged();
            rijndael.KeySize = Keysize;

            CryptoStream encryptor = new CryptoStream(outputStream, rijndael.CreateEncryptor(key, iv), CryptoStreamMode.Write);
            return encryptor;
        }

        public DATA Read()
        {
            byte[] key = PasswordBytes(_Password);

            using (FileStream filestream = new FileStream(_FileName, FileMode.Open))
            {
                HEADER _header = (HEADER)ReadObjectFromStream(filestream);
                using (CryptoStream cryptoStream = CreateDecryptionStream(key, filestream))
                {
                    DATA _data = (DATA)ReadObjectFromStream(cryptoStream);
                    return _data;
                }
            }
        }

        public HEADER ReadHeader()
        {
            using (FileStream filestream = new FileStream(_FileName, FileMode.Open))
            {
                HEADER _header = (HEADER)ReadObjectFromStream(filestream);
                return _header;
            }
        }

        public void Save(HEADER header, DATA data)
        {
            byte[] key = PasswordBytes(_Password);

            using (FileStream filestream = new FileStream(_FileName, FileMode.Create))
            {
                WriteObjectToStream(filestream, header);
                using (CryptoStream cryptoStream = CreateEncryptionStream(key, filestream))
                {
                    WriteObjectToStream(cryptoStream, data);
                }
            }
        }

        public void SetPasswor(SecureString password)
        {
            _Password = password;
        }

        private byte[] PasswordBytes(SecureString password)
        {
            string passPhrase = new string(SecureStringChars(password));
            string passPhrase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(passPhrase));

            byte[] key = Convert.FromBase64String(passPhrase64);

            byte[] _finalkey = new byte[32];
            for (int a = 0, b = 0; a < _finalkey.Length; a++, b++)
            {
                if (b > key.Length - 1)
                {
                    b = 0;
                }
                _finalkey[a] = key[b];
            }

            return _finalkey;
        }

        private object ReadObjectFromStream(Stream inputStream)
        {
            BinaryFormatter binForm = new BinaryFormatter();
            object obj = binForm.Deserialize(inputStream);
            return obj;
        }

        private char[] SecureStringChars(SecureString value)
        {
            char[] _result = new char[value.Length];

            IntPtr bstr = Marshal.SecureStringToBSTR(value);
            try
            {
                // Index in 2-byte (char) chunks
                //TODO: Some range validation might be good.
                for (int _i = 0; _i < _result.Length; _i++)
                {
                    byte _b = Marshal.ReadByte(bstr, _i * 2);
                    _result[_i] = (char)_b;
                }

                return _result;
            }
            finally
            {
                Marshal.FreeBSTR(bstr);
            }
        }

        private SecureString ToSecureString(string value)
        {
            SecureString _secureString = new SecureString();
            foreach (char c in value.ToCharArray())
            {
                _secureString.AppendChar(c);
            }
            return _secureString;
        }

        private void WriteObjectToStream(Stream outputStream, Object obj)
        {
            if (object.ReferenceEquals(null, obj))
            {
                return;
            }

            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(outputStream, obj);
        }
    }
}