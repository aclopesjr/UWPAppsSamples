using AsyncCommandSample.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace AsyncCommandSample.Model
{
    public class Service : BaseModel
    {
        private string url;
        public string Url
        {
            get { return url; }
            set { SetField(ref this.url, value); }
        }

        private int bytesSize;
        public int BytesSize
        {
            get { return bytesSize; }
            set { SetField(ref this.bytesSize, value); }
        }

        public Service()
        {
            CountBytesCommand = new AsyncCommand(CountBytesAsync);
        }

        public Service(string url)
            : this()
        {
            this.Url = url;
        }

        public ICommand CountBytesCommand { get; set; }
        public async Task CountBytesAsync()
        {
            CancellationToken token = new CancellationToken();
            await Task.Delay(TimeSpan.FromSeconds(3), token).ConfigureAwait(false);
            var client = new HttpClient();
            using (var response = await client.GetAsync(url, token).ConfigureAwait(false))
            {
                var data = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    this.BytesSize = data.Length;
                });
            }
        }
    }
}
