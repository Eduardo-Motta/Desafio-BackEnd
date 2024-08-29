using Domain.Repositories;
using Domain.Services.Contracts;
using Microsoft.Extensions.Logging;
using Shared.Utils;

namespace Domain.Services.Motorcycle
{
    public class DeleteMotorcycleService : IDeleteMotorcycleService
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IRentReposotory _rentRepository;
        private readonly ILogger _logger;

        public DeleteMotorcycleService(IRentReposotory rentRepository, IMotorcycleRepository motorcycleRepository, ILogger<DeleteMotorcycleService> logger, IRentReposotory rentReposotory)
        {
            _rentRepository = rentRepository;
            _motorcycleRepository = motorcycleRepository;
            _logger = logger;
        }

        public async Task<Either<Error, bool>> Delete(Guid motorcycleId, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Deleting motorcycle with id: {MotorcycleId}", motorcycleId);

                var motorcycle = await _motorcycleRepository.FindMotorcycleById(motorcycleId, cancellationToken);

                if (motorcycle is null)
                {
                    _logger.LogWarning("Motorcycle with this id not found: {MotorcycleId}", motorcycleId);
                    return Either<Error, bool>.LeftValue(new Error("Not found"));
                }

                var exitRentToMotorcycle = await _rentRepository.ExistsRentToMotorcycleId(motorcycleId, cancellationToken);

                if (exitRentToMotorcycle)
                {
                    _logger.LogWarning("A motorcycle with this id {MotorcycleId} has rental records associated with it", motorcycleId);
                    return Either<Error, bool>.LeftValue(new Error("The motorcycle cannot be removed because there are rental records associated with it"));
                }

                 await _motorcycleRepository.DeleteMotorcycle(motorcycle, cancellationToken);

                return Either<Error, bool>.RightValue(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error while delete the motorcycle");
                return Either<Error, bool>.LeftValue(new Error("An error occurred while delete the motorcycle"));
            }
        }
    }
}
