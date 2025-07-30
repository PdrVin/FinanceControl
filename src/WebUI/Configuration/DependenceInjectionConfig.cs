using Application.Interfaces;
using Application.Interfaces.Base;
using Application.Interfaces.Transaction;
using Application.Mapping;
using Application.Services.Account;
using Application.Services.Base;
using Application.Services.Invoices;
using Application.Services.Transaction;
using Domain.Interfaces;
using Domain.Interfaces.Base;
using Infra.Repositories;
using Infra.Repositories.Base;

namespace WebUI.Configuration;

public static class DependencyInjectionConfig
{
    public static void AddRegister(this IServiceCollection services, IConfiguration configuration)
    {
        #region Base
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped(typeof(IService<,,>), typeof(Service<,,>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        #endregion

        #region Services
        services.AddScoped<IBankAccountService, BankAccountService>();
        services.AddScoped<ICreditCardService, CreditCardService>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<IInvoicePaymentService, InvoicePaymentService>();
        services.AddScoped<ICardExpenseService, CardExpenseService>();
        services.AddScoped<IExpenseService, ExpenseService>();
        services.AddScoped<IIncomeService, IncomeService>();
        services.AddScoped<ITransferService, TransferService>();
        services.AddScoped<ITransactionService, TransactionService>();
        #endregion

        #region Repositories
        services.AddScoped<IBankAccountRepository, BankAccountRepository>();
        services.AddScoped<ICreditCardRepository, CreditCardRepository>();
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        services.AddScoped<IInvoicePaymentRepository, InvoicePaymentRepository>();
        services.AddScoped<ICardExpenseRepository, CardExpenseRepository>();
        services.AddScoped<IExpenseRepository, ExpenseRepository>();
        services.AddScoped<IIncomeRepository, IncomeRepository>();
        services.AddScoped<ITransferRepository, TransferRepository>();
        #endregion

        #region AutoMapper
        services.AddAutoMapper(typeof(MappingProfile).Assembly);
        #endregion
    }
}