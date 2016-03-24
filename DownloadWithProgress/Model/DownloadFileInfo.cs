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

        private int bytesReceived;
        public int BytesReceived
        {
            get { return bytesReceived; }
            set { SetField(ref bytesReceived, value); }
        }

        private int bytesToReceive;
        public int BytesToReceive
        {
            get { return bytesToReceive; }
            set { SetField(ref bytesToReceive, value); }
        }

        private int progress;
        public int Progress
        {
            get { return progress; }
            set { SetField(ref progress, value); }
        }

        public Action<CustomProgressChangedEventArgs> ProgressAction { get; set; }

        public DownloadFileInfo() { }

        public DownloadFileInfo(string absolutePath)
        {
            this.absoluteUri = new Uri(absolutePath);
            ProgressAction = DownloadProgress;
        }

        public async void DownloadProgress(CustomProgressChangedEventArgs e)
        {
            await CoreApplication.MainView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                this.BytesReceived = e.BytesReceived;
                this.BytesToReceive = e.BytesToReceive;
                this.Progress = e.ProgressPercentage;
            });
        }
    }
}
