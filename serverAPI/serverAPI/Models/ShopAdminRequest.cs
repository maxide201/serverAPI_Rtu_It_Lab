using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace serverAPI.Models
{
    public class ShopAdminRequest
    {
        public uint ShopId { get; set; }
        public string Password { get; set; }
    }
}
