using System.Text.RegularExpressions;
using Database.Repository.Application;
using Database.Repository.Director;
using Domain.Logic.Forms.Application.Rules;
using Domain.Logic.Forms.Helpers.Session;
using Domain.Models.Database;
using Domain.Models.Database.DTO;
using Domain.Models.Forms.Application;
using Domain.Models.Forms.Rules;

namespace Domain.Logic.Forms.Application;

public class DirectorOrchestration(
    IRepositoryForDirectors repositoryForDirectors,
    ISessionDateHandler sessionDateHandler,
    IApplicationBusinessRules applicationBusinessRules,
    IRepositoryForApplication repositoryForApplication) : IDirectorOrchestration
{
    private readonly IRepositoryForDirectors repositoryForDirectors = repositoryForDirectors;
    private readonly ISessionDateHandler sessionDateHandler = sessionDateHandler;
    private readonly IApplicationBusinessRules applicationBusinessRules = applicationBusinessRules;
    private readonly IRepositoryForApplication repositoryForApplication = repositoryForApplication;

    public Task<int> AquireGroupForBoard(int applicationid)
    {
        return repositoryForDirectors.AquireGroupForBoard(applicationid);
    }

    public Task<int> CountDirectorsInApplication(int id)
    {
        return repositoryForDirectors.CountDirectorsInApplication(id);
    }

    public Task<int> CountDirectorsInGroup(int groupId)
    {
        return repositoryForDirectors.CountDirectorsInGroup(groupId);
    }

    public Task<int> CountDirectorsOfGroupType(int applicationId, Domain.Models.Database.DirectorType directorType)
    {
        return repositoryForDirectors.CountDirectorsOfGroupType(applicationId, directorType);
    }

    public Task<int> CountGroups(int applicationId, DirectorType directorType)
    {
        return repositoryForDirectors.CountGroups(applicationId, directorType);
    }

    public Task<int> CreateGroup(int applicationId, string name, DirectorType directorType)
    {
        return repositoryForDirectors.CreateGroup(applicationId, name, directorType);
    }

    public Task<int> CreateDirector(
        int applicationId,
        int groupId)
    {
        return repositoryForDirectors.Create(applicationId, groupId);
    }

    public Task DeleteGroup(int applicationId, int groupId)
    {
        return repositoryForDirectors.DeleteGroup(applicationId, groupId);
    }

    public Task DeleteGroups(int applicationId, DirectorType directorType)
    {
        return repositoryForDirectors.DeleteGroups(applicationId, directorType);
    }

    public async Task DeleteDirectors(int applicationId, Domain.Models.Database.DirectorType directorType)
    {
        await repositoryForDirectors.DeleteDirectors(applicationId, directorType);
    }

    public async Task DeleteDirector(int applicationId, int directorId)
    {
        await repositoryForDirectors.Delete(applicationId, directorId);
    }

    public async Task<bool> DirectorExists(int applicationId, int directorId)
    {
        return await repositoryForDirectors.DirectorExists(applicationId, directorId);
    }

    public RuleOutcomeDTO EvaluateDirectorDateOfBirth(
        int? day,
        int? month,
        int? year)
    {
        var firstChecks = applicationBusinessRules.EvaluateDate(day, month, year);

        if (!firstChecks.IsSuccess)
        {
            return firstChecks;
        }

        if (!day.HasValue || !month.HasValue || !year.HasValue)
        {
            throw new NotImplementedException("Unable to cast to Date");
        }

        return applicationBusinessRules.IsDirectorsDateOfBirthValid(new DateOnly(year.Value, month.Value, day.Value));
    }

    public Task<DateOnly?> GetDirectorDateOfBirth(int applicationId, int id)
    {
        return repositoryForDirectors.GetDateOfBirth(applicationId, id);
    }

    public async Task<ApplicationDateDTO> GetDirectorDateOfBirthFromSessionOrDatabase(int applicationId, int id)
    {
        var hasValue = sessionDateHandler.HasValue();

        if (!hasValue)
        {
            if(id == 0)
            {
                /* we haven't saved this director yet */
                sessionDateHandler.Set(null);
            }
            else
            {
                sessionDateHandler.Set(await GetDirectorDateOfBirth(applicationId, id));
            }
        }

        return new ApplicationDateDTO()
        {
            Day = sessionDateHandler.GetDay(),
            Month = sessionDateHandler.GetMonth(),
            Year = sessionDateHandler.GetYear(),
            IsValidDate = sessionDateHandler.IsValidDate()
        };
    }

    public Task<DirectorDTO> GetDirectorDetails(int applicationId, int id)
    {
        return repositoryForDirectors.GetDetails(applicationId, id);
    }

    public Task<DirectorNameDTO> GetDirectorName(int applicationId, int id)
    {
        return repositoryForDirectors.GetName(applicationId, id);
    }

    public async Task<List<DirectorDTO>> GetDirectors(int applicationId)
    {
        var directors = await repositoryForDirectors.GetDirectors(applicationId);

        var providerName = await repositoryForApplication.GetCQCProviderName(applicationId);

        foreach (var director in directors)
        {
            if(director.GroupName == DirectorConstants.BoardOfDirectors)
            {
                director.GroupName = providerName;
            }
        }

        return directors;
    }

    public Task<List<DirectorDTO>> GetDirectors(
        int applicationId,
        DirectorType directorType,
        int? groupId = null)
    {
        return repositoryForDirectors.GetDirectors(applicationId, directorType, groupId);
    }

    public Task SetDirectorDateOfBirth(int applicationId, int id, DateOnly dateOfBirth)
    {
        return repositoryForDirectors.SetDateOfBirth(applicationId, id, dateOfBirth);
    }

    public Task SetDirectorName(int applicationId, int id, string forename, string surname)
    {
        return repositoryForDirectors.SetName(applicationId, id, forename, surname);
    }

    public async Task<RuleOutcomeDTO> OrchestratePersistanceOfDirectorDetails(
        int applicationId,
        int directorId,
        DirectorNameDTO directorNames,
        ApplicationDateDTO applicationDateDTO,
        int groupId)
    {
        sessionDateHandler.SetDay(applicationDateDTO.Day);
        sessionDateHandler.SetMonth(applicationDateDTO.Month);
        sessionDateHandler.SetYear(applicationDateDTO.Year);
        sessionDateHandler.UseSessionValues();

        var outcome = EvaluateDirectorDateOfBirth(applicationDateDTO.Day, applicationDateDTO.Month, applicationDateDTO.Year);

        if (outcome.IsSuccess)
        {
            var dob = sessionDateHandler.GetDate();

            if (directorId > 0)
            {
                await SetDirectorDateOfBirth(applicationId, directorId, dob);
            }
            else
            {
                /* Create a new director from session details */
                directorId = await CreateDirector(applicationId, groupId);

                await SetDirectorName(applicationId, directorId, directorNames.Forename, directorNames.Surname);

                await SetDirectorDateOfBirth(applicationId, directorId, dob);
            }

            sessionDateHandler.Reset();
        }

        return outcome;
    }

    public Task<string> GetGroupName(int groupId)
    {
        return repositoryForDirectors.GetGroupName(groupId);
    }

    public Task<bool?> GetGroupOneOrMoreIndividuals(int groupId)
    {
        return repositoryForDirectors.GetGroupOneOrMoreIndividuals(groupId);
    }

    public Task<List<DirectorGroupDTO>> GetGroups(int applicationId, DirectorType directorType)
    {
        return repositoryForDirectors.GetGroups(applicationId, directorType);
    }

    public Task SetGroupName(int groupId, string name)
    {
        return repositoryForDirectors.SetGroupName(groupId, name);
    }

    public async Task SetGroupOneOrMoreIndividuals(int groupId, bool? value)
    {
        if(value == false)
        {
            await repositoryForDirectors.DeleteDirectorsInGroup(groupId);
        }

        await repositoryForDirectors.SetGroupOneOrMoreIndividuals(groupId, value);
    }

    public Task<bool> GroupBelongsToApplication(int applicationId, int groupId)
    {
        return repositoryForDirectors.GroupBelongsToApplication(applicationId, groupId);
    }
}
