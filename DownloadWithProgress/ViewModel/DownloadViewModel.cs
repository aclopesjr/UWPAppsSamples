using DownloadWithProgress.Model;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading;
using System.Windows.Input;
using Windows.ApplicationModel.Core;

namespace DownloadWithProgress.ViewModel
{
    public class DownloadViewModel
    {
        public ObservableCollection<DownloadFileInfo> ListOfDownload { get; set; }

        public DownloadViewModel()
        {
            this.ListOfDownload = new ObservableCollection<DownloadFileInfo>();
            this.ListOfDownload.Add(GetNewDownloadFileInfo("http://www.walldesk.com.br/fotos/13102_alemanha-gratis.jpg"));

            //this.ListOfDownload.Add(new DownloadFileInfo("https://www2.bancobrasil.com.br/aapf/imagens/dadosTela.xml"));
            //this.ListOfDownload.Add(new DownloadFileInfo("http://172.17.219.100:8080/mov-centralizador/icones/novoApp/extrato/extratoPai.png"));
            //this.ListOfDownload.Add(new DownloadFileInfo("http://172.17.219.100:8080/mov-centralizador/icones/novoApp/saldo/saldoPai.png"));
            //this.ListOfDownload.Add(new DownloadFileInfo("http://172.17.219.100:8080/mov-centralizador/icones/novoApp/cheque/chequePai.png"));
            //this.ListOfDownload.Add(new DownloadFileInfo("http://172.17.219.100:8080/mov-centralizador/icones/novoApp/telefonia_movel/celularPai.png"));
            //this.ListOfDownload.Add(new DownloadFileInfo("http://172.17.219.100:8080/mov-centralizador/icones/novoApp/pagamento/pagamentoPai.png"));
            //this.ListOfDownload.Add(new DownloadFileInfo("http://172.17.219.100:8080/mov-centralizador/icones/novoApp/previdencia/previdenciaPai.png"));
            //this.ListOfDownload.Add(new DownloadFileInfo("http://172.17.219.100:8080/mov-centralizador/icones/novoApp/cartao/cartaoPai.png"));
            //this.ListOfDownload.Add(new DownloadFileInfo("http://172.17.219.100:8080/mov-centralizador/icones/novoApp/consulta/2viaPai.png"));
            //this.ListOfDownload.Add(new DownloadFileInfo("http://172.17.219.100:8080/mov-centralizador/icones/novoApp/cartao/inibeFaturaPai.png"));
            //this.ListOfDownload.Add(new DownloadFileInfo("http://172.17.219.100:8080/mov-centralizador/icones/novoApp/transferencia/transferenciaPai.png"));

            this.StartDownloadCommand = new ProtocolCommand(StartDownload);
        }

        private DownloadFileInfo GetNewDownloadFileInfo(string url)
        {
            var download = new DownloadFileInfo(url);
            download.DownloadProgressChanged += DownloadProgressChanged;
            return download;
        }

        private async void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            await CoreApplication.MainView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                (sender as DownloadFileInfo).BytesReceived = e.BytesReceived;
                (sender as DownloadFileInfo).BytesToReceive = e.BytesToReceive;
                (sender as DownloadFileInfo).Progress = e.ProgressPercentage;
            });
        }

        public ICommand StartDownloadCommand { get; set; }
        private void StartDownload()
        {
            foreach (var item in ListOfDownload)
            {
                Download.EnqueueDownload(item);
            }
        }

        public void DownloadProgress(DownloadFileInfo downloadInfo, int progress)
        {

        }
        
    }
}
