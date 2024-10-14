using Domain.Models.Database;
using Domain.Models.Database.DTO;
using Domain.Models.Forms.Application;
using Domain.Models.Forms.Rules;

namespace Domain.Logic.Forms.Application;

public interface IDirectorOrchestration
{
    Task<int> AquireGroupForBoard(int applicationid);

    Task<int> CountDirectorsInApplication(int id);

    Task<int> CountDirectorsInGroup(int groupId);

    Task<int> CountGroups(int applicationId, DirectorType directorType);

    Task<int> CountDirectorsOfGroupType(int applicationId, Domain.Models.Database.DirectorType directorType);

    Task<int> CreateDirector(int applicationId, int groupId);

    Task<int> CreateGroup(int applicationId, string name, DirectorType directorType);

    Task DeleteGroup(int applicationId, int corporateBodyId);

    Task DeleteGroups(int applicationId, DirectorType directorType);

    Task DeleteDirectors(int applicationId, DirectorType directorType);

    Task DeleteDirector(int applicationId, int directorId);

    Task<bool> DirectorExists(int applicationId, int directorId);

    Task<DateOnly?> GetDirectorDateOfBirth(int applicationId, int id);

    RuleOutcomeDTO EvaluateDirectorDateOfBirth(
        int? day,
        int? month,
        int? year);

    Task<ApplicationDateDTO> GetDirectorDateOfBirthFromSessionOrDatabase(int applicationId, int id);

    Task<DirectorDTO> GetDirectorDetails(int applicationId, int id);

    Task<DirectorNameDTO> GetDirectorName(int applicationId, int id);

    Task<List<DirectorDTO>> GetDirectors(int applicationId);

    Task<List<DirectorDTO>> GetDirectors(
        int applicationId,
        DirectorType directorType,
        int? groupId = null);

    Task<List<DirectorGroupDTO>> GetGroups(int applicationId, DirectorType directorType);

    Task<string> GetGroupName(int groupId);

    Task<bool?> GetGroupOneOrMoreIndividuals(int groupId);

    Task<bool> GroupBelongsToApplication(int applicationId, int groupId);

    Task<RuleOutcomeDTO> OrchestratePersistanceOfDirectorDetails(
        int applicationId,
        int directorId,
        DirectorNameDTO directorNames,
        ApplicationDateDTO applicationDateDTO,
        int groupId);

    Task SetDirectorDateOfBirth(int applicationId, int id, DateOnly dateOfBirth);

    Task SetDirectorName(int applicationId, int id, string forename, string surname);

    Task SetGroupName(int groupId, string name);

    Task SetGroupOneOrMoreIndividuals(int groupId, bool? value);
}
