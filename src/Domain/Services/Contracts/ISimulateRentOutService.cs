using Domain.Entities;
using Shared.Utils;

namespace Domain.Services.Contracts
{
    public interface ISimulateRentOutService
    {
        Task<Either<Error, RentEntity>> SimulateRent(string courierCnpj, Guid motorcycleId, Guid planId, DateTime expectedEndDate, CancellationToken cancellationToken);
    }
}
