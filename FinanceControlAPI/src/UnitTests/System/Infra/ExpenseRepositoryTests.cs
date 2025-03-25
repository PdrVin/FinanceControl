using Application.Interfaces.Base;
using Domain.Entities;
using Domain.Enums;

namespace UnitTests.System.Infra;

public class ExpenseRepositoryTests
{
    private readonly Mock<IRepository<Expense>> _mockRepo;
    private readonly IRepository<Expense> _repository;

    public ExpenseRepositoryTests()
    {
        _mockRepo = new Mock<IRepository<Expense>>();
        _repository = _mockRepo.Object;
    }

    private static List<Expense> GetMockExpenses()
    {
        return
        [
            new Expense("Aluguel", ExpenseCategory.Casa, 1500, DateTime.Now, Status.Pago, PayType.Pix, Account.Nubank),
            new Expense("Supermercado", ExpenseCategory.Supermercado, 500, DateTime.Now.AddDays(-10), Status.Pago, PayType.Dinheiro, Account.Carteira),
            new Expense("Mensalidade Escola", ExpenseCategory.Educação, 1200, DateTime.Now.AddMonths(-1), Status.Pago, PayType.Cartão, Account.Caixa),
            new Expense("Spotify", ExpenseCategory.Spotify, 29.99m, DateTime.Now.AddDays(-5), Status.Pendente, PayType.Payback, Account.PicPay),
            new Expense("Cinema", ExpenseCategory.Lazer, 100, DateTime.Now.AddDays(-15), Status.Pago, PayType.Pix, Account.Nubank),
            new Expense("Transporte Público", ExpenseCategory.Transporte, 80, DateTime.Now.AddDays(-20), Status.Pendente, PayType.Dinheiro, Account.Carteira)
        ];
    }

    [Fact(DisplayName = "GetAllAsync -> List of Expenses")]
    public async Task GetAllAsync_ShouldReturnListOfExpenses()
    {
        // Arrange
        var expenses = GetMockExpenses();
        _mockRepo.Setup(repo => repo.GetAllAsync(0, 25)).ReturnsAsync(expenses);

        // Act
        var result = await _repository.GetAllAsync(0, 25);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expenses.Count, result.Count);
    }

    [Fact(DisplayName = "GetByIdAsync -> Expense")]
    public async Task GetByIdAsync_ShouldReturnExpense()
    {
        // Arrange
        var expense = GetMockExpenses().First();
        var id = expense.Id;
        _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(expense);

        // Act
        var result = await _repository.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expense, result);
    }

    [Fact(DisplayName = "CreateAsync -> Create Expense")]
    public async Task CreateAsync_ShouldCreateExpense()
    {
        // Arrange
        var expense = GetMockExpenses()[0];
        _mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<Expense>())).ReturnsAsync(expense);

        // Act
        var result = await _repository.CreateAsync(expense);

        // Assert
        _mockRepo.Verify(repo => repo.CreateAsync(expense), Times.Once);
        Assert.Equal(expense, result);
    }

    [Fact(DisplayName = "UpdateAsync -> Update Expense")]
    public async Task UpdateAsync_ShouldUpdateExpenseAsync()
    {
        // Arrange
        var expense = GetMockExpenses()[1];
        _mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<Expense>()));

        // Act
        await _repository.UpdateAsync(expense);

        // Assert
        _mockRepo.Verify(repo => repo.UpdateAsync(expense), Times.Once);
    }

    [Fact(DisplayName = "DeleteAsync -> Delete Expense")]
    public async Task DeleteAsync_ShouldDeleteExpense()
    {
        // Arrange
        var expense = GetMockExpenses()[2];
        _mockRepo.Setup(repo => repo.DeleteAsync(It.IsAny<Guid>()));

        // Act
        await _repository.DeleteAsync(expense.Id);

        // Assert
        _mockRepo.Verify(repo => repo.DeleteAsync(expense.Id), Times.Once);
    }
}
