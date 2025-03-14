using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;

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

    [Fact(DisplayName = "GetAll -> Ok List")]
    public async Task GetAll_ShouldReturnOk_WithExpenses()
    {
        // Arrange
        var expenses = new List<Expense> { new("Compra", ExpenseCategory.Supermercado, 100, DateTime.Now, Status.Pago) };
        _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expenses);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnExpenses = Assert.IsType<List<Expense>>(okResult.Value);
        Assert.Single(returnExpenses);
    }

    [Fact(DisplayName = "GetById -> Ok + List")]
    public async Task GetById_ShouldReturnOk_WhenExpenseExists()
    {
        // Arrange
        var expense = new Expense("Compra", ExpenseCategory.Supermercado, 100, DateTime.Now);
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
        var expense = new Expense("Compra", ExpenseCategory.Supermercado, 100, DateTime.Now);
        _mockRepo.Setup(repo => repo.SaveAsync(expense)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Create(expense);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(ExpenseController.GetById), createdResult.ActionName);
    }

    [Fact(DisplayName = "Update -> NoContent (Valid)")]
    public void Update_ShouldReturnNoContent_WhenValid()
    {
        // Arrange
        var expense = new Expense("Compra", ExpenseCategory.Supermercado, 100, DateTime.Now);
        _mockRepo.Setup(repo => repo.Update(expense));

        // Act
        var result = _controller.Update(expense.Id, expense);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact(DisplayName = "Update -> BadRequest (DoNotMatch)")]
    public void Update_ShouldReturnBadRequest_WhenIdsDoNotMatch()
    {
        // Arrange
        var expense = new Expense("Compra", ExpenseCategory.Supermercado, 100, DateTime.Now);

        // Act
        var result = _controller.Update(Guid.NewGuid(), expense);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact(DisplayName = "Delete -> NoContent (Valid)")]
    public void Delete_ShouldReturnNoContent_WhenValid()
    {
        // Arrange
        var expense = new Expense("Compra", ExpenseCategory.Supermercado, 100, DateTime.Now);
        _mockRepo.Setup(repo => repo.Delete(expense));

        // Act
        var result = _controller.Delete(expense.Id, expense);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact(DisplayName = "Delete -> BadRequest (DoNotMatch)")]
    public void Delete_ShouldReturnBadRequest_WhenIdsDoNotMatch()
    {
        // Arrange
        var expense = new Expense("Compra", ExpenseCategory.Supermercado, 100, DateTime.Now);

        // Act
        var result = _controller.Delete(Guid.NewGuid(), expense);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }
}
