using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CopyShare.PictureHandling
{
    class PictureDownload
    {

        WebClient webClient = new WebClient();
        WebClient webClient2 = new WebClient();
        WebClient webClient3 = new WebClient();

        string fileNameExt;

        string picture1;
        string picture2;
        string picture3;

        public PictureDownload()
        {
            webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
            webClient.DownloadDataCompleted += WebClient_DownloadDataCompleted;

            webClient2.DownloadProgressChanged += WebClient2_DownloadProgressChanged;
            webClient2.DownloadDataCompleted += WebClient2_DownloadDataCompleted;

            webClient3.DownloadProgressChanged += WebClient3_DownloadProgressChanged;
            webClient3.DownloadDataCompleted += WebClient3_DownloadDataCompleted;

            getImages();

            download();

         
        }

        private void WebClient3_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            BitmapSource image = Handlers.ByteToBitmapSource(e.Result);
            MainWindow.image3.Source = image;

            MainWindow.progressBar.Value = 0;
            MainWindow.progressBar.Visibility = Visibility.Hidden;
        }

        private void WebClient3_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            MainWindow.progressBar_4.Value = e.BytesReceived * 100 / e.TotalBytesToReceive;
        }

        private void WebClient2_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            BitmapSource image = Handlers.ByteToBitmapSource(e.Result);

            MainWindow.image2.Source = image;


            MainWindow.progressBar.Value = 0;
            MainWindow.progressBar.Visibility = Visibility.Hidden;
        }

        private void WebClient2_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            MainWindow.progressBar_3.Value = e.BytesReceived * 100 / e.TotalBytesToReceive;
        }

        public void download()

        {
            if (picture1 != null)
            {
                try
                {
                    webClient.DownloadDataTaskAsync(MainWindow.pictureOnlineSource + "/" + picture1);
                    fileNameExt = "img1";
                }
                catch (Exception)
                {
                }
            }

            if (picture2 != null)
            {
                try
                {
                    webClient2.DownloadDataTaskAsync(MainWindow.pictureOnlineSource + "/" + picture2);
                    fileNameExt = "img2";
                }
                catch (Exception)
                {
                }
            }

            if (picture3 != null)
            {
                try
                {
                    webClient3.DownloadDataTaskAsync(MainWindow.pictureOnlineSource + "/" + picture3);
                    fileNameExt = "img3";
                }
                catch (Exception)
                {
                }
            }
        }

        public void getImages()
        {
            {
                string url = MainWindow.pictureOnlineSource;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string html = reader.ReadToEnd();
                        Regex regex = new Regex(GetDirectoryListingRegexForUrl(url));
                        MatchCollection matches = regex.Matches(html);
                        if (matches.Count > 0)
                        {
                            foreach (Match match in matches)
                            {
                                if (match.Success)
                                {
                                    if (match.Groups["name"].ToString().Contains("img1")) {
                                        picture1 = match.Groups["name"].ToString();
                                    }
                                    if (match.Groups["name"].ToString().Contains("img2"))
                                    {
                                        picture2 = match.Groups["name"].ToString();
                                    }
                                    if (match.Groups["name"].ToString().Contains("img3"))
                                    {
                                        picture3 = match.Groups["name"].ToString();
                                    }
                                }
                            }
                        }
                    }
                }

            }
        }

        private void WebClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            BitmapSource image = Handlers.ByteToBitmapSource(e.Result);

                MainWindow.image1.Source = image;
        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            MainWindow.progressBar_2.Value = e.BytesReceived * 100 / e.TotalBytesToReceive;
        }

        public static string GetDirectoryListingRegexForUrl(string url)
        {
                return "<a href=\".*\">(?<name>.*)</a>";
        }
    }
}
