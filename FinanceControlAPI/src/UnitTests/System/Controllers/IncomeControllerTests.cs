using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace UnitTests.System.Controllers;

public class IncomeControllerTests
{
    private readonly Mock<IIncomeRepository> _mockRepo;
    private readonly IncomeController _controller;

    public IncomeControllerTests()
    {
        _mockRepo = new Mock<IIncomeRepository>();
        _controller = new IncomeController(_mockRepo.Object);
    }

    [Fact(DisplayName = "GetAll -> Ok List")]
    public async Task GetAll_ShouldReturnOk_WithIncomes()
    {
        // Arrange
        var incomes = new List<Income>
        {
            new("Salário", IncomeCategory.Salário, 5000, DateTime.Now),
            new("Freelance", IncomeCategory.Ganho, 2000, DateTime.Now)
        };
        _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(incomes);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<Income>>(okResult.Value);
        Assert.Equal(2, returnValue.Count());
    }

    [Fact(DisplayName = "GetById -> Ok + List")]
    public async Task GetById_ShouldReturnOk_WhenIncomeExists()
    {
        // Arrange
        var income = new Income("Salário", IncomeCategory.Salário, 5000, DateTime.Now);
        _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(income);

        // Act
        var result = await _controller.GetById(Guid.NewGuid());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(income, okResult.Value);
    }

    [Fact(DisplayName = "GetById -> NotFound (NoList)")]
    public async Task GetById_ShouldReturnNotFound_WhenIncomeDoesNotExist()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Income)null);

        // Act
        var result = await _controller.GetById(Guid.NewGuid());

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact(DisplayName = "Create -> CreatedAt")]
    public async Task Create_ShouldReturnCreatedAtAction()
    {
        // Arrange
        var income = new Income("Salário", IncomeCategory.Salário, 5000, DateTime.Now);
        _mockRepo.Setup(repo => repo.SaveAsync(income)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Create(income);

        // Assert
        var createdAtAction = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(_controller.GetById), createdAtAction.ActionName);
        Assert.Equal(income, createdAtAction.Value);
    }

    [Fact(DisplayName = "Update -> NoContent (Valid)")]
    public void Update_ShouldReturnNoContent_WhenValid()
    {
        // Arrange
        var income = new Income("Salário", IncomeCategory.Salário, 5000, DateTime.Now);
        _mockRepo.Setup(repo => repo.Update(income));

        // Act
        var result = _controller.Update(income.Id, income);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact(DisplayName = "Update -> BadRequest (DoNotMatch)")]
    public void Update_ShouldReturnBadRequest_WhenIdDoesNotMatch()
    {
        // Arrange
        var income = new Income("Salário", IncomeCategory.Salário, 5000, DateTime.Now);

        // Act
        var result = _controller.Update(Guid.NewGuid(), income);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact(DisplayName = "Delete -> NoContent (Valid)")]
    public void Delete_ShouldReturnNoContent_WhenValid()
    {
        // Arrange
        var income = new Income("Salário", IncomeCategory.Salário, 5000, DateTime.Now);
        _mockRepo.Setup(repo => repo.Delete(income));

        // Act
        var result = _controller.Delete(income.Id, income);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact(DisplayName = "Delete -> BadRequest (DoNotMatch)")]
    public void Delete_ShouldReturnBadRequest_WhenIdDoesNotMatch()
    {
        // Arrange
        var income = new Income("Salário", IncomeCategory.Salário, 5000, DateTime.Now);

        // Act
        var result = _controller.Delete(Guid.NewGuid(), income);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }
}
