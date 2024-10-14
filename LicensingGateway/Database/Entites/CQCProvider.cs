using System.ComponentModel.DataAnnotations;
using Database.Entites.Core;
using Microsoft.EntityFrameworkCore;

namespace Database.Entites;

[Index(nameof(CQCProviderID), Name = "IX_CQCProvider_CQCProviderID")]
public class CQCProvider : BaseIntEntity
{
    [Required]
    public string Name { get; set; } = default!;

    public string? Address_Line_1 { get; set; } = default!;

    public string? Address_Line_2 { get; set; } = default!;

    public string? TownCity { get; set; } = default!;

    public string? Region { get; set; } = default!;

    public string? Postcode { get; set; } = default!;

    public string? PhoneNumber { get; set; } = default!;

    public string? WebsiteURL { get; set; } = default!;

    [Required, Length(5, 20)]
    public string CQCProviderID { get; set; } = default!;

    public virtual List<CQCProviderToRegulatedActivities> CQCProviderToRegulatedActivities { get; set; } = [];
}
