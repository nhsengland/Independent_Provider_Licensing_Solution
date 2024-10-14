using Azure.Storage.Queues.Models;
using Domain.Logic.Features.Team.ManageUsers;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Functions.Functions;

public class DeleteUserFunctions(
    ILogger<DeleteUserFunctions> logger,
    IManageUsersHandler manageUsersHandler)
{
    private readonly ILogger<DeleteUserFunctions> _logger = logger;
    private readonly IManageUsersHandler manageUsersHandler = manageUsersHandler;

    [Function(nameof(RemoveOktaUserFromGroup))]
    public async Task RemoveOktaUserFromGroup(
        [QueueTrigger("%queueClientConfiguration:RemoveOktaUserFromGroupQueueName%")] QueueMessage message,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("C# Queue trigger function processed: {message.MessageText}", message.MessageText);

        var userId = int.Parse(message.MessageText);

        await manageUsersHandler.RemoveOktaUserFromGroup(userId, cancellationToken);
    }
}
