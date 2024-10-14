using Database.Repository.Core.ReadWrite;
using Domain.Models.Database;
using Domain.Models.Database.DTO;

namespace Database.Repository.Director;
public interface IRepositoryForDirectors : IReadWriteIntPkRepository<Entites.Director>
{
    Task<int> AquireGroupForBoard(int applicationid);

    Task<int> CountDirectorsInApplication(int applicationId);

    Task<int> CountDirectorsInGroup(int groupId);

    Task<int> CountDirectorsOfGroupType(int applicationId, DirectorType directorType);

    Task<int> CountGroups(int applicationId, DirectorType directorType);

    Task<int> Create(
        int applicationId,
        int groupId);

    Task<int> CreateGroup(int applicationId, string name, Domain.Models.Database.DirectorType directorType);

    Task Delete(int applicationId, int directorId);

    Task DeleteGroup(int applicationId, int groupId);

    Task DeleteGroups(int applicationId, Domain.Models.Database.DirectorType directorType);

    Task DeleteDirectorsInGroup(int groupId);

    Task DeleteDirectors(int applicationId, DirectorType directorType);

    Task<bool> DirectorExists(int applicationId, int id);

    Task<List<DirectorDTO>> GetDirectors(int applicationId);

    Task<List<DirectorDTO>> GetDirectors(
        int applicationId,
        DirectorType directorType,
        int? groupId = null);

    Task<DirectorDTO> GetDetails(int applicationId, int id);

    Task<DateOnly?> GetDateOfBirth(int applicationId, int id);

    Task<DirectorNameDTO> GetName(int applicationId, int id);

    Task<string> GetGroupName(int id);

    Task<bool?> GetGroupOneOrMoreIndividuals(int id);

    Task<List<DirectorGroupDTO>> GetGroups(int id, DirectorType directorType);

    Task<bool> GroupBelongsToApplication(int applicationId, int groupId);

    Task SetDateOfBirth(int applicationId, int id, DateOnly dateOfBirth);

    Task SetGroupName(int id, string value);

    Task SetGroupOneOrMoreIndividuals(int id, bool? value);

    Task SetName(int applicationId, int id, string forename, string surname);
}
