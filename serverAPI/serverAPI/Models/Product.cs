using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace serverAPI.Models
{
    public class Product
    {
        public uint Id { get; set; }
        public uint ShopId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public double Cost { get; set; }
        public uint Count { get; set; }
    }
}
