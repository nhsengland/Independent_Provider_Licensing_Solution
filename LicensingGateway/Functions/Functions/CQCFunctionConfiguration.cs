namespace Functions.Functions;
public record CQCFunctionConfiguration
{
    public int RetryPolicyNumberOfAttempts { get; init; }
    public int RetryPolicyFirstRetryInterval { get; init; }
    public int RetryPolicyBackoffCoefficient { get; init; }
    public int DelayInMinuets { get; init; }
}
