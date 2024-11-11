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
    public IActionResult CreateRestaurant([FromBody] CustomerDTO customerDto)
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

}