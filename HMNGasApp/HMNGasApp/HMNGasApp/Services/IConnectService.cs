using System.Threading.Tasks;

namespace HMNGasApp.Services
{
    public interface IConnectService
    {
        Task<bool> CanConnect();
    }
}
