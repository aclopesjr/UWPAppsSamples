using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;

namespace BackgroundDownloadSample.Model
{
    public class BackgroundDownloadFile : BasePropertyChanged
    {
        public Uri UriPath { get; set; }

        public string FilePath 
        {
            get { return Path.GetFileName(UriPath.AbsolutePath); }
        }

        private ulong totalToReceive;
        public ulong TotalToReceive
        {
            get { return this.totalToReceive; }
            set { SetField(ref this.totalToReceive, value); }
        }

        private ulong received;
        public ulong Received
        {
            get { return this.received; }
            set { SetField(ref received, value); }
        }


        private double percentReceived;
        public double PercentReceived
        {
            get { return this.percentReceived; }
            set { SetField(ref percentReceived, value); }
        }

        public BackgroundDownloadFile(Uri uriPath)
        {
            this.UriPath = uriPath;
        }

        public BackgroundDownloadFile(string uriPath)
            : this( new Uri(uriPath))
        {
        }

        public async Task StartBackgroundDownloadAsync()
        {
            if (!this.UriPath.IsWellFormedOriginalString())
            {
                throw new UriFormatException($"Malformed uri for {this.UriPath}.");
            }

            var currentDowloads = await BackgroundDownloader.GetCurrentDownloadsAsync();

            foreach (var download in currentDowloads)
            {
                Debug.WriteLine($"Current Download: {download.RequestedUri}");
            }

            StorageFile fileDestination = await KnownFolders
                .PicturesLibrary
                .CreateFileAsync(this.FilePath, CreationCollisionOption.GenerateUniqueName);

            //using (IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication())
            //{
            //    using (IsolatedStorageFileStream fileStream =
            //        new IsolatedStorageFileStream(FilePath, FileMode.OpenOrCreate, isolatedStorageFile))
            //    {
            //        string path = fileStream.GetType()
            //            .GetField("m_FullPath", BindingFlags.Instance | BindingFlags.NonPublic)
            //            .GetValue(fileStream)
            //            .ToString();
            //        fileDestination = await StorageFile.GetFileFromPathAsync(path);
            //    }
            //}

            Debug.WriteLine($"Start: {this.UriPath}");

            BackgroundDownloader backgroundDownloader =
                new BackgroundDownloader();

            DownloadOperation downloadOperation = backgroundDownloader
                .CreateDownload(this.UriPath, fileDestination);

            Progress<DownloadOperation> progressCallback = new Progress<DownloadOperation>(DownloadProgress);

            await downloadOperation.StartAsync().AsTask(progressCallback);

            

            ResponseInformation response = downloadOperation.GetResponseInformation();

            Debug.WriteLine($"StatusCode: {response.StatusCode} - {downloadOperation.RequestedUri}");
        }

        private void DownloadProgress(DownloadOperation downloadOperation)
        {
            // DownloadOperation.Progress is updated in real-time while the operation is ongoing. Therefore,
            // we must make a local copy at the beginning of the progress handler, so that we can have a consistent
            // view of that ever-changing state throughout the handler's lifetime.
            BackgroundDownloadProgress currentProgress = downloadOperation.Progress;

            this.TotalToReceive = currentProgress.TotalBytesToReceive;
            this.Received = currentProgress.BytesReceived;
            //this.PercentReceived = this.Received / this.TotalToReceive * 100;

            Debug.WriteLine($"Progress: {currentProgress.BytesReceived} - {downloadOperation.RequestedUri}");
        }
    }
}
