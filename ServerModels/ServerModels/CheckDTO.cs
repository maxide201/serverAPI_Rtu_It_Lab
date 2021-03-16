using System;
using System.Collections.Generic;

namespace ServerModels
{
    public class CheckDTO
    {
        public uint Id { get; set; }
        public string ShopName { get; set; }
        public string ShopAddress { get; set; }
        public string UserName { get; set; }
        public DateTime PurchaseDate { get; set; }
        public IList<PurchaseDTO> Purchases { get; set; }
    }
}
