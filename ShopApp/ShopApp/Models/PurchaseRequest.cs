using System.Collections.Generic;

namespace ShopApp.Models
{
    public class PurchaseRequest
    {
        public uint UserId { get; set; }
        public uint ShopId { get; set; }
        public string PaymentMethod { get; set; }
        public List<ProductDTO> Products { get; set; }
    }
}
