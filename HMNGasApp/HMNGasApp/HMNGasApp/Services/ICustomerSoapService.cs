using HMNGasApp.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMNGasApp.Services
{
    public interface ICustomerSoapService
    {
        Task<List<Customer>> GetCustomersAsync();
    }
}
