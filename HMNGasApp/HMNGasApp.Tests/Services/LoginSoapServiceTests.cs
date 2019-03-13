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
            var client = new Mock<IXellentAPI>();
            var connectService = new Mock<IConnectService>();
            var config = new Mock<IConfig>();
            client.Setup(s => s.newLogin(It.IsAny<NewLoginRequest>())).
                    Returns(new newLoginResponse { ErrorCode = "", ResponseMessage = "securitykey", ResponseCode = ""});
            connectService.Setup(s => s.canConnect()).Returns(true);

            var service = new LoginSoapService(client.Object, connectService.Object, config.Object);

            var result = await service.NewLogin("73", "credentials");

            Assert.True(result.Item1);
            Assert.Equal("securitykey", result.Item2);
        }

        [Fact]
        public async Task NewLogin_given_invalid_credentials_returns_false_and_errorMessage()
        {
            var client = new Mock<IXellentAPI>();
            var connectService = new Mock<IConnectService>();
            var config = new Mock<IConfig>();
            client.Setup(s => s.newLogin(It.IsAny<NewLoginRequest>())).
                    Returns(new newLoginResponse { ErrorCode = "4", ResponseMessage = "", ResponseCode = "Not Ok" });
            connectService.Setup(s => s.canConnect()).Returns(true);

            var service = new LoginSoapService(client.Object, connectService.Object, config.Object);

            var result = await service.NewLogin("73", "team pull");

            Assert.False(result.Item1);
            Assert.Equal("Not Ok", result.Item2);
        }

        [Fact]
        public async Task NewLogin_no_connection_returns_false_and_errormessage()
        {
            var client = new Mock<IXellentAPI>();
            var connectService = new Mock<IConnectService>();
            var config = new Mock<IConfig>();
            connectService.Setup(s => s.canConnect()).Returns(false);

            var service = new LoginSoapService(client.Object, connectService.Object, config.Object);

            var result = await service.NewLogin("73", "team pull");

            Assert.False(result.Item1);
            Assert.Equal("Kunne ikke få forbindelse", result.Item2);
        }
    }
}
