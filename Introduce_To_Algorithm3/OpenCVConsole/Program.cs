using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;

namespace OpenCVConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            string id = biosId();    
            Console.WriteLine(id);
            Console.WriteLine(cpuId());
            Console.WriteLine(diskId());
            Console.WriteLine(baseId());
            return;

            //图片名称
            string imgFile = @"./Images/check1.jpg";
            //光线模式文件
            string lightPatternFile = @"";
            //移除背景光线的方法  0 different差  1 div 除
            int lightMethod = 0;
            // 分割的方法  1 connected component    2 connected components with statistic(统计)  3 find contour(轮廓线)
            int segmentMethod = 1;

            //转换为单通道灰度图
            Mat img = Cv2.ImRead(imgFile,ImreadModes.GrayScale);
            Mat cloneImg = img.Clone();
            //noise removal 噪音去除
            img = img.MedianBlur(3);
            
            //使用光照模型去除背景， 拍摄同样一张图片但是不带物体
            Mat light = Cv2.ImRead(@"./Images/blank.jpg", ImreadModes.GrayScale);
            light = light.MedianBlur(3);
            if (lightMethod == 0)
            {
                img = light - img;
            }
            else if (lightMethod == 1)
            {
                img.ConvertTo(img, MatType.CV_32F);
                light.ConvertTo(light, MatType.CV_32F);
                img = (1 - (light/img))*255;
                img.ConvertTo(img,MatType.CV_8U);
            }

            //二值化图像
            if (lightMethod == 0 || lightMethod == 1)
            {
                img = img.Threshold(30, 255, ThresholdTypes.Binary);
            }
            else
            {
                img = img.Threshold(140, 255, ThresholdTypes.BinaryInv);
            }

            Mat mat = new Mat(new Size(img.Rows,img.Cols),MatType.CV_8U);
            OutputArray outputArray = OutputArray.Create(mat);
            if (segmentMethod == 6)
            {
                // 1 connected component    2 connected components with statistic(统计)  3 find contour(轮廓线)
                
                int i = img.ConnectedComponents(outputArray);
                Console.WriteLine(i);
            }


            using (Window window = new Window("check"))
            {
                window.ShowImage(outputArray.GetMat());
                Cv2.WaitKey(0);
            }

            //Mat src = Cv2.ImRead("./Images/check1.jpg", ImreadModes.GrayScale);

            //// Histogram view
            //const int Width = 260, Height = 200;
            //Mat render = new Mat(new Size(Width, Height), MatType.CV_8UC3, Scalar.All(255));

            //// Calculate histogram
            //Mat hist = new Mat();
            //int[] hdims = { 256 }; // Histogram size for each dimension
            //Rangef[] ranges = { new Rangef(0, 256), }; // min/max 
            //Cv2.CalcHist(
            //    new Mat[] { src },
            //    new int[] { 0 },
            //    null,
            //    hist,
            //    1,
            //    hdims,
            //    ranges);

            //// Get the max value of histogram
            //double minVal, maxVal;
            //Cv2.MinMaxLoc(hist, out minVal, out maxVal);

            //Scalar color = Scalar.All(100);
            //// Scales and draws histogram
            //hist = hist * (maxVal != 0 ? Height / maxVal : 0.0);
            //for (int j = 0; j < hdims[0]; ++j)
            //{
            //    int binW = (int)((double)Width / hdims[0]);

            //    render.Rectangle(
            //        new Point(j * binW, render.Rows - (int)(hist.Get<float>(j))),
            //        new Point((j + 1) * binW, render.Rows),
            //        color);
            //}

            //using (new Window("Image", WindowMode.AutoSize | WindowMode.FreeRatio, src))
            //using (new Window("Histogram", WindowMode.AutoSize | WindowMode.FreeRatio, render))
            //{
            //    Cv2.WaitKey();
            //}

            //Mat mat = new Mat("./Images/check1.jpg",ImreadModes.GrayScale);

            //StringBuilder sb = new StringBuilder();
            //for (int i = 0; i < mat.Rows; i++)
            //{
            //    for (int j = 0; j < mat.Cols; j++)
            //    {
            //        double[] arr = mat.GetArray(i, j);
            //        sb.Append("(");
            //        for (int k = 0; k < arr.Length; k++)
            //        {
            //            sb.Append(arr[k] + ",");
            //        }
            //        sb.Append(")");

            //    }
            //    sb.AppendLine();
            //}

            //File.WriteAllText("1.txt",sb.ToString(),Encoding.UTF8);

            //using (Window window = new Window("Lena", WindowMode.Normal, mat))
            //{


            //    window.ShowImage(mat);

            //    Cv2.WaitKey(100000);
            //}


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

        private static void Window_OnMouseCallback(MouseEvent @event, int x, int y, MouseEvent flags)
        {
            Console.WriteLine(flags);

        }


        //Return a hardware identifier
        private static string identifier
        (string wmiClass, string wmiProperty, string wmiMustBeTrue)
        {
            string result = "";
            System.Management.ManagementClass mc =
        new System.Management.ManagementClass(wmiClass);
            System.Management.ManagementObjectCollection moc = mc.GetInstances();
            foreach (System.Management.ManagementObject mo in moc)
            {
                if (mo[wmiMustBeTrue].ToString() == "True")
                {
                    //Only get the first one
                    if (result == "")
                    {
                        try
                        {
                            result = mo[wmiProperty].ToString();
                            break;
                        }
                        catch
                        {
                        }
                    }
                }
            }
            return result;
        }


        //Return a hardware identifier
        private static string identifier(string wmiClass, string wmiProperty)
        {
            string result = "";
            System.Management.ManagementClass mc =
        new System.Management.ManagementClass(wmiClass);
            System.Management.ManagementObjectCollection moc = mc.GetInstances();
            foreach (System.Management.ManagementObject mo in moc)
            {
                //Only get the first one
                if (result == "")
                {
                    try
                    {
                        result = mo[wmiProperty].ToString();
                        break;
                    }
                    catch
                    {
                    }
                }
            }
            return result;
        }

        private static string cpuId()
        {
            //Uses first CPU identifier available in order of preference
            //Don't get all identifiers, as it is very time consuming
            string retVal = identifier("Win32_Processor", "UniqueId");
            if (retVal == "") //If no UniqueID, use ProcessorID
            {
                retVal = identifier("Win32_Processor", "ProcessorId");
                if (retVal == "") //If no ProcessorId, use Name
                {
                    retVal = identifier("Win32_Processor", "Name");
                    if (retVal == "") //If no Name, use Manufacturer
                    {
                        retVal = identifier("Win32_Processor", "Manufacturer");
                    }
                    //Add clock speed for extra security
                    retVal += identifier("Win32_Processor", "MaxClockSpeed");
                }
            }
            return retVal;
        }

        //BIOS Identifier
        private static string biosId()
        {
            return identifier("Win32_BIOS", "Manufacturer")
            + identifier("Win32_BIOS", "SMBIOSBIOSVersion")
            + identifier("Win32_BIOS", "IdentificationCode")
            + identifier("Win32_BIOS", "SerialNumber")
            + identifier("Win32_BIOS", "ReleaseDate")
            + identifier("Win32_BIOS", "Version");
        }
        //Main physical hard drive ID
        private static string diskId()
        {
            return identifier("Win32_DiskDrive", "Model")
            + identifier("Win32_DiskDrive", "Manufacturer")
            + identifier("Win32_DiskDrive", "Signature")
            + identifier("Win32_DiskDrive", "TotalHeads");
        }

        //Motherboard ID
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static string baseId()
        {
            //return identifier("Win32_BaseBoard", "Model")
            //+ identifier("Win32_BaseBoard", "Manufacturer")
            //+ identifier("Win32_BaseBoard", "Name")
            //+ identifier("Win32_BaseBoard", "SerialNumber");

            return identifier("Win32_BaseBoard", "SerialNumber");
        }
    }
}
