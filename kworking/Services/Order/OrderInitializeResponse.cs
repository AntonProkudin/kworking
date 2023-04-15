namespace kworking.Services.Order;

public class OrderInitializeResponse : BaseResponse
{
    public IEnumerable<Model.Order> Orders { get; set; }
}