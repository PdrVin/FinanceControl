using Application.DTOs.InvoicePayments;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Base;
using Domain.Interfaces;
using Application.Services.Base;

namespace Application.Services.Invoices;

public class InvoicePaymentService
    : Service<InvoicePaymentRequest, InvoicePaymentResponse, InvoicePayment>, IInvoicePaymentService
{
    private readonly IInvoicePaymentRepository _invoicePaymentRepository;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public InvoicePaymentService(
        IInvoicePaymentRepository repository,
        IInvoiceRepository invoiceRepository,
        IBankAccountRepository bankAccountRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
        : base(repository, unitOfWork, mapper)
    {
        _invoicePaymentRepository = repository;
        _invoiceRepository = invoiceRepository;
        _bankAccountRepository = bankAccountRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<InvoicePaymentResponse>> GetPaymentsByInvoiceIdAsync(Guid invoiceId)
    {
        var payments = await _invoicePaymentRepository
            .GetPaymentsByInvoiceIdAsync(invoiceId);

        return _mapper.Map<IEnumerable<InvoicePaymentResponse>>(payments);
    }

    public async Task<IEnumerable<InvoicePaymentResponse>> GetPaymentsByBankAccountIdAsync(Guid bankAccountId)
    {
        var payments = await _invoicePaymentRepository
            .GetPaymentsByBankAccountIdAsync(bankAccountId);

        return _mapper.Map<IEnumerable<InvoicePaymentResponse>>(payments);
    }

    public async Task<InvoicePaymentResponse> GetInvoicePaymentByIdAsync(Guid id)
    {
        var payment = await _invoicePaymentRepository.GetByIdAsync(id);
        return _mapper.Map<InvoicePaymentResponse>(payment);
    }

    public async Task CreateInvoicePaymentAsync(InvoicePaymentRequest request)
    {
        // Lógica de negócio: Validar a existência da fatura e da conta bancária
        var invoice = await _invoiceRepository.GetInvoiceByIdAsync(request.InvoiceId)
            ?? throw new KeyNotFoundException($"Invoice with id {request.InvoiceId} not found.");

        var bankAccount = await _bankAccountRepository.GetByIdAsync(request.BankAccountId)
            ?? throw new KeyNotFoundException($"Bank account with id {request.BankAccountId} not found.");

        // Lógica de negócio: Atualizar o saldo da conta bancária
        bankAccount.CurrentBalance -= request.Amount;

        var invoicePayment = _mapper.Map<InvoicePayment>(request);

        await _invoicePaymentRepository.SaveAsync(invoicePayment);
        _bankAccountRepository.Update(bankAccount);

        await _unitOfWork.CommitAsync();
    }

    public async Task UpdateInvoicePaymentAsync(Guid id, InvoicePaymentRequest request)
    {
        var existingPayment = await _invoicePaymentRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Invoice payment with id {id} not found.");

        var invoice = await _invoiceRepository.GetByIdAsync(request.InvoiceId)
            ?? throw new KeyNotFoundException($"Invoice with id {request.InvoiceId} not found.");

        var bankAccount = await _bankAccountRepository.GetByIdAsync(request.BankAccountId)
            ?? throw new KeyNotFoundException($"Bank account with id {request.BankAccountId} not found.");

        // Lógica de negócio: Atualizar o saldo da conta bancária
        bankAccount.CurrentBalance += existingPayment.Amount - request.Amount;

        _mapper.Map(request, existingPayment);

        _invoicePaymentRepository.Update(existingPayment);
        _bankAccountRepository.Update(bankAccount);

        await _unitOfWork.CommitAsync();
    }

    public async Task DeleteInvoicePaymentAsync(Guid id)
    {
        // Lógica de negócio: Reverter o saldo da conta bancária antes de deletar
        var existingPayment = await _invoicePaymentRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Invoice payment with id {id} not found.");

        var bankAccount = await _bankAccountRepository.GetByIdAsync(existingPayment.BankAccountId)
            ?? throw new KeyNotFoundException($"Bank account with id {existingPayment.BankAccountId} not found.");

        bankAccount.CurrentBalance += existingPayment.Amount;
        _bankAccountRepository.Update(bankAccount);

        _invoicePaymentRepository.Delete(id);
        await _unitOfWork.CommitAsync();
    }
}