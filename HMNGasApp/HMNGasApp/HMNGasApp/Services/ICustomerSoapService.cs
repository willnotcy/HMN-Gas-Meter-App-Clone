using HMNGasApp.WebServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMNGasApp.Services
{
    public interface ICustomerSoapService
    {
        Task<Customer> GetCustomerAsync();

        Task<bool> EditCustomerAsync(Customer Customer);
    }
}
