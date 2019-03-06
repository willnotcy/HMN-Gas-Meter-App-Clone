using System;
using System.Collections.Generic;
using System.Text;

namespace HMNGasApp.Model
{
    public class UserContext : IUserContext
    {
        public string Caller { get; set; }
        public string Company { get; set; }
        public string functionName { get; set; }
        public int Logg { get; set; }
        public int StartRows { get; set; }
        public int MaxRows { get; set; }
        public string securityKey { get; set; }
    }
}
