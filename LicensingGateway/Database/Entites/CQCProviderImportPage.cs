using System.ComponentModel.DataAnnotations;
using Database.Entites.Core;

namespace Database.Entites;
public class CQCProviderImportPage : BaseIntEntity
{
    [Required]
    public CQCProviderImportInstance CQCProviderImportInstance { get; set; } = default!;

    [Required]
    public int PageNumber { get; set; }

    [Required]
    public string CQCProviderID { get; set; } = default!;

    public int NumberOfAttemptsToImport { get; set; }

    public int? StatusCode { get; set; }
}
