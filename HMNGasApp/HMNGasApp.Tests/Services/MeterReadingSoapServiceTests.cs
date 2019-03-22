using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using HMNGasApp.WebServices;
using HMNGasApp.Model;
using HMNGasApp.Services;
using System.Threading.Tasks;

namespace HMNGasApp.Tests.Services
{
    public class MeterReadingSoapServiceTests
    {
        [Fact]
        public async Task GetInstallations_given_valid_login_and_customerId_returns_true_and_Installation()
        {
            var client = new Mock<IXellentAPI>();
            var config = new Mock<IConfig>();

            var installation = new Installation();
            var list = new List<Installation>
            {
                installation
            };

            client.Setup(s => s.getInstallations(It.IsAny<InstallationRequest>())).
                    Returns(new InstallationResponse() { Installations = list.ToArray()});

            var service = new MeterReadingSoapService(client.Object, config.Object);

            var result = await service.GetInstallationsAsync();

            Assert.True(result.Item1);
            Assert.NotNull(result.Item2);
        }

        [Fact]
        public async Task GetInstallations_given_soap_exception_returns_false()
        {
            var client = new Mock<IXellentAPI>();
            var config = new Mock<IConfig>();

            var installation = new Installation();
            var list = new List<Installation>
            {
                installation
            };

            client.Setup(s => s.getInstallations(It.IsAny<InstallationRequest>()))
                .Throws<Exception>();

            var service = new MeterReadingSoapService(client.Object, config.Object);

            var result = await service.GetInstallationsAsync();

            Assert.False(result.Item1);
            Assert.Null(result.Item2);
        }

        [Fact]
        public async Task GetMeterReadingOrder_given_valid_installation_returns_true_and_MeterReadingOrderResponse()
        {
            var client = new Mock<IXellentAPI>();
            var config = new Mock<IConfig>();

            var entry = new MeterReadingOrder();
            var list = new List<MeterReadingOrder>()
            {
                entry
            };

            client.Setup(s => s.getMeterReadingOrder(It.IsAny<MeterReadingOrderRequest>())).
                    Returns(new MeterReadingOrderResponse() { MeterReadingOrders = list.ToArray()});

            var service = new MeterReadingSoapService(client.Object, config.Object);
            var installation = new Installation();
            var activeReading = new MeterReadingOrder();

            var result = await service.GetMeterReadingOrderAsync(installation, activeReading);

            Assert.True(result.Item1);
            Assert.NotNull(result.Item2);
        }

        [Fact]
        public async Task GetMeterReadingOrder_given_soap_exception_returns_false()
        {
            var client = new Mock<IXellentAPI>();
            var config = new Mock<IConfig>();

            client.Setup(s => s.getMeterReadingOrder(It.IsAny<MeterReadingOrderRequest>()))
                .Throws<Exception>();

            var service = new MeterReadingSoapService(client.Object, config.Object);
            var installation = new Installation();
            var activeReading = new MeterReadingOrder();

            var result = await service.GetMeterReadingOrderAsync(installation, activeReading);

            Assert.False(result.Item1);
            Assert.Null(result.Item2);
        }

        [Fact]
        public async Task NewMeterReading_given_valid_request_returns_true_and_ResponseMessage()
        {
            var client = new Mock<IXellentAPI>();
            var config = new Mock<IConfig>();
            client.Setup(s => s.newMeterReading(It.IsAny<NewMeterReadingRequest>()))
                .Returns(new newMeterReadingResponse() { ErrorCode = "", ResponseCode = "", ResponseMessage = ""});

            var active = new MeterReadingOrder() { PrevReading = "7000"};
            var list = new List<MeterReadingOrder>()
            {
                active
            };
            client.Setup(s => s.getactiveMeterReadings(It.IsAny<ActiveMeterReadingRequest>()))
                .Returns(new MeterReadingOrderResponse() { MeterReadingOrders = list.ToArray()});

            var service = new MeterReadingSoapService(client.Object, config.Object);

            var result = await service.NewMeterReadingAsync("7373");

            Assert.True(result.Item1);
            Assert.NotNull(result.Item2);
        }

