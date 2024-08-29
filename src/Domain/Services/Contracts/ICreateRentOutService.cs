using Domain.Entities;
using Shared.Utils;

namespace Domain.Services.Contracts
{
    public interface ICreateRentOutService
    {
        Task<Either<Error, RentEntity>> Create(string courierCnpj, Guid motorcycleId, Guid planId, DateTime expectedEndDate, CancellationToken cancellationToken);
    }
}
