namespace Licensing.Gateway.Models.Application;

public class SaveAndNextViewModel
{
    public string SaveAndContinueButtonText { get; set; } = "Save and continue";

    public Domain.Models.Database.ApplicationPage SubmitPageTo { get; set; }
}
