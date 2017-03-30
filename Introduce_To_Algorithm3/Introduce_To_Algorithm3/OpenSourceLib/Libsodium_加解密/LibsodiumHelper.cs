using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Mappers;
using Sodium;

namespace Introduce_To_Algorithm3.OpenSourceLib.Libsodium_加解密
{
    /// <summary>
    /// Libsodium加解密帮助类
    /// 可以通过nuget添加
    /// https://bitbeans.gitbooks.io/libsodium-net/content/index.html
    /// libsodium requires the Visual C++ Redistributable for Visual Studio 2015.
    /// 所有的字符串认为是UTF-8的
    /// 另外一个可行的库是SecurityDriven.NET
    /// </summary>
    public static class LibsodiumHelper
    {

        #region 私钥加密 对称加密

        /**
         * 使用了如下两个算法
         * Encryption: XSalsa20 stream cipher
         * Authentication: Poly1305 MAC
         * 
         */

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="messageToEncrypt"></param>
        public static byte[] Encrypt(string messageToEncrypt)
        {
            //使用一次的随机数，可以公开
            //每个nonce只能供一个用户使用一次，这样就可以防止攻击者使用重放攻击。可选的实现方式是把每一次请求的Nonce保存到数据库。
            var nonce = Sodium.SecretBox.GenerateNonce(); //24 byte nonce
            //key需要保留，并妥善放置
            var key = Sodium.SecretBox.GenerateKey(); //32 byte key

            return SecretBox.Create(messageToEncrypt, nonce, key);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="messageToDecrypt"></param>
        public static byte[] Decrypt(byte[] messageToDecrypt)
        {
            //解密必须使用和加密一样的key和nonce
            //使用一次的随机数，可以公开
            //每个nonce只能供一个用户使用一次，这样就可以防止攻击者使用重放攻击。可选的实现方式是把每一次请求的Nonce保存到数据库。
            var nonce = Sodium.SecretBox.GenerateNonce(); //24 byte nonce
            //key需要保留，并妥善放置
            var key = Sodium.SecretBox.GenerateKey(); //32 byte key
            return SecretBox.Open(messageToDecrypt,nonce,key);
        }

        #endregion

        #region 不对称加解密

        /**
         * 
         * Key exchange: Curve25519
         * Encryption: XSalsa20 stream cipher
         * Authentication: Poly1305 MAC
         * 
         */
        //public KeyPair(byte[] publicKey, byte[] privateKey)
        //public static string BinaryToHex(byte[] data)
        //public static byte[] HexToBinary(string hex)
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="messageToEncrypt"></param>
        public static byte[] Encrypt2(string messageToEncrypt)
        {
            //私钥保存
            KeyPair keyPair = PublicKeyBox.GenerateKeyPair();
            //用公钥加密
            return SealedPublicKeyBox.Create(messageToEncrypt, keyPair.PublicKey);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="messageToDecrypt"></param>
        public static byte[] Decrypt2(byte[] messageToDecrypt)
        {
            //私钥保存，必须使用和加密一样的
            KeyPair keyPair = PublicKeyBox.GenerateKeyPair();
            return SealedPublicKeyBox.Open(messageToDecrypt, keyPair);
        }



        #endregion

        #region Genneric Hash

        /// <summary>
        /// 生成32byte hash,没有可以
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static byte[] Hash(String message)
        {
            return GenericHash.Hash(message,(string)null,32);
        }

        /// <summary>
        /// 生成64byte hash
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static byte[] HashWithKey(string message)
        {
            var key = GenericHash.GenerateKey();
            return GenericHash.Hash(message, key, 64);
        }

        #region SHA hash 

        /// <summary>
        /// sha 256 bytehash
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static byte[] Sha256(string message)
        {
            return CryptoHash.Sha256(message);
        }

        /// <summary>
        /// sha 512 hash
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static byte[] Sha512(string message)
        {
            return CryptoHash.Sha512(message);
        }

        #endregion


        #endregion


        #region 辅助类

        /// <summary>
        /// 获取libsodium的版本号
        /// </summary>
        public static string SodiumVersionString
        {
            get { return Sodium.SodiumCore.SodiumVersionString(); }
        }

        /// <summary>
        /// 将byte转为hex
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string BytesToHex(byte[] bytes)
        {
            return Sodium.Utilities.BinaryToHex(bytes);
        }

        /// <summary>
        /// hex to bytes
        /// </summary>
        /// <param name="hexStr"></param>
        /// <returns></returns>
        public static byte[] HexToBytes(string hexStr)
        {
            return Sodium.Utilities.HexToBinary(hexStr);
        }

        #endregion
    }
}
