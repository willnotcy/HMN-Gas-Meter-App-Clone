using HMNGasApp.WebServices;

namespace HMNGasApp.Services
{
    public interface IMeterReadingSoapService
    {
        (bool, Installation) GetInstallations();
        (bool, MeterReadingOrder) GetMeterReadingOrder(Installation installation, MeterReadingOrder active);
        (bool, string) NewMeterReading(string reading);
    }
}