using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HMNGasApp.Helpers
{
    public class CameraResultMessage
    {
        public static string Key = "CRM";

        public List<Stream> Digits { get; set; }

        public Stream Image { get; set; }
    }
}
