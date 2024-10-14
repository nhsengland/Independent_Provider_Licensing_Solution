namespace Domain.Logic.Forms.Helpers.Session;
public interface ISessionDateHandler
{
    int? GetDay();

    int? GetMonth();

    int? GetYear();

    bool HasValue();

    DateOnly GetDate();

    bool IsValidDate();

    void Reset();

    void Set(DateOnly? date);

    void SetDay(int? value);

    void SetMonth(int? value);

    void SetYear(int? value);

    void UseSessionValues();
}
