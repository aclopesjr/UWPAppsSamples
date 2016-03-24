using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;

namespace DownloadWithProgress.Model
{
    public static class Download
    {
        static Download()
        {
            queueOfDownloads.CollectionChanged += QueueOfDownloads_CollectionChanged;
        }

        public static IAsyncOperationWithProgress<string, CustomProgressChangedEventArgs> GetFileWithProgressAsync(this HttpClient httpClient,
            HttpRequestMessage request, CancellationToken cancellationToken, DownloadFileInfo downloadInfo)
        {
            var operation = httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            return AsyncInfo.Run<string, CustomProgressChangedEventArgs>( async (token, progress) =>
            {
                if (cancellationToken != CancellationToken.None)
                    token = cancellationToken;

                cancellationToken.ThrowIfCancellationRequested();

                return await GetStringTaskProviderAsync(operation, cancellationToken, progress, downloadInfo);
            });
        }

        public static async Task<string> GetStringTaskProviderAsync(Task<HttpResponseMessage> httpOperation,
            CancellationToken cancellationToken,
            IProgress<CustomProgressChangedEventArgs> progressCallback,
            DownloadFileInfo downloadInfo)
        {
            string result = string.Empty;

            int bytesToReceive = 0;
            int bytesReceived = 0;
            double progressPercentage = 0;

            var responseBuffer = new byte[100];
            var httpInitialResponse = await httpOperation;
            using (var responseStream = await httpInitialResponse.Content.ReadAsStreamAsync())
            {
                bytesToReceive = int.Parse(httpInitialResponse.Content.Headers.First(h => h.Key.Equals("Content-Length")).Value.First());

                int read;
                do
                {
                    read = await responseStream.ReadAsync(responseBuffer, 0, responseBuffer.Length);
                    result += Encoding.UTF8.GetString(responseBuffer, 0, read);
                    bytesReceived += read;

                    if (bytesToReceive > 0)
                        progressPercentage = ((double)bytesReceived / (double)bytesToReceive) * 100;

                    progressCallback.Report(new CustomProgressChangedEventArgs(bytesReceived, bytesToReceive, (int)progressPercentage));

                } while (read != 0);
            }
            return result;
        }

        private static ObservableCollection<DownloadFileInfo> queueOfDownloads { get; set; } = new ObservableCollection<DownloadFileInfo>();

        public static void EnqueueDownload(DownloadFileInfo downloadInfo)
        {
            queueOfDownloads.Add(downloadInfo);
        }

        private static void QueueOfDownloads_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action.Equals(NotifyCollectionChangedAction.Add))
            {
                Task.Run(() => StartDownloadAsync());
            }
        }

        private static async Task StartDownloadAsync()
        {
            if (queueOfDownloads.Count == 0)
                return;

            using (var httpClient = new HttpClient())
            {
                foreach (var download in queueOfDownloads)
                {
                    try
                    {
                        using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, download.AbsoluteUri))
                        {
                            var cancellationToken = new CancellationToken();
                            var operationWithProgress = httpClient.GetFileWithProgressAsync(request, cancellationToken, download);
                            operationWithProgress.Progress = (result, progress) =>
                            {
                                download.ProgressAction(progress);
                            };

                            await operationWithProgress;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                }
            }
        }
    }
}
