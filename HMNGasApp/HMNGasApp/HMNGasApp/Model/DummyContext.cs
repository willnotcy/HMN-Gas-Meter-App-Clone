using System;
using System.Collections.Generic;
using System.Text;

namespace HMNGasApp.Model
{
    public class DummyContext
    {
        public HashSet<Customer> Customers { get; set; }
        
        public DummyContext()
        {
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            for (int i = 0; i < 3; i++)
            {
                Customers.Add(new Customer()
                {
                    AccountNum = i.ToString(),
                    Email = "Vikb@itu.dk",
                    Address = "NørrebroGade " + i,
                    Name = "Viktor Funch Beck",
                    Phone = "27501015"
                });
            }
        }
    }
}
