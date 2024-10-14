using System.ComponentModel.DataAnnotations;
using Database.Entites.Core;
using Microsoft.EntityFrameworkCore;

namespace Database.Entites;

[Index(nameof(Code), IsUnique = true)]
public class ApplicationCode : BaseIntEntity
{
    [Required, Length(7, 7)]
    public string Code { get; set; } = default!;

    public bool IsCRS { get; set; }

    public bool IsHardToReplace { get; set; }

    public int? PreApplicationId { get; set; }

    public PreApplication? PreApplication { get; set; }

    public Application? Application { get; set; } = default!;

    [MaxLength(50)]
    public string? OktaUserId { get; set; }

    public string? NoPreApplication_CQCProviderName { get; set; }

    public string? NoPreApplication_FirstName { get; set; }

    public string? NoPreApplication_LastName { get; set; }

    [Length(1, 254)]
    public string? NoPreApplication_Email { get; set; }

    public bool IsUserDeleted { get; set; }
}
