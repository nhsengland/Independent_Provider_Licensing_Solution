using Microsoft.Extensions.Configuration;

namespace Database.Helpers;

public static class DatabaseConfigurationFactory
{
    public static DatabaseConnectionStringConfiguration Create(
        IConfiguration configuration,
        string connectionStringName)
    {
        var connectionString = configuration.GetConnectionString(connectionStringName);
        var errorNumbersToAdd = configuration.GetSection("SQL:DatabaseErrorNumbersForRetry").Value ?? "";
#pragma warning disable CS8604 // Possible null reference argument.
        var maxRetryDelay = int.Parse(configuration.GetSection("SQL:MaxRetryDelayInSeconds").Value);
        var maxRetryCount = int.Parse(configuration.GetSection("SQL:MaxRetryCount").Value);
#pragma warning restore CS8604 // Possible null reference argument.

        return new DatabaseConnectionStringConfiguration()
        {
            ConnectionString = connectionString ?? throw new Exception($"Unable to obtain connection string for: {connectionStringName}"),
            ErrorNumbersToAdd = errorNumbersToAdd.Split(',').Select(int.Parse).ToArray(),
            MaxRetryDelayInSeconds = maxRetryDelay,
            MaxRetryCount = maxRetryCount
        };
    }
}
