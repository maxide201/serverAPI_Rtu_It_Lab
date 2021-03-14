using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace serverAPI.Models
{
    public class Check
    {
        uint Id { get; set; }
        uint ShopId { get; set; }
        uint UserId { get; set; }
        DateTime PurchaseDate { get; set; }
        IList<Purchase> Purchases { get; set; }
    }
}
