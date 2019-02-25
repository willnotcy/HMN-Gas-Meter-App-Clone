using System.ComponentModel.DataAnnotations;

namespace HMNGasApp.Model
{
    public class Customer
    {
        [Key]
        public string AccountNum { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public Customer()
        {

        }
    }
}
