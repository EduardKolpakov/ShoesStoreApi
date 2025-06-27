using Microsoft.Identity.Client;

namespace ShoesStoreApi.Request
{
    public class NewShoesReq
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string Sizes { get; set; }
        public string ImageUrl { get; set; }
        public double Price { get; set; }
    }
}
