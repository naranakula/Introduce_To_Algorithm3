using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.Common.Utils.files;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;
using Microsoft.Win32;

namespace Introduce_To_Algorithm3.Common.Utils
{
    /// <summary>
    /// 开机启动工具
    /// 其原理是  在注册表中添加启动命令
    /// 注册表的路径是SOFTWARE\Microsoft\Windows\CurrentVersion\Run 
    /// 
    /// 命令可以有启动参数如："C:\Program Files\Microsoft Security Client\msseces.exe" -hide -runkey
    /// </summary>
    public static class StartupUtils
    {
        /// <summary>
        /// 设置应用程序自动开机运行
        /// </summary>
        /// <param name="keyName">要设置的注册表键</param>
        /// <param name="appToRun">开机运行的命令，可以带启动参数</param>
        /// <param name="isAutoStartup">true,设置自动启动，false,取消设置自动启动</param>
        /// <param name="setCurrentDir">是否重置当前工作路径到当前工作目录</param>
        public static void SetStartup(string keyName, string appToRun, bool isAutoStartup=true,bool setCurrentDir=true)
        {
            RegistryKey registry = null;

            try
            {
                string regName = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
                registry = Registry.LocalMachine.OpenSubKey(regName, true);//打开写权限
                if (registry == null)
                {
                    registry = Registry.LocalMachine.CreateSubKey(regName); //创建新项目或打开现有项，默认可写
                }

                if (registry == null)
                {
                    throw new Exception("获取注册表项失败");
                }

                if (isAutoStartup)
                {
                    registry.SetValue(keyName, appToRun);
                }
                else
                {
                    registry.SetValue(keyName, false);
                }

                if (setCurrentDir)
                {
                    DirectoryHold.ResetAppDir();
                }
            }
            catch(Exception ex)
            {
                Log4netHelper.Error("添加注册表失败："+ex);
            }
            finally
            {
                if (registry != null)
                {
                    registry.Close();
                }
            }
        }

        /// <summary>
        /// 设置当前应用程序自动开机运行
        /// </summary>
        /// <param name="keyName">要设置的注册表键</param>
        /// <param name="isAutoStartup">true,设置自动启动，false,取消设置自动启动</param>
        public static void SetStartup(string keyName,  bool isAutoStartup=true)
        {
            string appToRun = FileUtils.GetAppFullName();
            SetStartup(keyName,appToRun,isAutoStartup);
        }


        /// <summary>
        /// 设置当前应用程序自动开机运行
        /// </summary>
        /// <param name="isAutoStartup">true,设置自动启动，false,取消设置自动启动</param>
        public static void SetStartup( bool isAutoStartup = true)
        {
            string keyName = FileUtils.GetAppName(false);
            string appToRun = FileUtils.GetAppFullName();
            SetStartup(keyName, appToRun, isAutoStartup);
        }
    }
}
