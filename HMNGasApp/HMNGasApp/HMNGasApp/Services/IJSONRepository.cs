using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMNGasApp.Services
{
    public interface IJSONRepository
    {
        Task<Dictionary<string, string>> Read();
    }
}