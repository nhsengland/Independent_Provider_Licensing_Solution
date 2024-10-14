namespace Domain.Models.Exceptions;
public class CannotConnectToTheDatabaseException : Exception
{
    public CannotConnectToTheDatabaseException(string message) : base(message)
    {
    }
}
