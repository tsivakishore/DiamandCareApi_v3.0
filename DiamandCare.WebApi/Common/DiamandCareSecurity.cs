using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace DiamandCare.WebApi
{
    public static class DiamandCareSecurity
    {
        private static byte[] _key;
        private static byte[] _iv;

        static DiamandCareSecurity()
        {
            string _encryptionKey = "2016_HEIN_DiamandCare";
            Rfc2898DeriveBytes _pdb = new Rfc2898DeriveBytes(_encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            _key = _pdb.GetBytes(32);
            _iv = _pdb.GetBytes(16);
        }

        public static string ByteArrayToHexString(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", "");
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static string DecodeAndDecrypt(string cipherText)
        {
            string DecodeAndDecrypt = Decrypt(StringToByteArray(cipherText));
            return (DecodeAndDecrypt);
        }

        public static string EncryptAndEncode(string plaintext)
        {
            return ByteArrayToHexString(Encrypt(plaintext));
        }

        public static string Decrypt(Byte[] inputBytes)
        {
            Byte[] _outputBytes = inputBytes;

            string _plaintext = string.Empty;

            using (MemoryStream memoryStream = new MemoryStream(_outputBytes))
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, GetCryptoAlgorithm().CreateDecryptor(_key, _iv), CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(cryptoStream))
                    {
                        _plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }

            return _plaintext;
        }

        public static byte[] Encrypt(string inputText)
        {
            byte[] _inputBytes = UTF8Encoding.UTF8.GetBytes(inputText);

            byte[] _result = null;
            using (MemoryStream _memoryStream = new MemoryStream())
            {
                using (CryptoStream _cryptoStream = new CryptoStream(_memoryStream, GetCryptoAlgorithm().CreateEncryptor(_key, _iv), CryptoStreamMode.Write))
                {
                    _cryptoStream.Write(_inputBytes, 0, _inputBytes.Length);
                    _cryptoStream.FlushFinalBlock();
                    _result = _memoryStream.ToArray();
                }
            }

            return _result;
        }

        private static RijndaelManaged GetCryptoAlgorithm()
        {
            RijndaelManaged _algorithm = new RijndaelManaged();
            //set the mode, padding and block size
            _algorithm.Padding = PaddingMode.Zeros;
            _algorithm.Mode = CipherMode.CBC;
            _algorithm.KeySize = 128;
            _algorithm.BlockSize = 128;
            return _algorithm;
        }
    }
}