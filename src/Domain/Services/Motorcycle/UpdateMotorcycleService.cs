using Domain.Repositories;
using Domain.Services.Contracts;
using Microsoft.Extensions.Logging;
using Shared.Utils;

namespace Domain.Services.Motorcycle
{
    public class UpdateMotorcycleService : IUpdateMotorcycleService
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly ILogger _logger;

        public UpdateMotorcycleService(IMotorcycleRepository motorcycleRepository, ILogger<UpdateMotorcycleService> logger)
        {
            _motorcycleRepository = motorcycleRepository;
            _logger = logger;
        }

        public async Task<Either<Error, bool>> Update(Guid motorcycleId, string licensePlate, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Updating motorcycle with license plate: {LicensePlate}", licensePlate);

                var motercycle = await _motorcycleRepository.FindMotorcycleById(motorcycleId, cancellationToken);

                if (motercycle is null)
                {
                    _logger.LogWarning("A motorcycle with this id not found {Id}", motorcycleId);
                    return Either<Error, bool>.LeftValue(new Error("Not found"));
                }

                var motorcycleByLicensePlate = await _motorcycleRepository.FindMotorcycleByLicensePlate(licensePlate, cancellationToken);

                if (motorcycleByLicensePlate is not null && motorcycleByLicensePlate.Id != motercycle.Id)
                {
                    _logger.LogWarning("A motorcycle with this license plate already exists: {LicensePlate}", licensePlate);
                    return Either<Error, bool>.LeftValue(new Error("License plate is already in use"));
                }

                motercycle.Update(licensePlate);

                _logger.LogInformation("Updating motorcycle with license plate {LicensePlate} to new license plate {NewLicensePlate}", motercycle.LicensePlate, licensePlate);
                
                await _motorcycleRepository.UpdateMotorcycle(motercycle, cancellationToken);
                
                _logger.LogInformation("Motorcycle updated sucessfullly with new license plate: {LicensePlate}", licensePlate);

                return Either<Error, bool>.RightValue(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error while updating the motorcycle");
                return Either<Error, bool>.LeftValue(new Error("An error occurred while updating the motorcycle"));
            }
        }
    }
}
