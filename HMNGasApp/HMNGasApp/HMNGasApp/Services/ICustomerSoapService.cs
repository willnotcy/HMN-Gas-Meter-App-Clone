using HMNGasApp.WebServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMNGasApp.Services
{
    public interface ICustomerSoapService
    {
        Task<(bool, Customer)> GetCustomer();

        Task<bool> EditCustomerAsync(Customer Customer);
    }
}
