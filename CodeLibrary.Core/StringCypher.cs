using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace CodeLibrary.Core
{
    public static class StringCipher
    {
        // This constant determines the number of iterations for the password bytes generation function.
        private const int DerivationIterations = 1000;

        // This constant is used to determine the key size of the encryption algorithm in bits.
        // We divide this by 8 within the code below to get the equivalent number of bytes.
        private const int Keysize = 256;

        public static string Decrypt(string cipherText, SecureString passPhrase) => Decrypt(cipherText, new string(SecureStringChars(passPhrase)));

        public static byte[] Decrypt(byte[] cipherText, SecureString passPhrase)
        {
            return Decrypt(cipherText, new string(SecureStringChars(passPhrase)), out int decryptedByteCount);
        }

        public static string Encrypt(string plainText, SecureString passPhrase) => Encrypt(plainText, new string(SecureStringChars(passPhrase)));

        public static byte[] Encrypt(byte[] plainText, SecureString passPhrase) => Encrypt(plainText, new string(SecureStringChars(passPhrase)));

        public static char[] SecureStringChars(SecureString value)
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

        public static SecureString ToSecureString(string value)
        {
            SecureString _secureString = new SecureString();
            foreach (char c in value.ToCharArray())
            {
                _secureString.AppendChar(c);
            }
            return _secureString;
        }

        private static string Decrypt(string cipherText, string passPhrase)
        {
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            var data = Decrypt(cipherTextBytesWithSaltAndIv, passPhrase, out int decryptedByteCount);
            return Encoding.UTF8.GetString(data, 0, decryptedByteCount);
        }

        private static byte[] Decrypt(byte[] cipherTextBytesWithSaltAndIv, string passPhrase, out int decryptedByteCount)
        {
            // Get the complete stream of bytes that represent:
            // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]

            decryptedByteCount = 0;

            // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();

                                return plainTextBytes;
                            }
                        }
                    }
                }
            }
        }

        private static string Encrypt(string plainText, string passPhrase)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            var result = Encrypt(plainTextBytes, passPhrase);
            return Convert.ToBase64String(result);
        }

        private static byte[] Encrypt(byte[] data, string passPhrase)
        {
            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same Salt and IV values can be used when decrypting.
            var saltStringBytes = Generate256BitsOfRandomEntropy();
            var ivStringBytes = Generate256BitsOfRandomEntropy();
            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(data, 0, data.Length);
                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return cipherTextBytes;
                            }
                        }
                    }
                }
            }
        }

        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }
    }
}