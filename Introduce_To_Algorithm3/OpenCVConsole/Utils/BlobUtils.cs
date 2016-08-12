using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace OpenCVConsole.Utils
{
    /// <summary>
    /// 检测是否应该截图
    /// </summary>
    public static class BlobUtils
    {
        /// <summary>
        /// 背景图片
        /// </summary>
        private static string backgroundFile =  @"./Images/blank.jpg";

        /// <summary>
        /// 背景图片
        /// </summary>
        private static Mat backgroundMat = null;

        //移除背景光线的方法  0 different差  1 div 除 (根据测试除的效果略好于差，均大幅好于不用)
        private static int lightMethod = 0;

        /// <summary>
        /// 认为是一个物体的最小area 像素单位
        /// </summary>
        private const int MinArea = 2800;

        /// <summary>
        /// 认为是一个物体的最小宽度 像素单位
        /// </summary>
        private const int MinWidth = 60;

        /// <summary>
        /// 认为是一个物体的最小高度， 像素单位
        /// </summary>
        private const int MinHeight = 60;
        
        /// <summary>
        /// 上一次扫描的blob
        /// </summary>
        private static List<ConnectedComponents.Blob> lastBlobList = null;

        /// <summary>
        /// 上一次调用时间
        /// </summary>
        private static DateTime lastCallTime = DateTime.Now.AddDays(-1);

        /// <summary>
        /// 是否需要保存图片
        /// </summary>
        /// <param name="mat">指的是刚刚读进来的灰度图片</param>
        /// <returns></returns>
        public static bool IsNeedToSave(Mat mat)
        {
            if (mat == null || mat.IsDisposed)
            {
                return false;
            }

            #region 预判断

            if ((DateTime.Now - lastCallTime).TotalMilliseconds < 500)
            {
                //调用间隔至少为500ms
                return false;
            }

            lastCallTime = DateTime.Now;

            #endregion

            #region 加载背景图片
            if (backgroundMat == null || backgroundMat.IsDisposed)
            {
                if (!File.Exists(backgroundFile))
                {
                    Console.WriteLine("未找到背景图片：" + backgroundFile);
                    backgroundMat = null;
                }
                else
                {
                    try
                    {
                        backgroundMat = Cv2.ImRead(backgroundFile, ImreadModes.GrayScale);
                        //去噪
                        backgroundMat = backgroundMat.MedianBlur(3);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("加载背景图片失败:"+ex);
                        if (backgroundMat != null && backgroundMat.IsEnabledDispose)
                        {
                            backgroundMat.Dispose();
                        }
                        backgroundMat = null;
                    }
                }
            }
            #endregion

            #region 图片处理
            //图片去噪
            mat = mat.MedianBlur(3);
            int width = mat.Width;
            int height = mat.Height;
            //当前使用的背景图片
            Mat usedBackgroundMat = null;
            if (backgroundMat != null)
            {
                if (backgroundMat.Width != width || backgroundMat.Height != height)
                {
                    Console.WriteLine("背景图片与原图大小不一样");
                    usedBackgroundMat = backgroundMat.Resize(new Size(width, height));
                }
                else
                {
                    usedBackgroundMat = backgroundMat;
                }
            }

            //是否使用了lightMethod 光照模型去除背景， 拍摄同样一张图片但是不带物体
            bool usedLightMethod = false;
            if (usedBackgroundMat != null && usedBackgroundMat.Width == width && usedBackgroundMat.Height == height)
            {
                if (lightMethod == 0)
                {
                    mat = usedBackgroundMat - mat;
                    usedLightMethod = true;
                }
                else
                {
                    mat.ConvertTo(mat, MatType.CV_32F);
                    usedBackgroundMat.ConvertTo(usedBackgroundMat, MatType.CV_32F);
                    mat = (1 - (usedBackgroundMat / mat)) * 255;
                    mat.ConvertTo(mat, MatType.CV_8U);
                    usedLightMethod = true;
                }
            }

            //二值化图像
            if (usedLightMethod)
            {
                mat = mat.Threshold(30, 255, ThresholdTypes.Binary);
            }
            else
            {
                mat = mat.Threshold(140, 255, ThresholdTypes.Binary);
            }


            #endregion

            #region 联通组件

            ConnectedComponents components = mat.ConnectedComponentsEx();

            List<ConnectedComponents.Blob> blobList = new List<ConnectedComponents.Blob>();

            for (int i = 0; i < components.Blobs.Count; i++)
            {
                if (i == 0)
                {
                    //背景， 
                    continue;
                }

                ConnectedComponents.Blob blob = components.Blobs[i];
                //实际区域大小
                if (blob.Area > MinArea && blob.Width > MinWidth && blob.Height > MinHeight)
                {
                    if (blob.Width > width*0.9 && blob.Height > 0.9)
                    {
                        //发现超大物体，此物体有可能是背景或者其它干扰
                        Console.WriteLine("超大物体忽略");
                    }
                    else
                    {
                        //一瓶矿泉水  width = 227 height=171 area=15907
                        blobList.Add(blob);
                    }
                }
            }
            
            #endregion

            #region 判断是否需要截图
            
            //获取上一次的blobs
            List<ConnectedComponents.Blob> oldLastBlobs = lastBlobList;
            lastBlobList = blobList;

            if (blobList.Count == 0)
            {
                //没有图片，不需要保存
                return false;
            }

            //获取最大的宽度的项
            ConnectedComponents.Blob maxItem = blobList.OrderByDescending(r => r.Width).First();
           
            if (oldLastBlobs == null || oldLastBlobs.Count == 0)
            {
                //之前没有图片 或者 中间范围内没有图片
                if (maxItem.Width > width*0.7 && maxItem.Height>height*0.4)
                {
                    //最大的物体很大
                    return true;
                }
                else
                {
                    //查找位于中间的个体数量
                    List<ConnectedComponents.Blob> middleBlobs = FindMiddleObjects(width, blobList, 0.81);
                    if (middleBlobs.Count > 0)
                    {
                        //中间有物体
                        return true;
                    }
                    else
                    {
                        //中间没有物体或者物体没有完全到中间
                        return false;
                    }
                }
            }
            else
            {
                if (maxItem.Width > width*0.7 && maxItem.Height> height*0.4)
                {
                    //最大的物体很大
                    return true;
                }

                //之前图片有物体
                List<ConnectedComponents.Blob> newMiddleBlobs = FindMiddleObjects(width, blobList, 0.81);
                //获取中间旧的
                List<ConnectedComponents.Blob> oldMiddleBlobs = FindMiddleObjects(width, oldLastBlobs, 0.81);

                if (newMiddleBlobs.Count == 0)
                {
                    //中间没有，认为不需要截图
                    return false;
                }

                //新的中间有图
                if (oldMiddleBlobs.Count == 0)
                {
                    //之前有图片，但图片不在中间,新的又有了
                    return true;
                }
                else
                {
                    int minDiff = 1;//任务现在和之前相差超过minDiff个物体需要截图
                    //现在和以前均有图片
                    if ((newMiddleBlobs.Count - oldMiddleBlobs.Count) > minDiff)
                    {
                        //现在跟置前有两个以上的不同图片
                        return true;
                    }
                    else
                    {
                        
                        ////先按最左点排序，再按中心点排序，再按照面积排序  升序
                        newMiddleBlobs =  newMiddleBlobs.OrderBy(r => r.Left).ThenBy(r=>r.Width).ThenBy(r => r.Centroid.Y).ThenBy(r => r.Area).ToList();
                        oldMiddleBlobs = oldMiddleBlobs.OrderBy(r => r.Left).ThenBy(r => r.Width).ThenBy(r => r.Centroid.Y).ThenBy(r => r.Area).ToList();

                        var lcsTuple = LCS(newMiddleBlobs, oldMiddleBlobs);
                        List<ConnectedComponents.Blob> commonBlobs = lcsTuple.Item1;
                        List<ConnectedComponents.Blob> onlyNewBlobs = lcsTuple.Item2;
                        List<ConnectedComponents.Blob> onlyOldBlobs = lcsTuple.Item3;

                        if (commonBlobs.Count == 0)
                        {
                            //现在和以前没有公共部分，截图
                            return true;
                        }
                        else if (onlyNewBlobs.Count == 0 && onlyOldBlobs.Count == 0)
                        {
                            //全部是公共部分
                            return false;
                        }
                        else if(onlyOldBlobs.Count == 0)
                        {
                            //新的部分多了，除此之外都是公共的
                            return true;
                        }
                        else if (onlyNewBlobs.Count == 0)
                        {
                            //旧的部分多了，除此之外全是公共的
                            return false;
                        }
                        else
                        {
                            //旧的部分，新的部分，公共的部分都有
                            return true;
                        }

                    }
                }
            }

            #endregion
            
        }

        /// <summary>
        /// 根据图片判断是否需要保存
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsNeedToSave(String fileName)
        {
            try
            {
                using (Mat img = Cv2.ImRead(fileName, ImreadModes.GrayScale))
                {
                    return IsNeedToSave(img);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }


        /// <summary>
        /// 找到位于中心的个体数数量
        /// </summary>
        /// <param name="width">图片的整体宽度</param>
        /// <param name="blobs">要过滤的blobs</param>
        /// <param name="middlePercent">中间部分比例</param>
        /// <returns></returns>
        private static List<ConnectedComponents.Blob> FindMiddleObjects( int width,List<ConnectedComponents.Blob> blobs,double middlePercent=0.8)
        {
            if (blobs == null || blobs.Count == 0)
            {
                return  new List<ConnectedComponents.Blob>();
            }

            double left = (1 - middlePercent)/2*width;
            double right = width - left;
            return blobs.Where(r => r.Left > left && (r.Left + r.Width) < right).ToList();
        }

        /// <summary>
        /// 判断两个blob是否相同
        /// </summary>
        /// <param name="blob1"></param>
        /// <param name="blob2"></param>
        /// <returns></returns>
        private static bool IsSame(ConnectedComponents.Blob blob1, ConnectedComponents.Blob blob2)
        {
            //如果两个物体 ，宽度 高度，中心点 area 接近则认为它们是相同的
            if (Math.Abs(blob1.Width - blob2.Width) >= 32)
            {
                return false;
            }

            if (Math.Abs(blob1.Height - blob2.Height) >= 32)
            {
                return false;
            }

            if (Math.Abs(blob1.Centroid.Y - blob2.Centroid.Y) >= 22)
            {
                return false;
            }

            if (Math.Abs(blob1.Area - blob2.Area) >= 6000)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 最长公共子串, 只在最新中的，只在old中的
        /// </summary>
        /// <param name="newList">新的blob list</param>
        /// <param name="oldList">旧的blob</param>
        /// <returns></returns>
        public static Tuple<List<ConnectedComponents.Blob>, List<ConnectedComponents.Blob>, List<ConnectedComponents.Blob>> LCS(List<ConnectedComponents.Blob> newList,
            List<ConnectedComponents.Blob> oldList)
        {
            List<ConnectedComponents.Blob> lcsList = new List<ConnectedComponents.Blob>();//公共
            List<ConnectedComponents.Blob> onlyNewList = new List<ConnectedComponents.Blob>();//只在最新中的
            List<ConnectedComponents.Blob> onlyOldList = new List<ConnectedComponents.Blob>();//只在old中的
            if (newList == null || newList.Count == 0)
            {
                return new Tuple<List<ConnectedComponents.Blob>, List<ConnectedComponents.Blob>, List<ConnectedComponents.Blob>>(lcsList,onlyNewList,oldList);
            }

            if (oldList == null || oldList.Count == 0)
            {
                return new Tuple<List<ConnectedComponents.Blob>, List<ConnectedComponents.Blob>, List<ConnectedComponents.Blob>>(lcsList,newList,onlyOldList);
            }

            int newIndex = 0;//新个体的最后索引
            int oldIndex = 0;//旧个体的最后索引
            int length = 0;//lcs长度
            int[,] matrix = new int[newList.Count,oldList.Count];

            for (int i = 0; i < newList.Count; i++)
            {
                for (int j = 0; j < oldList.Count; j++)
                {
                    //当前个体对角线之前的那一个
                    int n = (i - 1 >= 0 && j - 1 >= 0) ? matrix[i - 1, j - 1] : 0;
                    if (IsSame(newList[i], oldList[j]))
                    {
                        //blob相同加1
                        matrix[i, j] = n + 1;
                    }
                    else
                    {
                        matrix[i, j] = 0;
                    }

                    //更新当前lcs
                    if (matrix[i, j] > length)
                    {
                        length = matrix[i, j];
                        newIndex = i;
                        oldIndex = j;
                    }
                }
            }
            if (length > 0)
            {
                //获取lcs
                for (int i = newIndex - length + 1; i <= newIndex; i++)
                {
                    lcsList.Add(newList[i]);
                }

                //only new
                for (int i = 0; i < newList.Count; i++)
                {
                    if (i < newIndex - length + 1 || i > newIndex)
                    {
                        onlyNewList.Add(newList[i]);
                    }
                }

                //only old
                for (int i = 0; i < oldList.Count; i++)
                {
                    if (i < oldIndex - length + 1 || i > oldIndex)
                    {
                        onlyOldList.Add(oldList[i]);
                    }
                }
            }
            else
            {
                return new Tuple<List<ConnectedComponents.Blob>, List<ConnectedComponents.Blob>, List<ConnectedComponents.Blob>>(lcsList,newList,oldList);
            }

            return new Tuple<List<ConnectedComponents.Blob>, List<ConnectedComponents.Blob>, List<ConnectedComponents.Blob>>(lcsList,onlyNewList,onlyOldList);
        }
             



    }
}
