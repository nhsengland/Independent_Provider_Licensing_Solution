using Database.Repository.Core.ReadWrite;

namespace Database.Repository.Feedback;

public class RepositoryForFeedback(ILicensingGatewayDbContext licensingGatewayDbContext) : ReadWriteIntPkRepository<Entites.Feedback>(licensingGatewayDbContext), IRepositoryForFeedback
{
}
