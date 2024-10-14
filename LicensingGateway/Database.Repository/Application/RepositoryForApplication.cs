using Database.Repository.Core.ReadWrite;
using Domain.Models.Database;
using Domain.Models.Database.DTO;
using Microsoft.EntityFrameworkCore;

namespace Database.Repository.Application;
public class RepositoryForApplication(
    ILicensingGatewayDbContext licensingGatewayDbContext) : ReadWriteIntPkRepository<Entites.Application>(licensingGatewayDbContext), IRepositoryForApplication
{
    public async Task<string> GetApplicationCode(int id)
    {
        var code = await licensingGatewayDbContext.Application.Include(a => a.ApplicationCode).Where(e => e.Id == id).Select(e => e.ApplicationCode.Code).FirstOrDefaultAsync();

        return code ?? string.Empty;
    }

    public async Task<int> GetApplicationCodeId(int id)
    {
        return await licensingGatewayDbContext.Application.Where(e => e.Id == id).Select(e => e.ApplicationCodeId).FirstOrDefaultAsync();
    }

    public async Task<ApplicationDTO> GetApplicationDTO(int id)
    {
        return await licensingGatewayDbContext.Application.Include(a => a.ApplicationCode).Where(e => e.Id == id)
            .Select(e => new ApplicationDTO
            {
                ApplicationCode = e.ApplicationCode.Code,
                CompanyNumberCheck = e.CompanyNumberCheck,
                CompanyNumber = e.CompanyNumber ?? string.Empty,
                CQCProviderID = e.CQCProviderID ?? string.Empty,
                CQCProviderName = e.CQCProviderName ?? string.Empty,
                CQCProviderAddress = e.CQCProviderAddress ?? string.Empty,
                CQCProviderPhoneNumber = e.CQCProviderPhoneNumber ?? string.Empty,
                ReferenceId = e.ReferenceId ?? string.Empty,
                SubmitApplication = e.SubmitApplication,
                UltimateController = e.UltimateController,
                DirectorsCheck = e.DirectorsCheck,
                CorporateDirectorsCheck = e.CorporateDirectorsCheck,
                DirectorsSatisfyG3FitAndProperRequirements = e.DirectorsSatisfyG3FitAndProperRequirements,
                DirectorsSatisfyG3FitAndProperRequirements_IfNoWhy = e.DirectorsSatisfyG3FitAndProperRequirementsIfNoWhy ?? string.Empty,
                LastFinancialYear = e.LastFinancialYear,
                NextFinancialYear = e.NextFinancialYear,
                OneOrMoreParentCompanies = e.OneOrMoreParentCompanies,
                NewlyIncorporatedCompany = e.NewlyIncorporatedCompany,
            })
            .FirstOrDefaultAsync() ?? throw new Exception("Entity not found");
    }

    public async Task<string> GetCompanyNumber(int id)
    {
        var companyNumber = await licensingGatewayDbContext.Application.Where(e => e.Id == id)
            .Select(e => e.CompanyNumber)
            .FirstOrDefaultAsync();

        return companyNumber ?? string.Empty;
    }

    public async Task<bool?> GetCompanyNumberCheck(int id)
    {
        return await licensingGatewayDbContext.Application.Where(e => e.Id == id)
            .Select(e => e.CompanyNumberCheck)
            .FirstOrDefaultAsync();
    }

    public async Task<ContactDetailsDTO> GetContactDetails(int id)
    {
        var contactDetails = await licensingGatewayDbContext.Application.Where(e => e.Id == id)
            .Select(e => new ContactDetailsDTO()
            {
                Forename = e.Forename ?? string.Empty,
                Surname = e.Surname ?? string.Empty,
                JobTitle = e.JobTitle ?? string.Empty,
                Email = e.Email ?? string.Empty,
                ElectronicCommunications = e.ElectronicCommunications,
            })
            .FirstOrDefaultAsync() ?? throw new Exception($"Application record not found: {id}");

        return contactDetails;
    }

    public async Task<bool?> GetDirectorsCheck(int id)
    {
        return await licensingGatewayDbContext.Application.Where(e => e.Id == id)
            .Select(e => e.DirectorsCheck)
            .FirstOrDefaultAsync();
    }

    public async Task<bool?> GetCorporateDirectorsCheck(int id)
    {
        return await licensingGatewayDbContext.Application.Where(e => e.Id == id)
            .Select(e => e.CorporateDirectorsCheck)
            .FirstOrDefaultAsync();
    }

    public async Task<string> GetCQCProviderAddress(int id)
    {
        var address = await licensingGatewayDbContext.Application.Where(e => e.Id == id)
            .Select(e => e.CQCProviderAddress)
            .FirstOrDefaultAsync();

        return address ?? string.Empty;
    }

    public async Task<CQCProviderDetailsDTO> GetCQCProviderDetails(int id)
    {
        var deatils = await licensingGatewayDbContext.Application.Where(e => e.Id == id)
            .Select(e => new CQCProviderDetailsDTO()
            {
                Id = e.CQCProviderID ?? string.Empty,
                Name = e.CQCProviderName ?? string.Empty,
                Address = e.CQCProviderAddress ?? string.Empty,
                PhoneNumber = e.CQCProviderPhoneNumber ?? string.Empty,
                WebsiteURL = e.CQCProviderWebsiteURL ?? string.Empty,
            })
            .FirstOrDefaultAsync();

        return deatils ?? throw new InvalidOperationException($"CQC provider details not found: {id}");
    }

    public async Task<string> GetCQCProviderID(int id)
    {
        var cqcProviderId = await licensingGatewayDbContext.Application.Where(e => e.Id == id)
            .Select(e => e.CQCProviderID)
            .FirstOrDefaultAsync();

        return cqcProviderId ?? string.Empty;
    }

    public async Task<string> GetCQCProviderName(int id)
    {
        var name = await licensingGatewayDbContext.Application.Where(e => e.Id == id)
            .Select(e => e.CQCProviderName)
            .FirstOrDefaultAsync();

        return name ?? string.Empty;
    }

    public async Task<string> GetCQCProviderPhoneNumber(int id)
    {
        var phoneNumber = await licensingGatewayDbContext.Application.Where(e => e.Id == id)
            .Select(e => e.CQCProviderPhoneNumber)
            .FirstOrDefaultAsync();

        return phoneNumber ?? string.Empty;
    }

    public async Task<string> GetCQCProviderWebsiteURL(int id)
    {
        var websiteURL = await licensingGatewayDbContext.Application.Where(e => e.Id == id)
            .Select(e => e.CQCProviderWebsiteURL)
            .FirstOrDefaultAsync();

        return websiteURL ?? string.Empty;
    }

    public async Task<ApplicationPage> GetCurrentPage(int id)
    {
        var currentPage = await licensingGatewayDbContext.Application.Where(e => e.Id == id)
            .Select(e => e.CurrentPage)
            .FirstOrDefaultAsync();

        return currentPage?.PageName ?? ApplicationPage.ApplicationCode;
    }

    public async Task<bool?> GetDirectorsSatisfyG3FitAndProperRequirements(int id)
    {
        return await licensingGatewayDbContext.Application.Where(e => e.Id == id)
            .Select(e => e.DirectorsSatisfyG3FitAndProperRequirements)
            .FirstOrDefaultAsync();
    }

    public async Task<string> GetDirectorsSatisfyG3FitAndProperRequirementsIfNoWhy(int id)
    {
        var ifNoWhy = await licensingGatewayDbContext.Application.Where(e => e.Id == id)
           .Select(e => e.DirectorsSatisfyG3FitAndProperRequirementsIfNoWhy)
           .FirstOrDefaultAsync();

        return ifNoWhy ?? string.Empty;
    }

    public async Task<DateOnly?> GetFinancialYearEndLast(int id)
    {
        return await licensingGatewayDbContext.Application.Where(e => e.Id == id)
            .Select(e => e.LastFinancialYear)
            .FirstOrDefaultAsync();
    }

    public async Task<DateOnly?> GetFinancialYearEndNext(int id)
    {
        return await licensingGatewayDbContext.Application.Where(e => e.Id == id)
            .Select(e => e.NextFinancialYear)
            .FirstOrDefaultAsync();
    }

    public async Task<bool?> GetNewlyIncorporatedCompany(int id)
    {
        return await licensingGatewayDbContext.Application.Where(e => e.Id == id)
            .Select(e => e.NewlyIncorporatedCompany)
            .FirstOrDefaultAsync();
    }

    public async Task<bool?> GetOneOrMoreParentCompanies(int id)
    {
        return await licensingGatewayDbContext.Application.Where(e => e.Id == id)
            .Select(e => e.OneOrMoreParentCompanies)
            .FirstOrDefaultAsync();
    }

    public async Task<string> GetReferenceId(int id)
    {
        var referenceID = await licensingGatewayDbContext.Application.Where(e => e.Id == id)
            .Select(e => e.ReferenceId)
            .FirstOrDefaultAsync();

        return referenceID ?? string.Empty;
    }

    public async Task<bool> GetSubmitApplication(int id)
    {
        return await licensingGatewayDbContext.Application.Where(e => e.Id == id)
            .Select(e => e.SubmitApplication)
            .FirstOrDefaultAsync();
    }

    public async Task<bool?> GetUltimateController(int id)
    {
        return await licensingGatewayDbContext.Application.Where(e => e.Id == id)
            .Select(e => e.UltimateController)
            .FirstOrDefaultAsync();
    }

    public async Task Set(int id, ContactDetailsDTO contactDetails)
    {
        await licensingGatewayDbContext.Application.Where(e => e.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(b => b.Forename, contactDetails.Forename)
                .SetProperty(b => b.Surname, contactDetails.Surname)
                .SetProperty(b => b.JobTitle, contactDetails.JobTitle)
                .SetProperty(b => b.Email, contactDetails.Email)
                .SetProperty(b => b.ElectronicCommunications, contactDetails.ElectronicCommunications)
            );
    }

    public async Task SetCompanyNumber(int id, string value)
    {
        await licensingGatewayDbContext.Application.Where(e => e.Id == id)
        .ExecuteUpdateAsync(b =>
                b.SetProperty(e => e.CompanyNumber, value)
            );
    }

    public async Task SetCompanyNumberCheck(int id, bool? value)
    {
        await licensingGatewayDbContext.Application.Where(e => e.Id == id)
        .ExecuteUpdateAsync(b =>
                b.SetProperty(e => e.CompanyNumberCheck, value)
            );
    }

    public async Task SetCorporateDirectorsCheck(int id, bool? value)
    {
        await licensingGatewayDbContext.Application.Where(e => e.Id == id)
        .ExecuteUpdateAsync(b =>
                b.SetProperty(e => e.CorporateDirectorsCheck, value)
            );
    }

    public async Task SetDirectorsCheck(int id, bool? value)
    {
        await licensingGatewayDbContext.Application.Where(e => e.Id == id)
        .ExecuteUpdateAsync(b =>
                b.SetProperty(e => e.DirectorsCheck, value)
            );
    }

    public async Task SetCQCProviderAddress(int id, string value)
    {
        await licensingGatewayDbContext.Application.Where(e => e.Id == id)
        .ExecuteUpdateAsync(b =>
                b.SetProperty(e => e.CQCProviderAddress, value)
            );
    }

    public async Task SetCQCProviderDetails(int id, CQCProviderDetailsWithoutIdDTO details)
    {
        await licensingGatewayDbContext.Application.Where(e => e.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(b => b.CQCProviderName, details.Name)
                .SetProperty(b => b.CQCProviderAddress, details.Address)
                .SetProperty(b => b.CQCProviderPhoneNumber, details.PhoneNumber)
                .SetProperty(b => b.CQCProviderWebsiteURL, details.WebsiteURL)
            );
    }

    public async Task SetCQCProviderID(int id, string value)
    {
        await licensingGatewayDbContext.Application.Where(e => e.Id == id)
        .ExecuteUpdateAsync(b =>
                b.SetProperty(e => e.CQCProviderID, value)
            );
    }

    public async Task SetCQCProviderName(int id, string value)
    {
        await licensingGatewayDbContext.Application.Where(e => e.Id == id)
        .ExecuteUpdateAsync(b =>
                b.SetProperty(e => e.CQCProviderName, value)
            );
    }

    public async Task SetCQCProviderPhoneNumber(int id, string value)
    {
        await licensingGatewayDbContext.Application.Where(e => e.Id == id)
        .ExecuteUpdateAsync(b =>
                b.SetProperty(e => e.CQCProviderPhoneNumber, value)
            );
    }

    public async Task SetCQCProviderWebsiteURL(int id, string value)
    {
        await licensingGatewayDbContext.Application.Where(e => e.Id == id)
        .ExecuteUpdateAsync(b =>
                b.SetProperty(e => e.CQCProviderWebsiteURL, value)
            );
    }

    public async Task SetCurrentPage(int id, ApplicationPage applicationPage)
    {
        await licensingGatewayDbContext.Application.Where(e => e.Id == id)
        .ExecuteUpdateAsync(b =>
                b.SetProperty(e => e.CurrentPageId, (int)applicationPage)
            );
    }

    public async Task SetDirectorsSatisfyG3FitAndProperRequirements(int id, bool? value)
    {
        await licensingGatewayDbContext.Application.Where(e => e.Id == id)
        .ExecuteUpdateAsync(b =>
                b.SetProperty(e => e.DirectorsSatisfyG3FitAndProperRequirements, value)
            );
    }

    public async Task SetDirectorsSatisfyG3FitAndProperRequirementsIfNoWhy(int id, string value)
    {
        await licensingGatewayDbContext.Application.Where(e => e.Id == id)
        .ExecuteUpdateAsync(b =>
                b.SetProperty(e => e.DirectorsSatisfyG3FitAndProperRequirementsIfNoWhy, value)
            );
    }

    public async Task SetLastFinancialYear(int id, DateOnly? value)
    {
        await licensingGatewayDbContext.Application.Where(e => e.Id == id)
        .ExecuteUpdateAsync(b =>
                b.SetProperty(e => e.LastFinancialYear, value)
            );
    }

    public async Task SetNewlyIncorporatedCompany(int id, bool? value)
    {
        await licensingGatewayDbContext.Application.Where(e => e.Id == id)
        .ExecuteUpdateAsync(b =>
                b.SetProperty(e => e.NewlyIncorporatedCompany, value)
            );
    }

    public async Task SetNextFinancialYear(int id, DateOnly? value)
    {
        await licensingGatewayDbContext.Application.Where(e => e.Id == id)
        .ExecuteUpdateAsync(b =>
                b.SetProperty(e => e.NextFinancialYear, value)
            );
    }

    public async Task SetReferenceID(int id, string referenceID)
    {
        await licensingGatewayDbContext.Application.Where(e => e.Id == id)
        .ExecuteUpdateAsync(b =>
                b.SetProperty(e => e.ReferenceId, referenceID)
            );
    }

    public async Task SetSubmitApplication(int id, DateTime dateTime)
    {
        await licensingGatewayDbContext.Application.Where(e => e.Id == id)
        .ExecuteUpdateAsync(b =>
                b.SetProperty(e => e.SubmitApplication, true)
                .SetProperty(e => e.DateModified, dateTime)
            );
    }

    public async Task SetUltimateController(int id, bool? value)
    {
        await licensingGatewayDbContext.Application.Where(e => e.Id == id)
        .ExecuteUpdateAsync(b =>
                b.SetProperty(e => e.UltimateController, value)
            );
    }

    public async Task SetOneOrMoreParentCompanies(int id, bool? value)
    {
        await licensingGatewayDbContext.Application.Where(e => e.Id == id)
        .ExecuteUpdateAsync(b =>
                b.SetProperty(e => e.OneOrMoreParentCompanies, value)
            );
    }
}
