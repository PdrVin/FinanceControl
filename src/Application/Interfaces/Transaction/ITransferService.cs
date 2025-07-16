using Application.DTOs.Transfers;
using Application.Interfaces.Base;
using Domain.Entities;

namespace Application.Interfaces;

public interface ITransferService : IService<TransferRequest, TransferResponse, Transfer>
{
    Task<IEnumerable<TransferResponse>> GetTransfersByAccountIdAsync(Guid accountId);
    Task<TransferResponse> GetTransferByIdAsync(Guid id);
    Task CreateTransferAsync(TransferRequest request);
    Task UpdateTransferAsync(Guid id, TransferRequest request);
    Task DeleteTransferAsync(Guid id);
}