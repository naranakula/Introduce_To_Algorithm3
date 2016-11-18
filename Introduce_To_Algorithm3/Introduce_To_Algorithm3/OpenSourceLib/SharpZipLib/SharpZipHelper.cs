using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace Introduce_To_Algorithm3.OpenSourceLib.SharpZipLib
{
    /// <summary>
    /// 解压
    /// </summary>
    public class SharpZipHelper
    {
        /// <summary>
        /// 解压zip包  成功解压返回true,否则返回false
        /// </summary>
        /// <param name="archiveZipFile">压缩的zip文件</param>
        /// <param name="outFolder">解压的目录,如果目录不存在，创建目录</param>
        /// <param name="exceptionHandler">异常处理</param>
        /// <param name="password">zip文件解压密码</param>
        public static bool ExtractZipFile(string archiveZipFile, string outFolder, Action<Exception> exceptionHandler = null, string password = null)
        {
            ZipFile zipFile = null;
            try
            {
                zipFile = new ZipFile(File.OpenRead(archiveZipFile));

                if (!string.IsNullOrWhiteSpace(password))
                {
                    zipFile.Password = password;
                }

                foreach (ZipEntry zipEntry in zipFile)
                {
                    if (!zipEntry.IsFile)
                    {
                        continue;
                    }

                    //可能包含目录
                    string entryFileName = zipEntry.Name;
                    byte[] buffer = new byte[4096];     // 4K is optimum
                    Stream zipStream = zipFile.GetInputStream(zipEntry);

                    //创建输出目录
                    string fullZipToPath = Path.Combine(outFolder, entryFileName);
                    string fullZipToDir = Path.GetDirectoryName(fullZipToPath);
                    if (!string.IsNullOrWhiteSpace(fullZipToDir))
                    {
                        if (!Directory.Exists(fullZipToDir))
                        {
                            //创建目录及子目录
                            Directory.CreateDirectory(fullZipToDir);
                        }
                    }


                    //如果文件存在，覆盖  在指定路径中创建或覆盖文件
                    using (FileStream stream = File.Create(fullZipToPath))
                    {
                        StreamUtils.Copy(zipStream, stream, buffer);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {

                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }

                return false;
            }
            finally
            {
                if (zipFile != null)
                {
                    zipFile.IsStreamOwner = true; // Makes close also shut the underlying stream
                    zipFile.Close(); // Ensure we release resources
                    zipFile = null;
                }
            }
        }
    
    }
}
