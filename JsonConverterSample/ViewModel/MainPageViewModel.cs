using JsonConverterSample.Commands;
using JsonConverterSample.Model;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.Storage;
using Windows.UI.Popups;

namespace JsonConverterSample.ViewModel
{
    public class MainPageViewModel : BaseModel
    {
        private ObservableCollection<Person> people;
        public ObservableCollection<Person> People
        {
            get { return this.people; }
            set { SetField(ref this.people, value); }
        }

        public MainPageViewModel()
        {
            this.People = new ObservableCollection<Person>();
            this.DeserializeCommand = new Command(DeserializeAsync);
            this.SerializeCommand = new Command(SerializeAsync);
        }

        public ICommand DeserializeCommand { get; set; }
        private async void DeserializeAsync()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateFormatString = "dd/MM/yyyy";

            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Json/People.json"));
            var json = await FileIO.ReadTextAsync(file);

            this.People = JsonConvert.DeserializeObject<ObservableCollection<Person>>(json, settings);
        }

        public ICommand SerializeCommand { get; set; }
        private async void SerializeAsync()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateFormatString = "dd/MM/yyyy";

            var json = JsonConvert.SerializeObject(People, Formatting.Indented, settings);

            MessageDialog dialog = new MessageDialog(json, "Serialized Json");
            await dialog.ShowAsync();
        }
    }
}
