using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using HMNGasApp.WebServices;
using HMNGasApp.Model;
using HMNGasApp.Services;

namespace HMNGasApp.Tests.Services
{
    public class MeterReadingSoapServiceTests
    {
        [Fact]
        public void GetInstallations_given_valid_login_and_customerId_returns_true_and_Installation()
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

            var result = service.GetInstallations();

            Assert.True(result.Item1);
            Assert.NotNull(result.Item2);
        }

        [Fact]
        public void GetMeterReadingOrder_given_valid_installation_returns_true_and_MeterReadingOrderResponse()
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
           
            var result = service.GetMeterReadingOrder(installation, activeReading);

            Assert.True(result.Item1);
            Assert.NotNull(result.Item2);
        }

        [Fact]
        public void NewMeterReading_given_valid_request_returns_true_and_ResponseMessage()
        {
            var client = new Mock<IXellentAPI>();
            var config = new Mock<IConfig>();
            client.Setup(s => s.newMeterReading(It.IsAny<NewMeterReadingRequest>()))
                .Returns(new newMeterReadingResponse() { ErrorCode = "", ResponseCode = "", ResponseMessage = ""});

            var active = new MeterReadingOrder();
            var list = new List<MeterReadingOrder>()
            {
                active
            };
            client.Setup(s => s.getactiveMeterReadings(It.IsAny<ActiveMeterReadingRequest>()))
                .Returns(new MeterReadingOrderResponse() { MeterReadingOrders = list.ToArray()});

            var installation = new Installation();
            var installationList = new List<Installation>
            {
                installation
            };
            client.Setup(s => s.getInstallations(It.IsAny<InstallationRequest>()))
                .Returns(new InstallationResponse() { Installations = installationList.ToArray()});

            var service = new MeterReadingSoapService(client.Object, config.Object);

            var result = service.NewMeterReading("7373");

            Assert.True(result.Item1);
            Assert.NotNull(result.Item2);
        }

        [Fact]
        public void GetActiveMeterReading_returns_active_readings()
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

            var result = service.GetActiveMeterReadings();

            Assert.True(result.Item1);
            Assert.NotNull(result.Item2);
        }
    }
}
