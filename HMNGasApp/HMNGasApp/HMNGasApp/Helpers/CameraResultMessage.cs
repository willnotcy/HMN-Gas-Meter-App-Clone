﻿using System.Collections.Generic;
using System.IO;

namespace HMNGasApp.Helpers
{
    public class CameraResultMessage
    {
        public static string Key = "CRM";

        public List<Stream> Digits { get; set; }

        public List<Stream> DigitsClone { get; set; }

        public Stream Image { get; set; }
    }
}
