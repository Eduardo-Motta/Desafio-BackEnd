using Domain.Entities;
using Shared.Utils;

namespace Domain.Services.Contracts
{
    public interface IAuthService
    {
        Task<Either<Error, UserEntity>> Authenticate(string identity, string password, CancellationToken cancellationToken);
    }
}
