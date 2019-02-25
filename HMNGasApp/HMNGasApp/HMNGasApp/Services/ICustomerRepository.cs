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
        Customer Find(int id);
        IEnumerable<Customer> Read();
        bool Update(Customer userAccount);
    }
}
