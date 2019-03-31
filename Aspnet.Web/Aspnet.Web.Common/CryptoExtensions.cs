using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Aspnet.Web.Common
{
    public static class CryptoExtensions
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Encrypt(this string value)
        {
            if (value.IsNullOrEmpty()) return "";
            return CryptoHelper.Encrypt(value);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Decrypt(this string value)
        {
            if (value.IsNullOrEmpty()) return "";
            return CryptoHelper.Decrypt(value);
        }
    }

    public static class CryptoHelper
    {
        private static readonly ICryptoTransform _encryptor;
        private static readonly ICryptoTransform _decryptor;
        private static readonly int BufferSize = 1024;
        private static readonly byte[] _iv = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        private static readonly string _key = "samples";
        private static readonly string _algorithm = "TripleDES";

        static CryptoHelper()
        {
            SymmetricAlgorithm provider = SymmetricAlgorithm.Create(_algorithm);
            provider.Key = Encoding.UTF8.GetBytes(_key);
            provider.IV = _iv;

            _encryptor = provider.CreateEncryptor();
            _decryptor = provider.CreateDecryptor();
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Encrypt(string text)
        {
            byte[] clearBuffer = Encoding.UTF8.GetBytes(text);
            MemoryStream clearStream = new MemoryStream(clearBuffer);
            MemoryStream encryptedStream = new MemoryStream();

            CryptoStream cryptoStream = new CryptoStream(encryptedStream, _encryptor, CryptoStreamMode.Write);
            int bytesRead = 0;
            byte[] buffer = new byte[BufferSize];
            do
            {
                bytesRead = clearStream.Read(buffer, 0, BufferSize);
                cryptoStream.Write(buffer, 0, bytesRead);
            } while (bytesRead > 0);

            cryptoStream.FlushFinalBlock();
            buffer = encryptedStream.ToArray();
            string encryptedText = Convert.ToBase64String(buffer);
            return encryptedText;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Decrypt(string text)
        {
            byte[] encryptedBuffer = Convert.FromBase64String(text);
            Stream encryptedStream = new MemoryStream(encryptedBuffer);

            MemoryStream clearStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(encryptedStream, _decryptor, CryptoStreamMode.Read);

            int bytesRead = 0;
            byte[] buffer = new byte[BufferSize];

            do
            {
                bytesRead = cryptoStream.Read(buffer, 0, BufferSize);
                clearStream.Write(buffer, 0, bytesRead);
            } while (bytesRead > 0);

            buffer = clearStream.GetBuffer();
            string clearText = Encoding.UTF8.GetString(buffer, 0, (int)clearStream.Length);

            return clearText;
        }
    }
}
