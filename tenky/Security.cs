using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Security.Cryptography;

namespace DroidBeta.Tenky.Security
{
    public static class Crypto
    {
        #region define
        private static readonly SHA1CryptoServiceProvider _sha1 = new SHA1CryptoServiceProvider();
        private static readonly SHA256CryptoServiceProvider _sha256 = new SHA256CryptoServiceProvider();
        private static readonly SHA384CryptoServiceProvider _sha384 = new SHA384CryptoServiceProvider();
        private static readonly SHA512CryptoServiceProvider _sha512 = new SHA512CryptoServiceProvider();
        private static readonly MD5CryptoServiceProvider _md5 = new MD5CryptoServiceProvider();
        #endregion

        public static string MD5(string str)
        {
            return BitConverter.ToString(_md5.ComputeHash(UTF8Encoding.Default.GetBytes(str))).Replace("-", "").ToLower();
        }

        #region SHA
        public static string SHA1(string str)
        {
            return BitConverter.ToString((byte[])_sha1.ComputeHash((byte[])Encoding.UTF8.GetBytes(str))).Replace("-", string.Empty).ToLower();
        }

        public static string SHA256(string str)
        {
            return BitConverter.ToString((byte[])_sha256.ComputeHash((byte[])Encoding.UTF8.GetBytes(str))).Replace("-", string.Empty).ToLower();
        }

        public static string SHA384(string str)
        {
            return BitConverter.ToString((byte[])_sha384.ComputeHash((byte[])Encoding.UTF8.GetBytes(str))).Replace("-", string.Empty).ToLower();
        }
        public static string SHA512(string str)
        {
            return BitConverter.ToString((byte[])_sha512.ComputeHash((byte[])Encoding.UTF8.GetBytes(str))).Replace("-", string.Empty).ToLower();
        }
        #endregion

        #region Base64
        public static string Base64(string str)
        {
            return Convert.ToBase64String((byte[])Encoding.Default.GetBytes(str));
        }

        public static string DeBase64(string str)
        {
            return Encoding.Default.GetString((byte[])Convert.FromBase64String(str));
        }
        #endregion

        #region AES
        private const string _defaultAesIv = "0000000000000000";

        public static string Aes(string str, string key) => Aes(str, key, _defaultAesIv, CipherMode.ECB, PaddingMode.PKCS7);
        
        public static string Aes(string str, string key, string iv) => Aes(str, key, iv, CipherMode.ECB, PaddingMode.PKCS7);
        
        public static string Aes(string str, string key, string iv, CipherMode cipherMode, PaddingMode paddingMode)
        {
            Byte[] keyArray = Encoding.UTF8.GetBytes(key);
            Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);
            var rijndael = new RijndaelManaged()
            {
                Key = keyArray,
                Mode = cipherMode,
                Padding = paddingMode,
                IV = Encoding.UTF8.GetBytes(iv)
            };
            ICryptoTransform cTransform = rijndael.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string DeAes(string str, string key) => DeAes(str, key, _defaultAesIv, CipherMode.ECB, PaddingMode.PKCS7);
        
        public static string DeAes(string str, string key, string iv) => DeAes(str, key, iv, CipherMode.ECB, PaddingMode.PKCS7);

        public static string DeAes(string str, string key, string iv, CipherMode cipherMode, PaddingMode paddingMode)
        {
            Byte[] keyArray = Encoding.UTF8.GetBytes(key);
            Byte[] toEncryptArray = Convert.FromBase64String(str);
            var rijndael = new RijndaelManaged()
            {
                Key = keyArray,
                Mode = cipherMode,
                Padding = paddingMode,
                IV = Encoding.UTF8.GetBytes(iv)
            };
            ICryptoTransform cTransform = rijndael.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Encoding.UTF8.GetString(resultArray);
        }
        #endregion

    }

}
