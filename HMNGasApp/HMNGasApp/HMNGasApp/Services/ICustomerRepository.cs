using HMNGasApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMNGasApp.Services
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> ReadAsync();
        Task<bool> UpdateAsync(Customer customer);
    }
}
