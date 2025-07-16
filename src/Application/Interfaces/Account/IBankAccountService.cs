using Application.DTOs.BankAccounts;
using Application.Helpers;
using Application.Interfaces.Base;
using Domain.Entities;

namespace Application.Interfaces;

public interface IBankAccountService : IService<BankAccountRequest, BankAccountResponse, BankAccount>
{
    Task<IEnumerable<BankAccountResponse>> GetAllBankAccountsAsync();
    Task<BankAccountResponse> GetBankAccountByIdAsync(Guid id);

    Task CreateBankAccountAsync(BankAccountRequest request);
    Task UpdateBankAccountAsync(Guid id, BankAccountRequest request);
}