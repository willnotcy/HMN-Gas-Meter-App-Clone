using System;
using System.Collections.Generic;
using System.Text;
using Tesseract;

namespace HMNGasApp.Services
{
    public interface ITesseract
    {
        ITesseractApi TesseractApi { get; }
    }
}
