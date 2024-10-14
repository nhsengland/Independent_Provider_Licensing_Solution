using Database.Entites.Core;

namespace Database.Entites;

public class EmailNotificationType : BaseEntity
{
    public Domain.Objects.Database.EmailNotificationType Type { get; set; } = default!;
}
