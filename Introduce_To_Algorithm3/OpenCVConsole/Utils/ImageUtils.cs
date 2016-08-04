using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace OpenCVConsole.Utils
{
    /// <summary>
    /// 图像帮助类
    /// </summary>
    public static class ImageUtils
    {
        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="imreadModes"></param>
        /// <returns></returns>
        public static Mat Read(string fileName,ImreadModes imreadModes)
        {
            return  new Mat(fileName,imreadModes);
        }

        /// <summary>
        /// 写图片
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="mat"></param>
        public static void Write(string fileName, Mat mat)
        {
            mat.ImWrite(fileName);
        }

        /// <summary>
        /// 转换为bitmap
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static Bitmap ConvertToBitmap(Mat mat)
        {
            return mat.ToBitmap();
        }
    }
}
