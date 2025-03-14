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
        _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expenses);

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expenses.Count, result.Count());
    }

    [Fact(DisplayName = "GetByIdAsync -> Expense")]
    public async Task GetByIdAsync_ShouldReturnExpense()
    {
        // Arrange
        var expenses = GetMockExpenses();
        var id = expenses.First().Id;
        _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(expenses.First());

        // Act
        var result = await _repository.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expenses.First(), result);
    }

    [Fact(DisplayName = "SaveAsync -> Save Expense")]
    public async Task SaveAsync_ShouldSaveExpense()
    {
        // Arrange
        var expense = GetMockExpenses()[0];
        _mockRepo.Setup(repo => repo.SaveAsync(It.IsAny<Expense>())).Returns(Task.CompletedTask);

        // Act
        await _repository.SaveAsync(expense);

        // Assert
        _mockRepo.Verify(repo => repo.SaveAsync(expense), Times.Once);
    }

    [Fact(DisplayName = "Update -> Update Expense")]
    public void Update_ShouldUpdateExpense()
    {
        // Arrange
        var expense = GetMockExpenses()[1];
        _mockRepo.Setup(repo => repo.Update(It.IsAny<Expense>()));

        // Act
        _repository.Update(expense);

        // Assert
        _mockRepo.Verify(repo => repo.Update(expense), Times.Once);
    }

    [Fact(DisplayName = "Delete -> Delete Expense")]
    public void Delete_ShouldDeleteExpense()
    {
        // Arrange
        var expense = GetMockExpenses()[2];
        _mockRepo.Setup(repo => repo.Delete(It.IsAny<Expense>()));

        // Act
        _repository.Delete(expense);

        // Assert
        _mockRepo.Verify(repo => repo.Delete(expense), Times.Once);
    }
}
