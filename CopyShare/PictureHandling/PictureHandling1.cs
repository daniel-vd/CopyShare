using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CopyShare
{
    class PictureHandling1
    {

        BitmapSource image;

        WebClient webClient = new WebClient();

        public void DownloadPicture()
        {
            MainWindow.progressBar.Visibility = Visibility.Visible;
            using (webClient)
            {
                webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
                webClient.DownloadDataCompleted += WebClient_DownloadDataCompleted;

                webClient.DownloadDataTaskAsync(MainWindow.pictureOnlineSource /*"https://organometallic-gues.000webhostapp.com/img/img3.txt"*/);
            }
        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            MainWindow.progressBar.Value = e.ProgressPercentage;
        }

        private void WebClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            image = ByteToBitmapSource(e.Result);

            MainWindow.image1.Source = image;
            //Clipboard.SetImage(image);

            MainWindow.progressBar.Value = 0;
            MainWindow.progressBar.Visibility = Visibility.Hidden;

        }

        public void UploadPicture()
        {
            using (webClient)
            {

                var dataObject = Clipboard.GetDataObject();
                var formats = dataObject.GetFormats(true);

                webClient.UploadProgressChanged += WebClient_UploadProgressChanged;
                webClient.UploadFileCompleted += WebClient_UploadFileCompleted;

                /* foreach (var item in Clipboard.GetFileDropList())
                 {
                     MessageBox.Show(item.ToString());
                 }*/

                if (Clipboard.ContainsImage())
                {

                    image = Clipboard.GetImage();

                    try
                    {
                        byte[] imageByte = BitmapSourceToByte(image);
                        File.WriteAllBytes(MainWindow.pictureOfflineSource + @"\tempIMG.txt", imageByte);

                        webClient.UploadFileAsync(new Uri(MainWindow.postURL), "POST", MainWindow.pictureOfflineSource.ToString());
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Make sure you have copied an image " + e);
                    }
                } else if (Clipboard.ContainsFileDropList())
                {
                    if (ClipBoardContainsDropFileImg())
                    {
                        MessageBox.Show("1");
                        foreach (var item in Clipboard.GetFileDropList())
                        {
                            try
                            {
                                webClient.UploadFile(new Uri(MainWindow.postURLImageFile), item);
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }
                    }
                }
            }
        }

        private bool ClipBoardContainsDropFileImg()
        {
            string[] imgExtensions = new string[4] { "PNG", "JPEG", "jpeg", "png" };
            foreach (var imgExt in Clipboard.GetFileDropList())
            {
                MessageBox.Show(imgExt);
                MessageBox.Show(imgExt.Substring(imgExt.LastIndexOf(".")));
                if (imgExtensions.Contains(imgExt.Substring(imgExt.LastIndexOf(".") + 1)))
                {
                    return true;
                }
            }
            return false;
        }

        private void WebClient_UploadFileCompleted(object sender, UploadFileCompletedEventArgs e)
        {
            MainWindow.progressBar.Value = 0;
            MainWindow.progressBar.Visibility = Visibility.Hidden;
        }

        private void WebClient_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            MainWindow.progressBar.Value = e.ProgressPercentage;
        }

        public static byte[] BitmapSourceToByte(System.Windows.Media.Imaging.BitmapSource source)
        {
            var encoder = new System.Windows.Media.Imaging.PngBitmapEncoder();
            var frame = System.Windows.Media.Imaging.BitmapFrame.Create(source);
            encoder.Frames.Add(frame);
            var stream = new MemoryStream();

            encoder.Save(stream);
            return stream.ToArray();
        }
        public static System.Windows.Media.Imaging.BitmapSource ByteToBitmapSource(byte[] bytes)
        {
            var stream = new MemoryStream(bytes);
            return System.Windows.Media.Imaging.BitmapFrame.Create(stream);
        }

    }
}
