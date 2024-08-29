using Domain.Entities;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        Task<UserEntity?> FindUser(string username, string password, CancellationToken cancellationToken);
        Task CreateUser(UserEntity entity, CancellationToken cancellationToken);
    }
}
