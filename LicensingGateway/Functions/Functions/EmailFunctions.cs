using Azure.Storage.Queues.Models;
using Domain.Logic.Integration.Email;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Functions.Functions;

public class EmailFunctions(
    ILoggerFactory loggerFactory,
    IEmailOrchestration emailOrchestration)
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<EmailFunctions>();
    private readonly IEmailOrchestration emailOrchestration = emailOrchestration;

    [Function(nameof(SendEmails))]
    public async Task SendEmails([QueueTrigger("%queueClientConfiguration:EmailNotificationQueueName%")] QueueMessage message)
    {
        try
        {
            await emailOrchestration.Orchestrate(int.Parse(message.Body));
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to orchestrate email delivery: {message}", message);
            throw;
        }
    }
}
