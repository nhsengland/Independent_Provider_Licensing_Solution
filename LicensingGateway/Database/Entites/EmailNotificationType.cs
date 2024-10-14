using Database.Entites.Core;

namespace Database.Entites;

public class EmailNotificationType : BaseIntEntity
{
    public Domain.Models.Database.EmailNotificationType Type { get; set; } = default!; 
}
