using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundDownloadSample.Model
{
    public class DownloadModel
    {
        public ObservableCollection<BackgroundDownloadFile> ListOfDownloaders =>
            new ObservableCollection<BackgroundDownloadFile>()
            {
                new BackgroundDownloadFile("https://www2.bancobrasil.com.br/aapf/imagens/versao.xml"),
                new BackgroundDownloadFile("https://www2.bancobrasil.com.br/aapf/imagens/dadosTela.xml"),
                new BackgroundDownloadFile("https://www2.bancobrasil.com.br/aapf/imagens/tipoCampo.xml"),
                new BackgroundDownloadFile("https://www2.bancobrasil.com.br/aapf/imagens/mascara.xml")
            };

        public async void StartAllDownloadsAsync()
        {
            foreach (var downloader in ListOfDownloaders)
            {
                await downloader.StartBackgroundDownloadAsync();
            }
        }
    }
}
