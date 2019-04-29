using Tesseract;

namespace HMNGasApp.Services
{
    /// <summary>
    /// Class for handling platform specific dependency
    /// </summary>
    public interface ITesseract
    {
        ITesseractApi TesseractApi { get; }
    }
}
