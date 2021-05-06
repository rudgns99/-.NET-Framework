using System;
using System.Collections.Generic;
using System.Text;

namespace GWF
{
    /// <summary>
    /// Hash Helper
    /// 
    /// MD5 and SHA1
    /// </summary>
    public class HashHelper
    {
        /// <summary>
        /// MD5 hash string
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetMD5HashHexString(string data)
        {
            if (string.IsNullOrEmpty(data))
                return string.Empty;

            byte[] pass = Encoding.UTF8.GetBytes(data);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

            string result = string.Empty;
            foreach (byte bt in md5.ComputeHash(pass))
            {
                result += string.Format("{0:X2}", bt);
            }

            return result;
        }

        /// <summary>
        /// MD5 Hash raw data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] GetMD5HashArray(string data)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

            return md5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(data));
        }

        /// <summary>
        /// SHA1 hash string
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetSHA1HashHexString(string data)
        {
            if (string.IsNullOrEmpty(data))
                return string.Empty;

            byte[] pass = Encoding.UTF8.GetBytes(data);
            System.Security.Cryptography.SHA1 sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();

            string result = string.Empty;
            foreach (byte bt in sha1.ComputeHash(pass))
            {
                result += string.Format("{0:X2}", bt);
            }

            return result;
        }

        /// <summary>
        /// SHA256 hash string
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetSHA256HashHexString(string data)
        {
            if (string.IsNullOrEmpty(data))
                return string.Empty;

            byte[] pass = Encoding.UTF8.GetBytes(data);
            System.Security.Cryptography.SHA256Managed sha256 = new System.Security.Cryptography.SHA256Managed();

            string result = string.Empty;
            foreach (byte bt in sha256.ComputeHash(pass))
            {
                result += string.Format("{0:X2}", bt);
            }

            return result;
        }

        /// <summary>
        /// SHA512 hash string
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetSHA512HashHexString(string data)
        {
            if (string.IsNullOrEmpty(data))
                return string.Empty;

            byte[] pass = Encoding.UTF8.GetBytes(data);
            System.Security.Cryptography.SHA512Managed sha512 = new System.Security.Cryptography.SHA512Managed();

            string result = string.Empty;
            foreach (byte bt in sha512.ComputeHash(pass))
            {
                result += string.Format("{0:X2}", bt);
            }

            return result;
        }

        /// <summary>
        /// Adler32 Checksum
        /// </summary>
        /// <param name="filePath">Checksum이 필요한 파일 경로(full path)</param>
        /// <returns>checksum</returns>
        public static string GetAdler32ChecksumHexString(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
                return string.Empty;

            Adler32 adler32 = new Adler32();

            using (System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                byte[] buffer = new byte[8192];
                int size = 0;
                while ((size = fs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    adler32.Update(buffer, 0, size);
                }
            }

            return adler32.Value.ToString();
        }
    }
}
