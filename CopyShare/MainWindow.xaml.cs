using AutoUpdaterDotNET;
using CopyShare.PictureHandling;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications.Messages;
using WK.Libraries.SharpClipboardNS;
using static WK.Libraries.SharpClipboardNS.SharpClipboard;
using Image = System.Windows.Controls.Image;

namespace CopyShare
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string textDataOfflineSource = @"C:\Users\vandi\Desktop\textData.txt";
        public static string textDataOnlineSource =  "https://organometallic-gues.000webhostapp.com/files/textData.txt";

        public static string pictureOfflineSource = /*@"C:\Users\vandi\Desktop\CopyShare\img";*/ Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\CopyShare\img\tempIMG.txt";
        public static string pictureOnlineSource = "https://organometallic-gues.000webhostapp.com/img";

        public static string postURL = "https://organometallic-gues.000webhostapp.com/index.php";
        public static string postURLImageFile = "https://organometallic-gues.000webhostapp.com/uploadIMG.php";

        System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();

        TextHandling textHandling = new TextHandling();

        public static TextBox textBox;
        public static ProgressBar progressBar;
        public static ProgressBar progressBar_2;
        public static ProgressBar progressBar_3;
        public static ProgressBar progressBar_4;
        public static Image image1;
        public static Image image2;
        public static Image image3;

        public MainWindow()
        {
            // Prevent app from being open multiple times
            String thisprocessname = Process.GetCurrentProcess().ProcessName;

            if (Process.GetProcesses().Count(p => p.ProcessName == thisprocessname) > 1)
                return;

            InitializeComponent();


            Console.WriteLine("hallo1");

            while (!CheckForInternetConnection())
            {
                MessageBox.Show("No connection.");
            }

            progressBar = progressBar1;
            progressBar_2 = progressBar2;
            progressBar_3 = progressBar3;
            progressBar_4 = progressBar4;

            image1 = imageWindow1;
            image2 = imageWindow2;
            image3 = imageWindow3;
            textBox = textBox1;

            new ClipboardEventsHandler();

            System.Windows.Forms.ContextMenu contextMenu1 = new System.Windows.Forms.ContextMenu();

            contextMenu1.MenuItems.Add("&Open Application");
            contextMenu1.MenuItems.Add("S&uspend Application");
            contextMenu1.MenuItems.Add("E&xit", new EventHandler(close_App));

            ni.Icon = new System.Drawing.Icon(@"C:\Users\vandi\Dropbox\Projects\CopyShare\CopyShare\Main.ico");
            ni.Visible = true;
            ni.DoubleClick +=
                delegate (object sender, EventArgs args)
                {
                    this.Show();
                    this.WindowState = WindowState.Normal;
                };
            ni.ContextMenu = contextMenu1;

            AutoUpdater.Start("https://organometallic-gues.000webhostapp.com/CopyShareUpdater.xml");


        }

        private void close_App(object sender, EventArgs e)
        {
            ni.Dispose();

            Environment.Exit(1);
        }



        //Runs when the 'send' button is clicked.
        //Sends the data in the textbox to the server
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            progressBar.Visibility = Visibility.Visible;

            textHandling.UploadText(textBox.Text);

            textBox1.Clear();
        }

        //Runs when the 'retrieve' button is clicked.
        //Retrieves the data from the server and puts in textbox and clipboard.
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            progressBar.Visibility = Visibility.Visible;

            textHandling.DownloadText();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            progressBar.Visibility = Visibility.Visible;

            new PictureUpload();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            new PictureDownload();

            Notifiercs notifiercs = new Notifiercs();
            notifiercs.notifier.ShowSuccess("Picture put in clipboard");
            
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("https://organometallic-gues.000webhostapp.com"))
                    return true;
            }
            catch
            {
                return false;
            }
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Minimized)
                this.Hide();

            base.OnStateChanged(e);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();


            base.OnClosing(e);
        }
    }
}
