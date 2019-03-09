using HMNGasApp.Services;
using Moq;
using Moq.Protected;
using System.Threading.Tasks;
using Xunit;

namespace HMNGasApp.Tests.Services
{
    public class LoginSoapServiceTests
    {
        [Fact]
        public async Task NewLogin_given_valid_credentials_returns_true_android()
        {
            var api = new Mock<ILoginSoapService>();

            api.Setup(s => s.NewLogin("73", "credentials")).ReturnsAsync((true, "securitykey"));

            var result = await api.Object.NewLogin("73", "credentials");
            
            //var 


            Assert.True(result.Item1);
            Assert.Equal("securitykey", result.Item2);
        }
    }
}
