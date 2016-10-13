using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetCommsConsole.NetComms.Tools
{
    /// <summary>
    ///  A wrapper around a stream to ensure it can be accessed in a thread safe way. 
    /// </summary>
    public class ThreadSafeStream : Stream, IDisposable
    {
        #region 私有变量

        /// <summary>
        /// 内部的stream
        /// </summary>
        private Stream _innerStream;

        /// <summary>
        /// 锁
        /// </summary>
        private object streamLocker = new object();

        #endregion

        /// <summary>
        /// If true the internal stream will be disposed once the data has been written to the network
        /// </summary>
        public bool DiposeInnerStreamOnDispose { get; set; }


        #region 构造函数

        /// <summary>
        /// Create a thread safe stream. Once any actions are complete the stream must be correctly disposed by the user.
        /// </summary>
        /// <param name="stream">The stream to make thread safe</param>
        public ThreadSafeStream(Stream stream)
        {
            this.DiposeInnerStreamOnDispose = false;
            this._innerStream = stream;
        }

        /// <summary>
        /// Create a thread safe stream.
        /// </summary>
        /// <param name="stream">The stream to make thread safe.</param>
        /// <param name="closeStreamAfterSend">If true the provided stream will be disposed once data has been written to the network. If false the stream must be disposed of correctly by the user</param>
        public ThreadSafeStream(Stream stream, bool closeStreamAfterSend)
        {
            this.DiposeInnerStreamOnDispose = closeStreamAfterSend;
            this._innerStream = stream;
        }

        #endregion


        #region Public方法

        /// <summary>
        /// return data from entire stream
        /// </summary>
        /// <param name="numberZeroBytesPrefex">if non zero will apeend 0 value bytes to the start of the returned array</param>
        /// <returns></returns>
        public byte[] ToArray(int numberZeroBytesPrefex = 0)
        {
            lock (streamLocker)
            {
                _innerStream.Seek(0, SeekOrigin.Begin);
                byte[] returnData = new byte[_innerStream.Length+numberZeroBytesPrefex];
                _innerStream.Read(returnData, numberZeroBytesPrefex, returnData.Length - numberZeroBytesPrefex);
                return returnData;
            }
        }


        public byte[] ToArray(long start, long length, int numberZeroBytesPrefix = 0, int numberZeroByteAppend = 0)
        {
            lock (streamLocker)
            {
                if (start + length > _innerStream.Length)
                {
                    throw new ArgumentOutOfRangeException("length","Provided start and length parameters reference past the end of the available stream.");
                }

                _innerStream.Seek(start,SeekOrigin.Begin);
                byte[] returnData = new byte[length+numberZeroBytesPrefix+numberZeroByteAppend];
                _innerStream.Read(returnData, numberZeroBytesPrefix, (int) length);
                return returnData;
            }
        }



        #endregion


        #region Stream接口

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override long Seek(long offset, SeekOrigin origin)
        {
            lock (streamLocker)
            {
                return _innerStream.Seek(offset, origin);
            }
        }

        /// <summary>
        /// 当在派生类中重写时，设置当前流的长度。
        /// </summary>
        /// <param name="value"></param>
        public override void SetLength(long value)
        {
            lock (streamLocker)
            {
                _innerStream.SetLength(value);
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override bool CanRead
        {
            get { throw new NotImplementedException(); }
        }

        public override bool CanSeek
        {
            get { throw new NotImplementedException(); }
        }

        public override bool CanWrite
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// the total length of the internal stream
        /// </summary>
        public override long Length
        {
            get
            {
                lock (streamLocker)
                {
                    return _innerStream.Length;
                }
            }
        }

        /// <summary>
        /// The current position of the internal stream
        /// </summary>
        public override long Position
        {
            get
            {
                lock (streamLocker)
                {
                    return _innerStream.Position;
                }
            }
            set
            {
                lock (streamLocker)
                {
                    _innerStream.Position = value;
                }
            }
        }
        #endregion
    }
}
