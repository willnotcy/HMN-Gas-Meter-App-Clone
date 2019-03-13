using HMNGasApp.WebServices;

namespace HMNGasApp.Services
{
    public interface IMeterReadingSoapService
    {
        (bool, Installation) GetInstallations();
        (bool, MeterReadingOrderResponse) GetMeterReadingOrder(Installation installation);
        (bool, string) NewMeterReading();
    }
}