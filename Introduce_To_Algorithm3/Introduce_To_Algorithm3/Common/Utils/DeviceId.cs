using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils
{
    /// <summary>
    /// 获取设备唯一id
    /// </summary>
    public static class DeviceId
    {
        /// <summary>
        /// 缓存的cacheDeviceId
        /// </summary>
        private static volatile string cacheDeviceId = string.Empty;

        /// <summary>
        /// 获取设备唯一id  32位 小写英文字母
        /// </summary>
        /// <returns></returns>
        public static string UniqueDeviceId()
        {
            if (!String.IsNullOrWhiteSpace(cacheDeviceId))
            {
                return cacheDeviceId;
            }

            //bios是固化到主板的程序，有了主板id，可以忽略biosid
            string deviceId = CpuId() + "_" + DiskId() + "_" + MotherboardId();
            Console.WriteLine(deviceId);
            cacheDeviceId = GetMD5HashFromString(deviceId);
            return cacheDeviceId;
        }

        /// <summary>
        /// MD5hash 转换为32位小写hash  由[0-f]组成
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMD5HashFromString(string str)

        {
            StringBuilder sb = new StringBuilder();
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] bytValue, bytHash;

                bytValue = Encoding.UTF8.GetBytes(str);

                bytHash = md5.ComputeHash(bytValue);
                
                for (int i = 0; i < bytHash.Length; i++)
                {
                    //转换为两位16进制
                    sb.Append(bytHash[i].ToString("x2"));
                }
            }
            string result = sb.ToString().ToLower().PadLeft(32,'_');
            return result.Substring(0, result.Length > 32 ? 32 : result.Length);
        }

        /// <summary>
        /// 根据wmiProperty从wmiClass获取信息
        /// </summary>
        /// <param name="wmiClass"></param>
        /// <param name="wmiProperty"></param>
        /// <returns></returns>
        private static string Identifier(string wmiClass, string wmiProperty)
        {
            string result = string.Empty;
            using (ManagementClass mc = new ManagementClass(wmiClass))
            {
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (var mo in moc)
                {
                    //Only get the first one
                    if (String.IsNullOrWhiteSpace(result))
                    {
                        try
                        {
                            result = mo[wmiProperty].ToString().Trim().ToLower();
                            break;
                        }
                        catch
                        {
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取Cpuid
        /// </summary>
        /// <returns></returns>
        private static string CpuId()
        {
            //Uses first CPU identifier available in order of preference
            //Don't get all identifiers, as it is very time consuming
            string retVal = Identifier("Win32_Processor", "UniqueId");
            if (String.IsNullOrWhiteSpace(retVal)) //If no UniqueID, use ProcessorID
            {
                retVal = Identifier("Win32_Processor", "ProcessorId");
                //if (string.IsNullOrWhiteSpace(retVal)) //If no ProcessorId, use Name
                //{
                //    retVal = Identifier("Win32_Processor", "Name");
                //    if (retVal == "") //If no Name, use Manufacturer
                //    {
                //        retVal = Identifier("Win32_Processor", "Manufacturer");
                //    }
                //    //Add clock speed for extra security
                //    retVal += Identifier("Win32_Processor", "MaxClockSpeed");
                //}
            }
            return retVal;
        }

        /// <summary>
        /// 获取biosid
        /// </summary>
        /// <returns></returns>
        private static string BiosId()
        {
            return Identifier("Win32_BIOS", "SerialNumber");
            //return Identifier("Win32_BIOS", "Manufacturer")
            //+ Identifier("Win32_BIOS", "SMBIOSBIOSVersion")
            //+ Identifier("Win32_BIOS", "IdentificationCode")
            //+ Identifier("Win32_BIOS", "SerialNumber")
            //+ Identifier("Win32_BIOS", "ReleaseDate")
            //+ Identifier("Win32_BIOS", "Version");
        }

        /// <summary>
        /// 硬盘
        /// </summary>
        /// <returns></returns>
        private static string DiskId()
        {
            //return Identifier("Win32_DiskDrive", "Model")
            //+ Identifier("Win32_DiskDrive", "Manufacturer")
            //+ Identifier("Win32_DiskDrive", "Signature")
            //+ Identifier("Win32_DiskDrive", "TotalHeads");

            return Identifier("Win32_DiskDrive", "Signature");
        }
        

        /// <summary>
        /// Motherboard ID
        /// </summary>
        /// <returns></returns>
        private static string MotherboardId()
        {
            //return identifier("Win32_BaseBoard", "Model")
            //+ identifier("Win32_BaseBoard", "Manufacturer")
            //+ identifier("Win32_BaseBoard", "Name")
            //+ identifier("Win32_BaseBoard", "SerialNumber");

            return Identifier("Win32_BaseBoard", "SerialNumber");
        }
    }
}
