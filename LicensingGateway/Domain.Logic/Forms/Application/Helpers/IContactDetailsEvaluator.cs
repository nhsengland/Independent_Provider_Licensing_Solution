using Domain.Models.Database.DTO;

namespace Domain.Logic.Forms.Application.Helpers;
public interface IContactDetailsEvaluator
{
    bool ContactDetailsAreEmpty(ContactDetailsDTO contactDetails);
}
