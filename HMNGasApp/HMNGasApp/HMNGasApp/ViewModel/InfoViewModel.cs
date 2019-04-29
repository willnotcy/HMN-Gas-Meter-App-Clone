using System.Windows.Input;
using Xamarin.Forms;
using HMNGasApp.Model;
using HMNGasApp.Services;
using System.Threading.Tasks;
using HMNGasApp.WebServices;
using System.Linq;
using System.Text.RegularExpressions;

namespace HMNGasApp.ViewModel
{
    public class InfoViewModel : BaseViewModel
    {
        private readonly ICustomerSoapService _service;
        private readonly IConfig _config;

        public ICommand LoadCommand { get; set; }
        public ICommand EditModeNameCommand { get; set; }
        public ICommand EditModeEmailCommand { get; set; }
        public ICommand EditModePhoneCommand { get; set; }
        public ICommand SetFocusCommand { get; }
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

        private string _gsrn;
        public string GSRN
        {
            get => _gsrn;
            set => SetProperty(ref _gsrn, value);
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

        private bool _editName;
        public bool EditName
        {
            get => _editName;
            set => SetProperty(ref _editName, value);
        }


        #endregion

        public InfoViewModel(ICustomerSoapService service, IConfig config)
        {
            _service = service;
            _config = config;
            LoadCommand = new Command(() => ExecuteLoadCommand());
            ReturnNavCommand = new Command(async () => await ExecuteReturnNavCommand());
            EditModeNameCommand = new Command(() => ExecuteEditModeNameCommand());
            EditModeEmailCommand = new Command(() => ExecuteEditModeEmailCommand());
            EditModePhoneCommand = new Command(() => ExecuteEditModePhoneCommand());
            SaveInfoCommand = new Command(async () => await ExecuteSaveInfoCommand());
            EditEnabledName = false;
            EditEnabledEmail = false;
            EditEnabledPhone = false;
            EditName = false; //Disables the ability to change name
        }

        private async Task ExecuteSaveInfoCommand()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            //Check if any changes has been made, and if not - don't save
            if (Customer.Name != Name.Trim() || Customer.Phone != Phone.Trim() || Customer.Email != Email.Trim())
            {
                Customer.Name = Name.Trim();
                Customer.Phone = Phone.Trim();
                Customer.Email = Email.Trim();

                if (VerifyEmail(Customer.Email))
                {

                    var result = await _service.EditCustomerAsync(Customer);

                    if (result)
                    {
                        await App.Current.MainPage.DisplayAlert("Success", "Dine oplysninger blev opdateret!", "Okay");
                        Readonly = true;
                        EditEnabledName = false;
                        EditEnabledEmail = false;
                        EditEnabledPhone = false;
                    }
                    else
                    {
                        //TODO: Get text from languagefile
                        await App.Current.MainPage.DisplayAlert("Fejl", "Noget gik galt, dine oplysninger blev ikke opdateret", "Okay");
                    }

                } else 
                    {
                        await App.Current.MainPage.DisplayAlert("Fejl", "Ugyldig email.", "Okay");
                    }
            }


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
            EditEnabledEmail = false;
            EditEnabledPhone = false;

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

            EditEnabledName = false;
            EditEnabledEmail = true;
            EditEnabledPhone = false;

            Readonly = false;

            IsBusy = false;

        }

        private bool VerifyEmail(string email) 
        {
            var emailPattern = "^(?(\")(\".+?(?<!\\\\)\"@)|(([0-9a-z]((\\.(?!\\.))|[-!#\\$%&'\\*\\+/=\\?\\^`\\{\\}\\|~\\w])*)(?<=[0-9a-z])@))(?(\\[)(\\[(\\d{1,3}\\.){3}\\d{1,3}\\])|(([0-9a-z][-\\w]*[0-9a-z]*\\.)+[a-z0-9][\\-a-z0-9]{0,22}[a-z0-9]))$";
            if(Regex.IsMatch(email, emailPattern)) 
                {
                    return true;
                }
            return false;

        }


        private void ExecuteEditModePhoneCommand()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            EditEnabledName = false;
            EditEnabledEmail = false;
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

            var result = _service.GetCustomer();
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

            await Navigation.PopAsync();

            IsBusy = false;
        }

        public void Init(Customer c)
        {
            Customer = c;
            AccountNum = c.AccountNum;
            Name = c.Name;
            Address = c.Address;
            Email = c.Email;
            Phone = c.Phone;
            //TODO: Add GSRN
            GSRN = "6969696";

            var latestReading = _config.MeterReadings.LastOrDefault();
            if(latestReading != null)
            {
                MeterNum = latestReading.MeterNum;
                LatestMeasure = latestReading.Reading.TrimEnd('0',',') + " m\u00B3";
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
