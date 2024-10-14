namespace Domain.Models.Database.DTO;
public record ApplicationCodeDTO
{
    public int ApplicationCodeId { get; init; }
    public int? ApplicationId { get; init; }
    public ApplicationPage? CurrentPage { get; init; }

    public int? PreApplicationId { get; init; }

    public bool IsHardToFindOrHardToReplace { get; init; }
}
