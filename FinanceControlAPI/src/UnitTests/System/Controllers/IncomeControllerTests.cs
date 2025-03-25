using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

    private static List<Income> GetMockIncomes()
    {
        return
        [
            new Income("Salário mensal", IncomeCategory.Salário, 5000.00m, new DateTime(2025, 3, 10), Status.Pago, PayType.Pix, Account.Nubank),
            new Income("Investimento em ações", IncomeCategory.Investimento, 1500.00m, new DateTime(2025, 3, 5), Status.Pago, PayType.Cartão, Account.Carteira),
            new Income("Presente de aniversário", IncomeCategory.Presente, 300.00m, new DateTime(2025, 3, 14), Status.Pendente, PayType.Dinheiro, Account.PicPay),
            new Income("Venda de objeto", IncomeCategory.Ganho, 200.00m, new DateTime(2025, 3, 12), Status.Pago, PayType.Payback, Account.Caixa),
            new Income("Devolução de empréstimo", IncomeCategory.Outros, 1000.00m, new DateTime(2025, 3, 8), Status.Pago, PayType.Pix, Account.Nubank)
        ];
    }

    [Fact(DisplayName = "GetAll -> Ok List")]
    public async Task GetAll_ShouldReturnOk_WithIncomes()
    {
        // Arrange
        var incomes = GetMockIncomes();
        _mockRepo.Setup(repo => repo.GetAllAsync(0, 25)).ReturnsAsync(incomes);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<IEnumerable<Income>>(okResult.Value, exactMatch: false);
        Assert.Equal(incomes.Count, returnValue.Count());
    }

    [Fact(DisplayName = "GetById -> Ok + List")]
    public async Task GetById_ShouldReturnOk_WhenIncomeExists()
    {
        // Arrange
        var income = GetMockIncomes()[0];
        _mockRepo.Setup(repo => repo.GetByIdAsync(income.Id)).ReturnsAsync(income);

        // Act
        var result = await _controller.GetById(income.Id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnExpense = Assert.IsType<Income>(okResult.Value);
        Assert.Equal(income.Id, returnExpense.Id);
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
        var income = GetMockIncomes()[1];
        _mockRepo.Setup(repo => repo.CreateAsync(income)).ReturnsAsync(income);

        // Act
        var result = await _controller.Create(income);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(_controller.GetById), createdResult.ActionName);
        Assert.Equal(income, createdResult.Value);
    }

    [Fact(DisplayName = "Update -> Ok (Valid)")]
    public async Task Update_ShouldReturnNoContent_WhenValid()
    {
        // Arrange
        var income = GetMockIncomes()[2];
        _mockRepo.Setup(repo => repo.UpdateAsync(income));

        // Act
        var result = await _controller.Update(income.Id, income);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnIncome = Assert.IsType<Income>(okResult.Value);
        Assert.Equal(income.Id, returnIncome.Id);
    }

    [Fact(DisplayName = "Update -> BadRequest (DoNotMatch)")]
    public async Task Update_ShouldReturnBadRequest_WhenIdDoesNotMatch()
    {
        // Arrange
        var income = GetMockIncomes()[3];

        // Act
        var result = await _controller.Update(Guid.NewGuid(), income);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact(DisplayName = "Delete -> NoContent (Valid)")]
    public async Task Delete_ShouldReturnNoContent_WhenValid()
    {
        // Arrange
        var income = GetMockIncomes()[4];
        _mockRepo.Setup(repo => repo.DeleteAsync(income.Id));

        // Act
        var result = await _controller.Delete(income.Id);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact(DisplayName = "Delete -> BadRequest (DoNotMatch)")]
    public async Task Delete_ShouldReturnBadRequest_WhenIdDoesNotMatch()
    {
        // Act
        var result = await _controller.Delete(Guid.NewGuid());

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}
