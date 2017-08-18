using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;

namespace Introduce_To_Algorithm3.Common.Utils.files
{
    public class DirectoryHold : IDisposable
    {
        public DirectoryHold(string s)
        {
            m_savedDir = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(s);
        }

        void IDisposable.Dispose()
        {
            if (!string.IsNullOrWhiteSpace(m_savedDir))
            {
                Directory.SetCurrentDirectory(m_savedDir);
                m_savedDir = null;
            }
        }

        private string m_savedDir;


        #region 重置工作目录到当前app所在路径

        /// <summary>
        /// 重置工作目录到应用程序所在的路径
        /// </summary>
        /// <returns>返回是否是否设置成功</returns>
        public static bool ResetCurrentDir(Action<Exception> exceptionHandler = null)
        {
            bool isReset = false;

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
                else
                {
                    NLogHelper.Error($"重置当前目录失败：{ex}");
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
                else
                {
                    NLogHelper.Error($"重置当前目录失败：{ex}");
                }
            }

            if (isReset)
            {
                return true;
            }

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
                else
                {
                    NLogHelper.Error($"重置当前目录失败：{ex}");
                }
            }
            
            return isReset;
        }

        /// <summary>
        /// 从文件名中查找文件，并返回文件的完全路径，如果文件不存在，返回空
        /// 先查找当前目录，在查找程序集所在目录
        /// </summary>
        /// <param name="shortFileName"></param>
        /// <returns></returns>
        public static string ResolveFile(string shortFileName)
        {
            if (string.IsNullOrWhiteSpace(shortFileName))
            {
                return string.Empty;
            }

            string fullFileName = string.Empty;
            try
            {
                FileInfo fileInfo = new FileInfo(shortFileName);
                if (fileInfo.Exists)
                {
                    fullFileName = fileInfo.FullName;
                }
            }
            catch
            {
                // ignored
            }

            if (!string.IsNullOrWhiteSpace(fullFileName))
            {
                return fullFileName;
            }


            try
            {
                string parent = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                FileInfo fileInfo = new FileInfo(Path.Combine(parent, shortFileName));
                if (fileInfo.Exists)
                {
                    fullFileName = fileInfo.FullName;
                }
            }
            catch
            {
                // ignored
            }

            if (!string.IsNullOrWhiteSpace(fullFileName))
            {
                return fullFileName;
            }

            try
            {
                string parent = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
                FileInfo fileInfo = new FileInfo(Path.Combine(parent, shortFileName));
                if (fileInfo.Exists)
                {
                    fullFileName = fileInfo.FullName;
                }
            }
            catch
            {
                // ignored
            }

            if (!string.IsNullOrWhiteSpace(fullFileName))
            {
                return fullFileName;
            }

            try
            {
                FileInfo fileInfo = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, shortFileName));
                if (fileInfo.Exists)
                {
                    fullFileName = fileInfo.FullName;
                }
            }
            catch
            {
                // ignored
            }

            if (!string.IsNullOrWhiteSpace(fullFileName))
            {
                return fullFileName;
            }

           
            return String.Empty;
        }

        /// <summary>
        /// 获取当前路径
        /// </summary>
        public static string CurrentDirectory
        {
            get
            {
                try
                {
                    return Directory.GetCurrentDirectory();//该实现兼容 net core
                    //return Environment.CurrentDirectory;
                }
                catch (Exception)
                {
                    //ignored
                    return string.Empty;
                }
            }
        }

        #endregion
    }
}
