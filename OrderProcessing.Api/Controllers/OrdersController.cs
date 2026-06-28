using Microsoft.AspNetCore.Mvc;
using OrderProcessing.DataStore;
using OrderProcessing.Domain.Entities;

namespace OrderProcessing.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IRepository _repository;

    public OrdersController(IRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Order>> Get()
    {
        return Ok(_repository.GetAllOrders());
    }

    [HttpGet("{id:int}")]
    public ActionResult<Order> Get(int id)
    {
        var order = _repository.GetAllOrders().FirstOrDefault(o => o.OrderId == id);
        return order is not null ? Ok(order) : NotFound();
    }
}
