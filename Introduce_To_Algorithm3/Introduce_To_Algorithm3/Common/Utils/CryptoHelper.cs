using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils
{
    /// <summary>
    /// 加解密辅助类
    /// </summary>
    public static class CryptoHelper
    {

        #region 哈希 HashAlgorithm 将一个变长binary string影射为一个hash值

        /*
继承结构
System.Object
  System.Security.Cryptography.HashAlgorithm(抽象类)
    System.Security.Cryptography.KeyedHashAlgorithm
    System.Security.Cryptography.MD5
    System.Security.Cryptography.RIPEMD160
    System.Security.Cryptography.SHA1
    System.Security.Cryptography.SHA256
    System.Security.Cryptography.SHA384
    System.Security.Cryptography.SHA512

提供了7种实现,常用的有MD5,SHA1,SHA256
安全性MD5<SHA1<SHA256
速度  MD5>SHA1>SHA256  建议使用SHA1
         * 
         */

        #region SHA1
        
        /// <summary>
        /// 如果输入为null,返回null,否则返回结果为160位的sha1的hash
        /// </summary>
        /// <param name="intpuBytes"></param>
        /// <returns></returns>
        public static byte[] Sha1Hash(byte[] intpuBytes)
        {
            if (intpuBytes == null)
            {
                return null;
            }
            
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                return sha1.ComputeHash(intpuBytes);
            }
        }

        #endregion

        #region SHA256

        /// <summary>
        /// 如果输入为null,返回null,否则返回结果为256位的sha256的hash
        /// </summary>
        /// <param name="intpuBytes"></param>
        /// <returns></returns>
        public static byte[] Sha256Hash(byte[] intpuBytes)
        {
            if (intpuBytes == null)
            {
                return null;
            }
            
            using (SHA256Managed sha256 = new SHA256Managed())
            {
                return sha256.ComputeHash(intpuBytes);
            }
        }


        #endregion

        #region MD5

        /// <summary>
        /// MD5已经不安全了
        /// 线程安全
        /// </summary>
        /// <param name="inputBytes">如果输入为null,直接返回null</param>
        /// <returns>如果输入为null,直接返回null，否则返回32位长度的byte数组</returns>
        public static byte[] Md5Hash(byte[] inputBytes)
        {
            if (inputBytes == null)
            {
                return null;
            }

            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                return md5.ComputeHash(inputBytes);
            }
        }

        #endregion


        #endregion

        #region 加密密钥生成

        /*
         * 
继承层次
System.Object
  System.Security.Cryptography.DeriveBytes
    System.Security.Cryptography.PasswordDeriveBytes
    System.Security.Cryptography.Rfc2898DeriveBytes

            建议使用Rfc2898DeriveBytes(PBKDF2标准),而不是PasswordDeriveBytes(PBKDF1标准)
         */

        /// <summary>
        /// 生成指定长度的随机数组
        /// </summary>
        /// <param name="password"></param>
        /// <param name="saltBytes">长度至少8位The salt size must be 8 bytes or larger.</param>
        /// <param name="numberToGenerate">要去大于0,要生成的随机数组的长度</param>
        public static byte[] GenerateRfc2898Bytes(string password,byte[] saltBytes,int numberToGenerate)
        {
            //Initializes a new instance of the Rfc2898DeriveBytes class using a password and salt to derive the key.

            using (Rfc2898DeriveBytes deriver = new Rfc2898DeriveBytes(password,saltBytes))
            {
                return deriver.GetBytes(numberToGenerate);
            }
        }

        /// <summary>
        /// 随机生成一个指定长度的salt
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] GenerateRandomBytes(int length = 8)
        {
            byte[] result = new byte[length];
            Random rand = new Random();
            int minValue = byte.MinValue;
            int maxValue = byte.MaxValue+1;
            for (int i = 0; i < length; i++)
            {
                //[0,255]
                result[i] =(byte) (rand.Next(minValue, maxValue));
            }

            return result;
        }

        #endregion

        #region 对称加密

        /*
         * 对称加密的继承层次
 System.Object
  System.Security.Cryptography.SymmetricAlgorithm
    System.Security.Cryptography.Aes
    System.Security.Cryptography.DES
    System.Security.Cryptography.RC2
    System.Security.Cryptography.Rijndael
    System.Security.Cryptography.TripleDES

        对称加密:加密解密密钥相同 常见的算法有des,3des,aes
        建议使用aes
        安全性aes>3des>des
        酸度aes>des>3des
         */

        #region Aes加密
        
        /// <summary>
        /// 生成Aes的密钥和初始向量
        /// item1是密钥item2是初始向量
        /// 生成的key是32个字节256位
        /// 生成的初始向量IV是16个字节128位
        /// </summary>
        /// <returns></returns>
        public static Tuple<byte[], byte[]> GenerateAesKeyAndIv()
        {
            using (AesManaged managedAes = new AesManaged())
            {
                byte[] keyBytes = managedAes.Key;
                byte[] ivBytes = managedAes.IV;
                //复制一份
                keyBytes = keyBytes.ToList().ToArray();
                ivBytes = ivBytes.ToList().ToArray();
                return new Tuple<byte[], byte[]>(keyBytes,ivBytes);
            }
        }

        /// <summary>
        /// 如果sourceBytes为null,返回null,否则使用aes算法加密
        /// </summary>
        /// <param name="keyBytes">密钥</param>
        /// <param name="ivBytes">初始向量</param>
        /// <param name="sourceBytes">要加密的字节数组</param>
        /// <returns></returns>
        public static byte[] AesEncrypt(byte[] keyBytes, byte[] ivBytes, byte[] sourceBytes)
        {
            if (sourceBytes == null)
            {
                return null;
            }
            using (AesManaged managedAes = new AesManaged())
            {
                //创建加密器
                using (ICryptoTransform encryptor = managedAes.CreateEncryptor(keyBytes,ivBytes))
                {
                    using (MemoryStream msStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream =
                            new CryptoStream(msStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(sourceBytes,0,sourceBytes.Length);
                        }
                        //将流内容写入字节数组，而与 System.IO.MemoryStream.Position 属性无关。
                        return msStream.ToArray();
                    }
                }
                
            }
        }

        /// <summary>
        /// 如果sourceBytes为null,返回null,否则使用aes算法解密
        /// </summary>
        /// <param name="keyBytes">密钥</param>
        /// <param name="ivBytes">初始向量</param>
        /// <param name="sourceBytes">要解密的字节数组</param>
        /// <returns></returns>
        public static byte[] AesDecrypt(byte[] keyBytes, byte[] ivBytes, byte[] sourceBytes)
        {
            if (sourceBytes == null)
            {
                return null;
            }
            using (AesManaged managedAes = new AesManaged())
            {
                //创建解密器
                using (ICryptoTransform decryptor = managedAes.CreateDecryptor(keyBytes,ivBytes))
                {
                    using (MemoryStream msStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream =
                            new CryptoStream(msStream, decryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(sourceBytes,0,sourceBytes.Length);
                        }
                        //将流内容写入字节数组，而与 System.IO.MemoryStream.Position 属性无关。
                        return msStream.ToArray();
                    }
                }

            }
        }

        #endregion

        #region Des加密

        /// <summary>
        /// 生成Des的密钥和初始向量
        /// item1是密钥item2是初始向量
        /// 生成的key是8个字节64位
        /// 生成的初始向量IV是8个字节64位
        /// </summary>
        /// <returns></returns>
        public static Tuple<byte[], byte[]> GenerateDesKeyAndIv()
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] keyBytes = des.Key;
                byte[] ivBytes = des.IV;
                //复制一份
                keyBytes = keyBytes.ToList().ToArray();
                ivBytes = ivBytes.ToList().ToArray();
                return new Tuple<byte[], byte[]>(keyBytes, ivBytes);
            }
        }

        /// <summary>
        /// 如果sourceBytes为null,返回null,否则使用Des算法加密
        /// </summary>
        /// <param name="keyBytes">密钥</param>
        /// <param name="ivBytes">初始向量</param>
        /// <param name="sourceBytes">要加密的字节数组</param>
        /// <returns></returns>
        public static byte[] DesEncrypt(byte[] keyBytes, byte[] ivBytes, byte[] sourceBytes)
        {
            if (sourceBytes == null)
            {
                return null;
            }
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                //创建加密器
                using (ICryptoTransform encryptor = des.CreateEncryptor(keyBytes, ivBytes))
                {
                    using (MemoryStream msStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream =
                            new CryptoStream(msStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(sourceBytes, 0, sourceBytes.Length);
                        }
                        //将流内容写入字节数组，而与 System.IO.MemoryStream.Position 属性无关。
                        return msStream.ToArray();
                    }
                }

            }
        }

        /// <summary>
        /// 如果sourceBytes为null,返回null,否则使用Des算法解密
        /// </summary>
        /// <param name="keyBytes">密钥</param>
        /// <param name="ivBytes">初始向量</param>
        /// <param name="sourceBytes">要解密的字节数组</param>
        /// <returns></returns>
        public static byte[] DesDecrypt(byte[] keyBytes, byte[] ivBytes, byte[] sourceBytes)
        {
            if (sourceBytes == null)
            {
                return null;
            }
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                //创建解密器
                using (ICryptoTransform decryptor = des.CreateDecryptor(keyBytes, ivBytes))
                {
                    using (MemoryStream msStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream =
                            new CryptoStream(msStream, decryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(sourceBytes, 0, sourceBytes.Length);
                        }
                        //将流内容写入字节数组，而与 System.IO.MemoryStream.Position 属性无关。
                        return msStream.ToArray();
                    }
                }

            }
        }

        #endregion

        #region 3Des加密

        /// <summary>
        /// 生成Des3的密钥和初始向量
        /// item1是密钥item2是初始向量
        /// 生成的key是24个字节192位
        /// 生成的初始向量IV是8个字节64位
        /// </summary>
        /// <returns></returns>
        public static Tuple<byte[], byte[]> GenerateDes3KeyAndIv()
        {
            using (TripleDESCryptoServiceProvider tripleDes = new TripleDESCryptoServiceProvider())
            {
                byte[] keyBytes = tripleDes.Key;
                byte[] ivBytes = tripleDes.IV;
                //复制一份
                keyBytes = keyBytes.ToList().ToArray();
                ivBytes = ivBytes.ToList().ToArray();
                return new Tuple<byte[], byte[]>(keyBytes, ivBytes);
            }
        }

        /// <summary>
        /// 如果sourceBytes为null,返回null,否则使用Des3算法加密
        /// </summary>
        /// <param name="keyBytes">密钥</param>
        /// <param name="ivBytes">初始向量</param>
        /// <param name="sourceBytes">要加密的字节数组</param>
        /// <returns></returns>
        public static byte[] Des3Encrypt(byte[] keyBytes, byte[] ivBytes, byte[] sourceBytes)
        {
            if (sourceBytes == null)
            {
                return null;
            }
            using (TripleDESCryptoServiceProvider des3 = new TripleDESCryptoServiceProvider())
            {
                //创建加密器
                using (ICryptoTransform encryptor = des3.CreateEncryptor(keyBytes, ivBytes))
                {
                    using (MemoryStream msStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream =
                            new CryptoStream(msStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(sourceBytes, 0, sourceBytes.Length);
                        }
                        //将流内容写入字节数组，而与 System.IO.MemoryStream.Position 属性无关。
                        return msStream.ToArray();
                    }
                }

            }
        }

        /// <summary>
        /// 如果sourceBytes为null,返回null,否则使用Des3算法解密
        /// </summary>
        /// <param name="keyBytes">密钥</param>
        /// <param name="ivBytes">初始向量</param>
        /// <param name="sourceBytes">要解密的字节数组</param>
        /// <returns></returns>
        public static byte[] Des3Decrypt(byte[] keyBytes, byte[] ivBytes, byte[] sourceBytes)
        {
            if (sourceBytes == null)
            {
                return null;
            }
            using (TripleDESCryptoServiceProvider des3 = new TripleDESCryptoServiceProvider())
            {
                //创建解密器
                using (ICryptoTransform decryptor = des3.CreateDecryptor(keyBytes, ivBytes))
                {
                    using (MemoryStream msStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream =
                            new CryptoStream(msStream, decryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(sourceBytes, 0, sourceBytes.Length);
                        }
                        //将流内容写入字节数组，而与 System.IO.MemoryStream.Position 属性无关。
                        return msStream.ToArray();
                    }
                }

            }
        }

        
        #endregion

        #endregion

        #region 非对称加密

        /*
         * 
微软的rsa只能使用公钥加密，私钥解密
非对称加密的继承层次
System.Object
  System.Security.Cryptography.AsymmetricAlgorithm
    System.Security.Cryptography.DSA
    System.Security.Cryptography.ECDiffieHellman
    System.Security.Cryptography.ECDsa
    System.Security.Cryptography.RSA
建议使用RSA
         * 
         * 
         */

        /// <summary>
        /// 返回xml格式的rsa 公钥私钥
        /// 返回1024位密钥
        /// Item1包含公钥私钥
        /// Item2只包含公钥
        /// </summary>
        /// <returns></returns>
        public static Tuple<String,String> GenerateRsaKeyInXmlFormat()
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                //true,包含私钥和公钥,false,只包含公钥
                string publicAndPrivateKey = rsa.ToXmlString(includePrivateParameters: true);
                string onlyPublicKey = rsa.ToXmlString(includePrivateParameters: false);
                return new Tuple<string, string>(publicAndPrivateKey,onlyPublicKey);
            }
        }
        /// <summary>
        /// .NET Framework 中提供的 RSA 算法规定：待加密的字节数不能超过密钥的长度值除以 8 再减去 11（即：RSACryptoServiceProvider.KeySize / 8 - 11）
        /// 如果keysize是1024(默认的),则最大是117
        /// </summary>
        private const int MaxNumBytePerEncryptDecrypt = 111;

        /// <summary>
        /// Rsa加密
        /// </summary>
        /// <param name="xmlKeyPair">加密仅需公钥</param>
        /// <param name="inputBytes"></param>
        /// <returns></returns>
        public static byte[] RsaEncrypt(string xmlKeyPair, byte[] inputBytes)
        {
            if (inputBytes == null)
            {
                return null;
            }
            //.NET Framework 中提供的 RSA 算法规定：待加密的字节数不能超过密钥的长度值除以 8 再减去 11（即：RSACryptoServiceProvider.KeySize / 8 - 11）

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                int maxNumPerEncrypt = rsa.KeySize / 8 - 12;
                
                rsa.FromXmlString(xmlKeyPair);
                //true,使用OAEP padding(只在xp以后支持),false使用PKCS#1 v1.5 padding
                return rsa.Encrypt(inputBytes, false);
            }
        }

        /// <summary>
        /// Rsa解密
        /// </summary>
        /// <param name="xmlKeyPair">解密需要私钥</param>
        /// <param name="inputBytes"></param>
        /// <returns></returns>
        public static byte[] RsaDecrypt(String xmlKeyPair, byte[] inputBytes)
        {
            if (inputBytes == null)
            {
                return null;
            }

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(xmlKeyPair);
                //true,使用OAEP padding(只在xp以后支持),false使用PKCS#1 v1.5 padding
                return rsa.Decrypt(inputBytes, false);
            }
        }
        
        #endregion

        #region 测试


        public static void TestMain(string[] args)
        {
            string text = "hello world";
            //sha1哈希
            byte[] bytes = Sha1Hash(Encoding.UTF8.GetBytes(text));
            //生成指定长度的随机字符串
            bytes = GenerateRfc2898Bytes("123456", GenerateRandomBytes(8), 1000);

            var tupleKeyIv = GenerateAesKeyAndIv();
            var bytes2 = AesEncrypt(tupleKeyIv.Item1, tupleKeyIv.Item2, bytes);
            var byte3 = AesDecrypt(tupleKeyIv.Item1, tupleKeyIv.Item2, bytes2);
            bool result = CollectionUtils.EqualsEx(bytes, byte3);

            tupleKeyIv = GenerateDesKeyAndIv();
            bytes2 = DesEncrypt(tupleKeyIv.Item1, tupleKeyIv.Item2, bytes);
            byte3 = DesDecrypt(tupleKeyIv.Item1, tupleKeyIv.Item2, bytes2);
            result = CollectionUtils.EqualsEx(bytes, byte3);

            tupleKeyIv = GenerateDes3KeyAndIv();
            bytes2 = Des3Encrypt(tupleKeyIv.Item1, tupleKeyIv.Item2, bytes);
            byte3 = Des3Decrypt(tupleKeyIv.Item1, tupleKeyIv.Item2, bytes2);
            result = CollectionUtils.EqualsEx(bytes, byte3);

            var keyPair = GenerateRsaKeyInXmlFormat();
            bytes2 = RsaEncrypt(keyPair.Item2, bytes);
            byte3 = RsaDecrypt(keyPair.Item1, bytes2);
            result = CollectionUtils.EqualsEx(bytes, byte3);
        }
        #endregion

    }
}
