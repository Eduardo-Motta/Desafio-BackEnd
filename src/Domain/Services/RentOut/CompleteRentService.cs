using Domain.Entities;
using Domain.Repositories;
using Domain.Services.Contracts;
using Microsoft.Extensions.Logging;
using Shared.Utils;

namespace Domain.Services.RentOut
{
    public class CompleteRentService : ICompleteRentService
    {
        private readonly IRentReposotory _rentOutReposotory;
        private readonly ILogger _logger;

        public CompleteRentService(IRentReposotory rentOutReposotory, ILogger<CompleteRentService> logger)
        {
            _rentOutReposotory = rentOutReposotory;
            _logger = logger;
        }

        public async Task<Either<Error, RentEntity>> Complete(Guid rentId, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Completing the Rental with Id: {RentId}", rentId);

                var rent = await _rentOutReposotory.FindRentById(rentId, cancellationToken);

                if (rent is null)
                {
                    _logger.LogWarning("The rent with this id not found: {RentId}", rentId);
                    return Either<Error, RentEntity>.LeftValue(new Error("Not found"));
                }

                if (rent.Status == Enums.ERentStatus.Completed)
                {
                    _logger.LogWarning("Rental status is now Completed");
                    return Either<Error, RentEntity>.LeftValue(new Error("The rental has already been completed"));
                }

                _logger.LogInformation("Ending rental");

                rent.CloseRental(DateTime.Now.Date);

                _logger.LogInformation("Rent recalculated {@Rent}", rent);

                await _rentOutReposotory.UpdateRent(rent, cancellationToken);

                _logger.LogInformation("Rental updated");

                return Either<Error, RentEntity>.RightValue(rent);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "error while creating the rental");
                return Either<Error, RentEntity>.LeftValue(new Error("An error occurred while complete the rental"));
            }
        }
    }
}
