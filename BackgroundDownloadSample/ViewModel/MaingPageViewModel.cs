using BackgroundDownloadSample.Commands;
using BackgroundDownloadSample.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BackgroundDownloadSample.ViewModel
{
    public class MaingPageViewModel
    {
        public DownloadModel DownloadModel => new DownloadModel();

        public ObservableCollection<BackgroundDownloadFile> ListOfDownloaders => DownloadModel.ListOfDownloaders;

        public ICommand StartAllDownloadsCommand => new Command(DownloadModel.StartAllDownloadsAsync);
    }
}
