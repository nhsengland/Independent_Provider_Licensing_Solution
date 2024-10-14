using System;
using Database.Entites;
using Database.Repository.Core.ReadWrite;
using Domain.Models.Database;
using Domain.Models.Database.DTO;
using Microsoft.EntityFrameworkCore;

namespace Database.Repository.Director;
public class RepositoryForDirectors(ILicensingGatewayDbContext licensingGatewayDbContext) : ReadWriteIntPkRepository<Entites.Director>(licensingGatewayDbContext), IRepositoryForDirectors
{
    public async Task<int> AquireGroupForBoard(int applicationid)
    {
        var typeId = (int)Domain.Models.Database.DirectorType.Board;

        var group = await licensingGatewayDbContext.DirectorGroup
            .Where(e => e.ApplicationId == applicationid && e.TypeId == typeId)
            .FirstOrDefaultAsync();

        if(group == null)
        {
            group = new DirectorGroup
            {
                ApplicationId = applicationid,
                Name = DirectorConstants.BoardOfDirectors,
                TypeId = typeId
            };

            licensingGatewayDbContext.DirectorGroup.Add(group);

            await licensingGatewayDbContext.DbContext.SaveChangesAsync();
        }

        return group.Id;
    }

    public async Task<int> CountDirectorsInApplication(int applicationId)
    {
        return await licensingGatewayDbContext.Director
            .Where(e => e.ApplicationId == applicationId)
            .CountAsync();
    }

    public async Task<int> CountDirectorsInGroup(int groupId)
    {
        return await licensingGatewayDbContext.Director
            .Where(e => e.GroupId == groupId)
            .CountAsync();
    }

    public async Task<int> CountDirectorsOfGroupType(int applicationId, Domain.Models.Database.DirectorType directorType)
    {
        var groupIds = licensingGatewayDbContext.DirectorGroup
            .Where(e => e.ApplicationId == applicationId && e.TypeId == (int)directorType)
            .Select(e => e.Id);

        return await licensingGatewayDbContext.Director
            .Where(e => e.ApplicationId == applicationId && groupIds.Contains(e.GroupId))
            .CountAsync();
    }

    public Task<int> CountGroups(int applicationId, Domain.Models.Database.DirectorType directorType)
    {
        return licensingGatewayDbContext.DirectorGroup
            .Where(e => e.ApplicationId == applicationId && e.TypeId == (int)directorType)
            .CountAsync();
    }

    public async Task<int> Create(
        int applicationId,
        int groupId)
    {
        var director = new Entites.Director
        {
            ApplicationId = applicationId,
            GroupId = groupId
        };

        licensingGatewayDbContext.Director.Add(director);

        await licensingGatewayDbContext.DbContext.SaveChangesAsync();

        return director.Id;
    }

    public async Task<int> CreateGroup(int applicationId, string name, Domain.Models.Database.DirectorType directorType)
    {
        var directorGroup = new DirectorGroup
        {
            ApplicationId = applicationId,
            Name = name,
            TypeId = (int)directorType
        };

        licensingGatewayDbContext.DirectorGroup.Add(directorGroup);

        await licensingGatewayDbContext.DbContext.SaveChangesAsync();

        return directorGroup.Id;
    }

    public async Task Delete(int applicationId, int directorId)
    {
        await licensingGatewayDbContext.Director.Where(e => e.ApplicationId == applicationId && e.Id == directorId).ExecuteDeleteAsync();
    }

    public async Task DeleteDirectors(int applicationId, Domain.Models.Database.DirectorType directorType)
    {
        /* Cascade delete client side 
         * include directors: load into memory so they are deleted */
        var groups = licensingGatewayDbContext.DirectorGroup.Include(e => e.Directors)
            .Where(e => e.ApplicationId == applicationId && e.TypeId == (int)directorType);

        await DeleteDricrectorsInGroups(groups);
    }

    public async Task DeleteDirectorsInGroup(int groupId)
    {
        /* Cascade delete client side 
         * include directors: load into memory so they are deleted */
        var groups = licensingGatewayDbContext.DirectorGroup.Include(e => e.Directors)
        .Where(e => e.Id == groupId);

        await DeleteDricrectorsInGroups(groups);
    }

    public Task<bool> DirectorExists(int applicationId, int id)
    {
        return licensingGatewayDbContext.Director.Where(e => e.ApplicationId == applicationId && e.Id == id)
            .AnyAsync();
    }

    public async Task DeleteGroup(int applicationId, int groupId)
    {
        /* Cascade delete client side 
         * include directors: load into memory so they are deleted */
        var group = licensingGatewayDbContext.DirectorGroup.Include(e => e.Directors).First(e => e.ApplicationId == applicationId && e.Id == groupId);

        await DeleteGroup(group);
    }

    public async Task DeleteGroups(int applicationId, Domain.Models.Database.DirectorType directorType)
    {
        /* Cascade delete client side 
         * include directors: load into memory so they are deleted */
        var groups = licensingGatewayDbContext.DirectorGroup.Include(e => e.Directors).Where(e => e.ApplicationId == applicationId && e.TypeId == (int)directorType);

        foreach (var group in groups)
        {
            await DeleteGroup(group);
        }
    }

    public Task<List<DirectorDTO>> GetDirectors(int applicationId)
    {
        return licensingGatewayDbContext.Director.Where(d => d.ApplicationId == applicationId)
            .Select(d => new DirectorDTO
            {
                Id = d.Id,
                Forename = d.Forename ?? string.Empty,
                Surname = d.Surname ?? string.Empty,
                DateOfBirth = d.DateOfBirth ?? DateOnly.MinValue,
                GroupName = d.Group!.Name,
                GroupId = d.GroupId
            })
            .ToListAsync();
    }

