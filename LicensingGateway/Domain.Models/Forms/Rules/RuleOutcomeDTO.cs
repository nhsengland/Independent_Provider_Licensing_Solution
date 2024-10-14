namespace Domain.Models.Forms.Rules;

public record RuleOutcomeDTO
{
    public List<string> ErrorMessages { get; set; } = [];

    public bool IsSuccess => ErrorMessages.Count == 0;
}
