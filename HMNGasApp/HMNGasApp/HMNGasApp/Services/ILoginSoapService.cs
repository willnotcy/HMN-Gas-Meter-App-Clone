using System.Threading.Tasks;

namespace HMNGasApp.Services
{
    public interface ILoginSoapService
    {
        Task NewLogin(string customerId, string password);
    }
}
