using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMNGasApp.Model;

namespace HMNGasApp.Services
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DummyContext _dummyContext;

        public CustomerRepository(DummyContext dummyContext)
        {
            _dummyContext = dummyContext;
        }

        public Customer Find(int id)
        {
            var entities = from c in _dummyContext.Customers
                           where c.AccountNum == id.ToString()
                           select new Customer
                           {
                               AccountNum = c.AccountNum,
                               Address = c.Address,
                               Email = c.Email,
                               Name = c.Name,
                               Phone = c.Phone
                           };
            return entities.FirstOrDefault();
        }

        public IEnumerable<Customer> Read()
        {
            var entities = from c in _dummyContext.Customers
                           select new Customer
                           {
                               AccountNum = c.AccountNum,
                               Address = c.Address,
                               Email = c.Email,
                               Name = c.Name,
                               Phone = c.Phone
                           };
            return entities;
        }

        public bool Update(Customer userAccount)
        {
            throw new NotImplementedException();
        }
    }
}
