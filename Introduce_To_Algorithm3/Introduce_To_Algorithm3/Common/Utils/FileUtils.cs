using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Introduce_To_Algorithm3.Common.Utils
{
    /// <summary>
    /// file utils
    /// </summary>
    public class FileUtils
    {
        /// <summary>
        /// is Encoding.Unicode
        /// Little endian UTF-16 FF FE
        /// we should use Unicode firstly
        /// </summary>
        /// <param name="sFilename"></param>
        /// <returns></returns>
        public static bool IsUnicode(string filename)
        {
            using (FileStream stream = File.OpenRead(filename))
            {
                byte[] data = new byte[2];
                int read = stream.Read(data, 0, 2);
                if (read == 2 && data[0] == 0xff && data[1] == 0xfe)
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Is bigEndianUnicode
        /// Big endian UTF-16 FE FF
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool IsBigEndianUnicode(string filename)
        {
            using (FileStream stream = File.OpenRead(filename))
            {
                byte[] data = new byte[2];
                int read = stream.Read(data, 0, 2);
                if (read == 2 && data[0] == 0xfe && data[1] == 0xff)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// is utf8
        /// •UTF-8：EF BB BF 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool IsUTF8(string filename)
        {
            using (FileStream stream = File.OpenRead(filename))
            {
                byte[] data = new byte[3];
                int read = stream.Read(data, 0, 3);
                if (read == 3 && data[0] == 0xef && data[1] == 0xbb && data[2] == 0xbf)
                {
                    return true;
                }
                return false;
            }
        }


        /// <summary>
        /// is little endian utf 32
        /// •UTF-32 Little-Endian：FF FE 00 00 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool IsUTF32(string filename)
        {
            using (FileStream stream = File.OpenRead(filename))
            {
                byte[] data = new byte[4];
                int read = stream.Read(data, 0, 4);
                if (read == 4 && data[0] == 0xff && data[1] == 0xFE && data[2] == 0x00 && data[3] == 0x00)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// is big endian utf 32
        /// •UTF-32 Big-Endian ：00 00 FE FF 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool IsBigEndianUtf32(string filename)
        {
            using (FileStream stream = File.OpenRead(filename))
            {
                byte[] data = new byte[4];
                int read = stream.Read(data, 0, 4);
                if (read == 4 && data[0] == 0x00 && data[1] == 0x00 && data[2] == 0xfe && data[3] == 0xff)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// This method attempts to automatically detect the encoding of a file based on the presence of byte order marks. Encoding formats UTF-8 and UTF-32 (both big-endian and little-endian) can be detected.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string[] ReadAllLines(string filename)
        {
            return File.ReadAllLines(filename);
        }

        /// <summary>
        /// read all lines using encoding
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string[] ReadAllLines(string filename, Encoding encoding)
        {
            return File.ReadAllLines(filename, encoding);
        }

        /// <summary>
        /// update source file to target. if target exist, overwrite it
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void UpdateFile(string source, string target)
        {
            if (IsFileUpdated(source, target))
            {
                return;
            }

            File.Copy(source, target, true);
            File.SetAttributes(target, File.GetAttributes(target) & ~FileAttributes.ReadOnly);
            File.SetLastWriteTimeUtc(target, File.GetLastWriteTimeUtc(source));
        }

        /// <summary>
        /// is file already updated
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsFileUpdated(string source, string target)
        {
            return File.Exists(source) && File.Exists(target) && FileSize(source) == FileSize(target) &&
                   File.GetLastWriteTimeUtc(target) >= File.GetLastWriteTimeUtc(source);
        }

        /// <summary>
        /// determine source and target file same in binary way
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="bufferSize"></param>
        /// <returns></returns>
        public static bool IsFileContentSame(string source, string target, int bufferSize = 4096)
        {
            if (!File.Exists(source) || !File.Exists(target))
            {
                return false;
            }
            byte[] sBuf = new byte[bufferSize];
            byte[] tBuf = new byte[bufferSize];
            using (FileStream sStream = File.OpenRead(source))
            using (FileStream tStream = File.OpenRead(target))
            {
                while (true)
                {
                    int sread = sStream.Read(sBuf, 0, bufferSize);
                    int tread = tStream.Read(tBuf, 0, bufferSize);
                    if (sread != tread)
                    {
                        return false;
                    }
                    if (sread == 0)
                    {
                        break;
                    }
                    for (int i = 0; i < sread; i++)
                    {
                        if (sBuf[i] != tBuf[i])
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// get the size of file
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static long FileSize(string filename)
        {
            return new FileInfo(filename).Length;
        }

        /// <summary>
        /// using Encoding.Unicode to write lines.
        /// If the file doesn't exists, create one. If exists, it will be overwrite
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="strings"></param>
        public static void WriteAllLines(string filename, IEnumerable<string> strings)
        {
            using (StreamWriter sw = new StreamWriter(filename, false, Encoding.Unicode))
            {
                foreach (var s in strings)
                {
                    sw.WriteLine(s);
                }
            }
        }

        /// <summary>
        /// using encoding to write lines.
        /// Encoding.Unicode for recommendation
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="strings"></param>
        /// <param name="encoding">Determines whether data is to be appended to the file. If the file exists and append is false, the file is overwritten. If the file exists and append is true, the data is appended to the file. Otherwise, a new file is created. </param>
        /// <param name="append"></param>
        public static void WriteAllLines(string filename, IEnumerable<string> strings, Encoding encoding, bool append = false)
        {
            using (StreamWriter sw = new StreamWriter(filename, append, encoding))
            {
                foreach (var s in strings)
                {
                    sw.WriteLine(s);
                }
            }
        }


        /// <summary>
        /// It automatically recognizes UTF-8, little-endian Unicode, and big-endian Unicode text if the file starts with the appropriate byte order marks. Otherwise, the UTF8Encoding is used.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static int LineCount(string filename)
        {
            using (StreamReader sr = new StreamReader(filename, true))
            {
                int count = 0;
                while (sr.ReadLine() != null)
                {
                    count++;
                }
                return count;
            }
        }

        /// <summary>
        /// It automatically recognizes UTF-8, little-endian Unicode, and big-endian Unicode text if the file starts with the appropriate byte order marks. Otherwise, the user-provided encoding is used.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static int LineCount(string filename, Encoding encoding)
        {
            using (StreamReader sr = new StreamReader(filename, encoding, true))
            {
                int count = 0;
                while (sr.ReadLine() != null)
                {
                    count++;
                }
                return count;
            }
        }



        /// <summary>
        /// compress a source file to target
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void Compress(string source, string target)
        {
            using (var sourceStream = File.OpenRead(source))
            {
                FileStream targetStream = new FileStream(target, FileMode.Create, FileAccess.Write);
                using (GZipStream gzStream = new GZipStream(targetStream, CompressionMode.Compress))
                {
                    sourceStream.CopyTo(gzStream);
                }
            }
        }

        /// <summary>
        /// decompress source file to target file
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void Decompress(string source, string target)
        {
            using (FileStream sourceStream = File.OpenRead(source))
            {
                using (GZipStream gzStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                {
                    using (FileStream targetStream = new FileStream(target, FileMode.Create, FileAccess.Write))
                    {
                        gzStream.CopyTo(targetStream);
                    }
                }
            }
        }


        /// <summary>
        /// serialize
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <param name="xmlFile"></param>
        public static void SerializeToXml(object obj, Type type, string xmlFile)
        {
            XmlSerializer serializer = new XmlSerializer(type);
            using (StreamWriter writer = new StreamWriter(xmlFile, false, Encoding.Unicode))
                serializer.Serialize(writer, obj);
        }

        /// <summary>
        /// serialize
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="xmlFile"></param>
        public static void SerializeToXml(object obj, string xmlFile)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            using (StreamWriter writer = new StreamWriter(xmlFile, false, Encoding.Unicode))
                serializer.Serialize(writer, obj);
        }

        /// <summary>
        /// deserialize
        /// </summary>
        /// <param name="xmlFile"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Object DeserializeToObject(string xmlFile, Type type)
        {
            XmlSerializer serializer = new XmlSerializer(type);
            using (StreamReader reader = new StreamReader(xmlFile, Encoding.Unicode))
                return serializer.Deserialize(reader);
        }
    }
}
