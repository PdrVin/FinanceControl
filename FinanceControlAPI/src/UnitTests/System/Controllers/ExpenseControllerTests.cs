using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace UnitTests.System.Controllers;

public class ExpenseControllerTests
{
    private readonly Mock<IExpenseRepository> _mockRepo;
    private readonly ExpenseController _controller;

    public ExpenseControllerTests()
    {
        _mockRepo = new Mock<IExpenseRepository>();
        _controller = new ExpenseController(_mockRepo.Object);
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

    [Fact(DisplayName = "GetAll -> Ok List")]
    public async Task GetAll_ShouldReturnOk_WithExpenses()
    {
        // Arrange
        var expenses = GetMockExpenses();
        _mockRepo.Setup(repo => repo.GetAllAsync(0, 25)).ReturnsAsync(expenses);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<IEnumerable<Expense>>(okResult.Value, exactMatch: false);
        Assert.Equal(expenses.Count, returnValue.Count());
    }

    [Fact(DisplayName = "GetById -> Ok + List")]
    public async Task GetById_ShouldReturnOk_WhenExpenseExists()
    {
        // Arrange
        var expense = GetMockExpenses()[0];
        _mockRepo.Setup(repo => repo.GetByIdAsync(expense.Id)).ReturnsAsync(expense);

        // Act
        var result = await _controller.GetById(expense.Id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnExpense = Assert.IsType<Expense>(okResult.Value);
        Assert.Equal(expense.Id, returnExpense.Id);
    }

    [Fact(DisplayName = "GetById -> NotFound (NoList)")]
    public async Task GetById_ShouldReturnNotFound_WhenExpenseDoesNotExist()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Expense)null);

        // Act
        var result = await _controller.GetById(Guid.NewGuid());

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact(DisplayName = "Create -> CreatedAt")]
    public async Task Create_ShouldReturnCreatedAtAction()
    {
        // Arrange
        var expense = GetMockExpenses()[1];
        _mockRepo.Setup(repo => repo.CreateAsync(expense)).ReturnsAsync(expense);

        // Act
        var result = await _controller.Create(expense);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(_controller.GetById), createdResult.ActionName);
        Assert.Equal(expense, createdResult.Value);
    }

    [Fact(DisplayName = "Update -> Ok (Valid)")]
    public async Task Update_ShouldReturnNoContent_WhenValid()
    {
        // Arrange
        var expense = GetMockExpenses()[2];
        _mockRepo.Setup(repo => repo.UpdateAsync(expense));

        // Act
        var result = await _controller.Update(expense.Id, expense);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnExpense = Assert.IsType<Expense>(okResult.Value);
        Assert.Equal(expense.Id, returnExpense.Id);
    }

    [Fact(DisplayName = "Update -> BadRequest (DoNotMatch)")]
    public async Task Update_ShouldReturnBadRequest_WhenIdsDoNotMatch()
    {
        // Arrange
        var expense = GetMockExpenses()[3];

        // Act
        var result = await _controller.Update(Guid.NewGuid(), expense);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact(DisplayName = "Delete -> NoContent (Valid)")]
    public async Task Delete_ShouldReturnNoContent_WhenValid()
    {
        // Arrange
        var expense = GetMockExpenses()[4];
        _mockRepo.Setup(repo => repo.DeleteAsync(expense.Id));

        // Act
        var result = await _controller.Delete(expense.Id);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact(DisplayName = "Delete -> BadRequest (DoNotMatch)")]
    public async Task Delete_ShouldReturnBadRequest_WhenIdsDoNotMatch()
    {
        // Act
        var result = await _controller.Delete(Guid.NewGuid());

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}
