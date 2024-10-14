namespace Database.LicenceHolder.Readonly.Repository;
public interface ILicenceHolderRespositoryForLicence
{
    Task<bool> HasActiveLicence(string cqcProviderId);
}
