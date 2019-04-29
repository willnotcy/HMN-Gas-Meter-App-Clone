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
            var config = new Config { Context = new UserContext() };
            var meterService = new MeterReadingSoapService(client.Object, config);           
            client.Setup(s => s.newLogin(It.IsAny<NewLoginRequest>())).
                    Returns(new newLoginResponse { ErrorCode = "", ResponseMessage = "securitykey", ResponseCode = ""});
            client.Setup(s => s.getMeterReadings(It.IsAny<MeterReadingsRequest>())).Returns(new MeterReadingsResponse { MeterReadings = new[] { new MeterReading()} });
            connectService.Setup(s => s.CanConnect()).Returns(true);

            var service = new LoginSoapService(client.Object, connectService.Object, config, meterService);

            var result = await service.NewLoginAsync("73", "credentials");

            Assert.True(result.Item1);
            Assert.Equal("securitykey", result.Item2);
            Assert.NotNull(config.MeterReadings);
        }

        [Fact]
        public async Task NewLogin_given_invalid_credentials_returns_false_and_errorMessage()
        {
            var client = new Mock<IXellentAPI>();
            var connectService = new Mock<IConnectService>();
            var meterService = new Mock<IMeterReadingSoapService>();
            var config = new Config();
            client.Setup(s => s.newLogin(It.IsAny<NewLoginRequest>())).
                    Returns(new newLoginResponse { ErrorCode = "4", ResponseMessage = "", ResponseCode = "Not Ok" });
            connectService.Setup(s => s.CanConnect()).Returns(true);

            var service = new LoginSoapService(client.Object, connectService.Object, config, meterService.Object);

            var result = await service.NewLoginAsync("73", "team pull");

            Assert.False(result.Item1);
            Assert.Equal("Not Ok", result.Item2);
            Assert.Null(config.MeterReadings);
        }

        [Fact]
        public async Task NewLogin_no_connection_returns_false_and_errormessage()
        {
            var client = new Mock<IXellentAPI>();
            var connectService = new Mock<IConnectService>();
            var meterService = new Mock<IMeterReadingSoapService>();
            var config = new Config();
            connectService.Setup(s => s.CanConnect()).Returns(false);

            var service = new LoginSoapService(client.Object, connectService.Object, config, meterService.Object);

            var result = await service.NewLoginAsync("73", "team pull");

            Assert.False(result.Item1);
            Assert.Equal("Kunne ikke få forbindelse", result.Item2);
            Assert.Null(config.MeterReadings);
        }

        [Fact]
        public async Task Logout_if_successfull_returns_true()
        {
            var client = new Mock<IXellentAPI>();
            var connectService = new Mock<IConnectService>();
            var meterService = new Mock<IMeterReadingSoapService>();
            var config = new Config { Context = new UserContext { SecurityKey = "anfkasjnfajk" } };
            client.Setup(c => c.logout(It.IsAny<LogoutRequest>())).Returns(new LogoutResponse { ErrorCode = "0", ResponseCode = "Ok", ResponseMessage = "" });

            var service = new LoginSoapService(client.Object, connectService.Object, config, meterService.Object);

            var result = await service.Logout();

            Assert.True(result);
            Assert.Equal("", config.Context.SecurityKey);
        }

        [Fact]
        public async Task Logout_if_unsuccessfull_returns_false()
        {
            var client = new Mock<IXellentAPI>();
            var connectService = new Mock<IConnectService>();
            var meterService = new Mock<IMeterReadingSoapService>();
            var config = new Config { Context = new UserContext { SecurityKey = "anfkasjnfajk" } };
            client.Setup(c => c.logout(It.IsAny<LogoutRequest>())).Returns(new LogoutResponse { ErrorCode = "1000", ResponseCode = "Not ok", ResponseMessage = "" });
            var service = new LoginSoapService(client.Object, connectService.Object, config, meterService.Object);

            var result = await service.Logout();

            Assert.False(result);
            Assert.Equal("anfkasjnfajk", config.Context.SecurityKey);
        }
        [Fact]
        public void ConfigGetApiKey_set_test()
        {
            //Arrange
            var input = "Test123";
            var expected = "Test123";
            
            //Act
            var config = new Config {ApiKey=input};

            var result = config.ApiKey;
            
            //Assert
            Assert.Equal(expected, result);
        }
    }
}
