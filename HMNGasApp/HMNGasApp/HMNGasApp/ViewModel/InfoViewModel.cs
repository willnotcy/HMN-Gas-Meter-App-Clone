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
        public ICommand EditModeCommand { get; set; }
        public ICommand ReturnNavCommand { get; set; }
        public ICommand SettingsPageNavCommand { get; set; }
        public ICommand SaveInfoCommand { get; set; }
        public bool ButtonVisibility = false;
        public Color textColor = (Color) Application.Current.Resources["PrimaryOrange"];

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

        private Color _textColor;
        public Color TextColor
        {
            get => _textColor;
            set => SetProperty(ref _textColor, value);
        }

        private bool _editEnabled;
        public bool EditEnabled
        {
            get => _editEnabled;
            set => SetProperty(ref _editEnabled, value);
        }


        #endregion

        public InfoViewModel(ICustomerSoapService service)
        {
            _service = service;
            LoadCommand = new Command(() => ExecuteLoadCommand());
            ReturnNavCommand = new Command(async () => await ExecuteReturnNavCommand());
            EditModeCommand = new Command(() => ExecuteEditModeCommand());
            SaveInfoCommand = new Command(async () => await ExecuteSaveInfoCommand());
            EditEnabled = false;
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

            Readonly = true;
            EditEnabled = false;

            IsBusy = false;
        }

        private void ExecuteEditModeCommand()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            EditEnabled = true;

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
