using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Utils
{
    public class Logger
    {
        /// <summary>
        /// locker
        /// </summary>
        private static object locker = new object();
        /// <summary>
        /// the instance we use
        /// </summary>
        private static Logger _instance = null;

        /// <summary>
        /// get instance
        /// </summary>
        /// <param name="logFile"></param>
        /// <param name="writeTimestamp"></param>
        /// <param name="writeToConsole"></param>
        /// <returns></returns>
        public static Logger GetInstance(string logFile, bool writeTimestamp = false, bool writeToConsole = false)
        {
            //yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffff
            if (_instance != null)
                return _instance;
            lock (locker)
            {
                if (_instance == null)
                {
                    _instance = new Logger(logFile, writeTimestamp, writeToConsole);
                }
            }
            return _instance;
        }


        public void Write(string s, params object[] args)
        {
            Write(string.Format(s, args));
        }

        public void Write(string s)
        {
            if (WriteTimestamp)
            {
                s = DateTime.Now.ToString(@"yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'fff") + "\t|" + s;
            }

            lock (locker)
            {
                if (WriteToConsole)
                    Console.Error.Write(s);
                writer.Write(s);
            }
        }


        public void WriteLine(string s, params object[] args)
        {
            WriteLine(string.Format(s, args));
        }

        public void WriteLine(string s)
        {
            if (WriteTimestamp)
            {
                s = DateTime.Now.ToString(@"yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'fff") + "\t|" + s;
            }

            lock (locker)
            {
                if (WriteToConsole)
                    Console.Error.WriteLine(s);
                writer.WriteLine(s);
            }
        }

        public void WriteLine()
        {
            lock (locker)
            {
                if (WriteToConsole)
                    Console.Error.WriteLine();
                writer.WriteLine();
            }
        }


        private Logger(string logFile, bool writeTimestamp, bool writeToConsole)
        {
            LogFileName = logFile;
            Open();
            WriteTimestamp = writeTimestamp;
            WriteToConsole = writeToConsole;
        }

        public void Flush()
        {
            lock (locker)
            {
                if (WriteToConsole)
                    Console.Error.Flush();
                if (writer != null)
                    writer.Flush();
            }
        }

        private void Open()
        {
            writer = new StreamWriter(LogFileName, true, Encoding.Unicode);
            writer.AutoFlush = true;
        }

        public void Refresh()
        {
            lock (locker)
            {
                if (writer != null)
                    writer.Close();

                writer = new StreamWriter(LogFileName, true, Encoding.Unicode);
                writer.AutoFlush = true;
            }
        }

        /// <summary>
        /// close log file
        /// </summary>
        public void Close()
        {
            lock (locker)
            {
                if (writer != null)
                {
                    writer.Close();
                    writer = null;
                }
            }
        }


        public bool WriteTimestamp { get; set; }
        public bool WriteToConsole { get; set; }
        public string LogFileName { get; private set; }
        private StreamWriter writer;

    }
}
