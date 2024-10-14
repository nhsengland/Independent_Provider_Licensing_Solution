using Domain.Logic.Forms.Application;
using Domain.Logic.Forms.Application.Page;
using Domain.Logic.Forms.Feedback;
using Domain.Logic.Forms.Helpers;
using Domain.Logic.Forms.Helpers.Session;
using Domain.Models.Database;
using Domain.Models.Forms.Application;
using Licensing.Gateway.Models.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Licensing.Gateway.Controllers;

[Authorize]
public class ApplicationController(
    ILogger<ApplicationController> logger,
    IApplicationOrchestration applicationOrchestration,
    IResponseConverter responseConverter,
    INextPageOrchestor nextPageOrchestor,
    IDirectorOrchestration directorOrchestration,
    ISessionOrchestration sessionOrchestration,
    ISessionDateHandler sessionDateHandler,
    IPageToControllerMapper pageToControllerMapper) : BaseController(
        logger,
        sessionOrchestration,
        applicationOrchestration,
        directorOrchestration,
        pageToControllerMapper)
{
    private readonly ILogger<ApplicationController> logger = logger;
    private readonly IApplicationOrchestration applicationOrchestration = applicationOrchestration;
    private readonly IResponseConverter responseConverter = responseConverter;
    private readonly INextPageOrchestor nextPageOrchestor = nextPageOrchestor;
    private readonly IDirectorOrchestration directorOrchestration = directorOrchestration;
    private readonly ISessionOrchestration sessionOrchestration = sessionOrchestration;
    private readonly ISessionDateHandler sessionDateHandler = sessionDateHandler;

    [HttpGet]
    [Route("start")]
    public IActionResult Index()
    {
        return RedirectToAction(nameof(ApplicationCode));
    }

    [HttpPost]
    [ActionName("Index")]
    [Route("start")]
    public IActionResult IndexPost()
    {
        return RedirectToAction(nameof(ApplicationCode));
    }

    [Route("company-details/application-code")]
    public async Task<IActionResult> ApplicationCode()
    {
        var id = GetApplicationIdFromSession();

        var model = new ApplicationCodeViewModel()
        {
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        };

        if (id == 0)
        {
            return View(model);
        }

        model.ApplicationCode = await applicationOrchestration.GetApplicationCode(id);

        return View(model);
    }

    [HttpPost]
    [Route("company-details/application-code")]
    public async Task<IActionResult> ApplicationCode(ApplicationCodeViewModel model)
    {
        SetSessionFormValidationError(false);

        if (string.IsNullOrWhiteSpace(model.ApplicationCode) || !int.TryParse(model.ApplicationCode, out var _) || model.ApplicationCode.Length != 7)
        {
            SetSessionFormValidationError(true);

            return RedirectToAction(nameof(ApplicationCode));
        }

        var userId = GetOktaUserId();

        var result = await applicationOrchestration.OrchestrateApplicationCodeAsync(model.ApplicationCode, userId);

        if (!result.Exists)
        {
            SetSessionFormValidationError(true);

            return RedirectToAction(nameof(ApplicationCode));
        }

        sessionOrchestration.Set(ApplicationFormConstants.SessionApplicationDatabaseId, result.ApplicationId);

        if(result.NextPage == ApplicationPage.Submitted)
        {
            return RedirectToAction(nameof(ApplicationPage.Submitted));
        }

        var review = result.NextPage == Domain.Models.Database.ApplicationPage.Review;

        sessionOrchestration.Set(ApplicationFormConstants.SessionApplicationReview, review);

        if (result.FirstTimeAccessingApplication)
        {
            return await RedirectToReviewOrPage(result.ApplicationId, result.NextPage);
        }

        if (review)
        {

            return RedirectToAction(nameof(Review));
        }

        return RedirectToAction(nameof(ResumeApplication));
    }

    [Route("company-details/resume-application")]
    public async Task<IActionResult> ResumeApplication()
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        var applicationResponses = await applicationOrchestration.GetApplicationResponses(id);

        var model = new ResumeApplicationViewModel
        {
            Responses = applicationResponses
        };

        if(model.Responses.CompanyDetails.Count == 0)
        {
            return RedirectToAction(applicationResponses.Action, applicationResponses.Controller);
        }

        return View(model);
    }

    [Route("company-details/cqc-provider-id")]
    public async Task<IActionResult> CQCProviderID()
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        return View(new CQCProviderViewModel()
        {
            CQCProviderID = await applicationOrchestration.GetCQCProviderID(id),
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        });
    }

    [HttpPost]
    [Route("company-details/cqc-provider-id")]
    public async Task<IActionResult> CQCProviderID(CQCProviderViewModel model)
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        SetSessionFormValidationError(false);

        if (string.IsNullOrWhiteSpace(model.CQCProviderID))
        {
            SetSessionFormValidationError(true);

            return RedirectToAction();
        }

        var result = await applicationOrchestration.OrchestrateCQCProvider(id, model.CQCProviderID.Trim());

        if (!result.providerExists)
        {
            return await SetPageAndRedirectToAction(id, Domain.Models.Database.ApplicationPage.CQCProviderNotFound);
        }

        if (result.providerHasActiveLicence)
        {
            return await SetPageAndRedirectToAction(id, Domain.Models.Database.ApplicationPage.CQCProviderHasActiveLicence);
        }

        return await SetPageAndRedirectToAction(id, Domain.Models.Database.ApplicationPage.CQCProviderConfirmation);
    }

    [Route("company-details/confirm-details")]
    public async Task<IActionResult> CQCProviderConfirmation()
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        var details = await applicationOrchestration.GetCQCProviderDetails(id);

        return View(new CQCProviderDetailsViewModel()
        {
            CQCProviderID = details.Id,
            Name = details.Name,
            Address = details.Address,
            PhoneNumber = details.PhoneNumber,
            WebsiteURL = details.WebsiteURL,
            IsCrsOrHtr = await applicationOrchestration.IsCrsOrHardToReplace(id),
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        });
    }

    [HttpPost]
    [Route("company-details/confirm-details")]
    public async Task<IActionResult> CQCProviderConfirmation(CQCProviderDetailsViewModel model)
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        SetSessionFormValidationError(false);

        if (string.IsNullOrWhiteSpace(model.CQCInformationIsCorrect))
        {
            SetSessionFormValidationError(true);

            return RedirectToAction();
        }

        return await SetPageAndRedirectToAction(id, nextPageOrchestor.NextPageAfterCQCProviderConfirmation(model.CQCInformationIsCorrect, GetReview()));
    }

    [Route("company-details/cqc-provider-id-not-found")]
    public async Task<IActionResult> CQCProviderNotFound()
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        var cqcProviderID = await applicationOrchestration.GetCQCProviderID(id);

        return View(new CQCProviderViewModel()
        {
            ValidationFailure = true,
            CQCProviderID = cqcProviderID
        });
    }

    [Route("company-details/cqc-provider-has-active-licence")]
    public IActionResult CQCProviderHasActiveLicence()
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        return View();
    }

    [Route("company-details/change-details")]
    public async Task<IActionResult> CQCProviderChange()
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        /* TODO: Pull CQC details from DB in one query */
        return View(new CQCProviderDetailsViewModel()
        {
            CQCProviderID = await applicationOrchestration.GetCQCProviderID(id),
            Name = await applicationOrchestration.GetCQCProviderName(id),
            Address = await applicationOrchestration.GetCQCProviderAddress(id),
            PhoneNumber = await applicationOrchestration.GetCQCProviderPhoneNumber(id),
            WebsiteURL = await applicationOrchestration.GetCQCProviderWebsiteURL(id),
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        });
    }

    [HttpPost]
    [Route("company-details/change-details")]
    public async Task<IActionResult> CQCProviderChangeAsync(CQCProviderDetailsViewModel _)
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        return await SetPageAndRedirectToAction(id, Domain.Models.Database.ApplicationPage.CQCProviderConfirmation);
    }

    [Route("company-details/provider-name")]
    public async Task<IActionResult> CQCProviderChangeName()
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        var name = await applicationOrchestration.GetCQCProviderName(id);

        return View(new CQCProviderDetailsViewModel()
        {
            Name = name,
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        });
    }

    [HttpPost]
    [Route("company-details/provider-name")]
    public async Task<IActionResult> CQCProviderChangeName(CQCProviderDetailsViewModel model)
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        SetSessionFormValidationError(false);

        await applicationOrchestration.SetCQCProviderName(id, model.Name?.Trim() ?? string.Empty);

        if (string.IsNullOrWhiteSpace(model.Name))
        {
            SetSessionFormValidationError(true);

            return RedirectToAction();
        }

        return await SetPageAndRedirectToAction(id, Domain.Models.Database.ApplicationPage.CQCProviderChange);
    }

    [Route("company-details/provider-address")]
    public async Task<IActionResult> CQCProviderChangeAddress()
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        var address = await applicationOrchestration.GetCQCProviderAddress(id);

        return View(new CQCProviderDetailsViewModel()
        {
            Address = address,
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        });
    }

    [HttpPost]
    [Route("company-details/provider-address")]
    public async Task<IActionResult> CQCProviderChangeAddress(CQCProviderDetailsViewModel model)
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        SetSessionFormValidationError(false);

        await applicationOrchestration.SetCQCProviderAddress(id, model.Address?.Trim() ?? string.Empty);

        if (string.IsNullOrWhiteSpace(model.Address))
        {
            SetSessionFormValidationError(true);

            return RedirectToAction();
        }

        return await SetPageAndRedirectToAction(id, Domain.Models.Database.ApplicationPage.CQCProviderChange);
    }

    [Route("company-details/provider-phone-number")]
    public async Task<IActionResult> CQCProviderChangePhoneNumber()
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        var phoneNumber = await applicationOrchestration.GetCQCProviderPhoneNumber(id);

        return View(new CQCProviderDetailsViewModel()
        {
            PhoneNumber = phoneNumber,
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        });
    }

    [HttpPost]
    [Route("company-details/provider-phone-number")]
    public async Task<IActionResult> CQCProviderChangePhoneNumber(CQCProviderDetailsViewModel model)
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        SetSessionFormValidationError(false);

        await applicationOrchestration.SetCQCProviderPhoneNumber(id, model.PhoneNumber?.Trim() ?? string.Empty);

        if (string.IsNullOrWhiteSpace(model.PhoneNumber))
        {
            SetSessionFormValidationError(true);

            return RedirectToAction();
        }

        return await SetPageAndRedirectToAction(id, Domain.Models.Database.ApplicationPage.CQCProviderChange);
    }

    [Route("company-details/provider-website")]
    public async Task<IActionResult> CQCProviderChangeWebsite()
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        var websiteURL = await applicationOrchestration.GetCQCProviderWebsiteURL(id);

        return View(new CQCProviderDetailsViewModel()
        {
            WebsiteURL = websiteURL,
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        });
    }

    [HttpPost]
    [Route("company-details/provider-website")]
    public async Task<IActionResult> CQCProviderChangeWebsite(CQCProviderDetailsViewModel model)
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        SetSessionFormValidationError(false);

        await applicationOrchestration.SetCQCProviderWebsiteURL(id, model.WebsiteURL?.Trim() ?? string.Empty);

        if (string.IsNullOrWhiteSpace(model.WebsiteURL))
        {
            SetSessionFormValidationError(true);

            return RedirectToAction();
        }

        return await SetPageAndRedirectToAction(id, Domain.Models.Database.ApplicationPage.CQCProviderChange);
    }

    [Route("company-details/company-number-check")]
    public async Task<IActionResult> CompanyNumberCheck()
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        return View(new CompanyNumberCheckViewModel()
        {
            CompanyNumberCheck = await applicationOrchestration.GetCompanyNumberCheck(id),
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        });
    }

    [HttpPost]
    [Route("company-details/company-number-check")]
    public async Task<IActionResult> CompanyNumberCheck(CompanyNumberCheckViewModel model)
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        SetSessionFormValidationError(false);

        var resopnse = model.CompanyNumberCheck?.Trim() ?? string.Empty;

        await applicationOrchestration.SetCompanyNumberCheck(id, resopnse);

        if (string.IsNullOrWhiteSpace(resopnse))
        {
            SetSessionFormValidationError(true);

            return RedirectToAction();
        }

        return await SetPageAndRedirectToAction(id, nextPageOrchestor.NextPageAfterCompanyNumberCheck(resopnse, GetReview()));
    }

    [Route("company-details/company-number")]
    public async Task<IActionResult> CompanyNumber()
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        return View(new CompanyNumberViewModel()
        {
            CompanyNumber = await applicationOrchestration.GetCompanyNumber(id),
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        });
    }

    [HttpPost]
    [Route("company-details/company-number")]
    public async Task<IActionResult> CompanyNumber(CompanyNumberViewModel model)
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        SetSessionFormValidationError(false);

        await applicationOrchestration.SetCompanyNumber(id, model.CompanyNumber?.Trim() ?? string.Empty);

        if (string.IsNullOrWhiteSpace(model.CompanyNumber) || model.CompanyNumber.Length != 8)
        {
            SetSessionFormValidationError(true);

            return RedirectToAction();
        }

        return await RedirectToReviewOrPage(id, Domain.Models.Database.ApplicationPage.NewlyIncorporatedCompany);
    }

    [Route("company-details/newly-incorporated-company")]
    public async Task<IActionResult> NewlyIncorporatedCompany()
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        return View(new NewlyIncorporatedCompanyViewModel()
        {
            NewlyIncorporatedCompany = await applicationOrchestration.GetNewlyIncorporatedCompany(id),
            ProviderName = await applicationOrchestration.GetCQCProviderName(id),
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        });
    }

    [HttpPost]
    [Route("company-details/newly-incorporated-company")]
    public async Task<IActionResult> NewlyIncorporatedCompany(NewlyIncorporatedCompanyViewModel model)
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        SetSessionFormValidationError(false);

        var resopnse = model.NewlyIncorporatedCompany?.Trim() ?? string.Empty;

        await applicationOrchestration.SetNewlyIncorporatedCompany(id, resopnse);

        if (string.IsNullOrWhiteSpace(resopnse))
        {
            SetSessionFormValidationError(true);

            return RedirectToAction();
        }

        return await SetPageAndRedirectToAction(id, nextPageOrchestor.NextPageAfterNewlyIncorporatedCompany(resopnse, GetReview()));
    }

    [Route("company-details/last-financial-year")]
    public async Task<IActionResult> LastFinancialYear()
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        var dateDto = await applicationOrchestration.GetFinancialYearEndLastFromSessionOrDatabase(id);

        var model = new LastFinancialYearViewModel()
        {
            Day = dateDto.Day,
            Month = dateDto.Month,
            Year = dateDto.Year,
            ProviderName = await applicationOrchestration.GetCQCProviderName(id),
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        };

        if (model.ValidationFailure)
        {
            var outcome = applicationOrchestration.EvaluateLastFinancialYearEnd(
                dateDto.Day,
                dateDto.Month,
                dateDto.Year);

            model.FailureMessages = outcome.ErrorMessages;
            model.IsValidDate = dateDto.IsValidDate;
        }

        return View(model);
    }

    [HttpPost]
    [Route("company-details/last-financial-year")]
    public async Task<IActionResult> LastFinancialYear(LastFinancialYearViewModel model)
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        SetSessionFormValidationError(false);

        var outcome = await applicationOrchestration.OrchestrateLastFinancialYearEndAsync(
            id,
            new ApplicationDateDTO() {
                Day = model.Day,
                Month = model.Month,
                Year = model.Year }
            );

        if (!outcome.IsSuccess)
        {
            SetSessionFormValidationError(true);

            return RedirectToAction();
        }

        return await RedirectToReviewOrPage(id, Domain.Models.Database.ApplicationPage.NextFinancialYear);
    }

    [Route("company-details/next-financial-year")]
    public async Task<IActionResult> NextFinancialYear()
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        var dateDto = await applicationOrchestration.GetFinancialYearEndNextFromSessionOrDatabase(id);

        var model = new NextFinancialYearViewModel()
        {
            Day = dateDto.Day,
            Month = dateDto.Month,
            Year = dateDto.Year,
            ProviderName = await applicationOrchestration.GetCQCProviderName(id),
            ValidationFailure = GetThenResetFormValidationErrorFromSession()
        };

        if (model.ValidationFailure)
        {
            var outcome = applicationOrchestration.EvaluateNextFinancialYearEnd(
                dateDto.Day,
                dateDto.Month,
                dateDto.Year);

            model.FailureMessages = outcome.ErrorMessages;
            model.IsValidDate = dateDto.IsValidDate;
        }

        return View(model);
    }

    [HttpPost]
    [Route("company-details/next-financial-year")]
    public async Task<IActionResult> NextFinancialYear(NextFinancialYearViewModel model)
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        SetSessionFormValidationError(false);

        var outcome = await applicationOrchestration.OrchestrateNextFinancialYearEndAsync(
            id,
            new ApplicationDateDTO()
                {
                    Day = model.Day,
                    Month = model.Month,
                    Year = model.Year
                }
            );

        if (!outcome.IsSuccess)
        {
            SetSessionFormValidationError(true);

            return RedirectToAction();
        }

        return await RedirectToReviewOrPage(id, Domain.Models.Database.ApplicationPage.DirectorCheck);
    }

    [Route("check-your-answers")]
    public async Task<IActionResult> Review()
    {
        sessionOrchestration.Set(ApplicationFormConstants.SessionApplicationReview, true);

        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        var model = await applicationOrchestration.GetApplicationReviewData(id);

        model.Responses.IsReviewPage = true;

        return View(new ApplicationReviewViewModel()
        {
            ApplicationReviewData = model
        });
    }

    [HttpPost]
    public async Task<IActionResult> Submit()
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        var applicationHasBeenSubmitted = await applicationOrchestration.GetSubmitApplication(id);

        if (applicationHasBeenSubmitted == true)
        {
            return RedirectToAction(nameof(Submitted));
        }

        var reviewData = await applicationOrchestration.GetApplicationReviewData(id);

        if (!reviewData.RuleOutcomeDTO.IsSuccess)
        {
            RedirectToAction();
        }

        await applicationOrchestration.SubmitApplication(id);

        return await SetPageAndRedirectToAction(id, ApplicationPage.Submitted);
    }

    [HttpGet]
    [Route("confirmation-page")]
    public async Task<IActionResult> Submitted()
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        sessionOrchestration.Set(FeedbackFormConstants.SessionFeedbackTypeId, (int)Domain.Models.Database.FeedbackType.ApplicationFormPartTwo);

        return View(new ApplicationSubmittedViewModel()
        {
            ApplicationReferenceID = await applicationOrchestration.GetReferenceId(id),
        });
    }

    [Route("exit")]
    public async Task<IActionResult> SaveAndExit()
    {
        var id = GetApplicationIdFromSession();

        if (id == 0)
        {
            return RedirectToAction(nameof(SessionTimeout));
        }

        var applicationUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/home/apply-for-a-nhs-provider-licence";

        await applicationOrchestration.SaveAndExitAsync(id, applicationUrl);

        var emailAddress = await applicationOrchestration.GetContactEmailAddress(id);

        sessionOrchestration.Set(ApplicationFormConstants.SessionEmailAddress, emailAddress);

        return RedirectToAction(
            ApplicationControllerConstants.Controller_Account_Method_SaveAndExit,
            ApplicationControllerConstants.Controller_Name_Account);
    }

    [Route("session-timeout")]
    public IActionResult SessionTimeout()
    {
        return View();
    }
}
