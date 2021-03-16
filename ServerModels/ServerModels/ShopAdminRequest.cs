using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerModels
{
    public class ShopAdminRequest
    {
        public uint ShopId { get; set; }
        public string Password { get; set; }
    }
}
