using CustomerService.DTOs;

namespace CustomerService.Models;

public class PaymentInfo
{
    public int Id { get; set; } //primary key
    public String CardNumber { get; set; }
    public String ExpirationDate { get; set; }

    public PaymentInfo(int id, string cardNumber, string expirationDate)
    {
        Id = id;
        CardNumber = cardNumber;
        ExpirationDate = expirationDate;
    }

    public PaymentInfo()
    {
    }
    
    public PaymentInfo(PaymentInfoDTO paymentInfoDto)
    {
        this.Id = paymentInfoDto.Id;
        this.CardNumber = paymentInfoDto.CardNumber;
        this.ExpirationDate = paymentInfoDto.ExpirationDate;
    }
    
    
}