using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Database.Entites.Core;

namespace Database.Entites;

public class EmailNotification : BaseEntity
{
    public DateTime DateCreated { get; set; }

    public bool HasBeenSent { get; set; }

    public DateTime? DateSent { get; set; }

    public int TypeId { get; set; }

    [Required]
    public EmailNotificationType Type { get; set; } = default!;

    public int UserId { get; set; }

    [Required, ForeignKey(nameof(UserId))]
    public User User { get; set; } = default!;

    public int? RequestedById { get; set; }

    [ForeignKey(nameof(RequestedById))]
    public User? RequestedBy { get; set; } = default!;
}
