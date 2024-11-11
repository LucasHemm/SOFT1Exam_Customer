using CustomerService.DTOs;

namespace CustomerService.Models;

public class Customer
{
    public int Id { get; set; } //primary key
    public string Email { get; set; }
    public PaymentInfo PaymentInfo { get; set; }
    public Address Address { get; set; }

    public Customer(int id, string email, PaymentInfo paymentInfo, Address address)
    {
        Id = id;
        Email = email;
        PaymentInfo = paymentInfo;
        Address = address;
    }

    public Customer()
    {
    }
    
    public Customer(CustomerDTO customerDto, Address address)
    {
        Id = customerDto.Id;
        Email = customerDto.Email;
        PaymentInfo = new PaymentInfo(customerDto.PaymentInfoDTO);
        Address = address;
    }
    
    
}