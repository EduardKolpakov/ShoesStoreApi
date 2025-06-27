namespace ShoesStoreApi.Request
{
    public class CartItemRequest
    {
        public int ShoesId { get; set; }
        public string Size { get; set; }
        public int Quantity { get; set; }
    }
}

