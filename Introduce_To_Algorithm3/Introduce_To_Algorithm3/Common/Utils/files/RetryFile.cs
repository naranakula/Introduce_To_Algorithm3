using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Utils
{
    public class RetryFile
    {
        #region Constructor
        private Retry _retry;
        private Action<RetryException> _notify;
        /// <summary>
        /// construct a default Retry File
        /// </summary>
        public RetryFile()
        {
            _retry = new Retry();
            _notify = null;
        }

        /// <summary>
        /// construct a RetryFile by using tryCount and tryInterval
        /// </summary>
        /// <param name="tryCount"></param>
        /// <param name="tryInterval"></param>
        public RetryFile(int tryCount,TimeSpan tryInterval,Action<RetryException> notify)
        {
            _retry = new Retry(tryCount, tryInterval);
            _notify = notify;
        }
        #endregion

        #region a default instance
        /// <summary>
        /// a default instance of RetryFile with 3 trycount and 1 minitue tryInterval
        /// </summary>
        private static RetryFile _default = new RetryFile();

        /// <summary>
        /// get a default instance of RetryFile with 3 trycount and 1 minitue tryInterval
        /// </summary>
        public static RetryFile Default { get { return _default; } }
        #endregion

        /// <summary>
        /// get last write time utc
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public DateTime GetLastWriteTimeUtc(string file)
        {
            return _retry.Invoke<string,DateTime>((f) => File.GetLastWriteTimeUtc(f), file,_notify);
        }

        /// <summary>
        /// get last write time
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public DateTime GetLastWriteTime(string file)
        {
            return _retry.Invoke<string, DateTime>(f => File.GetLastWriteTime(f), file,_notify);
        }

        /// <summary>
        /// delete file
        /// </summary>
        /// <param name="file"></param>
        public void Delete(string file)
        {
            _retry.Invoke(() => File.Delete(file), _notify);
        }

        /// <summary>
        /// copy a file from source to target and cann't be overwrited
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public void Copy(string source, string target)
        {
            _retry.Invoke(() => File.Copy(source, target), _notify);
        }

        /// <summary>
        /// copy a file from source to target
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="overwrite"></param>
        public void Copy(string source, string target, bool overwrite)
        {
            _retry.Invoke(() => File.Copy(source, target, overwrite), _notify);
        }

        /// <summary>
        /// whether a file exists
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool Exists(string path)
        {
            return _retry.Invoke<bool>(() => File.Exists(path), _notify);
        }

        /// <summary>
        /// get the creation time of file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public DateTime GetCreationTime(string file)
        {
            return _retry.Invoke<DateTime>(() => File.GetCreationTime(file), _notify);
        }

        /// <summary>
        /// get the creation time utc of file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public DateTime GetCreationTimeUtc(string file)
        {
            return _retry.Invoke<DateTime>(() => File.GetCreationTimeUtc(file), _notify);
        }

        /// <summary>
        /// get the last access time of sepecific
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public DateTime GetLastAccessTime(string file)
        {
            return _retry.Invoke<DateTime>(() => File.GetLastAccessTime(file), _notify);
        }

        /// <summary>
        /// get the last access time utc of sepecific
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public DateTime GetLastAccessTimeUtc(string file)
        {
            return _retry.Invoke<DateTime>(() => File.GetLastAccessTimeUtc(file), _notify);
        }

        /// <summary>
        /// get file size
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public long GetFileSize(string file)
        {
            return _retry.Invoke<long>(() => { return new FileInfo(file).Length; }, _notify);
        }

        /// <summary>
        /// move a file from source to target
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public void Move(string source, string target)
        {
            _retry.Invoke(()=>File.Move(source,target),_notify);
        }

        /// <summary>
        /// read lines from a file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public IEnumerable<string> ReadLines(string file)
        {
            return _retry.Invoke<IEnumerable<string>>(()=>File.ReadLines(file),_notify);
        }

        /// <summary>
        /// read lines from a file using specific encoding
        /// </summary>
        /// <param name="file"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public IEnumerable<string> ReadLines(string file,Encoding encoding)
        {
            return _retry.Invoke<IEnumerable<string>>(() => File.ReadLines(file,encoding), _notify);
        }

        /// <summary>
        /// get FileAttributes from specific file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public FileAttributes GetAttributes(string file)
        {
            return _retry.Invoke<FileAttributes>(()=>File.GetAttributes(file),_notify);
        }

        /// <summary>
        /// read all text from a file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string ReadAllText(string file)
        {
            return _retry.Invoke<string>(() => File.ReadAllText(file), _notify);
        }

        /// <summary>
        /// read all text from a file using a specific encoding
        /// </summary>
        /// <param name="file"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public string ReadAllText(string file, Encoding encoding)
        {
            return _retry.Invoke<string>(() => File.ReadAllText(file, encoding), _notify);
        }
    }
}
