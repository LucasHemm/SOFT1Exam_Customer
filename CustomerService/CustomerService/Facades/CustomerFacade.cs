using CustomerService.DTOs;
using CustomerService.Models;

namespace CustomerService.Facades;

public class CustomerFacade
{
    private readonly ApplicationDbContext _context;
    
    public CustomerFacade(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public Customer CreateCustomer(CustomerDTO customerDto)
    {
        Address address = (from a in _context.Addresses where a.City == customerDto.AddressDTO.City && a.Street == customerDto.AddressDTO.Street && a.ZipCode == customerDto.AddressDTO.ZipCode select a).FirstOrDefault() ?? new Address(customerDto.AddressDTO);
        Customer customer = new Customer(customerDto, address);
        _context.Customers.Add(customer);
        _context.SaveChanges();
        return customer;
    }
    
    
    
    
    
    
}