using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVConsole.Utils
{
    public static class DirectoryHold
    {

        /// <summary>
        /// 重置工作目录到应用程序所在的路径
        /// </summary>
        /// <returns>返回是否是否设置成功</returns>
        public static bool ResetAppDir(Action<Exception> exceptionHandler = null)
        {
            bool isReset = false;

            try
            {
                string dirPath = AppDomain.CurrentDomain.BaseDirectory;
                if (!string.IsNullOrWhiteSpace(dirPath))
                {
                    Directory.SetCurrentDirectory(dirPath);
                    isReset = true;
                }
            }
            catch (Exception ex)
            {
                isReset = false;
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }
            }

            if (isReset)
            {
                return true;
            }

            try
            {
                string dirPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
                if (!string.IsNullOrWhiteSpace(dirPath))
                {
                    Directory.SetCurrentDirectory(dirPath);
                    isReset = true;
                }
            }
            catch (Exception ex)
            {
                isReset = false;
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }
            }

            if (isReset)
            {
                return true;
            }

            try
            {
                string dirPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                if (!string.IsNullOrWhiteSpace(dirPath))
                {
                    Directory.SetCurrentDirectory(dirPath);
                    isReset = true;
                }

            }
            catch (Exception ex)
            {
                isReset = false;
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }
            }

            return isReset;
        }

    }
}
