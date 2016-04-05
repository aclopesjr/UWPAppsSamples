using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;

namespace DownloadWithProgress.Model
{
    public class DownloadFileInfo : BaseModel
    {
        private Uri absoluteUri;
        public Uri AbsoluteUri
        {
            get { return absoluteUri; }
            set { SetField(ref absoluteUri, value); }
        }

        private ulong bytesReceived;
        public ulong BytesReceived
        {
            get { return bytesReceived; }
            set { SetField(ref bytesReceived, value); }
        }

        private ulong? bytesToReceive;
        public ulong? BytesToReceive
        {
            get { return bytesToReceive; }
            set { SetField(ref bytesToReceive, value); }
        }

        private double progress;
        public double Progress
        {
            get { return progress; }
            set { SetField(ref progress, value); }
        }

        public event EventHandler<DownloadProgressChangedEventArgs> DownloadProgressChanged;
        public void OnDownloadProgressChanged(DownloadProgressChangedEventArgs e)
        {
            if (DownloadProgressChanged != null)
                DownloadProgressChanged(this, e);
        }

        public event EventHandler<DownloadCompletedEventArgs> DownloadCompleted;
        public void OnDownloadCompleted(DownloadCompletedEventArgs e)
        {
            if (DownloadCompleted != null)
                DownloadCompleted(this, e);
        }

        public event EventHandler<DownloadErrorEventArgs> DonwloadError;
        public void OnDonwloadError(DownloadErrorEventArgs e)
        {
            if (DonwloadError != null)
                DonwloadError(this, e);
        }

        public DownloadFileInfo() { }

        public DownloadFileInfo(string absolutePath)
        {
            this.absoluteUri = new Uri(absolutePath);
        }

    }
}
