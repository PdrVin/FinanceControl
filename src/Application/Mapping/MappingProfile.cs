using AutoMapper;
using Domain.Entities;
using Application.DTOs.BankAccounts;
using Application.DTOs.CreditCards;
using Application.DTOs.Transfers;
using Application.DTOs.Incomes;
using Application.DTOs.Expenses;
using Application.DTOs.CardExpenses;
using Application.DTOs.Invoices;
using Application.DTOs.InvoicePayments;

namespace Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Entidade -> DTO
        CreateMap<BankAccount, BankAccountResponse>();
        CreateMap<CreditCard, CreditCardResponse>();
        CreateMap<Invoice, InvoiceResponse>();
        CreateMap<InvoicePayment, InvoicePaymentResponse>();
        CreateMap<CardExpense, CardExpenseResponse>();
        CreateMap<Expense, ExpenseResponse>();
        CreateMap<Income, IncomeResponse>();
        CreateMap<Transfer, TransferResponse>();

        // DTO -> Entidade
        CreateMap<BankAccountRequest, BankAccount>();
        CreateMap<CreditCardRequest, CreditCard>();
        CreateMap<InvoiceRequest, Invoice>();
        CreateMap<InvoicePaymentRequest, InvoicePayment>();
        CreateMap<CardExpenseRequest, CardExpense>();
        CreateMap<ExpenseRequest, Expense>();
        CreateMap<IncomeRequest, Income>();
        CreateMap<TransferRequest, Transfer>();

        // DTO para Entidade Existente (para Update)
        CreateMap<BankAccountRequest, BankAccount>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<CreditCardRequest, CreditCard>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        
    }
}
