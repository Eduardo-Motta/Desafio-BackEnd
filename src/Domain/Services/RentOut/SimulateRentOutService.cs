using Domain.Entities;
using Domain.Repositories;
using Domain.Services.Contracts;
using Microsoft.Extensions.Logging;
using Shared.Utils;

namespace Domain.Services.RentOut
{
    public class SimulateRentOutService : ISimulateRentOutService
    {
        private readonly IPlanRepository _planRepository;
        private readonly ICourierRespository _courierRespository;
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly ILogger _logger;

        public SimulateRentOutService(IPlanRepository planRepository, ICourierRespository courierRespository, IMotorcycleRepository motorcycleRepository, ILogger<SimulateRentOutService> logger)
        {
            _planRepository = planRepository;
            _courierRespository = courierRespository;
            _motorcycleRepository = motorcycleRepository;
            _logger = logger;
        }

        public async Task<Either<Error, RentEntity>> SimulateRent(string courierCnpj, Guid motorcycleId, Guid planId, DateTime expectedEndDate, CancellationToken cancellationToken)
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
                    _logger.LogWarning("Motorcycle with this license plante not found: {MotorcycleId}", motorcycleId);
                    return Either<Error, RentEntity>.LeftValue(new Error("Motorcycle not found"));
                }

                var startDate = DateTime.Now.Date;
                var rentEntity = new RentEntity(courierByCnpj.Id, motorcycleId, plan, startDate, expectedEndDate.Date);

                return Either<Error, RentEntity>.RightValue(rentEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error while simulating the rental");
                return Either<Error, RentEntity>.LeftValue(new Error("An error occurred while simulating the rental"));
            }
        }
    }
}
