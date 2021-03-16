using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace serverAPI.Models
{
    public class PurchaseRequest
    {
        public uint UserId { get; set; }
        public uint ShopId { get; set; }
        public string PaymentMethod { get; set; }
        public List<ProductDTO> Products { get; set; }
    }
}
