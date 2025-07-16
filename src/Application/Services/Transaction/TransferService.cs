using Application.DTOs.Transfers;
using Application.Interfaces;
using Application.Services.Base;
using Domain.Entities;
using Domain.Interfaces.Base;
using Domain.Interfaces;
using AutoMapper;

namespace Application.Services.Transaction;

public class TransferService : Service<TransferRequest, TransferResponse, Transfer>, ITransferService
{
    private readonly ITransferRepository _transferRepository;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TransferService(
        ITransferRepository repository,
        IBankAccountRepository bankAccountRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
        : base(repository, unitOfWork, mapper)
    {
        _transferRepository = repository;
        _bankAccountRepository = bankAccountRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TransferResponse>> GetTransfersByAccountIdAsync(Guid accountId)
    {
        var transfers = await _transferRepository.GetTransfersByAccountIdAsync(accountId);
        return _mapper.Map<IEnumerable<TransferResponse>>(transfers);
    }

    public async Task<TransferResponse> GetTransferByIdAsync(Guid id)
    {
        var transfer = await _transferRepository.GetByIdAsync(id);
        return _mapper.Map<TransferResponse>(transfer);
    }

    public async Task CreateTransferAsync(TransferRequest request)
    {
        // Lógica de negócio: Checar a existência das contas e o saldo
        var sourceAccount = await _bankAccountRepository.GetByIdAsync(request.SourceAccountId);
        var destinationAccount = await _bankAccountRepository.GetByIdAsync(request.DestinationAccountId);

        if (sourceAccount == null)
            throw new KeyNotFoundException($"Source account with id {request.SourceAccountId} not found.");

        if (destinationAccount == null)
            throw new KeyNotFoundException($"Destination account with id {request.DestinationAccountId} not found.");

        if (sourceAccount.CurrentBalance < request.Amount)
            throw new InvalidOperationException("Source account does not have sufficient funds for this transfer.");

        // Atualizar saldos no domínio
        sourceAccount.CurrentBalance -= request.Amount;
        destinationAccount.CurrentBalance += request.Amount;

        _bankAccountRepository.Update(sourceAccount);
        _bankAccountRepository.Update(destinationAccount);

        var transfer = _mapper.Map<Transfer>(request);

        await _transferRepository.SaveAsync(transfer);
        await _unitOfWork.CommitAsync(); // Salva as 3 operações (2 updates e 1 save) em uma única transação
    }

    public async Task UpdateTransferAsync(Guid id, TransferRequest request)
    {
        var existingTransfer = await _transferRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Transfer with id {id} not found.");

        // Lógica de negócio: Reverter os saldos antigos e aplicar os novos
        var sourceAccount = await _bankAccountRepository.GetByIdAsync(existingTransfer.SourceAccountId)
            ?? throw new KeyNotFoundException($"Source account with id {existingTransfer.SourceAccountId} not found.");

        var destinationAccount = await _bankAccountRepository.GetByIdAsync(existingTransfer.DestinationAccountId)
            ?? throw new KeyNotFoundException(
                $"Destination account with id {existingTransfer.DestinationAccountId} not found.");

        sourceAccount.CurrentBalance += existingTransfer.Amount;
        destinationAccount.CurrentBalance -= existingTransfer.Amount;

        // Atualizar com os novos valores
        sourceAccount.CurrentBalance -= request.Amount;
        destinationAccount.CurrentBalance += request.Amount;

        _bankAccountRepository.Update(sourceAccount);
        _bankAccountRepository.Update(destinationAccount);

        var updatedTransfer = _mapper.Map<Transfer>(request);
        updatedTransfer.Id = id; // Manter o ID da transferência existente

        _transferRepository.Update(updatedTransfer);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeleteTransferAsync(Guid id)
    {
        // Lógica de negócio: Reverter os saldos antes de deletar a transferência
        var existingTransfer = await _transferRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Transfer with id {id} not found.");

        var sourceAccount = await _bankAccountRepository.GetByIdAsync(existingTransfer.SourceAccountId)
            ?? throw new KeyNotFoundException($"Source account with id {existingTransfer.SourceAccountId} not found.");

        var destinationAccount = await _bankAccountRepository.GetByIdAsync(existingTransfer.DestinationAccountId)
            ?? throw new KeyNotFoundException(
                $"Destination account with id {existingTransfer.DestinationAccountId} not found.");

        sourceAccount.CurrentBalance += existingTransfer.Amount;
        _bankAccountRepository.Update(sourceAccount);

        destinationAccount.CurrentBalance -= existingTransfer.Amount;
        _bankAccountRepository.Update(destinationAccount);

        _transferRepository.Delete(id);
        await _unitOfWork.CommitAsync();
    }
}