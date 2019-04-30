using HMNGasApp.Model;
using HMNGasApp.Services;
using HMNGasApp.WebServices;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace HMNGasApp.Tests.Services
{
    public class ConnectServiceTests
    {
        [Fact]
        public async Task ConnectService_canConnect_returns_true_given_HNG()
        {
            //Arrange
            var client = new Mock<IXellentAPI>();
            var config = new Config();

            //Act
            client.Setup(s => s.canConnect("HNG")).Returns(true);

            var connectService = new ConnectService(client.Object);

            var result = await connectService.CanConnect();
            
            //Assert
            Assert.True(result);
        }
        [Fact]
        public async Task ConnectService_canConnect_returns_false_given_no_connection()
        {
            //Arrange
            var client = new Mock<IXellentAPI>();
            var config = new Config();

            //Act
            client.Setup(s => s.canConnect("AOSDJASOIDJ")).Returns(false);

            var connectService = new ConnectService(client.Object);

            var result = await connectService.CanConnect();

            //Assert
            Assert.False(result);
        }
    }
}
