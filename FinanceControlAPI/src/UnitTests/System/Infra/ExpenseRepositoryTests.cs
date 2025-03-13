using Infra.Context;
using Infra.Repositories;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace UnitTests.System.Infra;

public class ExpenseRepositoryTests
{
    private static FinanceDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<FinanceDbContext>()
            .UseInMemoryDatabase(databaseName: "FinanceTestDB")
            .Options;

        return new FinanceDbContext(options);
    }

    // [Fact]
    // public async Task AddExpense_ShouldAddExpenseToDatabase()
    // {
    //     // Arrange
    //     var context = GetDbContext();
    //     var repository = new ExpenseRepository(context);
    //     var expense = new Expense("Compra no mercado", Category.Supermercado, 150.75m, DateTime.Now);

    //     // Act
    //     await repository.SaveAsync(expense);
    //     var expenses = await repository.GetAllAsync();

    //     // Assert
    //     Assert.Single(expenses);
    //     Assert.Equal("Compra no mercado", expenses.First().Description);
    // }
}


