using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace serverAPI.Models
{
    public class SuperAdminRequest
    {
        public string RootPassword { get; set; }
        public Shop Shop { get; set; }
    }
}
