using Application.Interfaces;
using Application.Interfaces.Base;
using Application.Mapping;
using Application.Services;
using Application.Services.Base;
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
        services.AddScoped(typeof(IService<,>), typeof(Service<,>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        #endregion

        #region Services
        services.AddScoped<IExpenseService, ExpenseService>();
        services.AddScoped<IIncomeService, IncomeService>();
        services.AddScoped<IInvoiceService, InvoiceService>();
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
        services.AddAutoMapper(typeof(EntityMappingProfile).Assembly);
        #endregion
    }
}