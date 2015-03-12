using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static void ResetAppDir()
        {
            Directory.SetCurrentDirectory(FileUtils.GetAppDir());
        }

        #endregion
    }
}
