using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace UnitTests.System.Controllers;

public class InvoiceControllerTests
{
    private readonly Mock<IInvoiceRepository> _mockRepo;
    private readonly InvoiceController _controller;

    public InvoiceControllerTests()
    {
        _mockRepo = new Mock<IInvoiceRepository>();
        _controller = new InvoiceController(_mockRepo.Object);
    }

    public static List<Invoice> GetMockInvoices()
    {
        return
        [
            new Invoice(
                Account.Nubank,
                new DateTime(2025, 4, 1),
                new DateTime(2025, 4, 10),
                [
                    new Expense("Aluguel", ExpenseCategory.Casa, 1500, DateTime.Now, Status.Pago, PayType.Cartão, Account.Nubank),
                    new Expense("Supermercado", ExpenseCategory.Supermercado, 500, DateTime.Now.AddDays(-10), Status.Pago, PayType.Cartão, Account.Nubank)
                ]
            ),
            new Invoice(
                Account.Caixa,
                new DateTime(2025, 3, 15),
                new DateTime(2025, 3, 25),
                [
                    new Expense("Mensalidade Escola", ExpenseCategory.Educação, 1200, DateTime.Now.AddMonths(-1), Status.Pago, PayType.Cartão, Account.Caixa),
                    new Expense("Spotify", ExpenseCategory.Spotify, 29.99m, DateTime.Now.AddDays(-5), Status.Pendente, PayType.Cartão, Account.Caixa)
                ]
            ),
            new Invoice(
                Account.PicPay,
                new DateTime(2025, 2, 10),
                new DateTime(2025, 2, 20),
                [
                    new Expense("Cinema", ExpenseCategory.Lazer, 100, DateTime.Now.AddDays(-15), Status.Pago, PayType.Cartão, Account.PicPay),
                    new Expense("Transporte Público", ExpenseCategory.Transporte, 80, DateTime.Now.AddDays(-20), Status.Pendente, PayType.Cartão, Account.PicPay)
                ]
            ),
            new Invoice(
                Account.Carteira,
                new DateTime(2025, 1, 5),
                new DateTime(2025, 1, 15),
                [
                    new Expense("Eletrônicos", ExpenseCategory.Eletrônicos, 2000, DateTime.Now.AddMonths(-1), Status.Pago, PayType.Cartão, Account.Carteira),
                    new Expense("Vestuário", ExpenseCategory.Vestuário, 300, DateTime.Now.AddDays(-30), Status.Pendente, PayType.Cartão, Account.Carteira)
                ]
            ),
            new Invoice(
                Account.Nubank,
                new DateTime(2025, 4, 10),
                new DateTime(2025, 4, 20),
                [
                    new Expense("Viagem", ExpenseCategory.Viagem, 4500, DateTime.Now.AddMonths(-2), Status.Pendente, PayType.Cartão, Account.Nubank),
                    new Expense("Supermercado", ExpenseCategory.Supermercado, 600, DateTime.Now.AddDays(-7), Status.Pago, PayType.Cartão, Account.Nubank)
                ]
            )
        ];
    }

    [Fact(DisplayName = "GetAll -> Ok List")]
    public async Task GetAll_ShouldReturnOk_WithInvoices()
    {
        // Arrange
        var invoices = GetMockInvoices();
        _mockRepo.Setup(repo => repo.GetAllAsync(0, 25)).ReturnsAsync(invoices);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<IEnumerable<Invoice>>(okResult.Value, exactMatch: false);
        Assert.Equal(invoices.Count, returnValue.Count());
    }

    [Fact(DisplayName = "GetById -> Ok + List")]
    public async Task GetById_ShouldReturnOk_WhenInvoiceExists()
    {
        // Arrange
        var invoice = GetMockInvoices()[0];
        _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(invoice);

        // Act
        var result = await _controller.GetById(Guid.NewGuid());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnInvoice = Assert.IsType<Invoice>(okResult.Value);
        Assert.Equal(invoice.Id, returnInvoice.Id);
    }

    [Fact(DisplayName = "GetById -> NotFound (NoList)")]
    public async Task GetById_ShouldReturnNotFound_WhenInvoiceDoesNotExist()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Invoice)null);

        // Act
        var result = await _controller.GetById(Guid.NewGuid());

        // Assert
        Assert.IsType<NotFoundResult>(result, exactMatch: false);
    }

    [Fact(DisplayName = "Create -> CreatedAt")]
    public async Task Create_ShouldReturnCreatedAtAction()
    {
        // Arrange
        var invoice = GetMockInvoices()[1];
        _mockRepo.Setup(repo => repo.CreateAsync(invoice)).ReturnsAsync(invoice);

        // Act
        var result = await _controller.Create(invoice);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(_controller.GetById), createdResult.ActionName);
        Assert.Equal(invoice, createdResult.Value);
    }

    [Fact(DisplayName = "Update -> Ok (Valid)")]
    public async Task Update_ShouldReturnNoContent_WhenValid()
    {
        // Arrange
        var invoice = GetMockInvoices()[2];
        _mockRepo.Setup(repo => repo.UpdateAsync(invoice));

        // Act
        var result = await _controller.Update(invoice.Id, invoice);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnInvoice = Assert.IsType<Invoice>(okResult.Value);
        Assert.Equal(invoice.Id, returnInvoice.Id);
    }

    [Fact(DisplayName = "Update -> BadRequest (DoNotMatch)")]
    public async Task Update_ShouldReturnBadRequest_WhenIdDoesNotMatch()
    {
        // Arrange
        var invoice = GetMockInvoices()[3];

        // Act
        var result = await _controller.Update(Guid.NewGuid(), invoice);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact(DisplayName = "Delete -> NoContent (Valid)")]
    public async Task Delete_ShouldReturnNoContent_WhenValid()
    {
        // Arrange
        var invoice = GetMockInvoices()[4];
        _mockRepo.Setup(repo => repo.DeleteAsync(invoice.Id));

        // Act
        var result = await _controller.Delete(invoice.Id);

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
