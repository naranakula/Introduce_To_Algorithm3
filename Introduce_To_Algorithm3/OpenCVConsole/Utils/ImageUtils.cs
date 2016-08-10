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
        /// 获取图片  Mat指矩阵
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
        /// 将bitmap转换为mat
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Mat ConvertToMat(Bitmap bitmap)
        {
            return bitmap.ToMat();
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


        //Mat mat = new Mat("./Images/lena.jpg", ImreadModes.Color);

        //    using (Window window = new Window("Lena", WindowMode.Normal, mat))
        //    {
        //        window.Move(10,10);
                
        //        window.ShowImage(mat);
        //        Cv2.WaitKey(10000);
        //    }


            //VideoCapture videoCapture = new VideoCapture(@"D:\BaiduYunDownload\希赛系统架构师视频\3 JG：第03章 系统开发基础.wmv");
            //Console.WriteLine(videoCapture.Fps+""+videoCapture.IsOpened());
            //int sleepTime = (int)Math.Round(1000/25.0);
            
            //using (Window window = new Window("capture"))
            //{
            //    using (Mat image = new Mat())
            //    {
            //        while (true)
            //        {
            //            videoCapture.Read(image);
            //            if (image.Empty())
            //            {
            //                break;
            //            }
            //            window.ShowImage(image);
            //            Cv2.WaitKey(sleepTime);
            //        }
            //    }
            //}

    }
}
