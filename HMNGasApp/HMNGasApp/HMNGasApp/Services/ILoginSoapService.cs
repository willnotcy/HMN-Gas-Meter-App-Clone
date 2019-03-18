using System.Threading.Tasks;

namespace HMNGasApp.Services
{
    public interface ILoginSoapService
    {
        Task<(bool, string)> NewLoginAsync(string customerId, string password);
        Task<bool> Logout();
    }
}
