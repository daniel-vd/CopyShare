using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CopyShare
{
    class Handlers
    {

        //Method for rewriting a BitmapSource to a writable stream for a text file
        public static byte[] BitmapSourceToByte(System.Windows.Media.Imaging.BitmapSource source)
        {
            var encoder = new System.Windows.Media.Imaging.PngBitmapEncoder();
            var frame = System.Windows.Media.Imaging.BitmapFrame.Create(source);
            encoder.Frames.Add(frame);
            var stream = new MemoryStream();

            encoder.Save(stream);
            return stream.ToArray();
        }
        
        //Recreating the BitmapSource out of the writable stream
        public static System.Windows.Media.Imaging.BitmapSource ByteToBitmapSource(byte[] bytes)
        {
            var stream = new MemoryStream(bytes);
            return System.Windows.Media.Imaging.BitmapFrame.Create(stream);
        }

          
        public static bool ClipBoardContainsDropFileImg()
        {
            if (Clipboard.ContainsFileDropList())
            {
                string[] imgExtensions = new string[6] { "PNG", "JPEG", "jpeg", "png", "jpg", "JPG" };
                foreach (var imgExt in Clipboard.GetFileDropList())
                {
                    //MessageBox.Show(imgExt);
                    //MessageBox.Show(imgExt.Substring(imgExt.LastIndexOf(".")));
                    if (imgExtensions.Contains(imgExt.Substring(imgExt.LastIndexOf(".") + 1)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
