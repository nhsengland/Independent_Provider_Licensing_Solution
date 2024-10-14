using Domain.Logic.Forms.Application.Page;
using Licensing.Gateway.Models;

namespace Licensing.Gateway.Factories;

public class ViewModelFactory(
    HomeControllerPageConfiguration homeControllerPageConfiguration) : IViewModelFactory
{
    private readonly HomeControllerPageConfiguration configuration = homeControllerPageConfiguration;

    public HomeViewModel Create()
    {
        return new HomeViewModel() {
            LicenceHolderApplicationURL = configuration.LicenceHolderApplicationURL,
            RegisterOfLicensedProvidersURL = configuration.RegisterOfLicensedProvidersURL,
            LicenceComplianceURL = configuration.LicenceComplianceURL,
            MonitoringAndEnforcementURL = configuration.MonitoringAndEnforcementURL
        };
    }
}
