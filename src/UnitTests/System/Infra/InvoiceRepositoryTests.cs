using Application.Interfaces.Base;
using Domain.Entities;
using Domain.Enums;

namespace UnitTests.System.Infra;

public class InvoiceRepositoryTests
{
    private readonly Mock<IRepository<Invoice>> _mockRepo;
    private readonly IRepository<Invoice> _repository;

    public InvoiceRepositoryTests()
    {
        _mockRepo = new Mock<IRepository<Invoice>>();
        _repository = _mockRepo.Object;
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

    [Fact(DisplayName = "GetAllAsync -> List of Invoices")]
    public async Task GetAllAsync_ShouldReturnListOfInvoices()
    {
        // Arrange
        var invoices = GetMockInvoices();
        _mockRepo.Setup(repo => repo.GetAllAsync(0, 25)).ReturnsAsync(invoices);

        // Act
        var result = await _repository.GetAllAsync(0, 25);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(invoices.Count, result.Count);
    }

    [Fact(DisplayName = "GetByIdAsync -> Invoice")]
    public async Task GetByIdAsync_ShouldReturnInvoice()
    {
        // Arrange
        var invoice = GetMockInvoices().First();
        var id = invoice.Id;
        _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(invoice);

        // Act
        var result = await _repository.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(invoice, result);
    }

    [Fact(DisplayName = "CreateAsync -> Create Invoice")]
    public async Task CreateAsync_ShouldCreateInvoice()
    {
        // Arrange
        var invoice = GetMockInvoices()[0];
        _mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<Invoice>())).ReturnsAsync(invoice);

        // Act
        var result = await _repository.CreateAsync(invoice);

        // Assert
        _mockRepo.Verify(repo => repo.CreateAsync(invoice), Times.Once);
        Assert.Equal(invoice, result);
    }

    [Fact(DisplayName = "UpdateAsync -> Update Invoice")]
    public async Task UpdateAsync_ShouldUpdateInvoice()
    {
        // Arrange
        var invoice = GetMockInvoices()[1];
        _mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<Invoice>()));

        // Act
        await _repository.UpdateAsync(invoice);

        // Assert
        _mockRepo.Verify(repo => repo.UpdateAsync(invoice), Times.Once);
    }

    [Fact(DisplayName = "DeleteAsync -> Delete Invoice")]
    public async Task DeleteAsync_ShouldDeleteInvoice()
    {
        // Arrange
        var invoice = GetMockInvoices()[2];
        _mockRepo.Setup(repo => repo.DeleteAsync(It.IsAny<Guid>()));

        // Act
        await _repository.DeleteAsync(invoice.Id);

        // Assert
        _mockRepo.Verify(repo => repo.DeleteAsync(invoice.Id), Times.Once);
    }
}
