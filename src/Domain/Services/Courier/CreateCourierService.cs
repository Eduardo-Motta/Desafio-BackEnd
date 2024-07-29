using Domain.Entities;
using Domain.Repositories;
using Domain.Services.Contracts;
using Microsoft.Extensions.Logging;
using Shared.Utils;

namespace Domain.Services.Courier
{
    public class CreateCourierService : ICreateCourierService
    {
        private readonly ICourierRespository _courierRespository;
        private readonly ILogger _logger;
        public CreateCourierService(ICourierRespository courierRespository, ILogger<CreateCourierService> logger)
        {
            _courierRespository = courierRespository;
            _logger = logger;
        }

        public async Task<Either<Error, Guid>> CreateCourier(CourierEntity courier, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Creating courier with CNPJ: {Cnpj}", courier.Cnpj);

                var courierByCnpj = await _courierRespository.FindCourierByCnpj(courier.Cnpj, cancellationToken);

                if (courierByCnpj is not null)
                {
                    _logger.LogInformation("A courier with this CNPJ already exists: {Cnpj}", courier.Cnpj);
                    return Either<Error, Guid>.LeftValue(new Error("Cnpj is already in use"));
                }

                var courierByDrivingLicense = await _courierRespository.FindCourierByDrivingLicense(courier.DrivingLicense, cancellationToken);

                if (courierByDrivingLicense is not null)
                {
                    _logger.LogInformation("A courier with driving license already exists: {DrivingLicense}", courier.DrivingLicense);
                    return Either<Error, Guid>.LeftValue(new Error("Driving License is already in use"));
                }

                await _courierRespository.CreateCourier(courier, cancellationToken);
                _logger.LogInformation("Courier created sucessfullly with CNPJ: {Cnpj}", courier.Cnpj);

                return Either<Error, Guid>.RightValue(courier.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error while creating the courier");
                return Either<Error, Guid>.LeftValue(new Error("An error occurred while creating the courier"));
            }
        }
    }
}
