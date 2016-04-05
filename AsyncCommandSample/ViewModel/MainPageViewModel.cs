using AsyncCommandSample.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AsyncCommandSample.ViewModel
{
    public class MainPageViewModel : BaseModel
    {
        public ObservableCollection<Model.Service> ListOfServices { get; set; }

        public MainPageViewModel()
        {
            ListOfServices = new ObservableCollection<Service>();
            ListOfServices.Add(new Service("http://www.microsoft.com"));
            ListOfServices.Add(new Service("http://www.msdn.com"));
            ListOfServices.Add(new Service("http://www.mva.com"));
        }
    }
}
