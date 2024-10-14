namespace Domain.Logic.Forms.Helpers;

public static class HtmlHelperAttributeFactory
{
    public static object CreateForEmail(string nameofElement)
    {
        return new { @id = nameofElement, @class = "nhsuk-input", autocomplete = "email", type = "email", spellcheck = "false", size = "254" };
    }

    public static object CreateForForename(string nameofElement)
    {
        return new { @id = nameofElement, @class = "nhsuk-input", autocomplete = "given-name", spellcheck = "false" };
    }

    public static object CreateForSurname(string nameofElement)
    {
        return new { @id = nameofElement, @class = "nhsuk-input", autocomplete = "family-name", spellcheck = "false" };
    }

    public static object CreateForPhoneNumber(string nameofElement)
    {
        return new { @id = nameofElement, @class = "nhsuk-input", autocomplete = "tel", spellcheck = "false" };
    }

    public static object CreateForAddress(string nameofElement)
    {
        return new { @id = nameofElement, @class = "nhsuk-textarea", autocomplete = "street-address", spellcheck = "false", rows = 5 };
    }

    public static object CreateForDate_DayOrMonth(string nameofElement, string className = "")
    {
        return new { @id = nameofElement, @class = $"nhsuk-input nhsuk-input--width-2 {className}", spellcheck = "false", pattern = "[0-9]{1,2}" };
    }

    public static object CreateForDate_Year(string nameofElement, string className = "")
    {
        return new { @id = nameofElement, @class = $"nhsuk-input nhsuk-input--width-4 {className}", spellcheck = "false", pattern = "[0-9]{4}" };
    }

    public static object CreateForApplicationCode(string nameofElement)
    {
        return new { @id = nameofElement, @class = "nhsuk-input  nhsuk-input--width-10", spellcheck = "false", pattern = "[A-z0-9À-ž]{7}" };
    }

    public static object CreateForCompanyNumber(string nameofElement)
    {
        return new { @id = nameofElement, @class = "nhsuk-input  nhsuk-input--width-10", spellcheck = "false", pattern = "[A-z0-9À-ž]{8}" };
    }

    public static object CreateForCheckBox(string nameofElement)
    {
        return new { @id = nameofElement, @class = "nhsuk-checkboxes__input" };
    }
}
