namespace Domain.Logic.Features.Messages.Factories;

public class MessagePropertyFactory : IMessagePropertyFactory
{
    public string CreateShortendVersion(string body)
    {
        if (string.IsNullOrWhiteSpace(body))
        {
            return string.Empty;
        }

        if (body.Length < 100)
        {
            return body;
        }

        return string.Concat(body.AsSpan(0, 100), "...");
    }
}
