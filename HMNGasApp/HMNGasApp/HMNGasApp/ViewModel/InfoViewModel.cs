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
        public ICommand EditCommand { get; set; }
        public ICommand ReturnNavCommand { get; set; }
        public ICommand SettingsPageNavCommand { get; set; }

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

        public InfoViewModel(ICustomerSoapService service)
        {
            _service = service;
            LoadCommand = new Command(() => ExecuteLoadCommand());
            ReturnNavCommand = new Command(async () => await ExecuteReturnNavCommand());
        }

        private void ExecuteEditCommand()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;


            IsBusy = false;
        }

        private void ExecuteLoadCommand()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            var customer = _service.GetCustomer();
            Init(customer);

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

        public void Init(Customer c)
        {
            AccountNum = c.AccountNum;
            Name = c.Name;
            Address = c.Address;
            Email = c.Email;
            Phone = c.Phone;
            MeterNum = c.MeterNum;
            LatestMeasure = "4025,345 m3";
            MeasureDate = "01-02-19";
        }
    }
}
