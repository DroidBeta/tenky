using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.IO;
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

        public enum Mode { Encrypt = 0, Decrypt = 1 };

        //TIP: Length of AES key should be 32
        #region AES
        private const string _defaultAesIv = "0000000000000000";

        public static string Aes(string str, string key) => AesHelper(str, key, _defaultAesIv, CipherMode.ECB, PaddingMode.PKCS7, Mode.Encrypt);

        public static string Aes(string str, string key, string iv) => AesHelper(str, key, iv, CipherMode.ECB, PaddingMode.PKCS7, Mode.Encrypt);

        public static string DeAes(string str, string key) => AesHelper(str, key, _defaultAesIv, CipherMode.ECB, PaddingMode.PKCS7, Mode.Decrypt);

        public static string DeAes(string str, string key, string iv) => AesHelper(str, key, iv, CipherMode.ECB, PaddingMode.PKCS7, Mode.Decrypt);

        public static string AesHelper(string str, string key, string iv, CipherMode cipherMode, PaddingMode paddingMode, Mode mode)
        {
            if (key.Length != 32)
            {
                throw new ArgumentOutOfRangeException("Key length should be 32!");
            }
            Byte[] keyArray = Encoding.UTF8.GetBytes(key);
            Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);
            var rijndael = new RijndaelManaged()
            {
                Key = keyArray,
                Mode = cipherMode,
                Padding = paddingMode,
                IV = Encoding.UTF8.GetBytes(iv)
            };
            ICryptoTransform cTransform;
            if (mode == Mode.Encrypt)
                cTransform = rijndael.CreateEncryptor();
            else if (mode == Mode.Decrypt)
                cTransform = rijndael.CreateDecryptor();
            else
                throw new ArgumentOutOfRangeException("Mode is not valid");
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            rijndael.Dispose();
            cTransform.Dispose();

            if (mode == Mode.Encrypt)
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            else
                return Encoding.UTF8.GetString(resultArray);

        }
        #endregion

        #region DES

        private static string _defaultDesIv = "00000000";

        public static string Des(string str, string key) => DesHelper(str, key, _defaultDesIv, Mode.Encrypt);

        public static string Des(string str, string key, string iv) => DesHelper(str, key, iv, Mode.Encrypt);

        public static string DeDes(string str, string key) => DesHelper(str, key, _defaultDesIv, Mode.Decrypt);

        public static string DeDes(string str, string key, string iv) => DesHelper(str, key, iv, Mode.Decrypt);

        public static string DesHelper(string str, string key, string iv, Mode mode)
        {
            if (key.Length != 8)
                throw new ArgumentOutOfRangeException("Key length should be 8!");

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Convert.FromBase64String(str);
            des.Key = Encoding.UTF8.GetBytes(key);
            des.IV = Encoding.UTF8.GetBytes(iv);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs;
            if (mode == Mode.Encrypt)
                cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            else if (mode == Mode.Decrypt)
                cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            else
                throw new ArgumentOutOfRangeException("Mode is not valid");
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            cs.Dispose();
            ms.Dispose();
            des.Dispose();

            if (mode == Mode.Encrypt)
                return Convert.ToBase64String(ms.ToArray());
            else
                return Encoding.Default.GetString(ms.ToArray());
        }

        #endregion

        #region CRC32
        public static ulong[] Crc32Table
        {
            get
            {
                if (Crc32Table == null)
                {
                    Crc32Table = new ulong[256];
                    Crc32Table = GetCRC32Table();
                }
                return Crc32Table;
            }

            private set { }
        }

        public static ulong[] GetCRC32Table()
        {
            ulong Crc;
            var _tmp = new ulong[256];
            int i, j;
            for (i = 0; i < 256; i++)
            {
                Crc = (ulong)i;
                for (j = 8; j > 0; j--)
                {
                    if ((Crc & 1) == 1)
                        Crc = (Crc >> 1) ^ 0xEDB88320;
                    else
                        Crc >>= 1;
                }
                _tmp[i] = Crc;
            }
            return _tmp;
        }

        public static ulong CRC32(string input)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(input); ulong value = 0xffffffff;
            int len = buffer.Length;
            for (int i = 0; i < len; i++)
            {
                value = (value >> 8) ^ Crc32Table[(value & 0xFF) ^ buffer[i]];
            }
            return value ^ 0xffffffff;
        }

        public static string CRC32b(string input) => CRC32(input).ToString("x");
        #endregion
    }

}
