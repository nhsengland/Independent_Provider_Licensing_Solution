using Domain.Objects.ViewModels.Messages;

namespace Domain.Logic.Features.Messages.Requests;

public class SendNotificationQuery
{
    public string UserOktaId { get; init; } = default!;

    public SendMessageViewModel Model { get; set; } = default!;
}
