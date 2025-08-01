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
        CreateMap<BankAccount, BankAccountResponse>().ReverseMap();
        CreateMap<CreditCard, CreditCardResponse>().ReverseMap();
        CreateMap<Invoice, InvoiceResponse>().ReverseMap();
        CreateMap<InvoicePayment, InvoicePaymentResponse>().ReverseMap();
        CreateMap<CardExpense, CardExpenseResponse>().ReverseMap();
        CreateMap<Expense, ExpenseResponse>().ReverseMap();
        CreateMap<Income, IncomeResponse>().ReverseMap();
        CreateMap<Transfer, TransferResponse>().ReverseMap();

        // DTO -> Entidade
        CreateMap<BankAccountRequest, BankAccount>().ReverseMap();
        CreateMap<CreditCardRequest, CreditCard>().ReverseMap();
        CreateMap<InvoiceRequest, Invoice>().ReverseMap();
        CreateMap<InvoicePaymentRequest, InvoicePayment>().ReverseMap();
        CreateMap<CardExpenseRequest, CardExpense>().ReverseMap();
        CreateMap<ExpenseRequest, Expense>().ReverseMap();
        CreateMap<IncomeRequest, Income>().ReverseMap();
        CreateMap<TransferRequest, Transfer>().ReverseMap();

        // DTO para Entidade Existente (para Update)
        CreateMap<BankAccountRequest, BankAccount>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ReverseMap();
        CreateMap<CreditCardRequest, CreditCard>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ReverseMap();
        CreateMap<IncomeRequest, Income>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ReverseMap();

    }
}
