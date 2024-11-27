using CustomerService.DTOs;
using CustomerService.Facades;
using CustomerService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.API;

[ApiController]
[Route("api/[controller]")]
public class CustomerApi : ControllerBase
{
    private readonly CustomerFacade _customerFacade;

    public CustomerApi(CustomerFacade customerFacade)
    {
        _customerFacade = customerFacade;
    }
    
    [HttpPost]
    public IActionResult CreateCustomer([FromBody] CustomerDTO customerDto)
    {
        try
        {
            return Ok(new CustomerDTO(_customerFacade.CreateCustomer(customerDto)));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    //Update customer
    [HttpPut]
    public IActionResult UpdateCustomer([FromBody] CustomerDTO customerDto)
    {
        try
        {
            return Ok(new CustomerDTO(_customerFacade.UpdateCustomer(customerDto)));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public IActionResult GetAllCustomers()
    {
        try
        {
            var customers = _customerFacade.GetAllCustomers();
            return Ok(customers);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    // GET: api/Customer
    [HttpGet("{id}")]
    public IActionResult GetCustomer(int id)
    {
        try
        {
            return Ok(new CustomerDTO(_customerFacade.GetCustomer(id)));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}