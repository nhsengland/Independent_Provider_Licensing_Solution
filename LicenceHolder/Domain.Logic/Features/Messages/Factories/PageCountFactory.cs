namespace Domain.Logic.Features.Messages.Factories;

public class PageCountFactory : IPageCountFactory
{
    public int Create(int totalCount, int pageSize)
    {
        if (pageSize == 0)
        {
            /* prevent divide by zero exception */
            return 0;
        }

        return (int)Math.Ceiling((double)totalCount / pageSize);
    }
}
