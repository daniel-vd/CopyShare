using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CopyShare.PictureHandling
{
    class PictureUpload
    {
        WebClient webClient = new WebClient();

        public PictureUpload()
        {
            MainWindow.progressBar.Visibility = Visibility.Visible;
            using (webClient)
            {
                webClient.UploadProgressChanged += WebClient_UploadProgressChanged;
                webClient.UploadFileCompleted += WebClient_UploadFileCompleted;

                //Check if clipboard data is a bitmapsource
                if (Clipboard.ContainsImage())
                {
                    byte[] imageBytes = Handlers.BitmapSourceToByte(Clipboard.GetImage());

                    try
                    {
                        File.WriteAllBytes(MainWindow.pictureOfflineSource, imageBytes);

                        webClient.UploadFileAsync(new Uri(MainWindow.postURLImageFile), MainWindow.pictureOfflineSource);
                    }
                    catch (Exception)
                    { 
                        MessageBox.Show("Error1");
                    }

                } else if (Clipboard.ContainsFileDropList()) {
                    if (Handlers.ClipBoardContainsDropFileImg())
                    {
                        UploadFilesAsync();
                    }
                }

            }
        }

        private async void UploadFilesAsync ()
        {
            foreach (var copiedPicture in Clipboard.GetFileDropList())
            {
                try
                {
                    await webClient.UploadFileTaskAsync(new Uri(MainWindow.postURLImageFile), copiedPicture);
                }
                catch (Exception e)
                {

                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void WebClient_UploadFileCompleted(object sender, UploadFileCompletedEventArgs e)
        {
            MainWindow.progressBar.Value = 0;
            MainWindow.progressBar.Visibility = Visibility.Hidden;
        }

        private void WebClient_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            MainWindow.progressBar.Value = e.BytesSent * 100 / e.TotalBytesToSend;
        }


    }
}
