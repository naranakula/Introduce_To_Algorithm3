using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Utils
{
    public class RetryDirectory
    {
        #region Constructor

        private Retry _retry;
        private Action<NotifyEventArgs> _notify;

        /// <summary>
        /// constructor a default retry directory
        /// </summary>
        public RetryDirectory()
        {
            _retry = new Retry();
            _notify = null;
        }

        /// <summary>
        /// construct a retry directory using tryCount and tryInterval
        /// </summary>
        /// <param name="tryCount"></param>
        /// <param name="tryInterval"></param>
        /// <param name="notify"></param>
        public RetryDirectory(int tryCount, TimeSpan tryInterval, Action<NotifyEventArgs> notify)
        {
            _retry = new Retry(tryCount, tryInterval);
            _notify = notify;
        }

        #endregion

        #region default instance

        /// <summary>
        /// a default instance of retryDirectory with 3 max tryCount and 1 minute tryInterval
        /// </summary>
        private static RetryDirectory _default = new RetryDirectory();

        /// <summary>
        /// a default instance of retryDirectory with 3 max tryCount and 1 minute tryInterval
        /// </summary>
        public static RetryDirectory Default { get { return _default; } }

        #endregion

        /// <summary>
        /// create directory
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public DirectoryInfo CreateDirectory(string path)
        {
            return _retry.Invoke(() => Directory.CreateDirectory(path), _notify);
        }

        /// <summary>
        /// delete an emtpy directory
        /// </summary>
        /// <param name="dir"></param>
        public void Delete(string dir)
        {
            _retry.Invoke(() => Directory.Delete(dir), _notify);
        }

        /// <summary>
        /// delete an directory
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="recursive"></param>
        public void Delete(string dir, bool recursive)
        {
            _retry.Invoke(() => Directory.Delete(dir, recursive), _notify);
        }

        /// <summary>
        /// determine whether a directory exist
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public bool Exists(string dir)
        {
            return _retry.Invoke(() => Directory.Exists(dir),_notify);
        }

        /// <summary>
        /// get the create time of directory
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public DateTime GetCreationTime(string dir)
        {
            return _retry.Invoke(() => Directory.GetCreationTime(dir), _notify);
        }

        /// <summary>
        /// get the creation time utc
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public DateTime GetCreationTimeUtc(string dir)
        {
            return _retry.Invoke(() => Directory.GetCreationTimeUtc(dir), _notify);
        }

        /// <summary>
        /// /get the current direcoty of application
        /// </summary>
        /// <returns></returns>
        public string GetCurrentDirectory()
        {
            return _retry.Invoke(() => Directory.GetCurrentDirectory(), _notify);
        }

        /// <summary>
        /// get the paths of subdirectories of the application
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public string[] GetDirectories(string dir)
        {
            return _retry.Invoke(() => Directory.GetDirectories(dir), _notify);
        }

        /// <summary>
        /// get the paths of subdirectories that match the specified search pattern
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public string[] GetDirectories(string dir, string searchPattern)
        {
            return _retry.Invoke(() => Directory.GetDirectories(dir,searchPattern), _notify);
        }

        /// <summary>
        /// get the paths of subdirectories that match the specified search pattern and search option
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public string[] GetDirectories(string dir, string searchPattern,SearchOption searchOption)
        {
            return _retry.Invoke(() => Directory.GetDirectories(dir, searchPattern,searchOption), _notify);
        }

        /// <summary>
        /// get the paths of files in the specified directory
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public string[] GetFiles(string dir)
        {
            return _retry.Invoke(() => Directory.GetFiles(dir), _notify);
        }

        /// <summary>
        /// Returns the names of files (including their paths) that match the specified search pattern in the specified directory.
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public string[] GetFiles(string dir,string searchPattern)
        {
            return _retry.Invoke(() => Directory.GetFiles(dir,searchPattern), _notify);
        }

        /// <summary>
        /// Returns the names of files (including their paths) that match the specified search pattern in the specified directory, using a value to determine whether to search subdirectories.
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="searchPattern"></param>
        /// <param name="searchOption"></param>
        /// <returns></returns>
        public string[] GetFiles(string dir, string searchPattern,SearchOption searchOption)
        {
            return _retry.Invoke(() => Directory.GetFiles(dir, searchPattern,searchOption), _notify);
        }

        /// <summary>
        /// get the last access time of directory
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public DateTime GetLastAccessTime(string dir)
        {
            return _retry.Invoke(() => Directory.GetLastAccessTime(dir), _notify);
        }

        /// <summary>
        /// get the last access time utc
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public DateTime GetLastAccessTimeUtc(string dir)
        {
            return _retry.Invoke(() => Directory.GetLastAccessTimeUtc(dir), _notify);
        }

        /// <summary>
        /// get the last write time of directory
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public DateTime GetLastWriteTime(string dir)
        {
            return _retry.Invoke(() => Directory.GetLastWriteTime(dir), _notify);
        }

        /// <summary>
        /// get the last write time utc
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public DateTime GetLastWriteTimeUtc(string dir)
        {
            return _retry.Invoke(() => Directory.GetLastWriteTimeUtc(dir), _notify);
        }

        /// <summary>
        /// Retrieves the parent directory of the specified path, including both absolute and relative paths.
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public DirectoryInfo GetParent(string dir)
        {
            return _retry.Invoke(() => Directory.GetParent(dir), _notify);
        }

        /// <summary>
        /// Moves a file or a directory and its contents to a new location.
        /// </summary>
        /// <param name="srcDir"></param>
        /// <param name="tgtDir"></param>
        public void Move(string srcDir, string tgtDir)
        {
            _retry.Invoke(()=>Directory.Move(srcDir,tgtDir),_notify);
        }

        /// <summary>
        /// Sets the application's current working directory to the specified directory.
        /// </summary>
        /// <param name="dir"></param>
        public void SetCurrentDirectory(string dir)
        {
            _retry.Invoke(() => Directory.SetCurrentDirectory(dir), _notify);
        }

    }
}