    public async Task<List<DirectorDTO>> GetDirectors(
        int applicationId,
        Domain.Models.Database.DirectorType directorType,
        int? groupId = null)
    {
        var query = licensingGatewayDbContext.DirectorGroup
            .Where(d =>
                d.ApplicationId == applicationId
                && d.TypeId == (int)directorType);

        if (groupId.HasValue)
        {
            query = query.Where(d => d.Id == groupId);
        }

        var groupIds = query.Select(e => e.Id);

        var directors = await licensingGatewayDbContext.Director.Include(d => d.Group).Where(e => groupIds.Contains(e.GroupId)).ToArrayAsync();

        var output = new List<DirectorDTO>();

        foreach (var d in directors)
        {
            output.Add(new DirectorDTO
            {
                Id = d.Id,
                Forename = d.Forename ?? string.Empty,
                Surname = d.Surname ?? string.Empty,
                DateOfBirth = d.DateOfBirth ?? DateOnly.MinValue,
                GroupName = d.Group!.Name,
                GroupId = d.GroupId
            });
        }

        return output;
    }

    public Task<DateOnly?> GetDateOfBirth(int applicationId, int id)
    {
        return licensingGatewayDbContext.Director.Where(e => e.ApplicationId == applicationId && e.Id == id)
            .Select(e => e.DateOfBirth)
            .FirstAsync();
    }

    public Task<DirectorDTO> GetDetails(int applicationId, int id)
    {
        return licensingGatewayDbContext
                .Director
                .Include(d => d.Group)
                .Where(e => e.ApplicationId == applicationId && e.Id == id)
            .Select(e => new DirectorDTO
            {
                Forename = e.Forename ?? string.Empty,
                Surname = e.Surname ?? string.Empty,
                DateOfBirth = e.DateOfBirth ?? DateOnly.MinValue,
                GroupId = e.GroupId,
                DirectorType = (Domain.Models.Database.DirectorType)e.Group.TypeId
            })
            .FirstAsync();
    }

    public Task<DirectorNameDTO> GetName(int applicationId, int id)
    {
        return licensingGatewayDbContext.Director.Where(e => e.ApplicationId == applicationId && e.Id == id)
            .Select(e => new DirectorNameDTO
            {
                Forename = e.Forename ?? string.Empty,
                Surname = e.Surname ?? string.Empty
            })
            .FirstAsync();
    }

    public async Task<string> GetGroupName(int id)
    {
        return await licensingGatewayDbContext.DirectorGroup.Where(e => e.Id == id)
            .Select(e => e.Name)
            .FirstAsync();
    }

    public async Task<bool?> GetGroupOneOrMoreIndividuals(int id)
    {
        return await licensingGatewayDbContext.DirectorGroup.Where(e => e.Id == id)
            .Select(e => e.OneOrMoreIndividuals)
            .FirstAsync();
    }

    public async Task SetDateOfBirth(int applicationId, int id, DateOnly dateOfBirth)
    {
        await licensingGatewayDbContext.Director.Where(e => e.ApplicationId == applicationId && e.Id == id)
        .ExecuteUpdateAsync(b =>
                b.SetProperty(e => e.DateOfBirth, dateOfBirth)
            );
    }

    public async Task SetName(int applicationId, int id, string forename, string surname)
    {
        await licensingGatewayDbContext.Director.Where(e => e.ApplicationId == applicationId && e.Id == id)
        .ExecuteUpdateAsync(setters => setters
                .SetProperty(e => e.Forename, forename)
                .SetProperty(e => e.Surname, surname)
            );
    }

    public async Task SetGroupName(int id, string value)
    {
        await licensingGatewayDbContext.DirectorGroup.Where(e => e.Id == id)
        .ExecuteUpdateAsync(setters => setters
                .SetProperty(e => e.Name, value)
            );
    }

    public async Task SetGroupOneOrMoreIndividuals(int id, bool? value)
    {
        await licensingGatewayDbContext.DirectorGroup.Where(e => e.Id == id)
        .ExecuteUpdateAsync(setters => setters
                .SetProperty(e => e.OneOrMoreIndividuals, value)
            );
    }

    public async Task<List<DirectorGroupDTO>> GetGroups(int id, Domain.Models.Database.DirectorType directorType)
    {
        return await licensingGatewayDbContext.DirectorGroup.Where(e => e.ApplicationId == id && e.TypeId == (int)directorType)
            .Select(e => new DirectorGroupDTO
            {
                Id = e.Id,
                ApplicationId = e.ApplicationId,
                Name = e.Name,
                OneOrMoreIndividuals = e.OneOrMoreIndividuals
            })
            .ToListAsync();
    }

    public async Task<bool> GroupBelongsToApplication(int applicationId, int groupId)
    {
        return await licensingGatewayDbContext.DirectorGroup.Where(e => e.ApplicationId == applicationId && e.Id == groupId)
            .AnyAsync();
    }

    private async Task DeleteGroup(DirectorGroup group)
    {
        licensingGatewayDbContext.DirectorGroup.Remove(group);

        await licensingGatewayDbContext.DbContext.SaveChangesAsync();
    }

    private async Task DeleteDricrectorsInGroups(IQueryable<DirectorGroup> groups)
    {
        foreach (var group in groups)
        {
            group.Directors.Clear();
        }

        await licensingGatewayDbContext.DbContext.SaveChangesAsync();
    }
}
