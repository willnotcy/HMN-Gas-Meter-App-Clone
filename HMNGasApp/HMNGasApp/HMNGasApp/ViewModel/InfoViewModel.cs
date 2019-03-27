using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using HMNGasApp.Model;
using HMNGasApp.Services;
using System.Threading.Tasks;
using HMNGasApp.WebServices;

namespace HMNGasApp.ViewModel
{
    public class InfoViewModel : BaseViewModel
    {
        private readonly ICustomerSoapService _service;
        private readonly IConfig _config;

        public ICommand LoadCommand { get; set; }
        public ICommand EditMode { get; set; }
        public ICommand ReturnNavCommand { get; set; }
        public ICommand SettingsPageNavCommand { get; set; }
        public ICommand SaveInfoCommand { get; set; }
        public bool ButtonVisibility = false;

        #region Info
        private WebServices.Customer _customer;
        public WebServices.Customer Customer
        {
            get { return _customer; }
            set { _customer = value; }
        }

        private string _accountNum;
        public string AccountNum
        {
            get => _accountNum;
            set => SetProperty(ref _accountNum, value);
        }

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _phone;
        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value);
        }
        
        private string _address;
        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _meterNum;
        public string MeterNum
        {
            get => _meterNum;
            set => SetProperty(ref _meterNum, value);
        }

        private string _latestMeasure;
        public string LatestMeasure
        {
            get => _latestMeasure;
            set => SetProperty(ref _latestMeasure, value);
        }

        private string _measureDate;
        public string MeasureDate
        {
            get => _measureDate;
            set => SetProperty(ref _measureDate, value);
        }
        #endregion

        public InfoViewModel(ICustomerSoapService service, IConfig config)
        {
            _service = service;
            _config = config;
            LoadCommand = new Command(() => ExecuteLoadCommand());
            ReturnNavCommand = new Command(async () => await ExecuteReturnNavCommand());
            EditMode = new Command(() => ExecuteEditMode());
            SaveInfoCommand = new Command(async () => await ExecuteSaveInfoCommand());
        }

        private async Task ExecuteSaveInfoCommand()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;
            Customer.Name = Name;
            Customer.Phone = Phone;
            Customer.Email = Email;
            Customer.Address = Address;

            var result = await _service.EditCustomerAsync(Customer);

            if(result)
            {
                await App.Current.MainPage.DisplayAlert("Success", "Dine oplysninger blev opdateret!", "Okay");
                await Navigation.PopModalAsync();
            } else
            {
                //TODO Get text from languagefile
                await App.Current.MainPage.DisplayAlert("Fejl", "Noget gik galt, dine oplysninger blev ikke opdateret", "Okay");
            }

            IsBusy = false;
        }

        private void ExecuteEditMode()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            MessagingCenter.Send(this, "EnableEdit");

            IsBusy = false;
        }

        private async void ExecuteLoadCommand()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            var result = await _service.GetCustomerAsync();
            if (result.Item1)
            {
                Init(result.Item2);
            } else
            {
                await App.Current.MainPage.DisplayAlert("Fejl", "Noget gik galt, da vi skulle hente dine oplysninger", "Ok");
            }

            IsBusy = false;
        }

        private async Task ExecuteReturnNavCommand ()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            await Navigation.PopModalAsync();

            IsBusy = false;
        }

        public void Init(WebServices.Customer c)
        {
            Customer = c;
            AccountNum = c.AccountNum;
            Name = c.Name;
            Address = c.Address;
            Email = c.Email;
            Phone = c.Phone;

            var latestReading = _config.MeterReadings.LastOrDefault();
            if(latestReading != null)
            {
                MeterNum = latestReading.MeterNum;
                LatestMeasure = latestReading.Reading + " m3";
                MeasureDate = latestReading.ReadingDate;
            } else
            {
                MeterNum = "";
                LatestMeasure = "";
                MeasureDate = "";
            }
        }
    }
}
