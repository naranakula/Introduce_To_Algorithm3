using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Utility
{
    public static class DESHelper
    {
        /// <summary>
        /// 使用数据加密标准 (DES) 算法加密
        /// </summary>
        /// <param name="pToEncrypt">带加密的字符串</param>
        /// <returns>如果发生异常返回null</returns>
        public static string MD5Encrypt(string pToEncrypt, string sKey)//加密
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
                des.Key = Encoding.ASCII.GetBytes(sKey);
                des.IV = Encoding.ASCII.GetBytes(sKey);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                StringBuilder ret = new StringBuilder();
                foreach (byte b in ms.ToArray())
                {
                    ret.AppendFormat("{0:X2}", b);
                }

                return ret.ToString();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 使用默认密匙和数据加密标准 (DES) 算法加密
        /// </summary>
        /// <param name="pToEncrypt"></param>
        /// <returns></returns>
        public static string MD5Encrypt(string pToEncrypt)
        {
            return MD5Encrypt(pToEncrypt,"VavicApp");
        }

        /// <summary>
        /// 使用数据加密标准 (DES) 算法解密
        /// </summary>
        /// <param name="pToDecrypt">要解密的字符串</param>
        /// <param name="sKey">设置数据加密标准 (DES) 算法的机密密钥</param>
        /// <returns>如果发生异常返回null</returns>
        public static string MD5Decrypt(string pToDecrypt, string sKey)//解密
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
                for (int x = 0; x < pToDecrypt.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }
                des.Key = Encoding.ASCII.GetBytes(sKey);
                des.IV = Encoding.ASCII.GetBytes(sKey);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                StringBuilder ret = new StringBuilder();
                return System.Text.Encoding.Default.GetString(ms.ToArray());
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 使用默认密匙和数据加密标准 (DES) 算法解密
        /// </summary>
        /// <param name="pToDecrypt">要解密的字符串</param>
        /// <returns></returns>
        public static string MD5Decrypt(string pToDecrypt)
        {
            return MD5Decrypt(pToDecrypt);
        }

        /// <summary>
        /// 创建一个DES的key
        /// </summary>
        /// <returns></returns>
        public static string GenerateKey()
        {
            DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();
            return Encoding.ASCII.GetString(desCrypto.Key);
        }

    }
}
