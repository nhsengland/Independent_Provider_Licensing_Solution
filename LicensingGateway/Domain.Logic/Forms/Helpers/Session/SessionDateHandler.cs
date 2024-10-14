using Domain.Logic.Forms.Application;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Domain.Logic.Forms.Helpers.Session;

public class SessionDateHandler(
    ISessionOrchestration sessionOrchestration,
    IDateEvaluation dateEvaluation) : ISessionDateHandler
{
    private readonly ISessionOrchestration sessionOrchestration = sessionOrchestration;
    private readonly IDateEvaluation dateEvaluation = dateEvaluation;

    public DateOnly GetDate()
    {
        return dateEvaluation.EvaluateDate($"{GetDay()}/{GetMonth()}/{GetYear()}") ?? throw new InvalidOperationException("Unable to evaluate date because its an invalid date");
    }

    public int? GetDay()
    {
        return sessionOrchestration.Get<int?>(ApplicationFormConstants.Session_Date_Day);
    }

    public int? GetMonth()
    {
        return sessionOrchestration.Get<int?>(ApplicationFormConstants.Session_Date_Month);
    }

    public int? GetYear()
    {
        return sessionOrchestration.Get<int?>(ApplicationFormConstants.Session_Date_Year);
    }

    public bool HasValue()
    {
        return sessionOrchestration.Get<bool>(ApplicationFormConstants.Session_Date_UseSessionValues);
    }

    public bool IsValidDate()
    {
        var day = GetDay();
        var month = GetMonth();
        var year = GetYear();

        return dateEvaluation.EvaluateDate($"{day}/{month}/{year}") != null;
    }

    public void Reset()
    {
        sessionOrchestration.Remove(ApplicationFormConstants.Session_Date_Day);
        sessionOrchestration.Remove(ApplicationFormConstants.Session_Date_Month);
        sessionOrchestration.Remove(ApplicationFormConstants.Session_Date_Year);
        sessionOrchestration.Remove(ApplicationFormConstants.Session_Date_UseSessionValues);
    }

    public void Set(DateOnly? date)
    {
        if(date == null)
        {
            SetDay(null);
            SetMonth(null);
            SetYear(null);
        }
        else
        {
            SetDay(date.Value.Day);
            SetMonth(date.Value.Month);
            SetYear(date.Value.Year);
        }
    }

    public void SetDay(int? value)
    {
        HandleSet(ApplicationFormConstants.Session_Date_Day, value);
    }

    public void SetMonth(int? value)
    {
        HandleSet(ApplicationFormConstants.Session_Date_Month, value);
    }

    public void SetYear(int? value)
    {
        HandleSet(ApplicationFormConstants.Session_Date_Year, value);
    }

    public void UseSessionValues()
    {
        sessionOrchestration.Set(ApplicationFormConstants.Session_Date_UseSessionValues, true);
    }

    private void HandleSet(string key, int? value)
    {
        if(value == null)
        {
            sessionOrchestration.Remove(key);
            return;
        }

        sessionOrchestration.Set(key, value);
    }
}
