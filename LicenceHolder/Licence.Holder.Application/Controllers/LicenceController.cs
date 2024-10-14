using Domain.Logic.Features.Licence;
using Domain.Logic.Features.Licence.Queries;
using Domain.Logic.Integrations.Session;
using Domain.Objects;
using Licence.Holder.Application.Models.Licence;
using Microsoft.AspNetCore.Mvc;

namespace Licence.Holder.Application.Controllers;

public class LicenceController(
    ILogger<LicenceController> logger,
    ILicenceControllerHandler licenseControllerHandler,
    ISessionOrchestration sessionOrchestration) : BaseController(sessionOrchestration)
{
    private readonly ILogger<LicenceController> logger = logger;
    private readonly ILicenceControllerHandler licenseControllerHandler = licenseControllerHandler;
    private readonly ISessionOrchestration sessionOrchestration = sessionOrchestration;

    [Route("licence/view-and-edit-licence-details")]
    public async Task<IActionResult> Index(int id, CancellationToken cancellationToken)
    {
        sessionOrchestration.Set(PageConstants.Session_License_Id, id);

        return View(await licenseControllerHandler.GetLicenceDetailsAsync(new LicenceRequest()
        {
            LicenseId = id,
            UserOktaId = GetOktaUserId()
        }, cancellationToken));
    }

    [Route("licence/view-and-edit-licence-details/manage-company-details")]
    public async Task<IActionResult> ManageCompanyDetails(
        CancellationToken cancellationToken)
    {
        int licenseId = GetLicenseIdFromSession();

        if (licenseId == 0)
        {
            var error = new InvalidOperationException($"License Id not found in session: {GetOktaUserId()}");
            logger.LogWarning(error, "License Id not found in session");
            return RedirectToAction(nameof(Index));
        }

        return View(await licenseControllerHandler.GetLicenceDetailsAsync(new LicenceRequest()
        {
            LicenseId = licenseId,
            UserOktaId = GetOktaUserId()
        }, cancellationToken));
    }

    [Route("licence/view-and-edit-licence-details/manage-company-details/change-registered-address")]
    public async Task<IActionResult> ChangeRegisteredAddress(
        CancellationToken cancellationToken)
    {
        var licenseId = GetLicenseIdFromSession();

        var licenseDetails = await licenseControllerHandler.GetLicenceDetailsAsync(new LicenceRequest()
        {
            LicenseId = licenseId,
            UserOktaId = GetOktaUserId()
        }, cancellationToken);

        var (line1, line2, townOrCiy, county, postcode) = Get_Session_Address();

        var model = new ChangeRegisteredAddressViewModel()
        {
            Address_Line_1 = line1,
            Address_Line_2 = line2,
            Address_TownOrCity = townOrCiy,
            Address_County = county,
            Address_Postcode = postcode,
            CompanyName = licenseDetails.CompanyName,
            ValidationFailure = GetFromSession_FormValidationFailure_ThenReset()
        };

        sessionOrchestration.Set(PageConstants.Session_Company_Id, licenseDetails.CompanyId);

        return View(model);
    }

    [HttpPost]
    [Route("licence/view-and-edit-licence-details/manage-company-details/change-registered-address")]
    public IActionResult ChangeRegisteredAddress(ChangeRegisteredAddressViewModel model)
    {
        var licenseId = GetLicenseIdFromSession();

        if (licenseId == 0)
        {
            var error = new InvalidOperationException("License Id not found in session");
            logger.LogWarning(error, "License Id not found in session");
            return RedirectToAction(nameof(Index));
        }

        Set_Session_Address(
            model.Address_Line_1,
            model.Address_Line_2,
            model.Address_TownOrCity,
            model.Address_County,
            model.Address_Postcode);

        if (!SessionAddressIsValid())
        {
            Set_Session_FormValidationFailure(true);
            return RedirectToAction();
        }

        return RedirectToAction(nameof(ReviewChangeOfRegisteredAddress));
    }

    [Route("licence/view-and-edit-licence-details/manage-company-details/review-change-of-registered-address")]
    public async Task<IActionResult> ReviewChangeOfRegisteredAddress(
        CancellationToken cancellationToken)
    {
        var (licenseId, _) = CheckSessionVariablesForAddress();

        var licenseDetails = await licenseControllerHandler.GetLicenceDetailsAsync(new LicenceRequest()
        {
            LicenseId = licenseId,
            UserOktaId = GetOktaUserId()
        }, cancellationToken);

        var (line1, line2, townOrCiy, county, postcode) = Get_Session_Address();

        var model = new ChangeRegisteredAddressViewModel()
        {
            Address_Line_1 = line1,
            Address_Line_2 = line2,
            Address_TownOrCity = townOrCiy,
            Address_County = county,
            Address_Postcode = postcode,
            CompanyName = licenseDetails.CompanyName
        };

        return View(model);
    }

    [Route("licence/view-and-edit-licence-details/manage-company-details/cancel-change-of-registered-address")]
    public IActionResult CancelChangeOfRegisteredAddress()
    {
        Remove_Session_Address();

        var licenseId = GetLicenseIdFromSession();

        return RedirectToAction(nameof(Index), new { id = licenseId });
    }

    [Route("licence/view-and-edit-licence-details/manage-company-details/review-change-of-registered-address")]
    [HttpPost]
    public async Task<IActionResult> ReviewChangeOfRegisteredAddress(ChangeRegisteredAddressViewModel _, CancellationToken cancellationToken)
    {
        var (licenseId, companyId) = CheckSessionVariablesForAddress();

        var (line1, line2, townOrCiy, county, postcode) = Get_Session_Address();

        var changeRequest = new CompanyAddressChangeRequest()
        {
            CompanyId = companyId,
            Address_Line_1 = line1,
            Address_Line_2 = line2,
            Address_TownOrCity = townOrCiy,
            Address_County = county,
            Address_Postcode = postcode,
            LicenseId = licenseId,
            UserOktaId = GetOktaUserId(),
        };

        await licenseControllerHandler.Create(changeRequest, cancellationToken);

        Remove_Session_Address();

        return RedirectToAction(nameof(ChangeRegisteredAddressConfirmation));
    }

    [Route("licence/view-and-edit-licence-details/manage-company-details/change-of-registered-address-confirmation")]
    public IActionResult ChangeRegisteredAddressConfirmation()
    {
        return View();
    }

    [Route("licence/view-and-edit-licence-details/manage-company-details/change-company-name")]
    public async Task<IActionResult> ChangeCompanyName(
        CancellationToken cancellationToken)
    {
        var licenseId = GetLicenseIdFromSession();

        var licenseDetails = await licenseControllerHandler.GetLicenceDetailsAsync(new LicenceRequest()
        {
            LicenseId = licenseId,
            UserOktaId = GetOktaUserId()
        }, cancellationToken);

        var companyName = sessionOrchestration.Get<string>(PageConstants.Session_Company_Name);

        var model = new ChangeCompanyNameViewModel()
        {
            CompanyName = companyName ?? string.Empty,
            CurrentCompanyName = licenseDetails.CompanyName
        };

        sessionOrchestration.Set(PageConstants.Session_Company_Id, licenseDetails.CompanyId);

        return View(model);
    }

    [HttpPost]
    [Route("licence/view-and-edit-licence-details/manage-company-details/change-company-name")]
    public IActionResult ChangeCompanyName(ChangeCompanyNameViewModel model)
    {
        var licenseId = GetLicenseIdFromSession();

        if (licenseId == 0)
        {
            var error = new InvalidOperationException("License Id not found in session");
            logger.LogWarning(error, "License Id not found in session");
            return RedirectToAction(nameof(Index));
        }

        if (string.IsNullOrWhiteSpace(model.CompanyName))
        {
            model.ValidationFailure = true;
            return View(model);
        }

        sessionOrchestration.Set(PageConstants.Session_Company_Name, model.CompanyName.Trim());

        return RedirectToAction(nameof(ReviewChangeOfCompanyName));
    }

    [Route("licence/view-and-edit-licence-details/manage-company-details/review-change-of-company-name")]
    public async Task<IActionResult> ReviewChangeOfCompanyName(
        CancellationToken cancellationToken)
    {
        var (licenseId, _, companyName) = CheckSessionVariablesForCompanyName();

        var licenseDetails = await licenseControllerHandler.GetLicenceDetailsAsync(new LicenceRequest()
        {
            LicenseId = licenseId,
            UserOktaId = GetOktaUserId()
        }, cancellationToken);

        var model = new ChangeCompanyNameViewModel()
        {
            CompanyName = companyName,
            CurrentCompanyName = licenseDetails.CompanyName
        };

        return View(model);
    }

    [Route("licence/view-and-edit-licence-details/manage-company-details/cancel-change-of-company-name")]
    public IActionResult CancelChangeOfCompanyName()
    {
        sessionOrchestration.Remove(PageConstants.Session_Company_Name);

        var licenseId = GetLicenseIdFromSession();

        return RedirectToAction(nameof(Index), new { id = licenseId });
    }

    [Route("licence/view-and-edit-licence-details/manage-company-details/review-change-of-company-name")]
    [HttpPost]
    public async Task<IActionResult> ReviewChangeOfCompanyName(ChangeCompanyNameViewModel _, CancellationToken cancellationToken)
    {
        var (licenseId, companyId, companyName) = CheckSessionVariablesForCompanyName();

        var changeRequest = new CompanyNameChangeRequest()
        {
            CompanyId = companyId,
            Name = companyName,
            LicenseId = licenseId,
            UserOktaId = GetOktaUserId(),
        };

        await licenseControllerHandler.Create(changeRequest, cancellationToken);

        sessionOrchestration.Remove(PageConstants.Session_Company_Name);

        return RedirectToAction(nameof(ChangeCompanyNameConfirmation));
    }

    [Route("licence/view-and-edit-licence-details/manage-company-details/change-of-company-name-confirmation")]
    public IActionResult ChangeCompanyNameConfirmation()
    {
        return View();
    }



    [Route("licence/view-and-edit-licence-details/manage-company-details/change-financial-year-end")]
    public async Task<IActionResult> ChangeFinancialYearEnd(
        CancellationToken cancellationToken)
    {
        var licenseId = GetLicenseIdFromSession();

        var licenseDetails = await licenseControllerHandler.GetLicenceDetailsAsync(new LicenceRequest()
        {
            LicenseId = licenseId,
            UserOktaId = GetOktaUserId()
        }, cancellationToken);

        var day = sessionOrchestration.Get<int>(PageConstants.Session_FinancialYearEnd_Day);
        var month = sessionOrchestration.Get<int>(PageConstants.Session_FinancialYearEnd_Month);
        var year = sessionOrchestration.Get<int>(PageConstants.Session_FinancialYearEnd_Year);

        var model = new ChangeFinancialYearEndViewModel()
        {
            Day = day == 0 ? null : day,
            Month = month == 0 ? null : month,
            Year = year == 0 ? null : year,
            CompanyName = licenseDetails.CompanyName
            
        };

        sessionOrchestration.Set(PageConstants.Session_Company_Id, licenseDetails.CompanyId);

        return View(model);
    }

    [HttpPost]
    [Route("licence/view-and-edit-licence-details/manage-company-details/change-financial-year-end")]
    public IActionResult ChangeFinancialYearEnd(ChangeFinancialYearEndViewModel model)
    {
        var licenseId = GetLicenseIdFromSession();

        if (licenseId == 0)
        {
            var error = new InvalidOperationException("License Id not found in session");
            logger.LogWarning(error, "License Id not found in session");
            return RedirectToAction(nameof(Index));
        }

        var date = licenseControllerHandler.IfValidDateConvert($"{model.Day}/{model.Month}/{model.Year}");

        var isValidDate = IsValidDate(date);

        if (model.Day.HasValue == false || model.Month.HasValue == false || model.Year.HasValue == false || !isValidDate)
        {
            model.ValidationFailure = true;
            model.IsValidDate = isValidDate;
            return View(model);
        }

        if (!licenseControllerHandler.IsDateGreaterThanToday(date))
        {
            model.ValidationFailure = true;
            model.IsDateInPast = true;
            return View(model);
        }

        sessionOrchestration.Set(PageConstants.Session_FinancialYearEnd_Day, model.Day.Value);
        sessionOrchestration.Set(PageConstants.Session_FinancialYearEnd_Month, model.Month.Value);
        sessionOrchestration.Set(PageConstants.Session_FinancialYearEnd_Year, model.Year.Value);

        return RedirectToAction(nameof(ReviewChangeOfFinancialYearEnd));
    }

    [Route("licence/view-and-edit-licence-details/manage-company-details/review-change-financial-year-end")]
    public async Task<IActionResult> ReviewChangeOfFinancialYearEnd(
        CancellationToken cancellationToken)
    {
        var (licenseId, _, day, month, year) = CheckSessionVariablesForFYE();

        var licenseDetails = await licenseControllerHandler.GetLicenceDetailsAsync(new LicenceRequest()
        {
            LicenseId = licenseId,
            UserOktaId = GetOktaUserId()
        }, cancellationToken);

        var model = new ChangeFinancialYearEndViewModel()
        {
            Day = day,
            Month = month,
            Year = year,
            CompanyName = licenseDetails.CompanyName,
            NewFinancialYearDate = new DateTime(year, month, day)
        };

        return View(model);
    }

    [Route("licence/view-and-edit-licence-details/manage-company-details/cancel-change-of-financial-year-end")]
    public IActionResult CancelChangeOfFinancialYearEnd()
    {
        sessionOrchestration.Remove(PageConstants.Session_FinancialYearEnd_Day);
        sessionOrchestration.Remove(PageConstants.Session_FinancialYearEnd_Month);
        sessionOrchestration.Remove(PageConstants.Session_FinancialYearEnd_Year);

        var licenseId = GetLicenseIdFromSession();

        return RedirectToAction(nameof(Index), new { id = licenseId });
    }

    [Route("licence/view-and-edit-licence-details/manage-company-details/review-change-financial-year-end")]
    [HttpPost]
    public async Task<IActionResult> ReviewChangeOfFinancialYearEnd(ChangeFinancialYearEndViewModel _, CancellationToken cancellationToken)
    {
        var (licenseId, companyId, day, month, year) = CheckSessionVariablesForFYE();

        var changeRequest = new CompanyFYEChangeRequest()
        {
            CompanyId = companyId,
            LicenseId = licenseId,
            UserOktaId = GetOktaUserId(),
            FinancialYearEnd = new DateOnly(year, month, day)
        };

        await licenseControllerHandler.Create(changeRequest, cancellationToken);

        sessionOrchestration.Remove(PageConstants.Session_FinancialYearEnd_Day);
        sessionOrchestration.Remove(PageConstants.Session_FinancialYearEnd_Month);
        sessionOrchestration.Remove(PageConstants.Session_FinancialYearEnd_Year);

        return RedirectToAction(nameof(ChangeFinancialYearEndConfirmation));
    }

    [Route("licence/view-and-edit-licence-details/manage-company-details/change-financial-year-end-confirmation")]
    public IActionResult ChangeFinancialYearEndConfirmation()
    {
        return View();
    }

    [Route("licence/feedback")]
    public IActionResult Feedback()
    {
        Set_Session_FeedbackType(Domain.Objects.Database.FeedbackType.Licence);

        return RedirectToAction("Index", "Feedback");
    }

    private (int licenseId, int companyId, string companyName) CheckSessionVariablesForCompanyName()
    {
        var licenseId = GetLicenseIdFromSession();
        var companyId = sessionOrchestration.Get<int>(PageConstants.Session_Company_Id);
        var companyName = sessionOrchestration.Get<string>(PageConstants.Session_Company_Name);

        if (licenseId == 0 || companyId == 0 || string.IsNullOrWhiteSpace(companyName))
        {
            var error = new InvalidOperationException($"Either LicenseId: {licenseId}, CompanyID: {companyId}, or company name: '{companyName}' not found in session");
            logger.LogError(error, "Required properties not found in session");
            throw error;
        }

        return (licenseId, companyId, companyName);
    }

    private (int licenseId, int companyId) CheckSessionVariablesForAddress()
    {
        var licenseId = GetLicenseIdFromSession();
        var companyId = sessionOrchestration.Get<int>(PageConstants.Session_Company_Id);
        var address = Get_Session_Address();

        if (licenseId == 0 || companyId == 0 || !SessionAddressIsValid())
        {
            var error = new InvalidOperationException($"Either LicenseId: {licenseId}, CompanyID: {companyId}, or address: '{address}' not found in session");
            logger.LogError(error, "Required properties not found in session");
            throw error;
        }

        return (licenseId, companyId);
    }

    private (int licenseId, int companyId, int day, int month, int year) CheckSessionVariablesForFYE()
    {
        var licenseId = GetLicenseIdFromSession();
        var companyId = sessionOrchestration.Get<int>(PageConstants.Session_Company_Id);
        var day = sessionOrchestration.Get<int>(PageConstants.Session_FinancialYearEnd_Day);
        var month = sessionOrchestration.Get<int>(PageConstants.Session_FinancialYearEnd_Month);
        var year = sessionOrchestration.Get<int>(PageConstants.Session_FinancialYearEnd_Year);

        if (licenseId == 0 || companyId == 0 || day == 0 || month == 0 || year == 0)
        {
            var error = new InvalidOperationException($"Either LicenseId: {licenseId}, CompanyID: {companyId}, day: '{day}', or month: '{month}', or year: '{year}' not found in session");
            logger.LogError(error, "Required properties not found in session");
            throw error;
        }

        return (licenseId, companyId, day, month, year);
    }

    private bool SessionAddressIsValid()
    {
        var (line1, _, townOrCity, _, postcode) = Get_Session_Address();

        if(string.IsNullOrWhiteSpace(line1)
            || string.IsNullOrWhiteSpace(townOrCity)
            || string.IsNullOrWhiteSpace(postcode))
        {
            return false;
        }

        return true;
    }

    private static bool IsValidDate(DateOnly? date)
    {
        return date != null;
    }

    private int GetLicenseIdFromSession()
    {
        return sessionOrchestration.Get<int>(PageConstants.Session_License_Id);
    }
}
