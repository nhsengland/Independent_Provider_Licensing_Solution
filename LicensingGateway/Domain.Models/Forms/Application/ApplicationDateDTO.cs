namespace Domain.Models.Forms.Application;

public record ApplicationDateDTO
{
    public int? Day { get; init; }
    public int? Month { get; init; }
    public int? Year { get; init; }
    public bool IsValidDate { get; init; }
    public DateOnly? Date { get; init; }
}
