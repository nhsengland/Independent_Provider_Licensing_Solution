using Domain.Logic.Features.Team.InviteUser;
using Domain.Objects.Integrations.StorageAccounts.Queues;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Functions.Functions;

public class CreateUserFunctions(
    ILogger<CreateUserFunctions> logger,
    IAddUserHandler addUserHandler)
{
    private readonly ILogger<CreateUserFunctions> _logger = logger;
    private readonly IAddUserHandler addUserHandler = addUserHandler;

    [Function(nameof(CreateOktaUser))]
    public async Task CreateOktaUser(
        [QueueTrigger("%queueClientConfiguration:CreateOktaUserQueueName%")] CreateOktaUserInputModel input,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("CreateOktaUser: {userid}, {emailNotificationId}", input.UserId, input.EmailNotificationId);

        await addUserHandler.CreateOktaUser(input, cancellationToken);
    }
}
