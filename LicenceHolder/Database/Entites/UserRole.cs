using System.ComponentModel.DataAnnotations.Schema;
using Database.Entites.Core;

namespace Database.Entites;

[Table("UserRole")]
public partial class UserRole : BaseEntity
{
    public Domain.Objects.Database.UserRole Role { get; set; }

    [InverseProperty(nameof(User.UserRole))]
    public virtual ICollection<User> Users { get; set; } = [];
}
