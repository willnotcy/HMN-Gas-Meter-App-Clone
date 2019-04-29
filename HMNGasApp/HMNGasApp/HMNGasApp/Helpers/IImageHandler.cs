using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
namespace HMNGasApp.Helpers
{
    public interface IImageHandler
    {
        Stream ToGrayScale(Stream original);
    }
}