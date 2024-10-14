namespace Domain.Logic.Features.Licence;

public interface IDateEvaluation
{
    DateOnly? EvaluateDate(string date);
}
