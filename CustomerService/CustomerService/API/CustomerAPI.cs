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
            _customerFacade.CreateCustomer(customerDto);
            return Ok("Customer created successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPut]
    public IActionResult UpdateCustomer([FromBody] CustomerDTO customerDto)
    {
        try
        {
            _customerFacade.UpdateCustomer(customerDto);
            return Ok("Customer updated successfully");
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

}