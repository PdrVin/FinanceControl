using Domain.Entities;
using Domain.Enums;
using Xunit;

namespace UnitTests.Entities.Expenses;

public class ExpenseTests
{
    [Fact(DisplayName = "Ctor -> Create Valid")]
    public void Constructor_ShouldCreateValidExpense()
    {
        // Arrange
        var description = "Compra no mercado";
        var category = ExpenseCategory.Supermercado;
        var amount = 150.75m;
        var date = DateTime.Now;
        var status = Status.Pendente;
        var payType = PayType.Pix;
        var account = Account.PicPay;

        // Act
        var expense = new Expense(description, category, amount, date, status, payType, account);

        // Assert
        Assert.Equal(description, expense.Description);
        Assert.Equal(category, expense.Category);
        Assert.Equal(amount, expense.Amount);
        Assert.Equal(date, expense.Date);
        Assert.Equal(status, expense.Status);
        Assert.Equal(payType, expense.PayType);
        Assert.Equal(account, expense.Account);
    }

    [Fact(DisplayName = "Ctor -> Exception (Empty Description)")]
    public void Constructor_ShouldThrowException_WhenDescriptionIsEmpty()
    {
        // Arrange
        var category = ExpenseCategory.Supermercado;
        var amount = 100m;
        var date = DateTime.Now;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Expense("", category, amount, date)
        );

        Assert.Equal("The description cannot be null or empty. (Parameter 'description')", exception.Message);
    }

    [Fact(DisplayName = "Ctor -> Exception (Amount Negative)")]
    public void Constructor_ShouldThrowException_WhenAmountIsNegative()
    {
        // Arrange
        var description = "Conta de luz";
        var category = ExpenseCategory.Casa;
        var amount = -50m;
        var date = DateTime.Now;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Expense(description, category, amount, date)
        );

        Assert.Equal("Amount must be greater than zero. (Parameter 'amount')", exception.Message);
    }
}
