using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CopyShare
{
    class TextHandling
    {

        WebClient webClient = new WebClient();

        public void UploadText(string content)
        {
            using (webClient)
            {
                webClient.UploadProgressChanged += WebClient_UploadProgressChanged;
                webClient.UploadFileCompleted += WebClient_UploadFileCompleted;

                File.WriteAllText(MainWindow.textDataOfflineSource.ToString(), content);

                webClient.UploadFileAsync(new Uri(MainWindow.postURL), "POST", MainWindow.textDataOfflineSource.ToString());
            }
        }

        private void WebClient_UploadFileCompleted(object sender, UploadFileCompletedEventArgs e)
        {
            MainWindow.progressBar.Value = 100;
            MainWindow.progressBar.Value = 0;
            MainWindow.progressBar.Visibility = Visibility.Hidden;
        }

        private void WebClient_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            MainWindow.progressBar.Value = (e.BytesSent * 100 / e.TotalBytesToSend) - 5;
        }

        public void DownloadText()
        {
            using (webClient)
            {
                webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
                webClient.DownloadStringCompleted += WebClient_DownloadStringCompleted;
                
                webClient.DownloadStringAsync(new Uri(MainWindow.textDataOnlineSource));

            }

        }

        private void WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            MainWindow.textBox.Text = e.Result;
            Clipboard.SetText(e.Result);
            MainWindow.progressBar.Value = 0;
            MainWindow.progressBar.Visibility = Visibility.Hidden;
        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            MainWindow.progressBar.Value = e.ProgressPercentage;
        }
    }
}
