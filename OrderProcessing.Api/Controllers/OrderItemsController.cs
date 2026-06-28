using Microsoft.AspNetCore.Mvc;
using OrderProcessing.DataStore;
using OrderProcessing.Domain.Entities;

namespace OrderProcessing.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderItemsController : ControllerBase
{
    private readonly IRepository _repository;

    public OrderItemsController(IRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<OrderItem>> Get()
    {
        return Ok(_repository.GetAllOrderItems());
    }

    [HttpGet("order/{orderId:int}")]
    public ActionResult<IEnumerable<OrderItem>> GetByOrder(int orderId)
    {
        var orderItems = _repository.GetAllOrderItems().Where(i => i.OrderId == orderId);
        return Ok(orderItems);
    }
}
