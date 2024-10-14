using Database.Entites;
using Database.Repository.Core.ReadWrite;

namespace Database.Repository.CQC;
public class RespositoryForCQCProviderImportInstance : ReadWriteGuidPkRepository<CQCProviderImportInstance>,  IRespositoryForCQCProviderImportInstance
{
    public RespositoryForCQCProviderImportInstance(ILicensingGatewayDbContext licensingGatewayDbContext) : base(licensingGatewayDbContext)
    {
    }
}
