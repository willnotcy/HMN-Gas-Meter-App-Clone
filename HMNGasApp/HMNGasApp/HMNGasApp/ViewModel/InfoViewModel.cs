using System.Windows.Input;
using Xamarin.Forms;
using HMNGasApp.Model;
using HMNGasApp.Services;
using System.Threading.Tasks;


namespace HMNGasApp.ViewModel
{
    public class InfoViewModel : BaseViewModel
    {
        private readonly ICustomerSoapService _service;

        public ICommand LoadCommand { get; set; }
        public ICommand EditModeNameCommand { get; set; }
        public ICommand EditModeEmailCommand { get; set; }
        public ICommand EditModePhoneCommand { get; set; }
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

        private bool _readonly;
        public bool Readonly
        {
            get => _readonly;
            set => SetProperty(ref _readonly, value);
        }

        private bool _editEnabledName;
        public bool EditEnabledName
        {
            get => _editEnabledName;
            set => SetProperty(ref _editEnabledName, value);
        }

        private bool _editEnabledEmail;
        public bool EditEnabledEmail
        {
            get => _editEnabledEmail;
            set => SetProperty(ref _editEnabledEmail, value);
        }

        private bool _editEnabledPhone;
        public bool EditEnabledPhone
        {
            get => _editEnabledPhone;
            set => SetProperty(ref _editEnabledPhone, value);
        }


        #endregion

        public InfoViewModel(ICustomerSoapService service)
        {
            _service = service;
            LoadCommand = new Command(() => ExecuteLoadCommand());
            ReturnNavCommand = new Command(async () => await ExecuteReturnNavCommand());
            EditModeNameCommand = new Command(() => ExecuteEditModeNameCommand());
            EditModeEmailCommand = new Command(() => ExecuteEditModeEmailCommand());
            EditModePhoneCommand = new Command(() => ExecuteEditModePhoneCommand());
            SaveInfoCommand = new Command(async () => await ExecuteSaveInfoCommand());
            EditEnabledName = false;
            EditEnabledEmail = false;
            EditEnabledPhone = false;
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
            } else
            {
                //TODO Get text from languagefile
                await App.Current.MainPage.DisplayAlert("Fejl", "Noget gik galt, dine oplysninger blev ikke opdateret", "Okay");
            }

            Readonly = true;
            EditEnabledName = false;
            EditEnabledEmail = false;
            EditEnabledPhone = false;

            IsBusy = false;
        }

        private void ExecuteEditModeNameCommand()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            EditEnabledName = true;

            Readonly = false;

            IsBusy = false;
        }
        private void ExecuteEditModeEmailCommand()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            EditEnabledEmail = true;

            Readonly = false;

            IsBusy = false;
        }
        private void ExecuteEditModePhoneCommand()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            EditEnabledPhone = true;

            Readonly = false;

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

            EditEnabledName = false;
            EditEnabledEmail = false;
            EditEnabledPhone = false;

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
            MeterNum = "1234567";
            LatestMeasure = "4025,345 m3";
            MeasureDate = "01-02-19";
        }
    }
}
