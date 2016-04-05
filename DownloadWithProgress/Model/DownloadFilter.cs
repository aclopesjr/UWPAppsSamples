using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace DownloadWithProgress.Model
{
    public class DownloadFilter : IHttpFilter
    {
        private IHttpFilter filter;

        public DownloadFilter(IHttpFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            this.filter = filter;
        }

        public void Dispose()
        {
            filter.Dispose();
        }

        public IAsyncOperationWithProgress<HttpResponseMessage, HttpProgress> SendRequestAsync(HttpRequestMessage request)
        {
            return AsyncInfo.Run<HttpResponseMessage, HttpProgress>(async (cancellationToken, progress) =>
            {
                var response = await filter.SendRequestAsync(request).AsTask(cancellationToken, progress);

                cancellationToken.ThrowIfCancellationRequested();

                return response;
            });
        }

        
    }
}
