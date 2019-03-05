namespace HMNGasApp.Model
{
    public interface ICustomer
    {
        string AccountNum { get; set; }
        string Address { get; set; }
        string Email { get; set; }
        string MeterNum { get; set; }
        string Name { get; set; }
        string Phone { get; set; }
    }
}