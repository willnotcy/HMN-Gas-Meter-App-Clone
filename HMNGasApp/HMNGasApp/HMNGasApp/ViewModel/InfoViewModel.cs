using System.Windows.Input;
using System.Threading.Tasks;
using Xamarin.Forms;
using HMNGasApp.Services;
using HMNGasApp.Model;
using System.Collections.Generic;

namespace HMNGasApp.ViewModel
{
    public class InfoViewModel
    {
        public ICommand LoadCommand { get; set; }
        public ICommand EditCommand { get; set; }

        private readonly ICustomerRepository _customerRepository;

        private string _accountNum;
        public string AccountNum
        {
            get => _accountNum;
            set { _accountNum = value; }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set { _email = value; }
        }

        private string _phone;
        public string Phone
        {
            get => _phone;
            set { _phone = value; }
        }

        private string _address;
        public string Address
        {
            get => _address;
            set { _address = value; }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set { _name = value; }
        }

        public InfoViewModel(ICustomerRepository customerRepository)
        {
            //TODO: Title = "MINE OPLYSNINGER";

            LoadCommand = new Command(() => ExecuteLoadCommand());
            EditCommand = new Command(() => ExecuteEditCommand());
            _customerRepository = customerRepository;
        }

        private void ExecuteEditCommand()
        {
        }

        private void ExecuteLoadCommand()
        {
            //Hack
            var customer = _customerRepository.Find(1);
        }

        public void init(Customer c)
        {
            AccountNum = c.AccountNum;
            Name = c.Name;
            Address = c.Address;
            Email = c.Email;
            Phone = c.Phone;
        }
    }
}
