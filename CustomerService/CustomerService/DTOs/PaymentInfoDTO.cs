using CustomerService.Models;

namespace CustomerService.DTOs;

public class PaymentInfoDTO
{
    public int Id { get; set; } //primary key
    public String CardNumber { get; set; }
    public String ExpirationDate { get; set; }


    public PaymentInfoDTO(int id, string cardNumber, string expirationDate)
    {
        Id = id;
        CardNumber = cardNumber;
        ExpirationDate = expirationDate;
    }

    public PaymentInfoDTO()
    {
    }

    public PaymentInfoDTO(PaymentInfo paymentInfo)
    {
        this.Id = paymentInfo.Id;
        this.CardNumber = paymentInfo.CardNumber;
        this.ExpirationDate = paymentInfo.ExpirationDate;
    }
}