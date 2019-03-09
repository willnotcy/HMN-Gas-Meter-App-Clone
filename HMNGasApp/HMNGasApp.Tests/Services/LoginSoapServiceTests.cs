using HMNGasApp.Model;
using HMNGasApp.Services;
using HMNGasApp.WebServices;
using Moq;
using Moq.Protected;
using System.Threading.Tasks;
using Xunit;

namespace HMNGasApp.Tests.Services
{
    public class LoginSoapServiceTests
    {
        [Fact]
        public async Task NewLogin_given_valid_credentials_returns_true_and_securitykey()
        {
            var service = new Mock<IXellentAPI>();
            var config = new Mock<IConfig>();
            service.Setup(s => s.newLogin(It.IsAny<NewLoginRequest>())).
                    Returns(new newLoginResponse { ErrorCode = "", ResponseMessage = "securitykey", ResponseCode = ""});

            var api = new LoginSoapService(service.Object, config.Object);

            var result = await api.NewLogin("73", "credentials");

            Assert.True(result.Item1);
            Assert.Equal("securitykey", result.Item2);
        }

        [Fact]
        public async Task NewLogin_given_invalid_credentials_returns_false_and_errorMessage()
        {
            var service = new Mock<IXellentAPI>();
            var config = new Mock<IConfig>();
            service.Setup(s => s.newLogin(It.IsAny<NewLoginRequest>())).
                    Returns(new newLoginResponse { ErrorCode = "4", ResponseMessage = "", ResponseCode = "Not Ok" });

            var api = new LoginSoapService(service.Object, config.Object);

            var result = await api.NewLogin("73", "team pull");

            Assert.False(result.Item1);
            Assert.Equal("Not Ok", result.Item2);
        }
    }
}
