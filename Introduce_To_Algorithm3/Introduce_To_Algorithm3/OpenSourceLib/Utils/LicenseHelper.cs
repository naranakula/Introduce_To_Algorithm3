using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portable.Licensing;
using Portable.Licensing.Security.Cryptography;
using Portable.Licensing.Validation;

namespace Introduce_To_Algorithm3.OpenSourceLib.Utils
{
    /// <summary>
    /// Portable.Licensing 
    /// </summary>
    public static class LicenseHelper
    {

        /// <summary>
        /// 用于加解密privatekey的
        /// 用来加密私钥的，产生的私钥是加密后的
        /// </summary>
        private const string PassPhrase = @"dae944211b1f4371bfd35550d7e4a9bf";

        /// <summary>
        /// 私有/共有 密钥对
        /// 
        /// 每次调用产生不同的对，
        /// 
        /// 生成一对公钥/私钥，公钥可以暴露在程序中，私钥自己保留，利用私钥签名，公钥解密
        /// Item1是private key, Item2 是public key
        /// </summary>
        /// <returns>Item1是private key, Item2 是public key</returns>
        public static Tuple<string, string> GenerateKeyPair()
        {
            KeyGenerator keyGenerator = Portable.Licensing.Security.Cryptography.KeyGenerator.Create();
            //使用ESDSA生成Public/Private  key pair
            KeyPair keyPair = keyGenerator.GenerateKeyPair();

            string privateKey = keyPair.ToEncryptedPrivateKeyString(PassPhrase);
            string publicKey = keyPair.ToPublicKeyString();

            return new Tuple<string, string>(privateKey,publicKey);
        }


        /// <summary>
        /// 创建一个license
        /// </summary>
        /// <returns></returns>
        public static string GenerateLicense(string privateKey)
        {
            var license = License.New().WithUniqueIdentifier(Guid.NewGuid())
                //.As(LicenseType.Standard)
                ////可以指定超时时间  不指定不过期
                //.ExpiresAt(DateTime.Now.AddDays(30))
                .As(LicenseType.Trial)
                //可以指定超时时间  不指定不过期
                .ExpiresAt(DateTime.Now.AddDays(30))
                .WithMaximumUtilization(1)//经测试这个并没有什么卵用
               //保存一些额外信息
                .WithProductFeatures(new Dictionary<string, string>
                {
                    {"授权给","青岛机场"},//通过一些额外信息限制使用方
                    {"Sales Module", "yes"},
                    {"Purchase Module", "yes"},
                    {"Maximum Transactions", "10000"}
                })
                //.LicensedTo("John Doe", "john.doe@yourmail.here")
                .CreateAndSignWithPrivateKey(privateKey, PassPhrase);

            string result = license.ToString();
            using (StreamWriter writer = new StreamWriter("License.lic", false, Encoding.UTF8))
            {
                writer.WriteLine(result);
            }
            return result;
        }

        /// <summary>
        /// 验证证书
        /// </summary>
        /// <param name="licenseFile"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public static bool ValidateLicense(string licenseFile,string publicKey)
        {
            string xmlStr = File.ReadAllText(licenseFile,Encoding.UTF8).Trim();
            License license = License.Load(xmlStr);
            //license.Expiration;
            //license.Type;
            //当你修改了任何一个信息后，签名就不对了，认证失败
            //当试用时验证是否过期，
            var validationFailures  = license.Validate().ExpirationDate()
                .When(lic => lic.Type == LicenseType.Trial).AssertValidLicense().ToList();

            //验证签字  签名就不对了，认证失败
            validationFailures = license.Validate().Signature(publicKey).AssertValidLicense().ToList();
            //通过制定与硬件相关的信息，限制用户只能在一台机器上使用
            //license.Validate().AssertThat(lic => lic.ProductFeatures.Get("HardwareId") == "133456", new GeneralValidationFailure() { Message = "Invalid Hardware.", HowToResolve = "Contact administrator" });
            validationFailures = license.Validate().ExpirationDate()
                .When(lic => lic.Type == LicenseType.Trial)
                .And().Signature(publicKey).AssertValidLicense().ToList();
            if (validationFailures.Any())
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
