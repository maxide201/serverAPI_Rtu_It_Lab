using System.Collections.Generic;

namespace ServerModels
{
    public class ShopDTO
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public IList<ProductDTO> Products { get; set; }
    }
}
