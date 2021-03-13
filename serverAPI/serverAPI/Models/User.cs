using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace serverAPI.Models
{
    public class User
    {
        [Key]
        public uint Id { get; set; }
        public IList<Purchase> Purchases { get; set; }
    }
}
