using HMNGasApp.WebServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMNGasApp.Services
{
    public interface ICustomerSoapService
    {
        Task<(bool, Customer)> GetCustomerAsync();

        Task<bool> EditCustomerAsync(Customer Customer);
    }
}
