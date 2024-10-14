namespace Domain.Logic.Features.Licence;

public interface IDateConverter
{
    DateOnly? ConvertToddmmyyyy(string date);

    DateOnly? ConvertTodmmyyyy(string date);

    DateOnly? ConvertToddmyyyy(string date);

    DateOnly? ConvertTodmyyyy(string date);
}
