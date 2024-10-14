namespace Domain.Logic.Features.Messages.Factories;

public interface ISkipFactory
{
    int Create(int pageNumber, int pageSize);
}
