namespace Domain.Models.Exceptions;
public class CQCAPIException : Exception
{
    public CQCAPIException(string message) : base(message)
    {
    }
}
