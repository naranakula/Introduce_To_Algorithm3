using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace OpenCVConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            VideoCapture videoCapture = new VideoCapture(@"D:\YoukuFiles\download\1.mp4");
            Console.WriteLine(videoCapture.Fps+""+videoCapture.IsOpened());
            int sleepTime = (int)Math.Round(1000/25.0);
            using (Window window = new Window("capture"))
            {
                using (Mat image = new Mat())
                {
                    while (true)
                    {
                        videoCapture.Read(image);
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
    }
}
