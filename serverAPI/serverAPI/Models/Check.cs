﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace serverAPI.Models
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
