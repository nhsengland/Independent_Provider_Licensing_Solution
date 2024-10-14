using Domain.Logic.Features.Licence.Queries;
using Domain.Objects.ViewModels.Licence;

namespace Domain.Logic.Features.Licence;

public interface ILicenceControllerHandler
{
    Task Create(CompanyAddressChangeRequest request, CancellationToken cancellationToken);

    Task Create(CompanyNameChangeRequest request, CancellationToken cancellationToken);

    Task Create(CompanyFYEChangeRequest request, CancellationToken cancellationToken);

    Task<LicenceDetailsViewModel> GetLicenceDetailsAsync(LicenceRequest request, CancellationToken cancellationToken);

    DateOnly? IfValidDateConvert(string date);

    bool IsDateGreaterThanToday(DateOnly? date);
}
