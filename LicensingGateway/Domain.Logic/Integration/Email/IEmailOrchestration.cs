namespace Domain.Logic.Integration.Email;
public interface IEmailOrchestration
{
    Task Orchestrate(int messageId);
}
