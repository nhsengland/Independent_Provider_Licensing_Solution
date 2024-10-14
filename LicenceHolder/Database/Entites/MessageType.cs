using Database.Entites.Core;

namespace Database.Entites;

public class MessageType : BaseEntity
{
    public Domain.Objects.Database.MessageType Type { get; set; }
}
