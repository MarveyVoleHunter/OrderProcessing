using Microsoft.AspNetCore.Mvc;
using OrderProcessing.DataStore;
using OrderProcessing.Domain.Entities;

namespace OrderProcessing.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IRepository _repository;

    public CustomersController(IRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Customer>> Get()
    {
        return Ok(_repository.GetAllCustomers());
    }

    [HttpGet("{id:int}")]
    public ActionResult<Customer> Get(int id)
    {
        var customer = _repository.GetCustomerById(id);
        return customer is not null ? Ok(customer) : NotFound();
    }
}
