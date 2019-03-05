using System.Threading.Tasks;

namespace HMNGasApp.Services
{
    public interface ILoginSoapService
    {
        Task<(bool, string)> NewLogin(string customerId, string password);
    }
}
