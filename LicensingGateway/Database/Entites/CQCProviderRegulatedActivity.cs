using Database.Entites.Core;
using Microsoft.EntityFrameworkCore;

namespace Database.Entites;

[Index(nameof(Code), Name = "IX_CQCProviderRegulatedActivities_Code")]
public class CQCProviderRegulatedActivity : BaseIntEntity
{
    public string Name { get; set; } = default!;

    public string Code { get; set; } = default!;   
}
