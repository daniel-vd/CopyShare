using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using ToastNotifications.Messages;
using WK.Libraries.SharpClipboardNS;
using static WK.Libraries.SharpClipboardNS.SharpClipboard;

namespace CopyShare
{
    class ClipboardEventsHandler
    {
        //Listen for copying data, e.g. text or a picture

        SharpClipboard clipboard = new SharpClipboard();
        TextHandling textHandling = new TextHandling();

        //To avoid getting an event multiple times, create a cache that empties 1500ms after the event happened
        private string clipboardCache = null;
        private readonly Timer clipboardTimer = new Timer(1500);

        public ClipboardEventsHandler()
        {
            clipboard.ObserveLastEntry = false;
            clipboard.ClipboardChanged += ClipboardChanged;
            clipboardTimer.Elapsed += ClipboardTimer_Elapsed;
        }

        private void ClipboardTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //Empty cache when timer elapses
            clipboardCache = null;
            clipboardTimer.Stop();
        }


        private void ClipboardChanged(Object sender, ClipboardChangedEventArgs e)
        {
            // Is the content copied of text type?
            if (Clipboard.ContainsText())
            {
                if (e.Content.ToString() != clipboardCache)
                {
                    // Get the cut/copied text.
                    clipboardCache = e.Content.ToString();
                    clipboardTimer.Start();

                    textHandling.UploadText(e.Content.ToString());

                    Notifiercs notifiercs = new Notifiercs();
                    notifiercs.notifier.ShowSuccess("Clipboard uploaded");
                }
            }

            // Is the content copied of image type?
            else if (Clipboard.ContainsImage() || Handlers.ClipBoardContainsDropFileImg())
            {
                // Get the cut/copied image.
                new PictureHandling.PictureUpload();
            }

            // Is the content copied of file type?
            else if (e.ContentType == SharpClipboard.ContentTypes.Files)
            {
                // Get the cut/copied file/files.
                //Debug.WriteLine(clipboard.ClipboardFiles.ToArray());

                // ...or use 'ClipboardFile' to get a single copied file.
                //Debug.WriteLine(clipboard.ClipboardFile);
            }

            // If the cut/copied content is complex, use 'Other'.
            else if (e.ContentType == SharpClipboard.ContentTypes.Other)
            {
                // Do something with 'clipboard.ClipboardObject' or 'e.Content' here...
            }
        }
    }
}
