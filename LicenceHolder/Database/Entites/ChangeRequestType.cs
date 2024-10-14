using Database.Entites.Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entites
{
    [Table(nameof(ChangeRequestType))]
    public class ChangeRequestType : BaseEntity
    {
        public Domain.Objects.Database.ChangeRequestType Type { get; set; }
    }
}
