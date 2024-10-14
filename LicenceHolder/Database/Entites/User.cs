using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Database.Entites.Core;
using Microsoft.EntityFrameworkCore;

namespace Database.Entites;

[Table("User")]
[Index(nameof(OktaId), Name = "IX_User_OktaId")]
[Index(nameof(Email), Name = "IX_User_Email", IsUnique = true)]
public partial class User : BaseEntity
{
    public int OrganisationId { get; set; }

    [StringLength(50)]
    public string? OktaId { get; set; } = null!;

    [Required, StringLength(100)]
    public string Firstname { get; set; } = null!;

    [Required, StringLength(100)]
    public string Lastname { get; set; } = null!;

    [Required, Length(1, 254)]
    public string Email { get; set; } = null!;

    public bool EmailIsVerified { get; set; }

    public int UserRoleId { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateLastModified { get; set; } = DateTime.UtcNow;

    [DefaultValue(false)]
    public bool IsDeleted { get; set; }

    [ForeignKey(nameof(OrganisationId))]
    [InverseProperty(nameof(Organisation.Users))]
    public virtual Organisation Organisation { get; set; } = null!;

    [ForeignKey(nameof(UserRoleId))]
    [InverseProperty(nameof(UserRole.Users))]
    [Required]
    public virtual UserRole UserRole { get; set; } = null!;

    public Guid? EntraId { get; set; } = null!;
}
