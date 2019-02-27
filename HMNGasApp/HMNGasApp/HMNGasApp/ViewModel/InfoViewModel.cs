using System.Windows.Input;
using System.Threading.Tasks;
using Xamarin.Forms;
using HMNGasApp.Services;
using HMNGasApp.Model;
using System.Collections.Generic;

namespace HMNGasApp.ViewModel
{
    public class InfoViewModel : BaseViewModel
    {
        public ICommand LoadCommand { get; set; }
        public ICommand EditCommand { get; set; }

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

        public InfoViewModel()
        {
            //TODO: Title = "MINE OPLYSNINGER";

            LoadCommand = new Command(() => ExecuteLoadCommand());
            //EditCommand = new Command(() => ExecuteEditCommand());
        }

        private void ExecuteEditCommand()
        {
        }

        private void ExecuteLoadCommand()
        {
            //Hack
            var customer = new Customer { AccountNum = "1343545", Address = "Kongehaven 24", Email = "apal@itu.dk", Name = "Alexander Pálsson", Phone = "27501015", MeterNum = "HMN 16.20.649" };
            Init(customer);
        }

        public void Init(Customer c)
        {
            AccountNum = c.AccountNum;
            Name = c.Name;
            Address = c.Address;
            Email = c.Email;
            Phone = c.Phone;
        }
    }
}
