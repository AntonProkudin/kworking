namespace kworking.Services.Order;

public class OrderAddRequest
{
    public int FromUserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }
}
