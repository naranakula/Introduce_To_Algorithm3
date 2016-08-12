using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Imaging;
using AForge.Imaging.Filters;

namespace AforgeNetConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Bitmap bitmap = new Bitmap("./Images/check1.jpg"))
            {
                using (Bitmap clone = bitmap.Clone() as Bitmap)
                {
                    Console.WriteLine("width = "+clone.Width+", height = "+clone.Height);
                    
                    FiltersSequence seq = new FiltersSequence();
                    
                    //添加灰度滤镜
                    seq.Add(Grayscale.CommonAlgorithms.BT709);
                  
                    Median median = new Median(5);
                    seq.Add(median);
                    seq.Add(new AdaptiveSmoothing());
                    //添加二值化过滤器
                    
                    seq.Add(new OtsuThreshold());
                    Bitmap result = seq.Apply(bitmap);
                    
                    ConnectedComponentsLabeling connectedFilter = new ConnectedComponentsLabeling();
                    connectedFilter.MinWidth = 50;
                    connectedFilter.MinHeight = 50;
                    
                    result = connectedFilter.Apply(result);
                    result.Save("1.jpg");
                    Blob[] blobs2 = connectedFilter.BlobCounter.GetObjectsInformation().Where(r=>r.Area>1000).ToArray();
                    Console.WriteLine(blobs2.Length);


                    BlobCounter extractor = new BlobCounter();
                    extractor.FilterBlobs = true;
                    
                    extractor.MinWidth = 50;
                    extractor.MinHeight = 50;
                    extractor.ProcessImage(result);
                    Blob[] blobs = extractor.GetObjectsInformation();
                    Console.WriteLine(blobs.Length);

                }
            }
            

        }
    }
}
