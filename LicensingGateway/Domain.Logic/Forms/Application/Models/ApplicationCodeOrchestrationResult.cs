using Domain.Models.Database;

namespace Domain.Logic.Forms.Application.Models;
public record ApplicationCodeOrchestrationResult
{
    public bool Exists { get; init; }

    public int ApplicationId { get; init; }

    public ApplicationPage NextPage { get; init; }

    public bool FirstTimeAccessingApplication { get; init; }
}
