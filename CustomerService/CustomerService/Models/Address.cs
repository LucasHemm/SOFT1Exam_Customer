using CustomerService.DTOs;

namespace CustomerService.Models;

public class Address
{
    public int Id { get; set; } //primary key
    public String Street { get; set; }
    public String City { get; set; }
    public String ZipCode { get; set; }

    public Address(int id, string street, string city, string zipCode)
    {
        Id = id;
        Street = street;
        City = city;
        ZipCode = zipCode;
    }


    public Address()
    {
    }
    
    public Address(AddressDTO addressDto)
    {
        this.Id = addressDto.Id;
        this.Street = addressDto.Street;
        this.City = addressDto.City;
        this.ZipCode = addressDto.ZipCode;
    }
}