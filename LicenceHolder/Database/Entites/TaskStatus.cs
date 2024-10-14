using Database.Entites.Core;

namespace Database.Entites
{
    public class TaskStatus : BaseEntity
    {
        public Domain.Objects.Database.TaskStatus Status { get; set; }
    }
}
