using Domain.Entities;
using Domain.Repositories;
using Domain.Services.Contracts;
using Microsoft.Extensions.Logging;
using Shared.Utils;

namespace Domain.Services.RentOut
{
    public class CreateRentOutService : ICreateRentOutService
    {
        private readonly IPlanRepository _planRepository;
        private readonly ICourierRespository _courierRespository;
        private readonly IRentReposotory _rentOutReposotory;
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly ILogger _logger;

        public CreateRentOutService(IRentReposotory rentOutReposotory, IPlanRepository planRepository, ICourierRespository courierRespository, IMotorcycleRepository motorcycleRepository, ILogger<CreateRentOutService> logger)
        {
            _planRepository = planRepository;
            _courierRespository = courierRespository;
            _rentOutReposotory = rentOutReposotory;
            _motorcycleRepository = motorcycleRepository;
            _logger = logger;
        }

        public async Task<Either<Error, RentEntity>> Create(string courierCnpj, Guid motorcycleId, Guid planId, DateTime expectedEndDate, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Find plan with Id: {PlanId}", planId);

                var plan = await _planRepository.FindPlanById(planId, cancellationToken);

                if (plan is null)
                {
                    _logger.LogWarning("A Plan with this id not found: {PlanId}", planId);
                    return Either<Error, RentEntity>.LeftValue(new Error("Plan not found"));
                }

                _logger.LogInformation("Find courier with CNPJ: {CourierCnpj}", courierCnpj);

                var courierByCnpj = await _courierRespository.FindCourierByCnpj(courierCnpj, cancellationToken);

                if (courierByCnpj is null)
                {
                    _logger.LogWarning("A courier with this CNPJ not found: {CourierCnpj}", courierCnpj);
                    return Either<Error, RentEntity>.LeftValue(new Error("Courier not found"));
                }

                if (courierByCnpj.DrivingLicenseCategory != Enums.EDrivingLicenseCategory.A && courierByCnpj.DrivingLicenseCategory != Enums.EDrivingLicenseCategory.AB)
                {
                    _logger.LogWarning("The courier with the CNPJ {CourierCnpj} does not have Category A. Current category: {DrivingLicenseCategory}", courierCnpj, courierByCnpj.DrivingLicenseCategory);
                    return Either<Error, RentEntity>.LeftValue(new Error("Only courier with category A can perform this operation"));
                }

                var motorcycle = await _motorcycleRepository.FindMotorcycleById(motorcycleId, cancellationToken);

                if (motorcycle is null)
                {
                    _logger.LogWarning("Motorcycle with this id not found: {MotorcycleId}", motorcycleId);
                    return Either<Error, RentEntity>.LeftValue(new Error("Motorcycle not found"));
                }

                _logger.LogInformation("Checking motorcycle availability for rental: {@Motorcycle}", motorcycle);

                var isTheMotorcycleRented = await _rentOutReposotory.ExistsRentInProgressToMotorcycleId(motorcycleId, cancellationToken);

                if (isTheMotorcycleRented)
                {
                    _logger.LogWarning("The motorcycle provided with this id is already rented: {MotorcycleId}", motorcycleId);
                    return Either<Error, RentEntity>.LeftValue(new Error("The motorcycle provided is already rented"));
                }

                var startDate = DateTime.Now.Date;
                var rentEntity = new RentEntity(courierByCnpj.Id, motorcycleId, plan, startDate, expectedEndDate);

                _logger.LogInformation("Saving rent: {Id}", rentEntity.Id);

                await _rentOutReposotory.CreateRent(rentEntity, cancellationToken);

                return Either<Error, RentEntity>.RightValue(rentEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error while creating the rental");
                return Either<Error, RentEntity>.LeftValue(new Error("An error occurred while creating the rental"));
            }
        }
    }
}
