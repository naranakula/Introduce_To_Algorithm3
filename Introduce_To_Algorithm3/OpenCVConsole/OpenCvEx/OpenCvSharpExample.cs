using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace OpenCVConsole.OpenCvEx
{
    /// <summary>
    /// OpenCvSharp的例子
    /// </summary>
    public static class OpenCvSharpExample
    {
        /// <summary>
        /// 访问像素
        /// </summary>
        public static void AccessPixel()
        {
            using (Mat mat = new Mat("Images/lena.jpg",ImreadModes.Color))
            {
                //bgr格式
                using (MatOfByte3 mat3 = new MatOfByte3(mat))
                {
                    var indexer = mat3.GetIndexer();
                    for (int y = 0; y < mat.Height; y++)
                    {
                        for (int x = 0; x < mat.Width; x++)
                        {
                            Vec3b color = indexer[y, x];
                            byte temp = color.Item0;
                            color.Item0 = color.Item2;//B <-- R
                            color.Item2 = temp;
                            indexer[y, x] = color;
                        }
                    }
                }

                using (new Window("window", mat))
                {
                    Cv2.WaitKey();
                }
            }
        }

        #region Mat转换
        /// <summary>
        /// 将mat转换为bitmap
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static Bitmap ConvertToBitmap(Mat mat)
        {
            return BitmapConverter.ToBitmap(mat);
        }

        /// <summary>
        /// 将bitmap转换为mat
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Mat ConvertToMat(Bitmap bitmap)
        {
            return BitmapConverter.ToMat(bitmap);
        }

        /// <summary>
        /// 将mat转换为byte array
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static byte[] ConvertToByteArray(Mat mat)
        {
            //转换为png格式
            byte[] barray = mat.ToBytes(".png");
            //可以直接以png格式保存到文件中
            return barray;
        }

        /// <summary>
        /// 将byte array 转换为mat
        /// </summary>
        /// <param name="barray">可以是从图片中读取</param>
        /// <returns></returns>
        public static Mat ConvertToMat(byte[] barray)
        {
            //从图片文件中读取byte数组，转换为mat
            Mat mat = Mat.FromImageData(barray, ImreadModes.Color);
            return mat;
        }

        /// <summary>
        /// 将mat转换为wpf使用的WriteableBitmap
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static WriteableBitmap ConvertToWriteableBitMap(Mat mat)
        {
            //wpf使用Image控件，将Image控件的Source设置为WriteableBitmap
            WriteableBitmap writeableBitmap = mat.ToWriteableBitmap();
            return writeableBitmap;
        }

        #endregion

        #region Capture Vedio

        /// <summary>
        /// 捕获视频
        /// </summary>
        public static void CaptureVedio()
        {
            VideoCapture capture = new VideoCapture(CaptureDevice.Any);
            //获取视频的fps  画面每秒传输帧数
            double fps = capture.Fps;
            fps = fps > 10 ? fps : 30;

            int sleepTime = (int) Math.Round(1000/fps);

            using (Window window = new Window("capture"))
            {
                using (Mat image = new Mat())
                {
                    while (true)
                    {
                        capture.Read(image);
                        if (image.Empty())
                        {
                            break;
                        }

                        window.ShowImage(image);
                        Cv2.WaitKey(sleepTime);
                    }
                }
            }

        }


        #endregion

    }
}
