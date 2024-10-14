using Domain.Logic.Forms.Feedback;
using Domain.Logic.Forms.Helpers.Session;
using Domain.Logic.Forms.PreApplication;
using Licensing.Gateway.Models;
using Licensing.Gateway.Models.PreApplication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Licensing.Gateway.Controllers;

public class PreApplicationController(
    ILogger<PreApplicationController> logger,
    IPreApplicationOrchestration preApplicationOrchestration,
    ISessionOrchestration sessionOrchestration) : Controller
{
    private readonly ILogger logger = logger;
    private readonly IPreApplicationOrchestration preApplicationOrchestration = preApplicationOrchestration;
    private readonly ISessionOrchestration sessionOrchestration = sessionOrchestration;

    [Route("application-part-one/start")]
    public IActionResult Index()
    {
        return View(new IndexViewModel());
    }

    [HttpPost]
    [ActionName("Index")]
    [Route("application-part-one/start")]
    public IActionResult IndexPost()
    {
        SetSessionFormValidationError(false);

        sessionOrchestration.Set(PreApplicationFormConstants.PreApplicationReviewPageReached, false);

        return RedirectToAction(nameof(IsRegisteredWithCQC));
    }

    [Route("application-part-one/cqc-registered")]
    public IActionResult IsRegisteredWithCQC()
    {
        if(sessionOrchestration.Any() == false)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        return View(new IsRegisteredWithCQCViewModel() {
            IsCQCRegistered = sessionOrchestration.Get<string>(PreApplicationFormConstants.PreApplication_Response_IsRegisteredWithCQC) ?? "",
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        });
    }

    [HttpPost]
    [Route("application-part-one/cqc-registered")]
    public IActionResult IsRegisteredWithCQC(IsRegisteredWithCQCViewModel model)
    {
        if (sessionOrchestration.Any() == false)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        sessionOrchestration.Set(PreApplicationFormConstants.CQCProviderNotFound, false);

        sessionOrchestration.Set(PreApplicationFormConstants.PreApplication_Response_IsRegisteredWithCQC, model.IsCQCRegistered);

        if (string.IsNullOrWhiteSpace(model.IsCQCRegistered))
        {
            SetSessionFormValidationError(true);
        }

        if (model.IsCQCRegistered == PreApplicationFormConstants.Yes)
        {
            SetSessionFormValidationError(false);

            return RedirectToAction(nameof(EnterYourCQCProviderID));
        }

        if (model.IsCQCRegistered == PreApplicationFormConstants.No)
        {
            SetSessionFormValidationError(false);
        }

        return RedirectToAction();
    }

    [Route("application-part-one/cqc-provider")]
    public IActionResult EnterYourCQCProviderID()
    {
        if (sessionOrchestration.Any() == false)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        return View(new EnterYourCQCProviderIDViewModel()
        {
            CQCProviderID = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_CQCProvider_ID) ?? "",
            ValidationFailure =  GetThenResetFormValidationErrorFromSession()
        });
    }

    [HttpPost]
    [Route("application-part-one/cqc-provider")]
    public async Task<IActionResult> EnterYourCQCProviderID(EnterYourCQCProviderIDViewModel model)
    {
        if (sessionOrchestration.Any() == false)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        sessionOrchestration.Set(PreApplicationFormConstants.Response_CQCProvider_ID, model.CQCProviderID?.Trim());

        SetSessionFormValidationError(false);

        if (string.IsNullOrWhiteSpace(model.CQCProviderID))
        {
            sessionOrchestration.Set(PreApplicationFormConstants.CQCProviderNotFound, false);

            SetSessionFormValidationError(true);

            return RedirectToAction();
        }

        var result = await preApplicationOrchestration.OrchestrateQCVProvider(model.CQCProviderID.Trim());

        if (result == null)
        {
            sessionOrchestration.Set(PreApplicationFormConstants.CQCProviderNotFound, true);

            return RedirectToAction();
        }

        if (result.HasLicence)
        {
            return RedirectToAction(nameof(CQCProviderHasActiveLicence));
        }

        sessionOrchestration.Remove(PreApplicationFormConstants.CQCProviderNotFound);

        sessionOrchestration.Set(PreApplicationFormConstants.Response_CQCProvider_Name, result.Name.Trim());

        sessionOrchestration.Set(PreApplicationFormConstants.Response_CQCProvider_Address, result.Address.Trim());

        sessionOrchestration.Set(PreApplicationFormConstants.Response_CQCProvider_PhoneNumber, result.PhoneNumber.Trim());

        return RedirectToAction(nameof(ConfirmCQCProviderInformation));
    }

    [Route("application-part-one/cqc-provider-has-active-licence")]
    public IActionResult CQCProviderHasActiveLicence()
    {
        if (sessionOrchestration.Any() == false)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        return View();
    }

    [Route("application-part-one/confirm-cqc-provider")]
    public IActionResult ConfirmCQCProviderInformation()
    {
        if (sessionOrchestration.Any() == false)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        return View(new ConfirmCQCProviderInformationViewModel()
        {
            CQCInformationIsCorrect = sessionOrchestration.Get<string>(PreApplicationFormConstants.PreApplication_Response_CQCInformationIsCorrect) ?? "",
            CQCProviderID = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_CQCProvider_ID) ?? "",
            CQCProvider_Name = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_CQCProvider_Name) ?? "",
            CQCProvider_Address = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_CQCProvider_Address) ?? "",
            CQCProvider_PhoneNumber = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_CQCProvider_PhoneNumber) ?? "",
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        });
    }

    [HttpPost]
    [Route("application-part-one/confirm-cqc-provider")]
    public IActionResult ConfirmCQCProviderInformation(ConfirmCQCProviderInformationViewModel model)
    {
        if (sessionOrchestration.Any() == false)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        sessionOrchestration.Set(PreApplicationFormConstants.PreApplication_Response_CQCInformationIsCorrect, model.CQCInformationIsCorrect);

        SetSessionFormValidationError(false);

        var cqcDataIsValid = CQCDataInSessionIsValid();

        if (model.CQCInformationIsCorrect == PreApplicationFormConstants.Yes & cqcDataIsValid)
        {
            return RedirectToAction(nameof(ProvidesHealthCareServices));
        }

        if (string.IsNullOrWhiteSpace(model.CQCInformationIsCorrect) || cqcDataIsValid == false)
        {
            SetSessionFormValidationError(true);
        }

        return RedirectToAction();
    }

    [Route("application-part-one/change-cqc-provider-address")]
    public IActionResult CQCProviderChangeAddress()
    {
        if (sessionOrchestration.Any() == false)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        return View(new CQCProviderDetailsViewModel()
        {
            Address = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_CQCProvider_Address) ?? "",
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        });
    }

    [HttpPost]
    [Route("application-part-one/change-cqc-provider-address")]
    public IActionResult CQCProviderChangeAddress(CQCProviderDetailsViewModel model)
    {
        if (sessionOrchestration.Any() == false)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        sessionOrchestration.Set(PreApplicationFormConstants.Response_CQCProvider_Address, model.Address);

        if (string.IsNullOrWhiteSpace(model.Address))
        {
            SetSessionFormValidationError(true);
            return RedirectToAction();
        }

        SetSessionFormValidationError(false);

        return ReturnToReivewOrRedirectToAction(nameof(ConfirmCQCProviderInformation));
    }

    [Route("application-part-one/change-cqc-provider-name")]
    public IActionResult CQCProviderChangeName()
    {
        if (sessionOrchestration.Any() == false)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        return View(new CQCProviderDetailsViewModel()
        {
            Name = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_CQCProvider_Name) ?? "",
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        });
    }

    [HttpPost]
    [Route("application-part-one/change-cqc-provider-name")]
    public IActionResult CQCProviderChangeName(CQCProviderDetailsViewModel model)
    {
        if (sessionOrchestration.Any() == false)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        sessionOrchestration.Set(PreApplicationFormConstants.Response_CQCProvider_Name, model.Name);

        if (string.IsNullOrWhiteSpace(model.Name))
        {
            SetSessionFormValidationError(true);
            return RedirectToAction();
        }

        SetSessionFormValidationError(false);

        return ReturnToReivewOrRedirectToAction(nameof(ConfirmCQCProviderInformation));
    }

    [Route("application-part-one/change-cqc-provider-phone-number")]
    public IActionResult CQCProviderChangePhoneNumber()
    {
        if (sessionOrchestration.Any() == false)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        return View(new CQCProviderDetailsViewModel()
        {
            PhoneNumber = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_CQCProvider_PhoneNumber) ?? "",
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        });
    }

    [HttpPost]
    [Route("application-part-one/change-cqc-provider-phone-number")]
    public IActionResult CQCProviderChangePhoneNumber(CQCProviderDetailsViewModel model)
    {
        if (sessionOrchestration.Any() == false)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        sessionOrchestration.Set(PreApplicationFormConstants.Response_CQCProvider_PhoneNumber, model.PhoneNumber);

        if (string.IsNullOrWhiteSpace(model.PhoneNumber))
        {
            SetSessionFormValidationError(true);
            return RedirectToAction();
        }

        SetSessionFormValidationError(false);

        return ReturnToReivewOrRedirectToAction(nameof(ConfirmCQCProviderInformation));
    }

    [Route("application-part-one/provides-health-care-services")]
    public IActionResult ProvidesHealthCareServices()
    {
        if (sessionOrchestration.Any() == false)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        return View(new ProvidesHealthCareServicesViewModel()
        {
            ProvidesHealthCareService = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_ProvidesHealthCareService) ?? "",
            ProviderName = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_CQCProvider_Name) ?? "",
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        });
    }

    [HttpPost]
    [Route("application-part-one/provides-health-care-services")]
    public IActionResult ProvidesHealthCareServices(ProvidesHealthCareServicesViewModel model)
    {
        if (sessionOrchestration.Any() == false)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        sessionOrchestration.Set(PreApplicationFormConstants.Response_ProvidesHealthCareService, model.ProvidesHealthCareService);

        SetSessionFormValidationError(false);

        if (model.ProvidesHealthCareService == PreApplicationFormConstants.Yes)
        {
            return ReturnToReivewOrRedirectToAction(nameof(CQCRegulatedActivites));
        }

        if (string.IsNullOrWhiteSpace(model.ProvidesHealthCareService))
        {
            SetSessionFormValidationError(true);
        }

        return RedirectToAction();
    }

    [Route("application-part-one/cqc-regulated-activities")]
    public async Task<IActionResult> CQCRegulatedActivites()
    {
        if (sessionOrchestration.Any() == false)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        var cqcProviderId = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_CQCProvider_ID) ?? string.Empty;

        var providerName = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_CQCProvider_Name) ?? string.Empty;

        return View(new CQCRegulatedActivitesViewModel()
        {
            Confirmation = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_CQCRegulatedActivites) ?? "",
            Activities = await preApplicationOrchestration.GetCQCProviderRequlatedActivites(cqcProviderId),
            ProviderID = cqcProviderId,
            ProviderName = providerName,
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        });
    }

    [HttpPost]
    [Route("application-part-one/cqc-regulated-activities")]
    public IActionResult CQCRegulatedActivites(CQCRegulatedActivitesViewModel model)
    {
        if (sessionOrchestration.Any() == false)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        sessionOrchestration.Set(PreApplicationFormConstants.Response_CQCRegulatedActivites, model.Confirmation);

        SetSessionFormValidationError(false);

        if (string.IsNullOrWhiteSpace(model.Confirmation) || model.Confirmation == PreApplicationFormConstants.No)
        {
            if (string.IsNullOrWhiteSpace(model.Confirmation))
            {
                SetSessionFormValidationError(true);
            }

            return RedirectToAction();
        }

        return ReturnToReivewOrRedirectToAction(nameof(ExclusiveServices));
    }

    [Route("application-part-one/exclusive-services")]
    public IActionResult ExclusiveServices()
    {
        if (sessionOrchestration.Any() == false)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        return View(new ExclusiveServicesViewModel()
        {
            ProviderName = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_CQCProvider_Name) ?? "",
            ExclusiveServices = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_ExclusiveServices) ?? "",
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        });
    }

    [HttpPost]
    [Route("application-part-one/exclusive-services")]
    public IActionResult ExclusiveServices(ExclusiveServicesViewModel model)
    {
        if (sessionOrchestration.Any() == false)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        sessionOrchestration.Set(PreApplicationFormConstants.Response_ExclusiveServices, model.ExclusiveServices);

        SetSessionFormValidationError(false);

        if (model.ExclusiveServices == PreApplicationFormConstants.No)
        {
            return ReturnToReivewOrRedirectToAction(nameof(Earnings));
        }
        else if (string.IsNullOrWhiteSpace(model.ExclusiveServices))
        {
            SetSessionFormValidationError(true);
        }

        return RedirectToAction();
    }

    [Route("application-part-one/earnings")]
    public IActionResult Earnings()
    {
        if (sessionOrchestration.Any() == false)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        return View(new EarningsViewModel()
        {
            Earnings = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_Earnings) ?? "",
            ProviderName = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_CQCProvider_Name) ?? "",
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        });
    }

    [HttpPost]
    [Route("application-part-one/earnings")]
    public IActionResult Earnings(EarningsViewModel model)
    {
        if (sessionOrchestration.Any() == false)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        sessionOrchestration.Set(PreApplicationFormConstants.Response_Earnings, model.Earnings);

        SetSessionFormValidationError(false);

        if (model.Earnings == model.EarningValues.First() || model.Earnings == model.EarningValues[1])
        {
            return ReturnToReivewOrRedirectToAction(nameof(ContactDetails));
        }
        else if (string.IsNullOrWhiteSpace(model.Earnings))
        {
            SetSessionFormValidationError(true);
        }

        return RedirectToAction();
    }

    [Route("application-part-one/contact-details")]
    public IActionResult ContactDetails()
    {
        if (sessionOrchestration.Any() == false)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        var model = new ContactDetailsViewModel()
        {
            ContactDetails_Forename = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_ContactDetails_Forename) ?? "",
            ContactDetails_Surname = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_ContactDetails_Surname) ?? "",
            ContactDetails_JobTitle = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_ContactDetails_JobTitle) ?? "",
            ContactDetails_Email = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_ContactDetails_Email) ?? "",
            ContactDetails_EmailConfirmation = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_ContactDetails_EmailConfirmation) ?? "",
            ContactDetails_PhoneNumber = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_ContactDetails_PhoneNumber) ?? "",
            ValidationFailure = GetFormValidationErrorFromSession()
        };

        if (GetThenResetFormValidationErrorFromSession())
        {
            model.ContactDetailsValidationFailures = preApplicationOrchestration.EvaluateContactFormForValidationFailures(
                model.ContactDetails_Forename,
                model.ContactDetails_Surname,
                model.ContactDetails_JobTitle,
                model.ContactDetails_PhoneNumber,
                model.ContactDetails_Email,
                model.ContactDetails_EmailConfirmation);
        }

        return View(model);
    }

    [HttpPost]
    [Route("application-part-one/contact-details")]
    public IActionResult ContactDetails(ContactDetailsViewModel model)
    {
        if (sessionOrchestration.Any() == false)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        sessionOrchestration.Set(PreApplicationFormConstants.Response_ContactDetails_Forename, model.ContactDetails_Forename);

        sessionOrchestration.Set(PreApplicationFormConstants.Response_ContactDetails_Surname, model.ContactDetails_Surname);

        sessionOrchestration.Set(PreApplicationFormConstants.Response_ContactDetails_JobTitle, model.ContactDetails_JobTitle);

        sessionOrchestration.Set(PreApplicationFormConstants.Response_ContactDetails_PhoneNumber, model.ContactDetails_PhoneNumber);

        sessionOrchestration.Set(PreApplicationFormConstants.Response_ContactDetails_Email, model.ContactDetails_Email);

        sessionOrchestration.Set(PreApplicationFormConstants.Response_ContactDetails_EmailConfirmation, model.ContactDetails_EmailConfirmation);

        model.ContactDetailsValidationFailures = preApplicationOrchestration.EvaluateContactFormForValidationFailures(
            model.ContactDetails_Forename,
            model.ContactDetails_Surname,
            model.ContactDetails_JobTitle,
            model.ContactDetails_PhoneNumber,
            model.ContactDetails_Email,
            model.ContactDetails_EmailConfirmation);

        if (model.ContactDetailsValidationFailures.Count == 0)
        {
            SetSessionFormValidationError(false);

            return ReturnToReivewOrRedirectToAction(nameof(Review));
        }

        SetSessionFormValidationError(true);

        return RedirectToAction();
    }

    [Route("application-part-one/review")]
    public IActionResult Review()
    {
        if (sessionOrchestration.Any() == false)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        sessionOrchestration.Set(PreApplicationFormConstants.PreApplicationReviewPageReached, true);

        return View(new ReviewViewModel()
        {
            IsCQCRegistered = sessionOrchestration.Get<string>(PreApplicationFormConstants.PreApplication_Response_IsRegisteredWithCQC) ?? "",
            CQCProviderID = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_CQCProvider_ID) ?? "",
            CQCProviderInformation_Name = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_CQCProvider_Name) ?? "",
            CQCProviderInformation_Address = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_CQCProvider_Address) ?? "",
            CQCProviderInformation_PhoneNumber = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_CQCProvider_PhoneNumber) ?? "",
            CQCInformationIsCorrect = sessionOrchestration.Get<string>(PreApplicationFormConstants.PreApplication_Response_CQCInformationIsCorrect) ?? "",
            ProvidesHealthCareService = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_ProvidesHealthCareService) ?? "",
            CQCRegulatedActivites = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_CQCRegulatedActivites) ?? "",
            ExclusiveServices = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_ExclusiveServices) ?? "",
            Earnings = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_Earnings) ?? "",
            ContactDetails_Forename = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_ContactDetails_Forename) ?? "",
            ContactDetails_Surname = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_ContactDetails_Surname) ?? "",
            ContactDetails_JobTitle = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_ContactDetails_JobTitle) ?? "",
            ContactDetails_PhoneNumber = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_ContactDetails_PhoneNumber) ?? "",
            ContactDetails_Email = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_ContactDetails_Email) ?? "",
        });
    }

    [HttpPost]
    [Route("application-part-one/submit")]
    public async Task<IActionResult> Submit(ReviewViewModel _)
    {
        if (sessionOrchestration.Any() == false)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        var IsCQCRegistered = sessionOrchestration.Get<string>(PreApplicationFormConstants.PreApplication_Response_IsRegisteredWithCQC) ?? "";

        if(IsCQCRegistered != PreApplicationFormConstants.Yes)
        {
            SetSessionFormValidationError(true);

            RedirectToAction(nameof(IsRegisteredWithCQC));
        }

        var cqcProviderId = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_CQCProvider_ID) ?? "";

        if (!CQCDataInSessionIsValid())
        {
            SetSessionFormValidationError(true);

            RedirectToAction(nameof(ConfirmCQCProviderInformation));
        }

        var healthCareService = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_ProvidesHealthCareService) ?? "";

        if (healthCareService != PreApplicationFormConstants.Yes)
        {
            SetSessionFormValidationError(true);

            RedirectToAction(nameof(ProvidesHealthCareServices));
        }

        var confirmationOfRegulatedActivities = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_CQCRegulatedActivites) ?? "";

        if (confirmationOfRegulatedActivities != PreApplicationFormConstants.Yes)
        {
            SetSessionFormValidationError(true);

            RedirectToAction(nameof(CQCRegulatedActivites));
        }

        var isExclusive = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_ExclusiveServices) ?? "";

        if (isExclusive != PreApplicationFormConstants.No)
        {
            RedirectToAction(nameof(ExclusiveServices));
        }

        var turnover = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_Earnings) ?? "";

        if (turnover == PreApplicationFormConstants.Earnings_Answer_1 || turnover == PreApplicationFormConstants.Earnings_Answer_2)
        {
            /* passed */
        }
        else
        {
            SetSessionFormValidationError(true);

            RedirectToAction(nameof(Earnings));
        }

        var firstname = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_ContactDetails_Forename) ?? "";

        var lastname = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_ContactDetails_Surname) ?? "";

        var jobtitle = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_ContactDetails_JobTitle) ?? "";

        var email = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_ContactDetails_Email) ?? "";

        var phoneNumber = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_ContactDetails_PhoneNumber) ?? "";

        if (string.IsNullOrWhiteSpace(firstname) || string.IsNullOrWhiteSpace(lastname) || string.IsNullOrWhiteSpace(jobtitle) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phoneNumber))
        {
            SetSessionFormValidationError(true);

            RedirectToAction(nameof(ContactDetails));
        }

        var referenceID = await preApplicationOrchestration.SubmitPreApplication(new Domain.Logic.Forms.PreApplication.DTO.PreApplicationDTO()
        {
            CQCProviderID = cqcProviderId,
            CQCProviderName = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_CQCProvider_Name) ?? string.Empty,
            CQCProviderAddress = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_CQCProvider_Address) ?? string.Empty,
            CQCProviderPhoneNumber = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_CQCProvider_PhoneNumber) ?? string.Empty,
            IsHealthCareService = healthCareService == PreApplicationFormConstants.Yes,
            ConfirmationOfRegulatedActivities = confirmationOfRegulatedActivities == PreApplicationFormConstants.Yes,
            IsExclusive = isExclusive == PreApplicationFormConstants.Yes,
            Turnover = turnover,
            FirstName = firstname,
            LastName = lastname,
            JobTitle = jobtitle,
            Email = email,
            PhoneNumber = phoneNumber
        });

        sessionOrchestration.Set(PreApplicationFormConstants.SessionPreApplicationReferenceId, referenceID);

        sessionOrchestration.Remove(PreApplicationFormConstants.PreApplication_Response_IsRegisteredWithCQC);
        sessionOrchestration.Remove(PreApplicationFormConstants.Response_CQCProvider_ID);
        sessionOrchestration.Remove(PreApplicationFormConstants.Response_CQCProvider_Name);
        sessionOrchestration.Remove(PreApplicationFormConstants.Response_CQCProvider_Address);
        sessionOrchestration.Remove(PreApplicationFormConstants.Response_CQCProvider_PhoneNumber);
        sessionOrchestration.Remove(PreApplicationFormConstants.Response_ProvidesHealthCareService);
        sessionOrchestration.Remove(PreApplicationFormConstants.Response_CQCRegulatedActivites);
        sessionOrchestration.Remove(PreApplicationFormConstants.Response_ExclusiveServices);
        sessionOrchestration.Remove(PreApplicationFormConstants.Response_Earnings);
        sessionOrchestration.Remove(PreApplicationFormConstants.Response_ContactDetails_Forename);
        sessionOrchestration.Remove(PreApplicationFormConstants.Response_ContactDetails_Surname);
        sessionOrchestration.Remove(PreApplicationFormConstants.Response_ContactDetails_JobTitle);
        sessionOrchestration.Remove(PreApplicationFormConstants.Response_ContactDetails_Email);
        sessionOrchestration.Remove(PreApplicationFormConstants.Response_ContactDetails_EmailConfirmation);
        sessionOrchestration.Remove(PreApplicationFormConstants.Response_ContactDetails_PhoneNumber);
        sessionOrchestration.Remove(PreApplicationFormConstants.FormValidationError);

        return RedirectToAction(nameof(Submitted));
    }

    [Route("application-part-one/submitted")]
    public IActionResult Submitted()
    {
        sessionOrchestration.Set(FeedbackFormConstants.SessionFeedbackTypeId, (int)Domain.Models.Database.FeedbackType.ApplicationFormPartOne);

        return View();
    }

    [Route("application-part-one/session-timeout")]
    public IActionResult SessionTimeout()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private RedirectToActionResult ReturnToReivewOrRedirectToAction(
        string actionName)
    {
        var review = sessionOrchestration.Get<bool>(PreApplicationFormConstants.PreApplicationReviewPageReached);

        if (review)
        {
            return RedirectToAction(nameof(Review));
        }

        return RedirectToAction(actionName);
    }

    private bool GetThenResetFormValidationErrorFromSession()
    {
        var currentValue = sessionOrchestration.Get<bool>(PreApplicationFormConstants.FormValidationError);
        SetSessionFormValidationError(false);
        return currentValue;
    }

    private bool GetFormValidationErrorFromSession()
    {
        return sessionOrchestration.Get<bool>(PreApplicationFormConstants.FormValidationError);
    }

    private void SetSessionFormValidationError(bool value)
    {
        sessionOrchestration.Set(PreApplicationFormConstants.FormValidationError, value);
    }

    private bool CQCDataInSessionIsValid()
    {
        var id = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_CQCProvider_ID) ?? "";
        var name = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_CQCProvider_Name) ?? "";
        var address = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_CQCProvider_Address) ?? "";
        var phoneNumber = sessionOrchestration.Get<string>(PreApplicationFormConstants.Response_CQCProvider_PhoneNumber) ?? "";

        if(string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(phoneNumber))
        {
            return false;
        }

        return true;
    }
}
