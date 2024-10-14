namespace Domain.Logic.Features.Licence;

public class DateEvaluation(
    IDateConverter dateConverter) : IDateEvaluation
{
    private readonly IDateConverter dateConverter = dateConverter;

    public DateOnly? EvaluateDate(string date)
    {
        DateOnly? result;

        result = dateConverter.ConvertToddmmyyyy(date);

        if (result is not null)
        {
            return result;
        }

        result = dateConverter.ConvertToddmyyyy(date);

        if (result is not null)
        {
            return result;
        }

        result = dateConverter.ConvertTodmmyyyy(date);

        if (result is not null)
        {
            return result;
        }

        return dateConverter.ConvertTodmyyyy(date);
    }
}
