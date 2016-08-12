using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCVConsole.Utils;

namespace OpenCVConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            while (true)
            {
                string fileName = @"./Images/";
                Console.WriteLine("输入图片名称");
                fileName += Console.ReadLine();
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss fff"));
                bool isNeedToSave = BlobUtils.IsNeedToSave(fileName);
                Console.WriteLine("IsNeedToSave = "+isNeedToSave);
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss fff"));
            }

            
            //图片名称
            string imgFile = @"./Images/check1.jpg";
            //光线模式文件
            string lightPatternFile = @"./Images/blank.jpg";
            //移除背景光线的方法  0 different差  1 div 除 (根据测试除的效果略好于差，均大幅好于不用)
            int lightMethod = 0;
            Console.WriteLine("lightmethod = "+lightMethod);
            // 分割的方法  1 connected component    2 connected components with statistic(统计)  3 find contour(轮廓线)
            int segmentMethod = 1;

            //转换为单通道灰度图
            Mat img = Cv2.ImRead(imgFile,ImreadModes.GrayScale);
            Mat cloneImg = img.Clone();
            //noise removal 噪音去除
            img = img.MedianBlur(3);
            
            //使用光照模型去除背景， 拍摄同样一张图片但是不带物体
            Mat light = Cv2.ImRead(lightPatternFile, ImreadModes.GrayScale);
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

            
            if (segmentMethod == 1)
            {
                // 1 connected component    2 connected components with statistic(统计)  3 find contour(轮廓线)
                //int nLabels = Cv2.ConnectedComponents(img, label, PixelConnectivity.Connectivity8, MatType.CV_32S);
                
                ConnectedComponents components = img.ConnectedComponentsEx();
                int nLabels = components.LabelCount;
                Point[][] points =img.FindContoursAsArray(RetrievalModes.External, ContourApproximationModes.ApproxSimple);
                int findCount = 0;
                for (int i = 0; i < points.Length; i++)
                {
                    if (points[i].Length > 100)
                    {
                        findCount++;
                    }
                }
                findCount--;
                Console.WriteLine(points.Length+"=find"+findCount);
                Console.WriteLine("number of objects = "+components.LabelCount);
                int count = 0;
                List<ConnectedComponents.Blob> list = new List<ConnectedComponents.Blob>();

                for (int i = 0; i < components.Blobs.Count; i++)
                {
                    if (i == 0)
                    {
                        continue;
                    }
                    ConnectedComponents.Blob blob = components.Blobs[i];
                    //实际区域大小
                    if (blob.Area > 2200 && blob.Width>50 && blob.Height>50)
                    {
                        //一瓶矿泉水  width = 227 height=171 area=15907
                        count++;
                        list.Add(blob);
                    }
                }


                list = list.OrderBy(r => r.Centroid.X).ToList();
                Console.WriteLine("实际个数是：" + count);
                Console.WriteLine("width="+img.Width+",height="+img.Height);
                foreach (var blob in list)
                {
                    Console.WriteLine("area="+blob.Area+", ("+blob.Centroid.X+","+ blob.Centroid.Y+")  width="+blob.Width+",height="+blob.Height+"left="+blob.Left);
                }

                Mat output = Mat.Zeros(img.Rows, img.Cols, MatType.CV_8UC3);
       
                for (int m = 1; m < nLabels; m++)
                {
                    //Mat mask = label.Equals(m);
                    //output.SetTo(Scalar.RandomColor(),mask);
                    
                    Scalar scalar = Scalar.RandomColor();
                    Vec3b vec3B = scalar.ToVec3b();
                    for (int i = 0; i < img.Rows; i++)
                    {
                        for (int j = 0; j < img.Cols; j++)
                        {
                            int num = components.Labels[i,j];
                            
                            if (num==m)
                            {
                                output.Set<Vec3b>(i, j, vec3B);
                            }
                        }
                    }
                }

                using (Window window = new Window("check"))
                {
                    window.ShowImage(output);
                    Cv2.WaitKey(0);
                }
            }


            using (Window window = new Window("check"))
            {
                window.ShowImage(cloneImg);
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

    }
}
