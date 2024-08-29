using Domain.Entities;
using Shared.Utils;

namespace Domain.Services.Contracts
{
    public interface ICreateCourierService
    {
        Task<Either<Error, Guid>> CreateCourier(CourierEntity courier, UserEntity user, CancellationToken cancellationToken);
    }
}
