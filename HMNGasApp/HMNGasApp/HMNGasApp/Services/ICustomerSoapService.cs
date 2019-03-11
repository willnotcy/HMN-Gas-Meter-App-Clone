using HMNGasApp.WebServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMNGasApp.Services
{
    public interface ICustomerSoapService
    {
        Customer GetCustomer();

        bool EditCustomer(Customer Customer);
    }
}
