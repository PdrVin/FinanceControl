using AutoMapper;
using Application.DTOs;
using Domain.Entities;

namespace Application.Mapping;

public class EntityMappingProfile : Profile
{
    public EntityMappingProfile()
    {
        CreateMap<Expense, ExpenseDto>().ReverseMap();
        CreateMap<Income, IncomeDto>().ReverseMap();
        CreateMap<Invoice, InvoiceDto>().ReverseMap();
    }
}
