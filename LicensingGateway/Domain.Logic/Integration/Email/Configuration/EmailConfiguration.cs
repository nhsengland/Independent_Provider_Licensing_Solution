namespace Domain.Logic.Integration.Email.Configuration;
public class EmailConfiguration
{
    public string APIBaseAddress { get; set; } = default!;

    public string APIAudienceHeader { get; set; } = default!;

    public string PreApplicationTemplateID { get; set; } = default!;

    public string ApplicationTemplateID { get; set; } = default!;

    public string MainApplicationSaveAndExitTemplateID { get; set; } = default!;
}
