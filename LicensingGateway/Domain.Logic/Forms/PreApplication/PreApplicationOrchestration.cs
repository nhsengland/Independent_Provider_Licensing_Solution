using Database.Entites;
using Database.Repository.Application;
using Database.Repository.Email;
using Domain.Logic.Forms.Factories;
using Domain.Logic.Forms.Helpers;
using Domain.Logic.Forms.PreApplication.DTO;
using Domain.Logic.Integration.CQC;
using Domain.Logic.Integration.CQC.Models;
using Domain.Logic.Integration.StorageAccount.Queues;

namespace Domain.Logic.Forms.PreApplication;
public class PreApplicationOrchestration(
    ICQCProviderOrchestration cqcProviderOrchestration,
    IRepositoryForPreApplication repositoryForPreApplication,
    IReferenceIDFactory referenceIDFactory,
    IRepositoryForEmailNotifications repositoryForEmailNotifications,
    IStorageAccountQueueWrapper storageAccountQueueWrapper,
    IDateTimeFactory dateTimeFactory) : IPreApplicationOrchestration
{
    private readonly ICQCProviderOrchestration cqcProviderOrchestration = cqcProviderOrchestration;
    private readonly IRepositoryForPreApplication repositoryForPreApplication = repositoryForPreApplication;
    private readonly IReferenceIDFactory referenceIDFactory = referenceIDFactory;
    private readonly IRepositoryForEmailNotifications repositoryForEmailNotifications = repositoryForEmailNotifications;
    private readonly IDateTimeFactory dateTimeFactory = dateTimeFactory;

    public List<(string key, string value)> EvaluateContactFormForValidationFailures(
        string ContactDetails_Forename,
        string ContactDetails_Surname,
        string ContactDetails_JobTitle,
        string ContactDetails_PhoneNumber,
        string ContactDetails_Email,
        string ContactDetails_EmailConfirmation)
    {
        var result = new List<(string key, string value)>();

        if (string.IsNullOrWhiteSpace(ContactDetails_Forename))
        {
            result.Add(new(nameof(ContactDetails_Forename), "Please enter your first name"));
        }

        if (string.IsNullOrWhiteSpace(ContactDetails_Surname))
        {
            result.Add(new(nameof(ContactDetails_Surname), "Please enter your last name"));
        }

        if (string.IsNullOrWhiteSpace(ContactDetails_JobTitle))
        {
            result.Add(new(nameof(ContactDetails_JobTitle), "Please enter your job title"));
        }

        if (string.IsNullOrWhiteSpace(ContactDetails_PhoneNumber))
        {
            result.Add(new(nameof(ContactDetails_PhoneNumber), "Please enter your phone number"));
        }

        if (string.IsNullOrWhiteSpace(ContactDetails_Email))
        {
            result.Add(new(nameof(ContactDetails_Email), "Please enter your email address"));
        }

        if (string.IsNullOrWhiteSpace(ContactDetails_EmailConfirmation))
        {
            result.Add(new(nameof(ContactDetails_EmailConfirmation), "Please enter your email confirmation address"));
        }

        if (!string.IsNullOrWhiteSpace(ContactDetails_Email) && !string.IsNullOrWhiteSpace(ContactDetails_EmailConfirmation))
        {
            if (!ContactDetails_Email.Contains('@'))
            {
                result.Add(new(nameof(ContactDetails_Email), "Please enter an email address in the correct format, like name@example.com"));
            }

            if (!ContactDetails_EmailConfirmation.Contains('@'))
            {
                result.Add(new(nameof(ContactDetails_EmailConfirmation), "Please enter an email address in the correct format, like name@example.com"));
            }

            if (ContactDetails_Email.Trim() != ContactDetails_EmailConfirmation.Trim())
            {
                result.Add(new(nameof(ContactDetails_Email), "The email addresses you have supplied do not match"));

                result.Add(new(nameof(ContactDetails_EmailConfirmation), "The email addresses you have supplied do not match"));
            }

            if(result.Count == 0)
            {
                string[] blackListOfEmailDomains = ["gmail.com", "yahoo.com", "outlook.com", "protonmail.com", "hotmail.com", "hotmail.co.uk", "aol.com", "icloud.com", "mail.com", "zoho.com", "yahoo.co.uk", "live.com", "ymail.com"];

                if (blackListOfEmailDomains.Any(ContactDetails_Email.Contains))
                {
                    result.Add(new(nameof(ContactDetails_Email), "We will not accept an application from the email domain you have supplied"));
                }
            }
        }

        return result;
    }

    public Task<string[]> GetCQCProviderRequlatedActivites(string cqcProviderId)
    {
        return cqcProviderOrchestration.GetProvidersRequlatedActivites(cqcProviderId);
    }

    public Task<CQCProviderInformation?> OrchestrateQCVProvider(string CQCProviderID)
    {
        return cqcProviderOrchestration.GetProviderInformation(CQCProviderID);
    }

    public async Task<string> SubmitPreApplication(PreApplicationDTO preApplicationDTO)
    {
        var dateTime = dateTimeFactory.Create();
        var preApplication = new Database.Entites.PreApplication
        {
            CQCProviderID = preApplicationDTO.CQCProviderID,
            CQCProviderName = preApplicationDTO.CQCProviderName,
            CQCProviderAddress = preApplicationDTO.CQCProviderAddress,
            CQCProviderPhoneNumber = preApplicationDTO.CQCProviderPhoneNumber,
            IsHealthCareService = preApplicationDTO.IsHealthCareService,
            ConfirmationOfRegulatedActivities = preApplicationDTO.ConfirmationOfRegulatedActivities,
            RegulatedActivities = string.Join(",", await GetCQCProviderRequlatedActivites(preApplicationDTO.CQCProviderID)),
            IsExclusive = preApplicationDTO.IsExclusive,
            Turnover = preApplicationDTO.Turnover,
            FirstName = preApplicationDTO.FirstName,
            LastName = preApplicationDTO.LastName,
            JobTitle = preApplicationDTO.JobTitle,
            Email = preApplicationDTO.Email,
            PhoneNumber = preApplicationDTO.PhoneNumber,
            DateGenerated = dateTime,
        };

        var id = await repositoryForPreApplication.AddAsync(preApplication);

        var referenceID = referenceIDFactory.CreateForPreApplication(dateTime, id);

        await repositoryForPreApplication.UpdateAsync(id, referenceID);

        var emailNotification = new EmailNotification()
        {
            PreApplicationId = id,
            TypeId = (int)Models.Database.EmailNotificationType.PreApplication,
        };

        await repositoryForEmailNotifications.AddAsync(emailNotification);

        await storageAccountQueueWrapper.PutMessageOntoEmailNotificationQueue(emailNotification.Id);

        return referenceID;
    }
}
