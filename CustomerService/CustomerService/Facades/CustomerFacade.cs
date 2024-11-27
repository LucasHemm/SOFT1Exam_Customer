using CustomerService.DTOs;
using CustomerService.Models;
using Microsoft.EntityFrameworkCore;

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
    
    public Customer GetCustomer(int id)
    {
        Console.WriteLine(id+"#######################");
        Customer customer = _context.Customers
            .Include(customer => customer.Address)
            .Include(customer => customer.PaymentInfo)
            .FirstOrDefault(customer => customer.Id == id);
        if (customer == null)
        {
            Console.WriteLine("#######################NOT FOUND");
            throw new Exception("customer not found");
        }

        Console.WriteLine(customer.Address.City + "#######################");
        return customer;
    }
    
    public Customer UpdateCustomer(CustomerDTO customerDto)
    {
        Customer customer = GetCustomer(customerDto.Id);
        customer.Email = customerDto.Email;
        UpdateAddress(customerDto.AddressDTO,customer);
        UpdatePaymentInfo(customerDto.PaymentInfoDTO,customer);
        _context.SaveChanges(); 
        return customer;
    }
    
    private void UpdateAddress(AddressDTO addressDto,Customer customer)
    {
        customer.Address.Street = string.IsNullOrEmpty(addressDto.Street) ? customer.Address.Street : addressDto.Street;
        customer.Address.City = addressDto.City;
        customer.Address.ZipCode = addressDto.ZipCode;
    }
    
    private void UpdatePaymentInfo(PaymentInfoDTO paymentInfoDto,Customer customer)
    {
        customer.PaymentInfo.CardNumber =  string.IsNullOrEmpty(paymentInfoDto.CardNumber) ? customer.PaymentInfo.CardNumber : paymentInfoDto.CardNumber;
        customer.PaymentInfo.ExpirationDate = string.IsNullOrEmpty(paymentInfoDto.ExpirationDate) ? customer.PaymentInfo.ExpirationDate : paymentInfoDto.ExpirationDate; 
    }

    public List<Customer> GetAllCustomers()
    {
        return _context.Customers.Include(customer => customer.Address)
            .Include(customer => customer.PaymentInfo).ToList();
    }
    
  
    
    
    
    
    
}