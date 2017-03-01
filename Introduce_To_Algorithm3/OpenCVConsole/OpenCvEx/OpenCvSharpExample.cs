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

        #region 测试

        #region 高光

        // PMIS_Tools_Controls.FmPictureEdit
        /// <summary>
        /// 高光20%
        /// </summary>
        /// <param name="bitOrigin"></param>
        /// <param name="Percent">如高光+20%，  Percent=0.2</param>
        /// <returns></returns>
        public static Bitmap ColorHighPenetrate(Bitmap bitOrigin, double Percent)
        {
            double num = 1.0 + Percent * 5.0 / 3.0;
            Bitmap bitmap = new Bitmap(bitOrigin.Width, bitOrigin.Height);
            Color color = default(Color);
            for (int i = 0; i < bitOrigin.Width; i++)
            {
                for (int j = 0; j < bitOrigin.Height; j++)
                {
                    color = bitOrigin.GetPixel(i, j);
                    int red = 0;
                    int green = 0;
                    int blue = 0;
                    int num2 = 0;
                    for (int k = 0; k < 3; k++)
                    {
                        if (k == 0)
                        {
                            num2 = (int)color.R;
                        }
                        else if (k == 1)
                        {
                            num2 = (int)color.G;
                        }
                        else if (k == 2)
                        {
                            num2 = (int)color.B;
                        }
                        if (num2 < 96)
                        {
                            num2 = (int)(num * (double)num2);
                        }
                        else
                        {
                            num2 = (int)(96.0 * num + (double)(num2 - 96) * ((255.0 - 96.0 * num) / 159.0));
                        }
                        if (num2 > 255)
                        {
                            num2 = 255;
                        }
                        else if (num2 < 0)
                        {
                            num2 = 0;
                        }
                        if (k == 0)
                        {
                            red = num2;
                        }
                        else if (k == 1)
                        {
                            green = num2;
                        }
                        else if (k == 2)
                        {
                            blue = num2;
                        }
                    }

                    red = red > 255 ? 255 : red;
                    green = green > 255 ? 255 : green;
                    blue = blue > 255 ? 255 : blue;

                    color = Color.FromArgb(red, green, blue);
                    bitmap.SetPixel(i, j, color);
                }
            }
            return bitmap;
        }

        #endregion

        #region 去除无机物

        /// <summary>
        /// 去除无机物，即去除蓝色
        /// </summary>
        /// <param name="bitOrigin"></param>
        /// <returns></returns>
        public static Bitmap EliminateBlue(Bitmap bitOrigin)
        {
            Bitmap bitmap = new Bitmap(bitOrigin.Width, bitOrigin.Height);
            Color color = default(Color);
            for (int i = 0; i < bitOrigin.Width; i++)
            {
                for (int j = 0; j < bitOrigin.Height; j++)
                {
                    color = bitOrigin.GetPixel(i, j);
                    int hueFromColor = getHueFromColor(color);
                    int intensityFromColor = getIntensityFromColor(color);
                    if (hueFromColor > 120 && hueFromColor < 170 && intensityFromColor > 25 && intensityFromColor < 200)
                    {
                        color = Color.FromArgb(190, 180, 180);
                    }
                    bitmap.SetPixel(i, j, color);
                }
            }
            return bitmap;
        }


        // PMIS_Tools_Controls.FmPictureEdit
        private static int getHueFromColor(Color color)
        {
            if (color.R != color.G && color.R != color.B)
            {
                double num = (double)((color.R - color.G + (color.R - color.B)) / 2.0);
                num /= Math.Sqrt(Math.Pow((double)(color.R - color.G), 2.0) + Math.Pow((double)(color.R - color.B), 2.0));
                num = Math.Acos(num) * 57.3;
                if (color.G < color.B)
                {
                    num = 360.0 - num;
                }
                return (int)(240.0 * num / 360.0);
            }
            return 160;
        }


        // PMIS_Tools_Controls.FmPictureEdit
        private static int getIntensityFromColor(Color color)
        {
            int result = (int)(240 * (color.R + color.G + color.B) / 3.0 / 255);
            return result;
        }

        #endregion

        #region 去除有机物


        /*
         * 黄色--有机物
         * 绿色--无机物
         * 蓝色--电子金属
         * 毒品氯胺酮是绿色 其它全部为橙黄色
         * 
         * 显示屏上显示为蓝色调，可以判断为金属材料。如匕首、刀具等
         * 显示屏上显示为绿色调，可以判断为爆炸物品。如鞭炮、爆竹等
         * 显示屏上显示为橙色调，可以判断为液体物品。如矿泉水、打火机及相关有机物
         * 
         * 
         */


        // PMIS_Tools_Controls.FmPictureEdit
        /// <summary>
        /// 剔除有机物，剔除黄色
        /// </summary>
        /// <param name="bitOrigin"></param>
        /// <returns></returns>
        public static Bitmap EliminateYellow(Bitmap bitOrigin)
        {
            Bitmap bitmap = new Bitmap(bitOrigin.Width, bitOrigin.Height);
            Color color = default(Color);
            for (int i = 0; i < bitOrigin.Width; i++)
            {
                for (int j = 0; j < bitOrigin.Height; j++)
                {
                    color = bitOrigin.GetPixel(i, j);
                    int hueFromColor = getHueFromColor(color);
                    int intensityFromColor = getIntensityFromColor(color);
                    if (hueFromColor > 10 && hueFromColor < 40 && intensityFromColor > 40 && intensityFromColor < 220)
                    {
                        int num = 77 * color.R + 151 * color.G + 28 * color.B >> 8;
                        num = num > 255 ? 255 : num;
                        color = Color.FromArgb(num, num, num);
                    }
                    bitmap.SetPixel(i, j, color);
                }
            }
            return bitmap;
        }


        #endregion

        #region 黑白

        public static Bitmap Gray(Bitmap bitOrigin)
        {
            Bitmap bitmap = new Bitmap(bitOrigin.Width, bitOrigin.Height);
            Color color = default(Color);
            for (int i = 0; i < bitOrigin.Width; i++)
            {
                for (int j = 0; j < bitOrigin.Height; j++)
                {
                    color = bitOrigin.GetPixel(i, j);
                    int grayFromColor = getGrayFromColor(color);
                    color = Color.FromArgb(grayFromColor, grayFromColor, grayFromColor);
                    bitmap.SetPixel(i, j, color);
                }
            }
            return bitmap;
        }

        // PMIS_Tools_Controls.FmPictureEdit
        private static int getGrayFromColor(Color color)
        {
            int result = (int)(0.299 * (double)color.R + 0.587 * (double)color.G + 0.114 * (double)color.B);
            return result > 255 ? 255 : result;
        }



        #endregion


        #endregion

    }
}
