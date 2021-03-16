using System;

namespace ServerModels
{
    public class Check
    {
        public uint Id { get; set; }
        public uint ShopId { get; set; }
        public uint UserId { get; set; }
        public string ShopName { get; set; }
        public string ShopAddress { get; set; }
        public string UserName { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
