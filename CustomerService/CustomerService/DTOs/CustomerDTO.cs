using CustomerService.Models;

namespace CustomerService.DTOs;

public class CustomerDTO
{
    public int Id { get; set; } //primary key
    public string Email { get; set; }
    public  string Password { get; set; }
    public PaymentInfoDTO PaymentInfoDTO { get; set; }
    public AddressDTO AddressDTO { get; set; }


    public CustomerDTO(int id, string email, string password, PaymentInfoDTO paymentInfoDto, AddressDTO addressDto)
    {
        Id = id;
        Email = email;
        Password = password;
        PaymentInfoDTO = paymentInfoDto;
        AddressDTO = addressDto;
    }

    public CustomerDTO()
    {
    }

    public CustomerDTO(Customer customer)
    {
        this.Id = customer.Id;
        this.Email = customer.Email;
        this.Password = customer.Password;
        this.PaymentInfoDTO = new PaymentInfoDTO(customer.PaymentInfo);
        this.AddressDTO = new AddressDTO(customer.Address);
    }
}