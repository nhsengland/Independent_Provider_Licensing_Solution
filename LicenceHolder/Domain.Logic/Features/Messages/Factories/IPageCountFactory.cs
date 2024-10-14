namespace Domain.Logic.Features.Messages.Factories;

public interface IPageCountFactory
{
    int Create(int totalCount, int pageSize);
}
