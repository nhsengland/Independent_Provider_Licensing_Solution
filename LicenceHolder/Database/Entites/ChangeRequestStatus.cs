using Database.Entites.Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entites
{
    [Table(nameof(ChangeRequestStatus))]
    public class ChangeRequestStatus : BaseEntity
    {
        public Domain.Objects.Database.ChangeRequestStatus Status { get; set; }
    }
}
