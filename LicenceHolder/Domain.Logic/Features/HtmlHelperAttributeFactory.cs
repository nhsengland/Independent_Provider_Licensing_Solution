namespace Domain.Logic.Features;

public static class HtmlHelperAttributeFactory
{
    public static object CreateForAddress(string nameofElement, string value)
    {
        return new { @id = nameofElement, @class = "nhsuk-textarea", autocomplete = "street-address", spellcheck = "false", rows = 5, value = value };
    }

    public static object CreateForDate_DayOrMonth(string nameofElement)
    {
        return new { @id = nameofElement, @class = "nhsuk-input nhsuk-input--width-2", spellcheck = "false", pattern = "[0-9]{1,2}" };
    }

    public static object CreateForDate_Year(string nameofElement)
    {
        return new { @id = nameofElement, @class = "nhsuk-input nhsuk-input--width-4", spellcheck = "false", pattern = "[0-9]{4}" };
    }

    public static object CreateForEmail(string nameofElement)
    {
        return new { @id = nameofElement, @class = "nhsuk-input  nhsuk-u-width-one-third", autocomplete = "email", type = "email", spellcheck = "false", size = "254" };
    }
}
