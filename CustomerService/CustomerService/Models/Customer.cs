using CustomerService.DTOs;

namespace CustomerService.Models;

public class Customer
{
    public int Id { get; set; } //primary key
    public string Email { get; set; }
    public  string Password { get; set; }
    public PaymentInfo PaymentInfo { get; set; }
    public Address Address { get; set; }

    public Customer(int id, string email, string password, PaymentInfo paymentInfo, Address address)
    {
        Id = id;
        Email = email;
        Password = password;
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
        Password = customerDto.Password;
        PaymentInfo = new PaymentInfo(customerDto.PaymentInfoDTO);
        Address = address;
    }
    
    
}