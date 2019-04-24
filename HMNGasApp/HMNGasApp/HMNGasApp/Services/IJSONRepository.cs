using System.Threading.Tasks;

namespace HMNGasApp.Services
{
    public interface IJSONRepository
    {
        Task<string> Read();
    }
}