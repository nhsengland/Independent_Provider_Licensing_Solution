namespace Domain.Logic.Features.Messages.Factories;

public class SkipFactory : ISkipFactory
{
    public int Create(int pageNumber, int pageSize)
    {
        return (pageNumber - 1) * pageSize;
    }
}
