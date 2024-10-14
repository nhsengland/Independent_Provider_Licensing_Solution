namespace Domain.Logic.Integration.Email.Models;
public record EmailBodyTemplate
{
    public string EmailAddress { get; set; } = default!;
    public string TemplateId { get; set; } = default!;
    public dynamic Personalisation { get; set; } = default!;
}

public record Personalisation
{
    public string firstname { get; set; } = default!;
    public string applicationReferenceID { get; set; } = default!;
}

public record PersonalisationSaveAndExit
{
    public string applicationURL { get; set; } = default!;
}
