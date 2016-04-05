using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Security.Credentials;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace DownloadWithProgress.Model
{
    public static class Download
    {
        //public static IAsyncOperationWithProgress<DownloadFileInfo, DownloadProgressChangedEventArgs>
        //    GetFileWithProgressAsync(this HttpClient httpClient, HttpRequestMessage request,
        //    CancellationToken cancellationToken, DownloadFileInfo downloadInfo)
        //{
        //    var operation = httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        //    return AsyncInfo.Run<DownloadFileInfo, DownloadProgressChangedEventArgs>( async (token, progress) =>
        //    {
        //        if (cancellationToken != CancellationToken.None)
        //            token = cancellationToken;

        //        cancellationToken.ThrowIfCancellationRequested();

        //        return await GetStringTaskProviderAsync(operation, cancellationToken, progress,  downloadInfo);
        //    });
        //}

        //public static async Task<DownloadFileInfo>
        //    GetStringTaskProviderAsync(Task<HttpResponseMessage> httpOperation, CancellationToken cancellationToken,
        //    IProgress<DownloadProgressChangedEventArgs> progressCallback, DownloadFileInfo downloadInfo)
        //{
        //    int bytesToReceive = 0;
        //    int bytesReceived = 0;
        //    double progressPercentage = 0;

        //    var bufferRead = new byte[0];
        //    var responseBuffer = new byte[100];
        //    var httpInitialResponse = await httpOperation;
        //    using (var responseStream = await httpInitialResponse.Content.ReadAsStreamAsync())
        //    {
        //        bytesToReceive = int.Parse(httpInitialResponse.Content.Headers.First(h => h.Key.Equals("Content-Length")).Value.First());

        //        int read;
        //        do
        //        {
        //            read = await responseStream.ReadAsync(responseBuffer, 0, responseBuffer.Length);
        //            bytesReceived += read;

        //            Array.Resize(ref bufferRead, bytesReceived);
        //            bufferRead.Concat(responseBuffer);

        //            if (bytesToReceive > 0)
        //                progressPercentage = ((double)bytesReceived / (double)bytesToReceive) * 100;

        //            progressCallback.Report(new DownloadProgressChangedEventArgs(bytesReceived, bytesToReceive, (int)progressPercentage));

        //        } while (read != 0);

        //    }
        //    return downloadInfo;
        //}

        private static Queue<DownloadFileInfo> queueOfDownloads { get; set; } = new Queue<DownloadFileInfo>();

        public static void EnqueueDownload(DownloadFileInfo downloadInfo)
        {
            queueOfDownloads.Enqueue(downloadInfo);

            Task.Run(() => StartDownloadAsync());
        }

        private static async Task StartDownloadAsync()
        {
            if (queueOfDownloads.Count == 0)
                return;

            //HttpBaseProtocolFilter baseFilter = new HttpBaseProtocolFilter();
            //baseFilter.CacheControl.WriteBehavior = HttpCacheWriteBehavior.NoCache;

            using (var httpClient = new HttpClient())
            {
                do
                {
                    try
                    {
                        var download = queueOfDownloads.Peek();

                        var progressTask = httpClient.GetAsync(download.AbsoluteUri);

                        progressTask.Progress = (result, progress) =>
                        {
                            double progressPercentage = 0;
                            if (progress.TotalBytesToReceive > 0)
                                progressPercentage = ((double)progress.BytesReceived / (double)progress.TotalBytesToReceive) * 100;

                            DownloadProgressChangedEventArgs args = new DownloadProgressChangedEventArgs(progress.BytesReceived,
                                progress.TotalBytesToReceive, progressPercentage);

                            download.OnDownloadProgressChanged(args);
                        };

                        var downloadResult = await progressTask;

                        if (downloadResult.IsSuccessStatusCode)
                        {
                            var ccc = await downloadResult.Content.ReadAsBufferAsync();

                            download.OnDownloadCompleted(new DownloadCompletedEventArgs());
                        }
                        else
                            download.OnDonwloadError(new DownloadErrorEventArgs());

                        queueOfDownloads.Dequeue();
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }

                } while (queueOfDownloads.Count > 0);
            }
        }
    }
}
