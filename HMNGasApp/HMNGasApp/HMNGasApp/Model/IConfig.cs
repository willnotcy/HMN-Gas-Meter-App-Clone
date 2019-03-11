namespace HMNGasApp.Model
{
    public interface IConfig
    {
        string ApiKey { get; set; }

        string SecurityKey { get; set; }

        string CustomerId { get; set; }

        string Name { get; set; }
    }
}