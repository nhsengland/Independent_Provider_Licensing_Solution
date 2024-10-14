using System.ComponentModel.DataAnnotations;
using Database.Entites.Core;

namespace Database.Entites;
public class EmailNotification : BaseIntEntity
{
    public DateTime DateGenerated { get; set; }

    public int? PreApplicationId { get; set; }

    public PreApplication? PreApplication { get; set; }

    public int? ApplicationId { get; set; }

    public Application? Application { get; set; }

    public string? ApplicationURL { get; set; }

    public bool HasBeenSent { get; set; }

    public DateTime? DateSent { get; set; }

    public int TypeId { get; set; }

    [Required]
    public EmailNotificationType Type { get; set; } = default!;
}
