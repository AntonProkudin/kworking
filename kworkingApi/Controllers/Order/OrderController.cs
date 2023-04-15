using kworkingApi.Controllers.Message;
using Microsoft.AspNetCore.Authorization;
using AuthorizeAttribute = kworkingApi.Helpers.AuthorizeAttribute;

namespace kworkingApi.Controllers.Order;
[ApiController]
[Route("[controller]")]

public class OrderController : Controller
{
    IOrderFunction _orderFunction;
    IUserFunction _userFunction;

    public OrderController(IOrderFunction orderFunction, IUserFunction userFunction)
    {
        _orderFunction = orderFunction;
        _userFunction = userFunction;
    }

    [HttpPost("Initialize")]
    public async Task<ActionResult> Initialize([FromBody] int userId)
    {
        var response = new OrderInitalizeResponse
        {
            Orders = await _orderFunction.GetOrders(userId)
        };

        return Ok(response);
    }

    [HttpPost("All")]
    public async Task<ActionResult> GetAll()
    {
        var response = new OrderAllResponse
        {
            Orders = await _orderFunction.GetAllOrders()
        };
        return Ok(response);
    }

    [HttpPost("Add")]
    public async Task Add([FromBody] OrderAddRequest request)
    {
        await _orderFunction.AddOrder(request.FromUserId,request.Name,request.Description,request.Category,request.Price);
    }

   //   [HttpDelete("{id:int}")]
   //   public async Task<ActionResult> Delete([FromBody] int OrderId)
   //   {
   //     var response = new OrderAddResponse
   //     {
   //       StatusCode = await _orderFunction.DeleteOrder(OrderId)
   //     };
   //     return Ok(response);
   //   }
      [HttpPost("Delete")]
      public async Task<ActionResult> Delete([FromBody] int OrderId)
      {
        var response = new OrderAddResponse
        {
          StatusCode = await _orderFunction.DeleteOrder(OrderId)
        };
        return Ok(response);
      }
}