using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace HMNGasApp.Helpers
{
    public class SHA
    {
        public static string SHA1Encrypt(string stringToEncrypt)
        {
            SHA1 sha = SHA1.Create();
            byte[] strBytes = new ASCIIEncoding().GetBytes(stringToEncrypt);
            sha.ComputeHash(strBytes);
            return BitConverter.ToString(sha.Hash).Replace("-", "");
        }
    }
}
