namespace kworkingApi.Functions.Order;

public interface IOrderFunction
{
    Task<int> DeleteOrder(int Orderid);
    Task<int> AddOrder(int fromUserId, string name, string description, string category, decimal price);
    Task<IEnumerable<Order>> GetOrders(int fromUserId);

    Task<IEnumerable<Order>> GetAllOrders();
}
