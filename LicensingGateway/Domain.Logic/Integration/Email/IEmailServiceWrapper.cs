using Domain.Logic.Integration.Email.Models;

namespace Domain.Logic.Integration.Email;
public interface IEmailServiceWrapper
{
    Task<bool> Send(EmailBodyTemplate template);
}
