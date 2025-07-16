using Application.DTOs.CreditCards;
using Application.Helpers;
using Application.Interfaces.Base;
using Domain.Entities;

namespace Application.Interfaces;

public interface ICreditCardService : IService<CreditCardRequest, CreditCardResponse, CreditCard>
{
    Task<IEnumerable<CreditCardResponse>> GetAllCreditCardsAsync();
    Task<CreditCardResponse> GetCreditCardByIdAsync(Guid id);

    Task CreateCreditCardAsync(CreditCardRequest request);
    Task UpdateCreditCardAsync(Guid id, CreditCardRequest request);
}