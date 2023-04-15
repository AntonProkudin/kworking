namespace kworkingApi.Controllers.Order
{
    public class OrderAllResponse
    {
        public IEnumerable<Functions.Order.Order> Orders { get; set; } = null!;
    }
}
