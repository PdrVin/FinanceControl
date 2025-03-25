using Application.Interfaces.Base;
using Domain.Entities;
using Domain.Enums;

namespace UnitTests.System.Infra;

public class IncomeRepositoryTests
{
    private readonly Mock<IRepository<Income>> _mockRepo;
    private readonly IRepository<Income> _repository;

    public IncomeRepositoryTests()
    {
        _mockRepo = new Mock<IRepository<Income>>();
        _repository = _mockRepo.Object;
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


    [Fact(DisplayName = "GetAllAsync -> List of Incomes")]
    public async Task GetAllAsync_ShouldReturnListOfIncomes()
    {
        // Arrange
        var incomes = GetMockIncomes();
        _mockRepo.Setup(repo => repo.GetAllAsync(0, 25)).ReturnsAsync(incomes);

        // Act
        var result = await _repository.GetAllAsync(0, 25);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(incomes.Count, result.Count);
    }

    [Fact(DisplayName = "GetByIdAsync -> Income")]
    public async Task GetByIdAsync_ShouldReturnIncome()
    {
        // Arrange
        var income = GetMockIncomes().First();
        var id = income.Id;
        _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(income);

        // Act
        var result = await _repository.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(income, result);
    }

    [Fact(DisplayName = "CreateAsync -> Create Income")]
    public async Task SaveAsync_ShouldSaveIncome()
    {
        // Arrange
        var income = GetMockIncomes()[0];
        _mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<Income>())).ReturnsAsync(income);

        // Act
        var result = await _repository.CreateAsync(income);

        // Assert
        _mockRepo.Verify(repo => repo.CreateAsync(income), Times.Once);
        Assert.Equal(income, result);
    }

    [Fact(DisplayName = "UpdateAsync -> Update Income")]
    public async Task Update_ShouldUpdateIncome()
    {
        // Arrange
        var income = GetMockIncomes()[1];
        _mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<Income>()));

        // Act
        await _repository.UpdateAsync(income);

        // Assert
        _mockRepo.Verify(repo => repo.UpdateAsync(income), Times.Once);
    }

    [Fact(DisplayName = "DeleteAsync -> Delete Income")]
    public async Task DeleteAsync_ShouldDeleteIncome()
    {
        // Arrange
        var income = GetMockIncomes()[2];
        _mockRepo.Setup(repo => repo.DeleteAsync(It.IsAny<Guid>()));

        // Act
        await _repository.DeleteAsync(income.Id);

        // Assert
        _mockRepo.Verify(repo => repo.DeleteAsync(income.Id), Times.Once);
    }
}
