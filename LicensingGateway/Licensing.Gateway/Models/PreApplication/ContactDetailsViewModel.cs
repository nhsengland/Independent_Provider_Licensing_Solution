using Microsoft.AspNetCore.Mvc;

namespace Licensing.Gateway.Models.PreApplication;

public class ContactDetailsViewModel : ApplicationBase
{
    [BindProperty]
    public string ContactDetails_Forename { get; set; } = default!;

    [BindProperty]
    public string ContactDetails_Surname { get; set; } = default!;

    [BindProperty]
    public string ContactDetails_JobTitle { get; set; } = default!;

    [BindProperty]
    public string ContactDetails_PhoneNumber { get; set; } = default!;

    [BindProperty]
    public string ContactDetails_Email { get; set; } = default!;

    [BindProperty]
    public string ContactDetails_EmailConfirmation { get; set; } = default!;

    public List<(string key, string value)> ContactDetailsValidationFailures { get; set; } = [];
}
