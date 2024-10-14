using Domain.Models.Database.DTO;

namespace Domain.Logic.Forms.Application.Helpers;
public class ContactDetailsEvaluator : IContactDetailsEvaluator
{
    public bool ContactDetailsAreEmpty(ContactDetailsDTO contactDetails)
    {
        if (!string.IsNullOrWhiteSpace(contactDetails.Forename))
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(contactDetails.Surname))
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(contactDetails.JobTitle))
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(contactDetails.Email))
        {
            return false;
        }

        return true;
    }
}
