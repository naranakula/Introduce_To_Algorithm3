using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.files
{
    public static class AssemblyHelper
    {
        /// <summary>
        /// 程序集的版本
        /// </summary>
        public static void GetAssemblyVersion()
        {
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            int major = version.Major;//1主版本号
            int minor = version.Minor;//2次版本号
            int build = version.Build;//3内部版本号
            int revision = version.Revision;//4修订号
        }

        /// <summary>
        /// 在资源管理器里查看属性时看到的版本
        /// </summary>
        public static void GetAssemblyFileVersion()
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            FileVersionInfo fileVersion = FileVersionInfo.GetVersionInfo(assembly.Location);
            //major number.minor number.build number.private part number
            int majorPart = fileVersion.FileMajorPart;//1主版本号
            int minorPart = fileVersion.FileMinorPart;//2次版本号
            int buildPart = fileVersion.FileBuildPart;//3内部版本号
            int privatePart = fileVersion.FilePrivatePart;//4修订号
        }
    }
}
