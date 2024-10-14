using Database.Contexts;
using Database.Logic;
using Database.Repositories.Core;
using Domain.Objects.Database;
using Domain.Objects.Database.DTO;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories.User;

public class RepositoryForUser(
    LicenceHolderDbContext dbContext,
    IStringLengthRestriction stringLengthRestriction) : ReadWriteRepository<Entites.User>(dbContext), IRepositoryForUser
{
    private readonly IStringLengthRestriction stringLengthRestriction = stringLengthRestriction;

    public Task Delete(string oktaId, CancellationToken cancellationToken)
    {
        return dbContext.Users
            .Where(u => u.OktaId == oktaId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public Task<bool> Exists(string oktaId, CancellationToken cancellationToken)
    {
        return dbContext.Users.AnyAsync(u => u.OktaId == oktaId, cancellationToken);
    }

    public async Task<UserDTO> GetDetails(string oktaId, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FirstAsync(u => u.OktaId == oktaId, cancellationToken: cancellationToken);

        return new UserDTO()
        {
            Id = user.Id,
            Forename = user.Firstname,
            Surname = user.Lastname
        };
    }

    public async Task<UserDTOWithEmail> GetDetailsWithEmail(
        string oktaId,
        CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FirstAsync(u => u.OktaId == oktaId, cancellationToken);

        return new UserDTOWithEmail()
        {
            Id = user.Id,
            Forename = user.Firstname,
            Surname = user.Lastname,
            Email = user.Email
        };
    }

    public async Task<int> GetIdAsync(string oktaId, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FirstAsync(u => u.OktaId == oktaId, cancellationToken);

        return user.Id;
    }

    public async Task<int> GetOrganisationId(string oktaId, CancellationToken cancellationToken)
    {
        return await dbContext.Users
            .Where(u => u.OktaId == oktaId)
            .Select(u => u.OrganisationId)
            .FirstAsync(cancellationToken);
    }

    public async Task<string> GetUserFullName(string oktaId, CancellationToken cancellationToken)
    {
        var userRole = await dbContext.Users
            .Where(u => u.OktaId == oktaId)
            .FirstAsync(cancellationToken);

        return $"{userRole.Firstname} {userRole.Lastname}";
    }

    public async Task<UserRole> GetUserRole(string oktaId, CancellationToken cancellationToken)
    {
        var userRole = await dbContext.Users
            .Where(u => u.OktaId == oktaId)
            .Select(u => u.UserRole)
            .FirstAsync(cancellationToken);

        return userRole.Role;
    }

    public async Task<bool> IsEmailInUse(string email, CancellationToken cancellationToken)
    {
        return await dbContext.Users.AnyAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<bool> UpdateUserWhenLoggingInAsync(
        string oktaId,
        string firstname,
        string lastname, 
        string email,
        bool emailIsVerified)
    {
        var user = await dbContext.Users.FirstAsync(u => u.OktaId == oktaId);

        user.Firstname = stringLengthRestriction.Restrict(firstname, 100);
        user.Lastname = stringLengthRestriction.Restrict(lastname, 100);
        user.Email = stringLengthRestriction.Restrict(email, 254);
        user.EmailIsVerified = emailIsVerified;

        await SaveChangesAsync();

        return user.IsDeleted;
    }

    public Task<bool> UserExistsInOrganisation(
        int userId,
        int organisationId,
        CancellationToken cancellationToken)
    {
        return dbContext.Users
            .AnyAsync(u => u.Id == userId && u.OrganisationId == organisationId, cancellationToken: cancellationToken);
    }
}