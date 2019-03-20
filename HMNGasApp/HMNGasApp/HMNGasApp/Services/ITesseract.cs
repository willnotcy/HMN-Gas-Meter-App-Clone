using System;
using System.Collections.Generic;
using System.Text;
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
