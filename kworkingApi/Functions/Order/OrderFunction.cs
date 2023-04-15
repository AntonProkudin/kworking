using kworkingApi.Entities;

namespace kworkingApi.Functions.Order;

public class OrderFunction : IOrderFunction
{
    KworkingContext _kworkingContext;
    IUserFunction _userFunction;
    public OrderFunction(KworkingContext kworkingContext, IUserFunction userFunction)
    {
        _kworkingContext = kworkingContext;
        _userFunction = userFunction;
    }

    public async Task<int> AddOrder(int fromUserId, string name, string description, string category, decimal price)
    {
        var entity = new TblOrder
        {
            FromUserId = fromUserId,
            Name = name,
            Description = description,
            Category = category,
            Price = price
        };

        _kworkingContext.TblOrders.Add(entity);
        var result = await _kworkingContext.SaveChangesAsync();
        return result;

    }
    public async Task<int> DeleteOrder(int OrderId)
    {
        TblOrder order = new TblOrder() { Id = OrderId };
        _kworkingContext.TblOrders.Attach(order);
        _kworkingContext.TblOrders.Remove(order);
        var result = await _kworkingContext.SaveChangesAsync();
        return result;

    }
    public async Task<IEnumerable<Order>> GetOrders(int fromUserId)
    {
        var entities = await _kworkingContext.TblOrders
            .Where(x => (x.FromUserId == fromUserId))
            .OrderBy(x => x.Category)
            .ToListAsync();

        return entities.Select(x => new Order
        {
            Id = x.Id,
            Name = x.Name,
            FromUserId = x.FromUserId,
            Description = x.Description,
            Category = x.Category,
            Price = x.Price,
        });
    }
    public async Task<IEnumerable<Order>> GetAllOrders()
    {
        var entities = await _kworkingContext.TblOrders.ToListAsync();

        return entities.Select(x => new Order
        {
            Id = x.Id,
            Name = x.Name,
            FromUserId = x.FromUserId,
            Description = x.Description,
            Category = x.Category,
            Price = x.Price,
        });
    }
}