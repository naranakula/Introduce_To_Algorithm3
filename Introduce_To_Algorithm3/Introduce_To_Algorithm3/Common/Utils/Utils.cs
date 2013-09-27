using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Utils
{
    public static class Utils
    {
        #region IO

        /// <summary>
        /// update srcFile to target directory
        /// </summary>
        /// <param name="srcFile"></param>
        /// <param name="tgtFile"></param>
        public static void UpdateToDir(string srcFile, string tgtDir)
        {
            UpdateToFile(srcFile,Path.Combine(tgtDir,Path.GetFileName(srcFile)));
        }

        /// <summary>
        /// update files match the specified search pattern from source dir to tgt dir
        /// </summary>
        /// <param name="srcDir"></param>
        /// <param name="searchPattern"></param>
        /// <param name="tgtDir"></param>
        public static void UpdatePatternToDir(string srcDir, string searchPattern, string tgtDir)
        {
            if (string.IsNullOrWhiteSpace(tgtDir))
            {
                return;
            }

            foreach (string srcFile in RetryDirectory.Default.GetFiles(srcDir,searchPattern))
            {
                UpdateToDir(srcFile, tgtDir);
            }
        }

        /// <summary>
        /// update tgtfile from srcfile
        /// </summary>
        /// <param name="srcFile"></param>
        /// <param name="tgtFile"></param>
        public static void UpdateToFile(string srcFile, string tgtFile)
        {
            if (IsFileUpdated(srcFile, tgtFile))
            {
                return;
            }

            RetryFile.Default.Copy(srcFile, tgtFile, true);
            File.SetAttributes(tgtFile, File.GetAttributes(tgtFile) & ~FileAttributes.ReadOnly);
            File.SetLastAccessTimeUtc(tgtFile, File.GetLastWriteTimeUtc(srcFile));
        }

        /// <summary>
        /// is srcfile updated to tgtfile
        /// </summary>
        /// <param name="srcFile"></param>
        /// <param name="tgtFile"></param>
        /// <returns></returns>
        public static bool IsFileUpdated(string srcFile, string tgtFile)
        {
            return RetryFile.Default.Exists(tgtFile) && RetryFile.Default.Exists(srcFile) && FileSize(srcFile) == FileSize(tgtFile) && RetryFile.Default.GetLastWriteTimeUtc(tgtFile) >= RetryFile.Default.GetLastWriteTimeUtc(srcFile);
        }

        /// <summary>
        /// get the size of file in byte
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static long FileSize(string file)
        {
            return new FileInfo(file).Length;
        }

        /// <summary>
        /// mirror srcDir and tgtdir
        /// </summary>
        /// <param name="srcDir"></param>
        /// <param name="searchPattern"></param>
        /// <param name="tgtDir"></param>
        public static void MirrorDir(string srcDir, string searchPattern, string tgtDir)
        {
            foreach (string file in RetryDirectory.Default.GetFiles(srcDir,searchPattern))
            {
                UpdateToDir(file, tgtDir);
            }

            foreach (string file in RetryDirectory.Default.GetFiles(tgtDir,searchPattern))
            {
                if(!RetryFile.Default.Exists(Path.Combine(srcDir,Path.GetFileName(file))))
                {
                    RetryFile.Default.Delete(file);
                }
            }
        }

        /// <summary>
        /// mirror srcDir and tgtDir
        /// </summary>
        /// <param name="srcDir"></param>
        /// <param name="tgtDir"></param>
        public static void MirrorDir(string srcDir, string tgtDir)
        {
            MirrorDir(srcDir, "*", tgtDir);
        }

        /// <summary>
        /// recursive mirror srcDir and tgtDir
        /// </summary>
        /// <param name="srcDir"></param>
        /// <param name="tgtDir"></param>
        public static void RecursiveMirrorDir(string srcDir, string tgtDir)
        {
            if (string.IsNullOrWhiteSpace(tgtDir))
            {
                return;
            }

            if (!RetryDirectory.Default.Exists(tgtDir))
            {
                RetryDirectory.Default.CreateDirectory(tgtDir);
            }

            foreach (string subSourceDir in RetryDirectory.Default.GetDirectories(srcDir))
            {
                RecursiveMirrorDir(subSourceDir, Path.Combine(tgtDir, Path.GetFileName(subSourceDir)));
            }

            MirrorDir(srcDir, tgtDir);
        }

        /// <summary>
        /// recursive update to directory
        /// </summary>
        /// <param name="srcDir"></param>
        /// <param name="searchPattern"></param>
        /// <param name="tgtDir"></param>
        public static void RecursiveUpdateToDir(string srcDir, string searchPattern, string tgtDir)
        {
            if (string.IsNullOrWhiteSpace(tgtDir))
            {
                return;
            }

            if (!RetryDirectory.Default.Exists(tgtDir))
            {
                RetryDirectory.Default.CreateDirectory(tgtDir);
            }

            foreach (string subSourceDir in RetryDirectory.Default.GetDirectories(srcDir))
            {
                RecursiveUpdateToDir(subSourceDir, searchPattern, Path.Combine(tgtDir, Path.GetFileName(subSourceDir)));
            }

            foreach (string srcFile in RetryDirectory.Default.GetFiles(srcDir,searchPattern))
            {
                UpdateToDir(srcFile, tgtDir);
            }
        }

        /// <summary>
        /// recursive update to directory
        /// </summary>
        /// <param name="srcDir"></param>
        /// <param name="tgtDir"></param>
        public static void RecursiveUpdateToDir(string srcDir, string tgtDir)
        {
            RecursiveUpdateToDir(srcDir, "*", tgtDir);
        }

        #endregion
    }

}
