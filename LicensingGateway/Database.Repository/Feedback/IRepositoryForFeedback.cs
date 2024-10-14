using Database.Repository.Core.ReadWrite;

namespace Database.Repository.Feedback;

public interface IRepositoryForFeedback : IReadWriteIntPkRepository<Entites.Feedback>
{
}