        [Fact]
        public async Task NewMeterReading_given_invalid_request_returns_false()
        {
            var client = new Mock<IXellentAPI>();
            var config = new Mock<IConfig>();
            client.Setup(s => s.newMeterReading(It.IsAny<NewMeterReadingRequest>()))
                .Returns(new newMeterReadingResponse() { ErrorCode = "4", ResponseCode = "invalid", ResponseMessage = "" });

            var active = new MeterReadingOrder() { PrevReading = "7000" };
            var list = new List<MeterReadingOrder>()
            {
                active
            };
            client.Setup(s => s.getactiveMeterReadings(It.IsAny<ActiveMeterReadingRequest>()))
                .Returns(new MeterReadingOrderResponse() { MeterReadingOrders = list.ToArray() });

            var service = new MeterReadingSoapService(client.Object, config.Object);

            var result = await service.NewMeterReadingAsync("7373");

            Assert.False(result.Item1);
            Assert.Equal("invalid", result.Item2);
        }

        [Fact]
        public async Task GetActiveMeterReading_returns_active_readings()
        {
            var client = new Mock<IXellentAPI>();
            var config = new Mock<IConfig>();

            var entry = new MeterReadingOrder();
            var list = new List<MeterReadingOrder>()
            {
                entry
            };

            client.Setup(s => s.getactiveMeterReadings(It.IsAny<ActiveMeterReadingRequest>())).
                    Returns(new MeterReadingOrderResponse() { MeterReadingOrders = list.ToArray() });

            var service = new MeterReadingSoapService(client.Object, config.Object);
 
            var activeReading = new MeterReadingOrder();

            var result = await service.GetActiveMeterReadingsAsync();

            Assert.True(result.Item1);
            Assert.NotNull(result.Item2);
        }

        [Fact]
        public async Task GetActiveMeterReading_given_no_active_readings_returns_false()
        {
            var client = new Mock<IXellentAPI>();
            var config = new Mock<IConfig>();

            var entry = new MeterReadingOrder();
            var list = new List<MeterReadingOrder>()
            {
                entry
            };

            client.Setup(s => s.getactiveMeterReadings(It.IsAny<ActiveMeterReadingRequest>()))
                .Throws<Exception>();

            var service = new MeterReadingSoapService(client.Object, config.Object);

            var activeReading = new MeterReadingOrder();

            var result = await service.GetActiveMeterReadingsAsync();

            Assert.False(result.Item1);
            Assert.Null(result.Item2);
        }

        [Fact]
        public async Task GetMeterReadings_given_valid_dates_returns_list_of_readings()
        {
            var client = new Mock<IXellentAPI>();
            var config = new Mock<IConfig>();
            client.Setup(s => s.getMeterReadings(It.IsAny<MeterReadingsRequest>())).Returns(new MeterReadingsResponse { MeterReadings = new[] { new MeterReading { } } });

            var service = new MeterReadingSoapService(client.Object, config.Object);

            var result = await service.GetMeterReadings(DateTime.Today.AddYears(-5), DateTime.Today);

            Assert.True(result.Item1);
            Assert.NotEmpty(result.Item2);
        }

        [Fact]
        public async Task GetMeterReadings_given_invalid_dates_returns_false()
        {
            var client = new Mock<IXellentAPI>();
            var config = new Mock<IConfig>();
            client.Setup(s => s.getMeterReadings(It.IsAny<MeterReadingsRequest>())).Returns(new MeterReadingsResponse { MeterReadings = new[] { new MeterReading { } } });

            var service = new MeterReadingSoapService(client.Object, config.Object);

            var result = await service.GetMeterReadings(DateTime.Today.AddYears(5), DateTime.Today);

            Assert.False(result.Item1);
            Assert.Null(result.Item2);
        }
    }
}
