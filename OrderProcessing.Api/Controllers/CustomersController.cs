using Microsoft.AspNetCore.Mvc;
using OrderProcessing.DataStore;
using OrderProcessing.DiscountService;
using OrderProcessing.Domain.Entities;

namespace OrderProcessing.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IRepository _repository;
    private readonly DiscountCalculator _discountCalculator;

    public CustomersController(IRepository repository, DiscountCalculator discountCalculator)
    {
        _repository = repository;
        _discountCalculator = discountCalculator;
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

    [HttpPatch("{id:int}/ApplyDiscount")]
    public ActionResult<Customer> ApplyDiscount(int id)
    {
        var customer = _repository.GetCustomerById(id);

        if (customer is null)
        {
            return NotFound();
        }

        _discountCalculator.ApplyDiscountsToCustomer(customer);

        return Ok(customer);
    }
}
