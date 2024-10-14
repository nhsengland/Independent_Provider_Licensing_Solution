namespace Licensing.Gateway.Models.Application;

public class ApplicationBase
{
    public bool ValidationFailure { get; set; } = false;

    public List<string> FailureMessages { get; set; } = [];
}
