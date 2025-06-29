namespace Domain.Interfaces.Base;

public interface IUnitOfWork
{
    Task CommitAsync();
}