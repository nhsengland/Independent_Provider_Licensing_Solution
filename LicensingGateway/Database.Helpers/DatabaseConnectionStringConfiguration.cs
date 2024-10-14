namespace Database.Helpers;

public record DatabaseConnectionStringConfiguration
{
    public string ConnectionString { get; init; } = default!;

    public int[] ErrorNumbersToAdd { get; init; } = default!;

    public int MaxRetryDelayInSeconds { get; init; }

    public int MaxRetryCount { get; init; }
}
