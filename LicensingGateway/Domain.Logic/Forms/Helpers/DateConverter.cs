
using System.Globalization;

namespace Domain.Logic.Forms.Helpers;
public class DateConverter : IDateConverter
{
    private const string formatddmmyyyy = "dd/MM/yyyy";
    private const string formatdmmyyyy = "d/MM/yyyy";
    private const string formatddmyyyy = "dd/M/yyyy";
    private const string formatdmyyyy = "d/M/yyyy";
    private readonly CultureInfo culture = CultureInfo.GetCultureInfo("en-GB");
    private readonly DateTimeStyles styles = DateTimeStyles.None;

    public DateOnly? ConvertToddmmyyyy(string input)
    {
        if (DateOnly.TryParseExact(input, formatddmmyyyy, culture, styles, out var date))
        {
            return date;
        }

        return null;
    }

    public DateOnly? ConvertToddmyyyy(string input)
    {
        if(DateOnly.TryParseExact(input, formatddmyyyy, culture, styles, out var date))
        {
            return date;
        }

        return null;
    }

    public DateOnly? ConvertTodmmyyyy(string input)
    {
        if(DateOnly.TryParseExact(input, formatdmmyyyy, culture, styles, out var date))
        {
            return date;
        }

        return null;
    }

    public DateOnly? ConvertTodmyyyy(string input)
    {
        if(DateOnly.TryParseExact(input, formatdmyyyy, culture, styles, out var date))
        {
            return date;
        }

        return null;
    }
}
