using Domain.Entities;
using Domain.Repositories;
using Domain.Services.Contracts;
using Microsoft.Extensions.Logging;
using Shared.Utils;

namespace Domain.Services.Motorcycle
{
    public class FindMotorcycleService : IFindMotorcycleService
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly ILogger _logger;

        public FindMotorcycleService(IMotorcycleRepository motorcycleRepository, ILogger<FindMotorcycleService> logger)
        {
            _motorcycleRepository = motorcycleRepository;
            _logger = logger;
        }

        public async Task<Either<Error, IEnumerable<MotorcycleEntity>>> All(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Starting service to search for motorcycles");

                var motorcycles = await _motorcycleRepository.AllMotorcycles(cancellationToken);

                return Either<Error, IEnumerable<MotorcycleEntity>>.RightValue(motorcycles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while searching for the motorcycles");
                return Either<Error, IEnumerable<MotorcycleEntity>>.LeftValue(new Error("An error occurred while searching for the motorcycles"));
            }
        }

        public async Task<Either<Error, MotorcycleEntity>> FindByLicensePlate(string licensePlate, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Starting service to search for motorcycle by license plante");

                var motorcycle = await _motorcycleRepository.FindMotorcycleByLicensePlate(licensePlate, cancellationToken);

                if (motorcycle is null)
                {
                    _logger.LogWarning("Motorcycle with this license plante not found: {LicensePlate}", licensePlate);
                    return Either<Error, MotorcycleEntity>.LeftValue(new Error("Not found"));
                }

                return Either<Error, MotorcycleEntity>.RightValue(motorcycle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while searching for the motorcycle");
                return Either<Error, MotorcycleEntity>.LeftValue(new Error("An error occurred while searching for the motorcycle"));
            }
        }

        public async Task<Either<Error, IEnumerable<MotorcycleEntity>>> AllAvailableMotorcycles(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Starting service to search for all available motorcycles");

                var motorcycles = await _motorcycleRepository.AllAvailableMotorcycles(cancellationToken);

                return Either<Error, IEnumerable<MotorcycleEntity>>.RightValue(motorcycles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while searching for all available motorcycles");
                return Either<Error, IEnumerable<MotorcycleEntity>>.LeftValue(new Error("An error occurred while searching for available motorcycles"));
            }
        }
    }
}
