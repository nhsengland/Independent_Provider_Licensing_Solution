using System.ComponentModel.DataAnnotations;
using Database.Entites.Core;

namespace Database.Entites;
public class PreApplication : BaseIntEntity
{
    public DateTime DateGenerated { get; set; }

    [Length(5, 100)]
    public string? ReferenceId { get; set; } = default!;

    [Required]
    public string CQCProviderID { get; set; } = default!;

    [Required]
    public string CQCProviderName { get; set; } = default!;

    [Required]
    public string CQCProviderAddress { get; set; } = default!;

    [Required]
    public string CQCProviderPhoneNumber { get; set; } = default!;

    [Required]
    public bool IsHealthCareService { get; set; }

    [Required]
    public bool ConfirmationOfRegulatedActivities { get; set; }

    [Required]
    public string RegulatedActivities { get; set; } = default!;

    [Required]
    public bool IsExclusive { get; set; }

    [Required]
    public string Turnover { get; set; } = default!;

    [Required]
    public string FirstName { get; set; } = default!;

    [Required]
    public string LastName { get; set; } = default!;

    [Required]
    public string JobTitle { get; set; } = default!;

    [Required]
    public string PhoneNumber { get; set; } = default!;

    [Required, Length(1, 254)]
    public string Email { get; set; } = default!;
}
