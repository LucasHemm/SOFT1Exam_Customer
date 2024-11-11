using CustomerService.Models;

namespace CustomerService.DTOs;

public class AddressDTO
{
    public int Id { get; set; } //primary key
    public String Street { get; set; }
    public String City { get; set; }
    public String ZipCode { get; set; }

    public AddressDTO(int id, string street, string city, string zipCode)
    {
        Id = id;
        Street = street;
        City = city;
        ZipCode = zipCode;
    }

    public AddressDTO()
    {
    }
    public AddressDTO(Address address)
    {
        this.Id = address.Id;
        this.Street = address.Street;
        this.City = address.City;
        this.ZipCode = address.ZipCode;
    }
}