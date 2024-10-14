namespace Domain.Objects.Exceptions;

public class NotFoundException(string message) : Exception(message)
{
}
